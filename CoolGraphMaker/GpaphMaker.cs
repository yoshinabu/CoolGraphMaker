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

        // data class which reads data from xml
        ConfigurationData config;

        public GpaphMaker()
        {
            InitializeComponent();

            RaedData();
            InitializeMenu();

            DrawGraph();

        }

        // Implementation
        private void InitializeMenu()
        {
            // Add graph selection combo box
            ToolStripComboBox combo = new ToolStripComboBox("graphSelection");

            foreach (DataPair d in config.data.Items)
            {
                combo.Items.Add(d.GraphTitle);
            }
            combo.SelectedIndex = 0;

            menuStrip.Items.Add(combo);
        }

        private void RaedData()
        {
            config = new ConfigurationData();
            config.ReadConfiguration();

            ////// Reading sample
            ////// 
            ////string prop = "";
            ////foreach (DataPair item in config.data.Items)
            ////{
            ////    // Reading index of this pari
            ////    prop += item.Index;
            ////    prop += ", ";

            ////    // Reading graph title of each pair
            ////    prop += item.GraphTitle;
            ////    prop += "\n";

            ////    // Do not set value of each item.
            ////}

            ////MessageBox.Show(prop, prop, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);


        }

        private void DrawGraph()
        {
            //DrawTitle();
            DrawGraphArea();
            //DrawLegent();
            //DrawInfo();
        }

        private void DrawInfo()
        {
            throw new NotImplementedException();
        }

        private void DrawLegent()
        {
            throw new NotImplementedException();
        }

        private void DrawGraphArea()
        {
            // Still under construction.
            // Here just sample drawing.
            var canvas = new Bitmap(graphArea.Width, graphArea.Height);

            Graphics g = Graphics.FromImage(canvas);

            g.FillRectangle(Brushes.Black, 10, 20, 100, 80);
            g.Dispose();

            graphArea.Image = canvas;


            //throw new NotImplementedException();
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
