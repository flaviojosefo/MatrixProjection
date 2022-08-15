using System;

namespace MatrixProjection {

    public struct Light {

        public Vector3 Position { get; set; }

        private Vector3 direction;
        public Vector3 Direction { get => direction; set => direction = value.Normalized; }

        public float Intensity { get; set; }

        public Light(Vector3 position, float intensity = 1.0f) {

            Position = position;
            direction = new Vector3(0, -1, 0);
            Intensity = intensity;
        }
    }
}
