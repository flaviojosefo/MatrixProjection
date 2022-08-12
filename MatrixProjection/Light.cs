using System;

namespace MatrixProjection {

    public struct Light {

        public Vector3 Position { get; set; }

        public Vector3 Direction { get; set; }

        public float Intensity { get; set; }

        public Light(Vector3 position, float intensity = 1.0f) {

            Position = position;
            Intensity = intensity;
            Direction = new Vector3(0, -1, 0);
        }
    }
}
