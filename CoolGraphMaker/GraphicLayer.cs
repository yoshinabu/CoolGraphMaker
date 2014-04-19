using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CoolGraphMaker
{
    class GraphicLayer
    {
        public string name {get; set;}
        public Bitmap canvas { get; set; }
        public int zOrder { get; set; }

        public GraphicLayer(string name, Bitmap canvas, int zOrder)
        {
            this.name = name;
            this.canvas = canvas;
            this.zOrder = zOrder;
        }
    }
}
