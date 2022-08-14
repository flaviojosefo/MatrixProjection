namespace MatrixProjection {

    public class Cube : Mesh {

        public override Triangle[] Polygons { get; protected set; } = new Triangle[12];

        public Cube() {

            float size = 1.0f;

            if (true) {

                // Front
                CreateTri(new Vector3(-size * 0.5f, -size * 0.5f, -size * 0.5f),
                          new Vector3(-size * 0.5f, size * 0.5f, -size * 0.5f),
                          new Vector3(size * 0.5f, size * 0.5f, -size * 0.5f));

                CreateTri(new Vector3(-size * 0.5f, -size * 0.5f, -size * 0.5f),
                          new Vector3(size * 0.5f, size * 0.5f, -size * 0.5f),
                          new Vector3(size * 0.5f, -size * 0.5f, -size * 0.5f));

                // Right
                CreateTri(new Vector3(size * 0.5f, -size * 0.5f, -size * 0.5f),
                          new Vector3(size * 0.5f, size * 0.5f, -size * 0.5f),
                          new Vector3(size * 0.5f, size * 0.5f, size * 0.5f));

                CreateTri(new Vector3(size * 0.5f, -size * 0.5f, -size * 0.5f),
                          new Vector3(size * 0.5f, size * 0.5f, size * 0.5f),
                          new Vector3(size * 0.5f, -size * 0.5f, size * 0.5f));

                // Back
                CreateTri(new Vector3(size * 0.5f, -size * 0.5f, size * 0.5f),
                          new Vector3(size * 0.5f, size * 0.5f, size * 0.5f),
                          new Vector3(-size * 0.5f, size * 0.5f, size * 0.5f));

                CreateTri(new Vector3(size * 0.5f, -size * 0.5f, size * 0.5f),
                          new Vector3(-size * 0.5f, size * 0.5f, size * 0.5f),
                          new Vector3(-size * 0.5f, -size * 0.5f, size * 0.5f));

                // Left
                CreateTri(new Vector3(-size * 0.5f, -size * 0.5f, size * 0.5f),
                          new Vector3(-size * 0.5f, size * 0.5f, size * 0.5f),
                          new Vector3(-size * 0.5f, size * 0.5f, -size * 0.5f));

                CreateTri(new Vector3(-size * 0.5f, -size * 0.5f, size * 0.5f),
                          new Vector3(-size * 0.5f, size * 0.5f, -size * 0.5f),
                          new Vector3(-size * 0.5f, -size * 0.5f, -size * 0.5f));

                // Top
                CreateTri(new Vector3(-size * 0.5f, size * 0.5f, -size * 0.5f),
                          new Vector3(-size * 0.5f, size * 0.5f, size * 0.5f),
                          new Vector3(size * 0.5f, size * 0.5f, size * 0.5f));

                CreateTri(new Vector3(-size * 0.5f, size * 0.5f, -size * 0.5f),
                          new Vector3(size * 0.5f, size * 0.5f, size * 0.5f),
                          new Vector3(size * 0.5f, size * 0.5f, -size * 0.5f));

                // Bottom
                CreateTri(new Vector3(-size * 0.5f, -size * 0.5f, -size * 0.5f),
                          new Vector3(size * 0.5f, -size * 0.5f, size * 0.5f),
                          new Vector3(-size * 0.5f, -size * 0.5f, size * 0.5f));

                CreateTri(new Vector3(-size * 0.5f, -size * 0.5f, -size * 0.5f),
                          new Vector3(size * 0.5f, -size * 0.5f, -size * 0.5f),
                          new Vector3(size * 0.5f, -size * 0.5f, size * 0.5f));

            } /*else  {

                Polygons = new Vector3[6][];

                CreateQuad(new Vector3(size * 0.5f, 0, 0),
                           new Vector3(0, -size * 0.5f, 0),
                           new Vector3(0, 0, size * 0.5f));

                CreateQuad(new Vector3(-size * 0.5f, 0, 0),
                           new Vector3(0, size * 0.5f, 0),
                           new Vector3(0, 0, size * 0.5f));

                CreateQuad(new Vector3(0, size * 0.5f, 0),
                           new Vector3(size * 0.5f, 0),
                           new Vector3(0, 0, size * 0.5f));

                CreateQuad(new Vector3(0, -size * 0.5f, 0),
                           new Vector3(-size * 0.5f, 0),
                           new Vector3(0, 0, size * 0.5f));

                CreateQuad(new Vector3(0, 0, size * 0.5f),
                           new Vector3(-size * 0.5f, 0),
                           new Vector3(0, size * 0.5f, 0));

                CreateQuad(new Vector3(0, 0, -size * 0.5f),
                           new Vector3(size * 0.5f, 0, 0),
                           new Vector3(0, size * 0.5f, 0));
            }*/
        }
    }
}
