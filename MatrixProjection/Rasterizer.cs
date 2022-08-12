using System;
using System.Collections.Generic;

namespace MatrixProjection {

    public class Rasterizer : IRenderer {

        public RenderMode RenderMode { get; set; } = RenderMode.Solid;
        public List<Fragment> Fragments { get; } = new List<Fragment>();

        private readonly Camera camera;
        private readonly Light light;

        private readonly int width, height;

        // '\u25A0' -> OLD BLOCK | '\u2588' -> Full Block | '\u2593' -> Dark Shade | '\u2592' -> Medium Shade | '\u2591' -> Light Shade
        private ShadeChar currSymbol = ShadeChar.Full;
        private ConsoleColor currColor = ConsoleColor.White;

        public Rasterizer(Camera camera, Light light) {

            this.camera = camera;
            this.light = light;

            width = Console.WindowWidth;
            height = Console.WindowHeight - 1;
        }

        public void Render(RenderObject rObject) {

            Fragments.Clear();

            Triangle[] updatedTri = new Triangle[rObject.Mesh.Polygons.Length];

            for (int i = 0; i < rObject.Mesh.Polygons.Length; i++) {

                updatedTri[i] = new Triangle(new Vector[rObject.Mesh.Polygons[i].VertexCount]);

                for (int j = 0; j < rObject.Mesh.Polygons[i].VertexCount; j++) {

                    // Object to World space (Local to World coords)
                    updatedTri[i][j] = Mat4x4.MatMul(rObject.Mesh.Polygons[i][j], rObject.ModelMatrix);
                }

                float lightDP = Vector.DotProduct(updatedTri[i].Normal, light.Direction);
                ShadeTri(ref updatedTri[i], lightDP);

                for (int j = 0; j < updatedTri[i].VertexCount; j++) {

                    // World to View
                    Vector4 tri4 = Mat4x4.MatMul((Vector4)updatedTri[i][j], camera.ViewMatrix);

                    // View to Projection
                    tri4 = Mat4x4.MatMul(tri4, camera.ProjMatrix);

                    // Perspective Division
                    updatedTri[i][j] = camera.IsOrthographic() ? (Vector)tri4 : (Vector)tri4 / tri4.W;

                    // Scale Vectors
                    updatedTri[i][j] *= camera.IsOrthographic() ? 10.0f : 40.0f;  // Magic numbers >:(
                }
            }

            switch (RenderMode) {

                case RenderMode.Vertices:
                    PlotVertices(updatedTri);
                    break;

                case RenderMode.Mesh:
                    PlotMesh(updatedTri);
                    break;

                case RenderMode.Solid:
                    PlotFaces(updatedTri);
                    break;
            }
        }

        private void ShadeTri(ref Triangle tri, float dotProduct) {

            //if (dotProduct < 0.0f) return ShadeChar.Null;

            if (dotProduct <= 0.1f) {

                tri.Color = ConsoleColor.DarkGray;
                tri.Symbol = ShadeChar.Low;

            } else if (dotProduct < 0.5f) {

                tri.Color = ConsoleColor.Gray;
                tri.Symbol = ShadeChar.Medium;

            } else if (dotProduct < 0.7f) {

                tri.Color = ConsoleColor.Gray;
                tri.Symbol = ShadeChar.High;

            } else {

                tri.Color = ConsoleColor.White;
                tri.Symbol = ShadeChar.Full;
            }
        }

        public void PlotPoint(Vector v) {

            Fragments.Add(new Fragment(v, currSymbol, currColor));
        }

        // Draws Vertices only
        public void PlotVertices(Triangle[] projected) {

            currSymbol = ShadeChar.Full;
            currColor = ConsoleColor.White;

            for (int i = 0; i < projected.Length; i++) {

                for (int j = 0; j < projected[i].VertexCount; j++) {

                    projected[i][j] = ConvertToScreen(projected[i][j]);

                    PlotPoint(projected[i][j]);
                }
            }
        }

        // Draws Mesh
        public void PlotMesh(Triangle[] projected) {

            for (int i = 0; i < projected.Length; i++) {

                if (BackfaceCulled(projected[i])) continue;

                currSymbol = ShadeChar.Full;
                currColor = ConsoleColor.White;

                for (int j = 0; j < projected[i].VertexCount; j++) {

                    projected[i][j] = ConvertToScreen(projected[i][j]);
                }

                for (int j = 0; j < projected[i].VertexCount; j++) {

                    // Fills every possible spot between two given points to form a line
                    PlotLine(projected[i][j], projected[i][(j + 1) % projected[i].VertexCount]);
                }
            }
        }

        // Draws Solid
        public void PlotFaces(Triangle[] projected) {

            for (int i = 0; i < projected.Length; i++) {

                if (BackfaceCulled(projected[i])) continue;

                for (int j = 0; j < projected[i].VertexCount; j++) {

                    projected[i][j] = ConvertToScreen(projected[i][j]);
                }

                currSymbol = projected[i].Symbol;
                currColor = projected[i].Color;

                bool flat = false;
                float prevY = -1;
                Triangle sorted = SortVertices(projected[i]);

                for (int j = 0; j < projected[i].VertexCount; j++) {

                    // Verify if triangle is flat
                    if ((int)projected[i][j].Y == (int)projected[i][(j + 1) % projected[i].VertexCount].Y) {

                        flat = true;

                        if (prevY > projected[i][j].Y) {

                            FillBottomFlatTriangle(sorted[0], sorted[1], sorted[2]);
                            break;

                        } else {

                            FillTopFlatTriangle(sorted[0], sorted[1], sorted[2]);
                            break;
                        }

                    } else {

                        prevY = projected[i][j].Y;
                    }
                }

                // if not, divide triangle into two flat, smaller triangles
                if (!flat) {

                    Vector v4 = new Vector(sorted[0].X + (sorted[1].Y - sorted[0].Y) /
                                          (sorted[2].Y - sorted[0].Y) *
                                          (sorted[2].X - sorted[0].X),
                                           sorted[1].Y);

                    FillTopFlatTriangle(sorted[0], sorted[1], v4);
                    FillBottomFlatTriangle(sorted[1], v4, sorted[2]);
                }
            }
        }

        private void FillTopFlatTriangle(Vector v1, Vector v2, Vector v3) {

            float invslope1 = (v2.X - v1.X) / (v2.Y - v1.Y);
            float invslope2 = (v3.X - v1.X) / (v3.Y - v1.Y);

            float curx1 = v1.X + 0.5f; // +0.5f adds small offset to not draw
            float curx2 = v1.X + 0.5f; // extra pixel on first top/bottom edge

            for (int scanlineY = (int)v1.Y; scanlineY <= v2.Y; scanlineY++) {

                PlotLine(new Vector(curx1, scanlineY), new Vector(curx2, scanlineY));
                curx1 += invslope1;
                curx2 += invslope2;
            }
        }

        private void FillBottomFlatTriangle(Vector v1, Vector v2, Vector v3) {

            float invslope1 = (v3.X - v1.X) / (v3.Y - v1.Y);
            float invslope2 = (v3.X - v2.X) / (v3.Y - v2.Y);

            float curx1 = v3.X + 0.5f; // +0.5f adds small offset to not draw
            float curx2 = v3.X + 0.5f; // extra pixel on first top/bottom edge

            for (int scanlineY = (int)v3.Y; scanlineY >= v1.Y; scanlineY--) {

                PlotLine(new Vector(curx1, scanlineY), new Vector(curx2, scanlineY));
                curx1 -= invslope1;
                curx2 -= invslope2;
            }
        }

        // Does not reverse 'Y' value (actual console Y)
        // Reversed 'Y' value is only used at the time of drawing and out of bounds verification
        private Vector ConvertToScreen(Vector v) {

            return new Vector((int)(v.X + (width * 0.5f)), (int)(v.Y - (height * 0.5f)));
        }

        private Vector ConvertToRaster(Vector v) {

            return new Vector((int)((-v.X + 1) * 0.5 * width),
                              (int)((1 - (-v.Y + 1) * 0.5) * height));
        }

        private bool BackfaceCulled(Triangle polygon) {

            // Shoelace formula: https://en.wikipedia.org/wiki/Shoelace_formula#Statement
            // Division by 2 is not necessary, since all we care about is if the value is positive/negative

            float sum = 0.0f;

            for (int i = 0; i < polygon.VertexCount; i++) {

                sum += (polygon[i].X * polygon[(i + 1) % polygon.VertexCount].Y) -
                       (polygon[i].Y * polygon[(i + 1) % polygon.VertexCount].X);
            }

            return sum >= 0;
        }

        // Insertion Sort Algorithm
        // https://en.wikipedia.org/wiki/Insertion_sort
        private Triangle SortVertices(Triangle polygon) {

            Triangle polygonCopy = polygon;

            for (int i = 1; i < polygonCopy.VertexCount; i++) {

                Vector chosen = polygonCopy[i];
                int j = i - 1;

                while (j >= 0 && polygonCopy[j].Y > chosen.Y) {

                    polygonCopy[j + 1] = polygonCopy[j];
                    j--;
                }

                polygonCopy[j + 1] = chosen;
            }

            return polygonCopy;
        }

        #region Bresenham's Line Algorithm

        /* Wiki w/ pseudocode -> https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm#All_cases
         * 
         * Note: Better results with 'int' values than with 'float' (less jittery) */

        public void PlotLine(Vector from, Vector to) {

            if (Math.Abs((int)to.Y - (int)from.Y) < Math.Abs((int)to.X - (int)from.X)) {

                if ((int)from.X > (int)to.X) {

                    PlotLineLow(to, from);

                } else {

                    PlotLineLow(from, to);
                }

            } else {

                if ((int)from.Y > (int)to.Y) {

                    PlotLineHigh(to, from);

                } else {

                    PlotLineHigh(from, to);
                }
            }
        }

        private void PlotLineLow(Vector from, Vector to) {

            int dx = (int)to.X - (int)from.X;
            int dy = (int)to.Y - (int)from.Y;

            int yi = 1;

            if (dy < 0) {

                yi = -1;
                dy = -dy;
            }

            int D = (2 * dy) - dx;
            int y = (int)from.Y;

            for (int i = (int)from.X; i <= (int)to.X; i++) {

                PlotPoint(new Vector(i, y));

                if (D > 0) {

                    y += yi;
                    D += 2 * (dy - dx);

                } else {

                    D += 2 * dy;
                }
            }
        }

        private void PlotLineHigh(Vector from, Vector to) {

            int dx = (int)to.X - (int)from.X;
            int dy = (int)to.Y - (int)from.Y;

            int xi = 1;

            if (dx < 0) {

                xi = -1;
                dx = -dx;
            }

            int D = (2 * dx) - dy;
            int x = (int)from.X;

            for (int i = (int)from.Y; i < (int)to.Y; i++) {

                PlotPoint(new Vector(x, i));

                if (D > 0) {

                    x += xi;
                    D += 2 * (dx - dy);

                } else {

                    D += 2 * dx;
                }
            }
        }

        #endregion
    }
}
