using System;

namespace MatrixProjection {

    public struct Light {

        public Vector Position { get; set; }

        public Vector Direction { get; set; }

        public float Intensity { get; set; }

        public Light(Vector position, float intensity = 1.0f) {

            Position = position;
            Intensity = intensity;
            Direction = new Vector(0, -1, 0);
        }
    }
}
