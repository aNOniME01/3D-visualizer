using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3D_visualizer
{
    internal class ProjectionPlane
    {
        public int Size { get; private set; }

        public Vector3 Location { get; private set; }

        public ProjectionPlane(int x, int y, int planeSize)
        {
            Size = planeSize;
            Location = new Vector3(x, y, planeSize);
        }
    }
}
