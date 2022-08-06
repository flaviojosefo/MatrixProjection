using System;

namespace MatrixProjection {

    public class Transform {

        public Vector Position { get; set; }
        public Vector Rotation { get; set; }
        public Vector Scale { get; set; } = new Vector(1, 1, 1);

        public Transform() { }

        public void Move(Vector translation) => Position += translation;

        public void Rotate(Vector rotation) => Rotation += rotation;
    }
}
