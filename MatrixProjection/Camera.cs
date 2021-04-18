using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class Camera {

        public Vector Position { get; set; }

        public Vector Direction { get; set; } = new Vector(0, 0, -1.0f);

        private Mat4x4 Translation;

        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        public Mat4x4 ViewMatrix => GetViewMatrix();

        public Camera(Vector startPos = new Vector()) {

            Position = startPos;
        }

        public void Translate(Vector v) {

            Position += v;
        }

        private Mat4x4 GetViewMatrix() {

            float cosPitch = (float)Math.Cos(Pitch * (Math.PI / 180.0f));
            float sinPitch = (float)Math.Sin(Pitch * (Math.PI / 180.0f));
            float cosYaw = (float)Math.Cos(Yaw * (Math.PI / 180.0f));
            float sinYaw = (float)Math.Sin(Yaw * (Math.PI / 180.0f));

            Vector xAxis = new Vector(cosYaw, 0, -sinYaw);
            Vector yAxis = new Vector(sinYaw * sinPitch, cosPitch, cosYaw * sinPitch);
            Vector zAxis = new Vector(sinYaw * cosPitch, -sinPitch, cosPitch * cosYaw);

            Translation = new float[4, 4] {
                {1,0,0,-Position.X},
                {0,1,0,-Position.Y},
                {0,0,1,-Position.Z},
                {0,0,0,1}
            };

            //return new float[4, 4] {
            //    {xAxis.X, yAxis.X, zAxis.X, 0},
            //    {xAxis.Y, yAxis.Y, zAxis.Y, 0},
            //    {xAxis.Z, yAxis.Z, zAxis.Z, 0},
            //    {-Vector.DotProduct(xAxis, Position), -Vector.DotProduct(yAxis, Position), -Vector.DotProduct(zAxis, Position), 1}
            //};

            return Mat4x4.MatMul(Mat4x4.Identity, Translation);
        }
    }
}
