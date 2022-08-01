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

        public bool Perspective { get; private set; }

        public float FocalLength { get; private set; }


        public Camera(int x, int y, int z)
        {
            Location = new Vector3(x, y, z);

            Perspective = false;

            FocalLength = 300;
        }

        #region Get

        public bool GetPerspective() => Perspective;

        #endregion

        #region Set

        public void SetPerspective(bool perspective) => Perspective = perspective;

        #endregion
    }
}
