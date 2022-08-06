using System.Collections.Generic;

namespace MatrixProjection {

    public interface IRenderer {

        RenderMode RenderMode { get; set; }
        List<Fragment> Fragments { get; }

        void Render(RenderObject renderObject);
    }
}
