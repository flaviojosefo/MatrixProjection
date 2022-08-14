using System;

namespace MatrixProjection {

    public class Camera {

        // Width / Height (Pixel ratio)
        private const float ASPECT_RATIO = (8 * 240) / (float)(16 * 63);

        public Vector3 Position { get; set; }

        public Vector3 Up { get; private set; }
        public Vector3 Forward { get; private set; }
        public Vector3 Right { get; private set; }

        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Roll { get; set; }

        public Projection Projection { get; set; } = Projection.Perspective;

        public float Fov { get; set; } = 90.0f;
        public float NearPlane { get; set; } = 0.1f;
        public float FarPlane { get; set; } = 100.0f;

        public Mat4x4 ViewMatrix => GetViewMatrix();
        public Mat4x4 ProjMatrix => IsOrthographic() ? orthoProjection : perspProjection;

        private readonly Mat4x4 orthoProjection;
        private readonly Mat4x4 perspProjection; // Dynamic Fov needs a dynamic perspective projection!

        public Camera(Vector3 startPos = new Vector3()) {

            Position = startPos;

            // Ortographic Projection
            orthoProjection = new float[4, 4] {
                {1 * ASPECT_RATIO, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 1}
            };

            // Perspective Projection
            float fovRad = 1.0f / (float)Math.Tan(Fov * 0.5f * (Math.PI / 180.0f));

            perspProjection = new float[4, 4] {
                {fovRad * ASPECT_RATIO,0,0,0},
                {0,fovRad,0,0},
                {0,0,-FarPlane / (FarPlane - NearPlane),-(FarPlane * NearPlane) / (FarPlane - NearPlane)},
                {0,0,1,0}
            };

            // https://www.scratchapixel.com/lessons/3d-basic-rendering/perspective-and-orthographic-projection-matrix/opengl-perspective-projection-matrix

            //float scale = (float)Math.Tan(Fov * 0.5f * (Math.PI / 180.0f));
            //float r = ASPECT_RATIO * scale, l = -r;
            //float t = scale, b = -t;
            //perspProjection = new float[4, 4] {
            //    {(2 * NearPlane) / (r - l),0,0,0},
            //    {0,(2 * NearPlane) / (t - b),0,0},
            //    {(r + l) / (r - l),(t + b) / (t - b),-((FarPlane + NearPlane) / (FarPlane - NearPlane)),-1},
            //    {0,0,-((2 * FarPlane * NearPlane) / (FarPlane - NearPlane)),0}
            //};
        }

        private Mat4x4 GetViewMatrix() {

            float cosPitch = (float)Math.Cos(Pitch * (Math.PI / 180.0f));
            float sinPitch = (float)Math.Sin(Pitch * (Math.PI / 180.0f));

            float cosYaw = (float)Math.Cos(Yaw * (Math.PI / 180.0f));
            float sinYaw = (float)Math.Sin(Yaw * (Math.PI / 180.0f));

            float cosRoll = (float)Math.Cos(Roll * (Math.PI / 180.0f));
            float sinRoll = (float)Math.Sin(Roll * (Math.PI / 180.0f));

            Mat4x4 rotX = new float[4, 4] {
                {1, 0, 0, 0},
                {0, cosPitch, -sinPitch, 0},
                {0, sinPitch, cosPitch, 0},
                {0, 0, 0, 1}
            };

            Mat4x4 rotY = new float[4, 4] {
                {cosYaw, 0, sinYaw, 0},
                {0, 1, 0, 0},
                {-sinYaw, 0, cosYaw, 0},
                {0, 0, 0, 1}
            };

            Mat4x4 rotZ = new float[4, 4] {
                {cosRoll, -sinRoll, 0, 0},
                {sinRoll, cosRoll, 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            };

            Mat4x4 camRot = Mat4x4.MatMul(rotZ, rotY);
            camRot = Mat4x4.MatMul(camRot, rotX);

            Vector3 up = new Vector3(0, 1, 0);
            Vector3 target = new Vector3(0, 0, 1); // Camera towards at +Z

            Vector3 lookDir = Mat4x4.MatMul(camRot, target);
            target = Position + lookDir;

            return LookAt(Position, target, up);
        }

        // https://learnopengl.com/Getting-started/Camera
        public Mat4x4 LookAt(Vector3 worldPos, Vector3 targetPos, Vector3 newUp) {

            Forward = (targetPos - worldPos).Normalized;

            Up = (newUp - (Forward * Vector3.DotProduct(newUp, Forward))).Normalized;

            Right = Vector3.CrossProduct(Up, Forward);

            Mat4x4 rotation = new float[4, 4] {
                {Right.X,Right.Y,Right.Z,0},
                {Up.X,Up.Y,Up.Z,0},
                {Forward.X,Forward.Y,Forward.Z,0},
                {0,0,0,1}
            };

            Mat4x4 translation = new float[4, 4] {
                {1,0,0,-Position.X},
                {0,1,0,-Position.Y},
                {0,0,1,-Position.Z},
                {0,0,0,1}
            };

            return Mat4x4.MatMul(rotation, translation);
        }

        public bool IsOrthographic() => Projection == Projection.Ortographic;
    }
}
