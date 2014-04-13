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

        public GpaphMaker()
        {
            InitializeComponent();

            xmlSource = System.Xml.Linq.XElement.Load(@".\GraphDataSetting.xml");
            graphicLayers = new List<GraphicLayer>();
            InitializeMenu();

            // Start drawing
            DrawGraph();

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
            //DrawMiscInfo();

            DrawAllLayers();
        }

        private void DrawMiscInfo()
        {
            throw new NotImplementedException();
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
            int majorLineSpan = (int)selectedPair.Attribute("MajorOfXAxis");
            int minorLineSpan = (int)selectedPair.Attribute("MinorOfXAxis");
            string scaleType = (string)selectedPair.Attribute("ScaleOfXAxis");

            DrawMajorAndMinorLines(ref g, max, min, majorLineSpan, minorLineSpan, scaleType);

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
            int majorLineSpan = (int)selectedPair.Attribute("MajorOfYAxis");
            int minorLineSpan = (int)selectedPair.Attribute("MinorOfYAxis");
            string scaleType = (string)selectedPair.Attribute("ScaleOfYAxis");

            DrawMajorAndMinorLines(ref g, max, min, majorLineSpan, minorLineSpan, scaleType, false);
            g.Dispose();

            // Add to layers
            GraphicLayer layer = new GraphicLayer("y", canvas, graphicLayers.Count());
            graphicLayers.Add(layer);
            canvas.Save(layer.zOrder + layer.name + ".png",
                System.Drawing.Imaging.ImageFormat.Png);
        }


        private void DrawMajorAndMinorLines(ref Graphics g, int max, int min, int majorLineSpan, int minorLineSpan, string scaleType = "linear", bool drawX = true)
        {

            // do nothing and return if major line span is 0
            // This is stupid setting .
            if (majorLineSpan == 0)
            {
                return;
            }

            // Draw major lines
            if (scaleType.CompareTo("linear") == 0)
            {
                for (int majorLine = min; majorLine < max; )
                {
                    // distance in percent/100 from 0 point
                    float distance = GetDistance(min, max, majorLine, 0, drawX);

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

                    majorLine += majorLineSpan;
                }
            }
            else if (scaleType.CompareTo("log") == 0)
            {
                for (int majorLine = min; majorLine < max; )
                {
                    // distance in percent/100 from 0 point
                    float distance = GetDistance(min, max, majorLine, 1, drawX);

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

                    majorLine *= majorLineSpan;
                }
            }

            // Do we need minor lines ?
            if (minorLineSpan == 0)
            {
                return;
            }


            // Draw minor lines
            if (scaleType.CompareTo("linear") == 0)
            {
                for (int minorLine = min; minorLine < max; )
                {
                    // If this line is drawn as major line, do not draw minor line.
                    if (minorLine % majorLineSpan == 0)
                    {
                        minorLine += minorLineSpan;
                        continue;
                    }

                    // distance in percent/100 from 0 point
                    float distance = GetDistance(min, max, minorLine, 0, drawX);
                    
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
                for (int minorLine = min; minorLine < max; )
                {
                    // If this line is drawn as major line, do not draw minor line.
                    if (minorLine % majorLineSpan == 0)
                    {
                        minorLine *= minorLineSpan;
                        continue;
                    }

                    // distance in percent/100 from 0 point
                    float distance = GetDistance(min, max, minorLine, 1, drawX);

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

                    // 0 * X is always 0, in case of this first calculation, make this as 1.
                    if (minorLine == 0)
                        minorLine = 1;

                    minorLine *= minorLineSpan;
                }
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
        private float GetDistance(int min, int max, int value, int type = 0, bool calcXAxis = true)
        {
            float distance = 0.0f;

            int totalDistance = max - min;
            int relativeValue = value - min;


            switch (type)
            {
                case 0: // Linear
                    distance = (float)relativeValue / (float)totalDistance;
                    break;
                case 1: // Log
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
                    double valueInLog = 0.0d;
                    if (calcXAxis)
                    {
                        totalDistanceInLog = System.Math.Log(totalDistance, xlogBase);
                        valueInLog = System.Math.Log(value, xlogBase);
                    }
                    else
                    {
                        totalDistanceInLog = System.Math.Log(totalDistance, ylogBase);
                        valueInLog = System.Math.Log(value, ylogBase);
                    }

                    // In case of 0, consider as 100%.
                    // This condition should not be happend...
                    if (totalDistanceInLog == 0)
                        return 1.0f;

                    distance = (float)valueInLog / (float)totalDistanceInLog;
                    break;
                default:
                    throw new Exception("This type is not supported.");
            }

            return distance;
        }

        private void DrawTitle()
        {
            Rectangle clientArea = graphArea.ClientRectangle;

            Bitmap canvas = new Bitmap(graphArea.Width, graphArea.Height);
            Graphics g = Graphics.FromImage(canvas);


            // Which graph is selected ?
            string titleText = graphSelectionComboBox.Text;
            
            // Find out proper font
            Font properFont = FindProperFont(titleText);

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
        private Font FindProperFont(string titleText)
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
    }
}
