using OpenTK.Windowing.Common;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTk_Test.GamObj
{
    public abstract class genObj
    {
        public bool inDestroy;
        public float x;
        public float y;

        public genObj(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public abstract void OnRenderFrame(FrameEventArgs args, SKCanvas canvas);
        public abstract void OnUpdateFrame(FrameEventArgs args);
        public abstract void OnKeyDown(KeyboardKeyEventArgs e);
        public abstract void OnKeyUp(KeyboardKeyEventArgs e);
    }
}
