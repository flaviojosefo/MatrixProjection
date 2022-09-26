using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixProjection {

    public sealed class RayTracer : IRenderer {

        public RenderMode RenderMode { get; set; } = RenderMode.Solid;
        public List<Fragment> Fragments { get; } = new List<Fragment>();

        private readonly Camera camera;
        private readonly Light light;

        private readonly int width, height;

        public RayTracer(Camera camera, Light light) {

            this.camera = camera;
            this.light = light;

            width = Console.WindowWidth;
            height = Console.WindowHeight - 1;
        }

        // https://stackoverflow.com/questions/4190949/create-multiple-threads-and-wait-for-all-of-them-to-complete
        // https://stackoverflow.com/questions/4130194/what-is-the-difference-between-task-and-thread
        public void Render(RenderObject rObject) {

            Fragments.Clear();

            // Object to World Space

            Triangle[] updatedTri = new Triangle[rObject.Mesh.Polygons.Length];

            for (int i = 0; i < rObject.Mesh.Polygons.Length; i++) {

                updatedTri[i] = new Triangle(new Vector3[rObject.Mesh.Polygons[i].VertexCount]);

                for (int j = 0; j < rObject.Mesh.Polygons[i].VertexCount; j++) {

                    // Local to World coords
                    updatedTri[i][j] = Mat4x4.MatMul(rObject.Mesh.Polygons[i][j], rObject.ModelMatrix);
                }
            }

            // Ray Tracing Algorithm

            // Precompute the camera's matrix
            Mat4x4 cameraMatrix = camera.CameraMatrix;

            // Cache the rays' origin
            Vector3 origin = camera.Position;

            for (int y = 0; y < height; y++) {

                for (int x = 0; x < width; x++) {

                    Vector3 pRay = CreatePrimaryRay(origin, new Vector3(x, y), cameraMatrix);

                    for (int i = 0; i < updatedTri.Length; i++) {

                        if (Intersects(origin, pRay, updatedTri[i], out Vector3 hit)) {

                            Fragments.Add(new Fragment(new Vector3(x, y), ShadeChar.Full));
                        }
                    }
                }
            }

            // ##### UNCOMMENT THIS TO PARALLELIZE RAY TRACING #####

            //List<Fragment>[] frags = new List<Fragment>[4];

            //Parallel.Invoke(
            //    () => {
            //        RayTrace((0,0.5f), (0,0.5f), origin, cameraMatrix, updatedTri, out frags[0]);
            //    },

            //    () => {
            //        RayTrace((0, 0.5f), (0.5f, 1), origin, cameraMatrix, updatedTri, out frags[1]);
            //    },

            //    () => {
            //        RayTrace((0.5f, 1), (0, 0.5f), origin, cameraMatrix, updatedTri, out frags[2]);
            //    },

            //    () => {
            //        RayTrace((0.5f, 1), (0.5f, 1), origin, cameraMatrix, updatedTri, out frags[3]);
            //    }
            //);

            //for (int i = 0; i < frags.Length; i++) {

            //    foreach(Fragment f in frags[i]) {

            //        Fragments.Add(f);
            //    }
            //}

            // #####################################################
        }

        private void RayTrace((float, float) heightPartition, (float, float) widthPartition, Vector3 origin, Mat4x4 cameraMatrix, Triangle[] updatedTri, out List<Fragment> frags) {

            frags = new List<Fragment>();

            for (int y = (int)(height * heightPartition.Item1); y < (int)(height * heightPartition.Item2); y++) {

                for (int x = (int)(width * widthPartition.Item1); x < (int)(width * widthPartition.Item2); x++) {

                    Vector3 pRay = CreatePrimaryRay(origin, new Vector3(x, y), cameraMatrix);

                    for (int i = 0; i < updatedTri.Length; i++) {

                        if (Intersects(origin, pRay, updatedTri[i], out Vector3 hit)) {

                            frags.Add(new Fragment(new Vector3(x, y), ShadeChar.Full));
                        }
                    }
                }
            }
        }

        private Vector3 CreatePrimaryRay(Vector3 origin, Vector3 screenPos, Mat4x4 camMatrix) {

            float aspectRatio = (8 * width) / (float)(16 * height);

            float fovTan = (float)Math.Tan(camera.Fov * 0.5f * (Math.PI / 180.0f));

            // Image plane is set at 1 unit of distance (Z) by convention
            // 1 since Camera is looking at +Z
            Vector3 imgPlaneCoord = new Vector3(
                ((2 * ((screenPos.X + 0.5f) / width)) - 1) * fovTan * aspectRatio,
                (1 - (2 * ((screenPos.Y + 0.5f) / height))) * fovTan,
                1);

            // Multiply our plane coordinate by the camera matrix to
            // account for camera translation and rotation
            imgPlaneCoord = Mat4x4.MatMul(imgPlaneCoord, camMatrix);

            // Get the vector going towards that plane coordinate
            return (imgPlaneCoord - origin).Normalized;
        }

        // Ray-Triangle Intersection w/ the Geometric Solution
        private bool Intersects(Vector3 origin, Vector3 rayDir, Triangle tri, out Vector3 hitPoint) {

            /* P = O + (t * R) -> Ray Parametric Equation
             * 
             * P is the point on a plane (in which the triangle is present)
             * O is the origin of the ray
             * t is the distance from O to P
             * R is the ray direction
             * 
             * ##########
             * 
             * Ax + By + Cz + D = 0 -> Plane Equation
             * 
             * PlaneNormal can be seen as (A, B, C)
             * D is the distance from the origin to the plane
             * (x, y, z) are the coords of any point on the plane */

            // Step 1: Finding P

            // Assign default value to hitPoint
            hitPoint = Vector3.Zero;

            // Cache the triangle's normal vector
            Vector3 triNormal = tri.Normal;

            // Calculate DP of tri's Normal and Ray
            float normalRayDP = Vector3.DotProduct(triNormal, rayDir);

            // Dot Product smaller or equal to 0 indicates parallelism of ray-plane (no intersection)
            if (Math.Abs(normalRayDP) < float.Epsilon) 
                return false;

            // Compute D -> D = −(Ax + By + Cz)
            float d = -Vector3.DotProduct(triNormal, tri[0]);

            // Compute t -> t = −((N ⋅ O) + D) / (N ⋅ R)
            float t = -((Vector3.DotProduct(triNormal, origin) + d) / normalRayDP);

            // Check if triangle is behind the ray (distance is negative = no intersection)
            if (t < 0.0f) 
                return false;

            // Compute intersection point (P)
            Vector3 p = origin + t * rayDir;

            // ##########

            // Step 2: Inside-Outside Test
            // P must be on the left of all triangle edges

            // Calculate each edge and side
            for (int i = 0; i < tri.VertexCount; i++) {

                // The current edges' vectors
                Vector3 edge = tri[(i + 1) % tri.VertexCount] - tri[i];
                Vector3 vp = p - tri[i];

                // Vector perpendicular to tri's plane
                Vector3 c = Vector3.CrossProduct(edge, vp);

                // If P is on the right side of the edge, P lies outside the triangle
                if (Vector3.DotProduct(triNormal, c) < 0.0f) 
                    return false;
            }

            // Assign P as the hitPoint before leaving method
            hitPoint = p;

            // If no exception is found the ray intersects the triangle
            return true;
        }
    }
}
