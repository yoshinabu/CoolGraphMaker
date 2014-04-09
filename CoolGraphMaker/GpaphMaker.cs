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
        readonly int leftMergin = 15;
        readonly int topMergin = 10;
        readonly int rightMergin = 5;
        readonly int bottomMergin = 20;

        // configuration xml source
        System.Xml.Linq.XElement xmlSource;

        public GpaphMaker()
        {
            InitializeComponent();

            xmlSource = System.Xml.Linq.XElement.Load(@".\GraphDataSetting.xml");
            
            InitializeMenu();

            // Start drawing
            //DrawGraph();

        }

        // Implementation
        private void InitializeMenu()
        {
            // Add graph selection combo box
            ToolStripComboBox combo = new ToolStripComboBox("graphSelection");

            var pairs = (
                from p in xmlSource.Elements()
                select p);
            foreach (System.Xml.Linq.XElement pair in pairs)
            {
                combo.Items.Add(pair.Attribute("GraphTitle").Value.ToString());
            }
            combo.SelectedIndex = 0;
            menuStrip.Items.Add(combo);
        }

        private void DrawGraph()
        {
            //DrawTitle();
            DrawGraphArea();
            //DrawLegend();
            //DrawInfo();
        }

        private void DrawInfo()
        {
            throw new NotImplementedException();
        }

        private void DrawLegend()
        {
            throw new NotImplementedException();
        }

        private void DrawGraphArea()
        {
            DrawOutbound();

            // Still under construction.
            // Here just sample drawing.
            //var canvas = new Bitmap(graphArea.Width, graphArea.Height);

            //Graphics g = Graphics.FromImage(canvas);

            //g.FillRectangle(Brushes.Black, 10, 20, 100, 80);
            //g.Dispose();

            //graphArea.Image = canvas;


            //throw new NotImplementedException();
        }

        private void DrawOutbound()
        {
            Rectangle clientArea = graphArea.ClientRectangle;

            int left, top, right, bottom = 0;

            int xUnit = clientArea.Width / 100;
            int yUnit = clientArea.Height / 100;

            left = xUnit * leftMergin;
            top = yUnit * topMergin;
            right = clientArea.Right - (xUnit * rightMergin);
            bottom = clientArea.Bottom - (yUnit * bottomMergin);

            Rectangle outboundRect = new Rectangle(left, top, right - left, bottom - top);
            var canvas = new Bitmap(graphArea.Width, graphArea.Height);

            Graphics g = Graphics.FromImage(canvas);
            g.DrawRectangle(Pens.Red, outboundRect);

            g.Dispose();
            graphArea.Image = canvas;


            
        }


        private void DrawTitle()
        {
            throw new NotImplementedException();
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
