using System;

namespace MatrixProjection {

    public class Camera {

        // Width / Height (Pixel ratio)
        private const float ASPECT_RATIO = (8 * 240) / (float)(16 * 63);

        public Vector3 Position { get; set; }

        public Vector3 Up { get; private set; }
        public Vector3 Forward { get; private set; }
        public Vector3 Right { get; private set; }

        public float Pitch { get; set; } // Degrees
        public float Yaw { get; set; }   // Degrees

        public Projection Projection { get; set; } = Projection.Perspective;

        public float Fov { get; set; } = 90.0f;
        public float NearPlane { get; set; } = 0.1f;
        public float FarPlane { get; set; } = 100.0f;

        public Mat4x4 ViewMatrix => GetViewMatrix(); // World-To-Camera Matrix
        public Mat4x4 ProjMatrix => IsOrthographic() ? orthoProjection : perspProjection;

        private readonly Mat4x4 orthoProjection;
        private readonly Mat4x4 perspProjection; // Dynamic Fov needs a dynamic perspective projection!

        public Camera(Vector3 startPos = new Vector3()) {

            Position = startPos;

            // Ortographic Projection
            orthoProjection = new float[4, 4] {
                {1 / ASPECT_RATIO, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 1}
            };

            // Perspective Projection
            float fovTan = 1.0f / (float)Math.Tan(Fov * 0.5f * (Math.PI / 180.0f));

            perspProjection = new float[4, 4] {
                {fovTan / ASPECT_RATIO,0,0,0},
                {0,fovTan,0,0},
                {0,0,-FarPlane / (FarPlane - NearPlane),1},
                {0,0,-(FarPlane * NearPlane) / (FarPlane - NearPlane),0}
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

        // Mimics an FPS camera
        // https://www.3dgep.com/understanding-the-view-matrix/#FPS_Camera
        private Mat4x4 GetViewMatrix() {

            // LEFT-Handed Coordinate System

            // Rotation in X = Positive when 'looking down'
            // Rotation in Y = Positive when 'looking right'
            // Rotation in Z = Positive when 'tilting left'

            float toRad = (float)(Math.PI / 180.0f);

            float cosPitch = (float)Math.Cos(Pitch * toRad);
            float sinPitch = (float)Math.Sin(Pitch * toRad);

            float cosYaw = (float)Math.Cos(Yaw * toRad);
            float sinYaw = (float)Math.Sin(Yaw * toRad);

            Right = new Vector3(cosYaw, 0, -sinYaw);

            Up = new Vector3(sinYaw * sinPitch, cosPitch, cosYaw * sinPitch);

            Forward = new Vector3(sinYaw * cosPitch, -sinPitch, cosPitch * cosYaw);

            // The inverse camera's translation
            Vector3 transl = new Vector3(-Vector3.DotProduct(Right, Position),
                                         -Vector3.DotProduct(Up, Position),
                                         -Vector3.DotProduct(Forward, Position));

            Mat4x4 viewMatrix = new float[4, 4] {
                {Right.X,Up.X,Forward.X,0},
                {Right.Y,Up.Y,Forward.Y,0},
                {Right.Z,Up.Z,Forward.Z,0},
                {transl.X,transl.Y,transl.Z,1}
            };

            return viewMatrix;
        }

        // https://learnopengl.com/Getting-started/Camera
        // https://docs.microsoft.com/pt-pt/windows/win32/direct3d9/d3dxmatrixlookatlh
        // Returns a LEFT-Handed View Matrix | Default up is Vector.Up
        public Mat4x4 LookAt(Vector3 target, Vector3 up) {

            Forward = (target - Position).Normalized;               // Z axis

            Right = Vector3.CrossProduct(up, Forward).Normalized;   // X axis

            Up = Vector3.CrossProduct(Forward, Right);              // Y axis

            float toDeg = (float)(180.0f / Math.PI);

            Pitch = -(float)Math.Asin(Forward.Y) * toDeg;
            Yaw = (float)Math.Atan2(Forward.X, Forward.Z) * toDeg;

            // The inverse camera's translation
            Vector3 transl = new Vector3(-Vector3.DotProduct(Right, Position),
                                         -Vector3.DotProduct(Up, Position),
                                         -Vector3.DotProduct(Forward, Position));

            Mat4x4 viewMatrix = new float[4, 4] {
                {Right.X,Up.X,Forward.X,0},
                {Right.Y,Up.Y,Forward.Y,0},
                {Right.Z,Up.Z,Forward.Z,0},
                {transl.X,transl.Y,transl.Z,1}
            };

            return viewMatrix;
        }

        public bool IsOrthographic() => Projection == Projection.Ortographic;
    }
}
