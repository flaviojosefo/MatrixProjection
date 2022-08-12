using System;
using System.Collections.Generic;

namespace MatrixProjection {

    public class RayTracer : IRenderer {

        public RenderMode RenderMode { get; set; } = RenderMode.Solid;
        public List<Fragment> Fragments { get; } = new List<Fragment>();

        public RayTracer() { }

        public void Render(RenderObject rObject) {


        }
    }
}
