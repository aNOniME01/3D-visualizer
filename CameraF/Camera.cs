using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3D_visualizer
{
    internal class Camera
    {
        public Vector3 Location { get; private set; }

        public float FocalLength { get; private set; }
        public float Skew { get; private set; }

        public Vector2 ImageRes { get; private set; }
        public Vector2  SenzorSize { get; private set; }


        public Camera(int x, int y, int z)
        {
            Location = new Vector3(x, y, z);

            FocalLength = 10;
            Skew = 0;
            
            ImageRes = new Vector2(400, 400);
            SenzorSize = new Vector2(200,200);
        }

    }
}
