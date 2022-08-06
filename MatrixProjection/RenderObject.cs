using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProjection {

    public class RenderObject {

        public Transform Transform { get; } = new Transform();
        public Mesh Mesh { get; private set; }

        public Mat4x4 ModelMatrix => GetModelMatrix();

        public RenderObject() { }

        private Mat4x4 GetModelMatrix() {

            Mat4x4 rotationX = new float[4, 4] {
                {1, 0, 0, 0},
                {0, (float)Math.Cos(Transform.Rotation.X), (float)-Math.Sin(Transform.Rotation.X), 0},
                {0, (float)Math.Sin(Transform.Rotation.X), (float)Math.Cos(Transform.Rotation.X), 0},
                {0, 0, 0, 1}
            };

            Mat4x4 rotationY = new float[4, 4] {
                {(float)Math.Cos(Transform.Rotation.Y), 0, (float)Math.Sin(Transform.Rotation.Y), 0},
                {0, 1, 0, 0},
                {(float)-Math.Sin(Transform.Rotation.Y), 0, (float)Math.Cos(Transform.Rotation.Y), 0},
                {0, 0, 0, 1}
            };

            Mat4x4 rotationZ = new float[4, 4] {
                {(float)Math.Cos(Transform.Rotation.Z), (float)-Math.Sin(Transform.Rotation.Z), 0, 0},
                {(float)Math.Sin(Transform.Rotation.Z), (float)Math.Cos(Transform.Rotation.Z), 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            };

            // XYZ rotation = (((Z × Y) × X) × Vector) or (Z×Y×X)×V
            Mat4x4 rotation = Mat4x4.MatMul(rotationZ, rotationY);
            rotation = Mat4x4.MatMul(rotation, rotationX);

            Mat4x4 translation = new float[4, 4] {
                {1,0,0,Transform.Position.X},
                {0,1,0,Transform.Position.Y},
                {0,0,1,Transform.Position.Z},
                {0,0,0,1}
            };

            Mat4x4 scaling = new float[4, 4] {
                {Transform.Scale.X,0,0,0},
                {0,Transform.Scale.Y,0,0},
                {0,0,Transform.Scale.Z,0},
                {0,0,0,1}
            };

            // Model Matrix = S × R × T (read right to left)
            return Mat4x4.MatMul(Mat4x4.MatMul(translation, rotation), scaling);
        }

        public static RenderObject Create<T>() where T : Mesh, new() => 
            new RenderObject { Mesh = new T() };
    }
}
