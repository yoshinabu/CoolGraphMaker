using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoolGraphMaker
{
    public partial class GpaphMaker : Form
    {
        
        /// <summary>
        /// Set mergin of left, top, right, bottom to draw texts
        /// </summary>
        readonly float leftMergin = 15;
        readonly float topMergin = 10;
        readonly float rightMergin = 5;
        readonly float bottomMergin = 20;

        // 1 step size 
        // think like 1 % of each size
        float xUnit;
        float yUnit;

        // configuration xml source
        System.Xml.Linq.XElement xmlSource;

        // Contains all drawing layers
        List<GraphicLayer> graphicLayers;

        // Graph area
        Rectangle graphBorderRect;
        Point graphBorderLTPoint; // left and top
        Point graphBorderLBPoint; // left and top
        Point graphBorderRTPoint; // right and bottom
        Point graphBorderRBPoint; // right and bottom

        // Scale string drawing point
        List<Point> xScalePoints;
        List<float> xScaleValues;
        List<Point> yScalePoints;
        List<float> yScaleValues;

        // Data to be drawn
        List<List<float>> xDataSet;
        List<List<float>> yDataSet;

        // First drawing has done
        bool hasDrawnOnce;
        enum SCALETYPE {
            SCALE_LINEAR = 0,
            SCALE_LOG,
            SCALE_NONDEF,
        };

        public GpaphMaker()
        {
            InitializeComponent();

            hasDrawnOnce = false;
            xmlSource = System.Xml.Linq.XElement.Load(@".\GraphDataSetting.xml");
            graphicLayers = new List<GraphicLayer>();
            InitializeMenu();

            xDataSet = new List<List<float>>();
            yDataSet = new List<List<float>>();

            xScalePoints = new List<Point>();
            xScaleValues = new List<float>();
            yScalePoints = new List<Point>();
            yScaleValues = new List<float>();

            // Start drawing
            DrawGraph();

            hasDrawnOnce = true;
        }

        // Implementation
        // Add graph selection combo box
        private void InitializeMenu()
        {
            // Use Linq to retrive <Pair> elements
            var pairs = (
                from p in xmlSource.Elements()
                select p);

            // Add GraphTitle to combo box
            foreach (System.Xml.Linq.XElement pair in pairs)
            {
                graphSelectionComboBox.Items.Add(pair.Attribute("GraphTitle").Value.ToString());
            }
            graphSelectionComboBox.SelectedIndex = 0;
        }

        private void DrawGraph()
        {
            CalculateGeneralInfo();
            DrawGraphArea();
            DrawTitle();
            //DrawLegend();
            DrawMiscInfo();
            DrawLine();

            DrawAllLayers();
        }

        private void DrawMiscInfo()
        {

        }

        private void DrawLegend()
        {
            throw new NotImplementedException();
        }
        private void CalculateGeneralInfo()
        {
            Rectangle clientArea = graphArea.ClientRectangle;

            xUnit = clientArea.Width / 100;
            yUnit = clientArea.Height / 100;

            // Calculate each edge
            graphBorderLTPoint.X = (int)(xUnit * leftMergin);
            graphBorderLTPoint.Y = (int)(yUnit * topMergin);
            graphBorderLBPoint.X = graphBorderLTPoint.X;
            graphBorderLBPoint.Y = (int)(clientArea.Bottom - (yUnit * bottomMergin));
            graphBorderRTPoint.X = (int)(clientArea.Right - (xUnit * rightMergin));
            graphBorderRTPoint.Y = graphBorderLTPoint.Y;
            graphBorderRBPoint.X = graphBorderRTPoint.X;
            graphBorderRBPoint.Y = (int)(clientArea.Bottom - (yUnit * bottomMergin));

            graphBorderRect = new Rectangle(
                graphBorderLTPoint.X, graphBorderLTPoint.Y,
                graphBorderRBPoint.X - graphBorderLTPoint.X,
                graphBorderRBPoint.Y - graphBorderLTPoint.Y);
        }
        private void DrawGraphArea()
        {
            DrawBackGround();
            DrawGraphBorder();
            DrawXVerticalLines();
            DrawYHorizontalLines();
        }

        private void DrawBackGround()
        {
            Bitmap canvas = new Bitmap(graphArea.Width, graphArea.Height);
            //canvas.MakeTransparent();
            graphArea.DrawToBitmap(canvas, graphArea.ClientRectangle);
            
            Graphics g = Graphics.FromImage(canvas);
            g.Dispose();

            // Add to layers
            GraphicLayer layer = new GraphicLayer("backGround", canvas, graphicLayers.Count());
            graphicLayers.Add(layer);
            canvas.Save(layer.zOrder + layer.name + ".png", 
                System.Drawing.Imaging.ImageFormat.Png);

        }

        private void DrawGraphBorder()
        {
            Bitmap canvas = new Bitmap(graphArea.Width, graphArea.Height);
            Graphics g = Graphics.FromImage(canvas);
            
            g.DrawRectangle(Pens.Red, graphBorderRect);
            g.Dispose();

            // Add to layers
            GraphicLayer layer = new GraphicLayer("border", canvas, graphicLayers.Count());
            graphicLayers.Add(layer);
            canvas.Save(layer.zOrder + layer.name + ".png",
                System.Drawing.Imaging.ImageFormat.Png);
        }

        private void DrawXVerticalLines()
        {
            Rectangle clientArea = graphArea.ClientRectangle;

            Bitmap canvas = new Bitmap(graphArea.Width, graphArea.Height);
            Graphics g = Graphics.FromImage(canvas);

            // Retrieve settings to draw lines from xml
            IEnumerable<System.Xml.Linq.XElement> pairs =
                xmlSource.Elements("Pair");

            string graph = graphSelectionComboBox.SelectedItem.ToString();

            System.Xml.Linq.XElement selectedPair =
                (from pair in pairs
                 where pair.Attribute("GraphTitle").Value.ToString() == graph
                 select pair).First();

            int max = (int)selectedPair.Attribute("MaximumOfX");
            int min = (int)selectedPair.Attribute("MinimunOfX");
            float majorLineSpan = (float)selectedPair.Attribute("MajorOfXAxis");
            float minorLineSpan = (float)selectedPair.Attribute("MinorOfXAxis");
            string scaleType = (string)selectedPair.Attribute("ScaleOfXAxis");
            int logBase = (int)selectedPair.Attribute("LogBaseOfXAxis");

            DrawMajorAndMinorLines(ref g, max, min, majorLineSpan, minorLineSpan, scaleType, logBase);

            if (xScalePoints.Count != xScaleValues.Count)
            {
                MessageBox.Show("Scale values and points to be drawn in Y scale are not same size.");
                return;
            }
            for  (int index = 0; index < xScalePoints.Count; index++)
            {
                DrawScaleString(xScaleValues[index], xScalePoints[index]);
            }
            g.Dispose();

            // Add to layers
            GraphicLayer layer = new GraphicLayer("x", canvas, graphicLayers.Count());
            graphicLayers.Add(layer);
            canvas.Save(layer.zOrder + layer.name + ".png",
                System.Drawing.Imaging.ImageFormat.Png);
        }

        private void DrawYHorizontalLines()
        {
            Rectangle clientArea = graphArea.ClientRectangle;

            Bitmap canvas = new Bitmap(graphArea.Width, graphArea.Height);
            Graphics g = Graphics.FromImage(canvas);

            // Retrieve settings to draw lines from xml
            IEnumerable<System.Xml.Linq.XElement> pairs =
                xmlSource.Elements("Pair");

            string graph = graphSelectionComboBox.SelectedItem.ToString();

            System.Xml.Linq.XElement selectedPair =
                (from pair in pairs
                 where pair.Attribute("GraphTitle").Value.ToString() == graph
                 select pair).First();
            
            int max = (int)selectedPair.Attribute("MaximumOfY");
            int min = (int)selectedPair.Attribute("MinimumOfY");
            float majorLineSpan = (float)selectedPair.Attribute("MajorOfYAxis");
            float minorLineSpan = (float)selectedPair.Attribute("MinorOfYAxis");
            string scaleType = (string)selectedPair.Attribute("ScaleOfYAxis");
            int logBase = (int)selectedPair.Attribute("LogBaseOfYAxis");

            DrawMajorAndMinorLines(ref g, max, min, majorLineSpan, minorLineSpan, scaleType, logBase, false);
            
            // Draw scale string
            if (yScalePoints.Count != yScaleValues.Count)
            {
                MessageBox.Show("Scale values and points to be drawn in Y scale are not same size.");
                return;
            }
            for (int index = 0; index < yScalePoints.Count; index++)
            {
                DrawScaleString(yScaleValues[index], yScalePoints[index], false);
            }

            g.Dispose();

            // Add to layers
            GraphicLayer layer = new GraphicLayer("y", canvas, graphicLayers.Count());
            graphicLayers.Add(layer);
            canvas.Save(layer.zOrder + layer.name + ".png",
                System.Drawing.Imaging.ImageFormat.Png);
        }


        private void DrawMajorAndMinorLines(ref Graphics g, int max, int min, float majorLineSpan, float minorLineSpan, string scaleType = "linear", int logBase = 10, bool drawX = true)
        {

            // do nothing and return if major line span is 0
            // This is stupid setting .
            if (majorLineSpan == 0)
            {
                return;
            }

            // Set minimum value of scale
            if (drawX)
                xScaleValues.Add(min);
            else
                yScaleValues.Add(min);

            // Draw major lines
            if (scaleType.CompareTo("linear") == 0)
            {
                for (float majorLine = min; majorLine <= max; )
                {
                    // distance in percent/100 from 0 point
                    float distance = GetDistance(min, max, majorLine, SCALETYPE.SCALE_LINEAR, drawX);

                    // calculate actual distance in applicatin
                    float actualDistance = 0.0f;
                    if (drawX)
                    {
                        actualDistance = graphBorderRect.Width * distance;
                    } 
                    else
                    {
                        actualDistance = graphBorderRect.Height * distance;
                    }

                    // Specify point draw from and to
                    Point start = new Point();
                    if (drawX == true)
                    {
                        start.X = graphBorderLBPoint.X + (int)actualDistance;
                        start.Y = graphBorderLBPoint.Y;
                    }
                    else // Drawing Y axis horizontal line
                    {
                        start.X = graphBorderLBPoint.X;
                        start.Y = graphBorderLBPoint.Y - (int)actualDistance;
                    }


                    Point end = new Point();
                    if (drawX == true)
                    {
                        end.X = start.X;
                        end.Y = graphBorderLTPoint.Y;
                    }
                    else
                    {
                        end.X = graphBorderRTPoint.X;
                        end.Y = start.Y;
                    }

                    g.DrawLine(Pens.Red, start, end);

                    // Keep points to draw scale string
                    if (drawX)
                        xScalePoints.Add(start);
                    else
                        yScalePoints.Add(start);

                    majorLine += majorLineSpan;

                    // Keep values to draw scale string
                    if (drawX)
                        xScaleValues.Add(majorLine);
                    else
                        yScaleValues.Add(majorLine);
                }
            }
            else if (scaleType.CompareTo("log") == 0)
            {
                for (float majorLine = min; majorLine < max; )
                {
                    // distance in percent/100 from 0 point
                    float distance = GetDistance(min, max, majorLine, SCALETYPE.SCALE_LOG, drawX);

                    // calculate actual distance in applicatin
                    float actualDistance = 0.0f;
                    if (drawX)
                    {
                        actualDistance = graphBorderRect.Width * distance;
                    }
                    else
                    {
                        actualDistance = graphBorderRect.Height * distance;
                    }

                    // Specify point draw from and to
                    Point start = new Point();
                    if (drawX == true)
                    {
                        start.X = graphBorderLBPoint.X + (int)actualDistance;
                        start.Y = graphBorderLBPoint.Y;
                    }
                    else
                    {
                        start.X = graphBorderLBPoint.X;
                        start.Y = graphBorderLBPoint.Y - (int)actualDistance;
                    }


                    Point end = new Point();
                    if (drawX == true)
                    {
                        end.X = start.X;
                        end.Y = graphBorderLTPoint.Y;
                    }
                    else
                    {
                        end.X = graphBorderRBPoint.X;
                        end.Y = start.Y;
                    }

                    g.DrawLine(Pens.Red, start, end);

                    // 0 * X is always 0, in case of this first calculation, make this as 1.
                    if (majorLine == 0)
                        majorLine = 1;

                    // Keep points to draw scale string
                    if (drawX)
                        xScalePoints.Add(start);
                    else
                        yScalePoints.Add(start);

                    majorLine *= majorLineSpan;

                    // Keep values to draw scale string
                    if (drawX)
                        xScaleValues.Add(majorLine);
                    else
                        yScaleValues.Add(majorLine);
                }
            }

            // Keep final points to draw scale string
            if (drawX)
                xScalePoints.Add(graphBorderRBPoint);
            else
                yScalePoints.Add(graphBorderLTPoint);


            // Do we need minor lines ?
            if (minorLineSpan == 0)
            {
                return;
            }


            // Draw minor lines
            if (scaleType.CompareTo("linear") == 0)
            {
                for (float minorLine = min; minorLine < max; )
                {
                    
                    // If this line is drawn as major line, do not draw minor line.
                    if (minorLine % majorLineSpan == 0)
                    {
                        minorLine += minorLineSpan;
                        continue;
                    }

                    // distance in percent/100 from 0 point
                    float distance = GetDistance(min, max, minorLine, SCALETYPE.SCALE_LINEAR, drawX);
                    
                    // calculate actual distance in applicatin
                    float actualDistance = 0.0f;
                    if (drawX)
                    {
                        actualDistance = graphBorderRect.Width * distance;
                    }
                    else
                    {
                        actualDistance = graphBorderRect.Height * distance;
                    }

                    // Specify point draw from and to
                    Point start = new Point();
                    if (drawX == true)
                    {
                        start.X = graphBorderLBPoint.X + (int)actualDistance;
                        start.Y = graphBorderLBPoint.Y;
                    }
                    else // Drawing Y axis horizontal line
                    {
                        start.X = graphBorderLBPoint.X;
                        start.Y = graphBorderLBPoint.Y - (int)actualDistance;
                    }

                    Point end = new Point();
                    if (drawX == true)
                    {
                        end.X = start.X;
                        end.Y = graphBorderLTPoint.Y;
                    }
                    else
                    {
                        end.X = graphBorderRBPoint.X;
                        end.Y = start.Y;
                    }

                    Pen minorPen = new Pen(Brushes.Blue);
                    minorPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    g.DrawLine(minorPen, start, end);

                    minorLine += minorLineSpan;
                }
            }
            else if (scaleType.CompareTo("log") == 0)
            {
                //
                // Minor lines are not implemented at this moment
                // 

                //float nextMajorLine = min * logBase;

                //for (float minorLine = min; minorLine < max; )
                //{

                //    // If this line is drawn as major line, do not draw minor line.
                //    if (minorLine % nextMajorLine == 0)
                //    {
                //        minorLine += minorLineSpan;
                //        continue;
                //    }

                //    // distance in percent/100 from 0 point
                //    float distance = GetDistance(min, max, minorLine, SCALETYPE.SCALE_LOG, drawX);

                //    // calculate actual distance in applicatin
                //    float actualDistance = 0.0f;
                //    if (drawX)
                //    {
                //        actualDistance = graphBorderRect.Width * distance;
                //    }
                //    else
                //    {
                //        actualDistance = graphBorderRect.Height * distance;
                //    }

                //    // Specify point draw from and to
                //    Point start = new Point();
                //    if (drawX == true)
                //    {
                //        start.X = graphBorderLBPoint.X + (int)actualDistance;
                //        start.Y = graphBorderLBPoint.Y;
                //    }
                //    else // Drawing Y axis horizontal line
                //    {
                //        start.X = graphBorderLBPoint.X;
                //        start.Y = graphBorderLBPoint.Y - (int)actualDistance;
                //    }

                //    Point end = new Point();
                //    if (drawX == true)
                //    {
                //        end.X = start.X;
                //        end.Y = graphBorderLTPoint.Y;
                //    }
                //    else
                //    {
                //        end.X = graphBorderRBPoint.X;
                //        end.Y = start.Y;
                //    }

                //    Pen minorPen = new Pen(Brushes.Blue);
                //    minorPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                //    g.DrawLine(minorPen, start, end);

                //    // 0 * X is always 0, in case of this first calculation, make this as 1.
                //    if (minorLine == 0)
                //        minorLine = 1;

                //    minorLine += minorLineSpan;
                //}
            }
        }


        /// <summary>
        /// This function returns distance in percentage.
        /// Type can be 0 => Linear, 1 => Log
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="value"></param>
        /// <param name="type">Set 1 if you want to calc as Log. Otherwise calculation will be as Linear.</param>
        /// <param name="calcXAxis">Set false if you want to calc about Y. Otherwise calculation will be about X axis.</param>
        /// <returns></returns>
        private float GetDistance(int min, int max, float value, SCALETYPE type = SCALETYPE.SCALE_LINEAR, bool calcXAxis = true)
        {
            float distance = 0.0f;

            switch (type)
            {
                case SCALETYPE.SCALE_LINEAR: // Linear
                    int totalDistance = max - min;
                    int relativeValue = (int)(value - min);
                    distance = (float)relativeValue / (float)totalDistance;
                    break;

                case SCALETYPE.SCALE_LOG: // Log
                    IEnumerable<System.Xml.Linq.XElement> pairs =
                        xmlSource.Elements("Pair");

                    string graph = graphSelectionComboBox.SelectedItem.ToString();

                    System.Xml.Linq.XElement selectedPair =
                        (from pair in pairs
                         where pair.Attribute("GraphTitle").Value.ToString() == graph
                         select pair).First();

                    double xlogBase = (double)selectedPair.Attribute("LogBaseOfXAxis");
                    double ylogBase = (double)selectedPair.Attribute("LogBaseOfYAxis");

                    double totalDistanceInLog = 0.0d;
                    double relativeValueInLog = 0.0d;
                    if (calcXAxis)
                    {
                        double maxDistanceInLog = System.Math.Log(max, xlogBase);
                        double minDistanceInLog = System.Math.Log(min, xlogBase);
                        totalDistanceInLog = maxDistanceInLog - minDistanceInLog;
                        relativeValueInLog = System.Math.Log(value, xlogBase) - minDistanceInLog;
                    }
                    else
                    {
                        double maxDistanceInLog = System.Math.Log(max, ylogBase);
                        double minDistanceInLog = System.Math.Log(min, ylogBase);
                        totalDistanceInLog = maxDistanceInLog - minDistanceInLog;
                        relativeValueInLog = System.Math.Log(value, ylogBase) - minDistanceInLog;
                    }

                    // In case of 0, consider as 100%.
                    // This condition should not be happend...
                    if (totalDistanceInLog == 0)
                        return 1.0f;

                    distance = (float)relativeValueInLog / (float)totalDistanceInLog;
                    break;
                default:
                    throw new Exception("This type is not supported.");
            }

            return distance;
        }

        private void DrawScaleString(float majorLine, Point majorLineStart, bool drawX = true)
        {
            Rectangle clientArea = graphArea.ClientRectangle;

            Bitmap canvas = new Bitmap(graphArea.Width, graphArea.Height);
            Graphics g = Graphics.FromImage(canvas);

            // Calc text extent
            SizeF stringSize = g.MeasureString(majorLine.ToString(), DefaultFont);

            // Calc and align Y axis texts to left
            SizeF maxStringSize = new SizeF();
            if (!drawX)
            {
                // Find maximun string in Y scale
                foreach (float v in yScaleValues)
                {
                    SizeF current = g.MeasureString(v.ToString(), DefaultFont);
                    if (current.Width > maxStringSize.Width)
                        maxStringSize = current;
                }
            }


            Font scaleFont = FindProperScaleFont("temp");

            // Draw !
            // X position is moved to left to centerize text
            if (drawX)
                g.DrawString(majorLine.ToString(), scaleFont, Brushes.Black, 
                    majorLineStart.X - (int)stringSize.Width/2, majorLineStart.Y);
            else
                g.DrawString(majorLine.ToString(), scaleFont, Brushes.Black,
                    majorLineStart.X - (int)maxStringSize.Width, majorLineStart.Y - (int)stringSize.Height/2);


            g.Dispose();

            // Add to layers
            GraphicLayer layer = new GraphicLayer("scale", canvas, graphicLayers.Count());
            graphicLayers.Add(layer);
            canvas.Save(layer.zOrder + layer.name + ".png",
                System.Drawing.Imaging.ImageFormat.Png);
          
        }

        private void DrawTitle()
        {
            Rectangle clientArea = graphArea.ClientRectangle;

            Bitmap canvas = new Bitmap(graphArea.Width, graphArea.Height);
            Graphics g = Graphics.FromImage(canvas);


            // Which graph is selected ?
            string titleText = graphSelectionComboBox.Text;
            
            // Find out proper font
            Font properFont = FindProperTitleFont(titleText);

            /// Calculate title area
            /// This x and y are center of title area
            // Calculate X from left
            Point titleCenter = new Point();
            titleCenter.X = clientArea.Width / 2;
            // Calculate Y from top
            titleCenter.Y = (int)(yUnit * topMergin / 2);

            // Calculate proper point to draw title
            SizeF actualSize = g.MeasureString(titleText, properFont);
            Point titleTopLeft = new Point();
            titleTopLeft.X = titleCenter.X - (int)(actualSize.Width / 2);
            titleTopLeft.Y = titleCenter.Y - (int)(actualSize.Height / 2);

            // Draw !
            g.DrawString(titleText, properFont, Brushes.Black, titleTopLeft);
            g.Dispose();

            // Add to layers
            GraphicLayer layer = new GraphicLayer("title", canvas, graphicLayers.Count());
            graphicLayers.Add(layer);
            canvas.Save(layer.zOrder + layer.name + ".png",
                System.Drawing.Imaging.ImageFormat.Png);
        }

        /// <summary>
        /// Retern font most likely fit to this title height.
        /// </summary>
        /// <param name="titleText"></param>
        /// <returns></returns>
        private Font FindProperTitleFont(string titleText)
        {
            Bitmap canvas = new Bitmap(graphArea.Width, graphArea.Height);
            graphArea.DrawToBitmap(canvas, graphArea.ClientRectangle);
            Graphics g = Graphics.FromImage(canvas);

            // This is height of title area.
            // Find font size which fit to this height
            Font newFont = new Font(DefaultFont.FontFamily, 1);
            int textHeight = (int)(yUnit * topMergin);
            for (int i = 1; i < 100; i++)
            {
                Font tmpFont = new Font(DefaultFont.FontFamily, i);
                SizeF textSize = g.MeasureString(titleText, tmpFont);
                
                if (textSize.Height > textHeight)
                    break;

                newFont = tmpFont;
            }

            g.Dispose();
            return newFont;
        }

        /// <summary>
        /// Retern font most likely best for scale.
        /// Temporaly implementation. Please implement as you like.
        /// </summary>
        /// <param name="scaleText"></param>
        /// <returns></returns>
        private Font FindProperScaleFont(string scaleText)
        {
            Bitmap canvas = new Bitmap(graphArea.Width, graphArea.Height);
            graphArea.DrawToBitmap(canvas, graphArea.ClientRectangle);
            Graphics g = Graphics.FromImage(canvas);

            // This is height of title area.
            // Find font size which fit to this height
            Font newFont = DefaultFont;

            g.Dispose();
            return newFont;
        }
        private void DrawLine()
        {
            Rectangle clientArea = graphArea.ClientRectangle;

            Bitmap canvas = new Bitmap(graphArea.Width, graphArea.Height);
            Graphics g = Graphics.FromImage(canvas);

            // Retrieve selected pair
            IEnumerable<System.Xml.Linq.XElement> pairs =
                xmlSource.Elements("Pair");

            string graph = graphSelectionComboBox.SelectedItem.ToString();

            System.Xml.Linq.XElement selectedPair =
                (from pair in pairs
                 where pair.Attribute("GraphTitle").Value.ToString() == graph
                 select pair).First();
            
            // This is for X data
            System.Xml.Linq.XElement xData =
                (from dataLine in selectedPair.Elements()
                 where dataLine.Attribute("XData").Value.ToString() == "Yes"
                 select dataLine).First();

            // This is for Y data
            System.Xml.Linq.XElement yData =
                (from dataLine in selectedPair.Elements("DataLine")
                 where dataLine.Attribute("XData").Value.ToString() == "No"
                 select dataLine).First();

            // Read and calculate point to draw
            ReadData(ref xData, ref yData);

            // Number of data to plot should be same
            // Otherwise we can't plot data correctly.
            if (xDataSet.Count != yDataSet.Count)
            {
                MessageBox.Show("Number of x data and y data is different.");
                return;
            }

            // Check each data has same number of data.
            for (int i = 0; i < xDataSet.Count(); i++)
            {
                if (xDataSet[i].Count != yDataSet[i].Count)
                {
                    string message = "Number of x data and y data is different at " + i;
                    MessageBox.Show(message);
                    return;
                }
            }



            int xMax = (int)selectedPair.Attribute("MaximumOfX");
            int xMin = (int)selectedPair.Attribute("MinimunOfX");
            float xmajorLineSpan = (float)selectedPair.Attribute("MajorOfXAxis");
            float xMinorLineSpan = (float)selectedPair.Attribute("MinorOfXAxis");
            string xScaleType = (string)selectedPair.Attribute("ScaleOfXAxis");
            SCALETYPE xScale = xScaleType == "log" ? SCALETYPE.SCALE_LOG : SCALETYPE.SCALE_LINEAR;

            int yMax = (int)selectedPair.Attribute("MaximumOfY");
            int yMin = (int)selectedPair.Attribute("MinimumOfY");
            float yMajorLineSpan = (float)selectedPair.Attribute("MajorOfYAxis");
            float yMinorLineSpan = (float)selectedPair.Attribute("MinorOfYAxis");
            string yScaleType = (string)selectedPair.Attribute("ScaleOfYAxis");
            SCALETYPE yScale = yScaleType == "log" ? SCALETYPE.SCALE_LOG : SCALETYPE.SCALE_LINEAR;


            // Draw all data set !
            for (int index = 0; index < xDataSet.Count(); index++)
            {

                List<float> xDataOfOneLine = xDataSet[index];
                List<float> yDataOfOneLine = yDataSet[index];


                List<PointF> drawPoints = new List<PointF>();
                for (int i = 0; i < xDataOfOneLine.Count; i++)
                {
                    PointF tmp = new PointF();
                    float distance = GetDistance(xMin, xMax, xDataOfOneLine[i], xScale, true);
                    float actualDistance = distance * graphBorderRect.Width;

                    tmp.X = graphBorderLBPoint.X + actualDistance;

                    distance = GetDistance(yMin, yMax, yDataOfOneLine[i], yScale, false);
                    actualDistance = distance * graphBorderRect.Height;
                    tmp.Y = graphBorderLBPoint.Y - actualDistance;

                    drawPoints.Add(tmp);
                }

                // Draw this line !
                // Starting from 1, means draw line 0 to 1, 1 to 2,...
                for (int i = 1; i < drawPoints.Count(); i++)
                {
                    // This is drawing circle
                    //g.FillEllipse(Brushes.Black, drawPoints[i].X, drawPoints[i].Y, 2.5f, 2.5f);

                    // Drawing line 
                    g.DrawLine(Pens.Black, drawPoints[i - 1], drawPoints[i]);
                }

            }
            g.Dispose();

            // Add to layers
            GraphicLayer layer = new GraphicLayer("data", canvas, graphicLayers.Count());
            graphicLayers.Add(layer);
            canvas.Save(layer.zOrder + layer.name + ".png",
                System.Drawing.Imaging.ImageFormat.Png);
        }

        private void ReadData(ref System.Xml.Linq.XElement xData, ref System.Xml.Linq.XElement yData)
        {
            // This is a way to read csv data using TextFieldParser
            Microsoft.VisualBasic.FileIO.TextFieldParser parser = 
                new Microsoft.VisualBasic.FileIO.TextFieldParser(@".\Sample_Data.txt");

            parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
            parser.SetDelimiters("\t");

            string[] current;
            while (!parser.EndOfData)
            {
                try
                {
                    current = parser.ReadFields();
                }
                catch (Microsoft.VisualBasic.FileIO.MalformedLineException e)
                {

                }
            }


            // This is a way to read csv using Linq (bit modern)
            string[] lines = System.IO.File.ReadAllLines(@".\Sample_Data.txt");


            var columnQuery =
                from line in lines
                let elements = line.Split('\t')
                select elements;

            var results = columnQuery.ToList();

            // X related data
            int xDataStart = (int)xData.Attribute("StartDataColumn");
            int xDataEnd = (int)xData.Attribute("EndDataColumn");
            int xFirstDataRow = (int)xData.Attribute("FirstDataRow");

            for (int row = xFirstDataRow; row < results.Count(); )
            {
                List<float> xDataSetOfOneLine = new List<float>();

                // hmmm, could be done with skip or skipwhile...?
                // Data row should be -1. Because line number is specified from 1 
                //  but starting from 0 in program.
                string[] tmp = results[row - 1];
                for (int col = xDataStart - 1, index = 0; col < xDataEnd - 1; col++, index++)
                {
                    xDataSetOfOneLine.Add(float.Parse(tmp[col]));
                }

                xDataSet.Add(xDataSetOfOneLine);

                row += 12;
            }

            // y related data
            int yDataStart = (int)yData.Attribute("StartDataColumn");
            int yDataEnd = (int)yData.Attribute("EndDataColumn");
            int yFirstDataRow = (int)yData.Attribute("FirstDataRow");

            for (int row = yFirstDataRow; row < results.Count(); )
            {
                List<float> yDataSetOfOneLine = new List<float>();

                string[] tmp = results[row - 1];
                for (int col = yDataStart - 1, index = 0; col < yDataEnd - 1; col++, index++)
                {
                    yDataSetOfOneLine.Add(float.Parse(tmp[col]));
                }

                row += 12;

                yDataSet.Add(yDataSetOfOneLine);
            }
        }

        private void DrawAllLayers()
        {
            // Draw back ground image
            Bitmap canvas = new Bitmap(graphArea.Width, graphArea.Height);
            Graphics g = Graphics.FromImage(canvas);

            foreach (GraphicLayer layer in graphicLayers)
            {
                g.DrawImage(layer.canvas, new Point(0, 0));
            }

            g.Dispose();
            graphArea.Image = canvas;
        }


        // Event handler
        private void GpaphMaker_DragDrop(object sender, DragEventArgs e)
        {
            string message = "File is dropped. This function is not supported yed.";
            MessageBox.Show(message);
            // Implement your code here.

        }

        private void GpaphMaker_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void graphSelection_Click(object sender, EventArgs e)
        {
            // Implement graph selection change
        }

        private void GpaphMaker_ResizeEnd(object sender, EventArgs e)
        {


            // Clear previous data
            graphicLayers.Clear();
            graphArea.Image = null;
            xDataSet.Clear();
            yDataSet.Clear();
            xScalePoints.Clear();
            xScaleValues.Clear();
            yScalePoints.Clear();
            yScaleValues.Clear();

            // Draw again!
            DrawGraph();
        }

        private void graphSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // This should be handled after first drawing
            if (!hasDrawnOnce)
                return;
            
            // Clear previous data
            graphicLayers.Clear();
            graphArea.Image = null;
            xDataSet.Clear();
            yDataSet.Clear();
            xScalePoints.Clear();
            xScaleValues.Clear();
            yScalePoints.Clear();
            yScaleValues.Clear();

            // Draw again!
            DrawGraph(); 
        }
    }
}
