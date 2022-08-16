using System;

namespace MatrixProjection {

    public class RenderObject {

        public Transform Transform { get; } = new Transform();
        public Mesh Mesh { get; private set; }

        public Mat4x4 ModelMatrix => GetModelMatrix();

        public RenderObject() { }

        private Mat4x4 GetModelMatrix() {

            float toRad = (float)(Math.PI / 180.0f);

            // Precompute 'cos' and 'sin' of all rotation angles
            float cosRotX = (float)Math.Cos(Transform.Rotation.X * toRad);
            float sinRotX = (float)Math.Sin(Transform.Rotation.X * toRad);

            float cosRotY = (float)Math.Cos(Transform.Rotation.Y * toRad);
            float sinRotY = (float)Math.Sin(Transform.Rotation.Y * toRad);

            float cosRotZ = (float)Math.Cos(Transform.Rotation.Z * toRad);
            float sinRotZ = (float)Math.Sin(Transform.Rotation.Z * toRad);

            // Use LEFT-Handed rotation matrices (as seen in DirectX)
            // https://docs.microsoft.com/en-us/windows/win32/direct3d9/transforms#rotate

            Mat4x4 rotationX = new float[4, 4] {
                {1, 0, 0, 0},
                {0, cosRotX, sinRotX, 0},
                {0, -sinRotX, cosRotX, 0},
                {0, 0, 0, 1}
            };

            Mat4x4 rotationY = new float[4, 4] {
                {cosRotY, 0, -sinRotY, 0},
                {0, 1, 0, 0},
                {sinRotY, 0, cosRotY, 0},
                {0, 0, 0, 1}
            };

            Mat4x4 rotationZ = new float[4, 4] {
                {cosRotZ, sinRotZ, 0, 0},
                {-sinRotZ, cosRotZ, 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            };

            // XYZ rotation = (((Z × Y) × X) × Vector3) or (Z×Y×X)×V
            Mat4x4 rotation = Mat4x4.MatMul(rotationZ, rotationY);
            rotation = Mat4x4.MatMul(rotation, rotationX);

            Mat4x4 translation = new float[4, 4] {
                {1,0,0,0},
                {0,1,0,0},
                {0,0,1,0},
                {Transform.Position.X,Transform.Position.Y,Transform.Position.Z,1}
            };

            //translation = Mat4x4.Transpose(translation);

            Mat4x4 scaling = new float[4, 4] {
                {Transform.Scale.X,0,0,0},
                {0,Transform.Scale.Y,0,0},
                {0,0,Transform.Scale.Z,0},
                {0,0,0,1}
            };

            // Model Matrix = T × R × S (right to left order)
            return Mat4x4.MatMul(Mat4x4.MatMul(scaling, rotation), translation);
        }

        public static RenderObject Create<T>() where T : Mesh, new() => 
            new RenderObject { Mesh = new T() };
    }
}
