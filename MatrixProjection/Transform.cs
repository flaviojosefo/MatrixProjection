using System;

namespace MatrixProjection {

    public class Transform {

        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; } = Vector3.One;

        public Transform() { }

        public void Move(Vector3 translation) => Position += translation;

        public void Rotate(Vector3 rotation) => Rotation += rotation;
    }
}
