using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTk_Test.GamObj
{
    internal class Bullet : genObj
    {

        SKPaint? TestBrush;
        Vector2 Direction;
        float Speed;
        double life = 2000;


        public Bullet(float x, float y, Vector2 Direction, float Speed) : base(x, y)
        {

            this.Direction = Direction;
            this.Speed = Speed;
            TestBrush = new SKPaint
            {
                Color = SKColors.Gold ,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Center,
                TextSize = 24
            };
        }

        public override void OnKeyDown(KeyboardKeyEventArgs e) { }

        public override void OnKeyUp(KeyboardKeyEventArgs e) { }

        public override void OnRenderFrame(FrameEventArgs args, SKCanvas canvas)
        {
            canvas.DrawCircle(x, y, 2, TestBrush);
            //canvas.DrawText("Life " + life.ToString("0") + "s", 400, 130, TestBrush);

            this.y += Speed * Direction.Y / 10;
            this.x += Speed * Direction.X / 10;

            if (life < 0)
                inDestroy = true;

            life -= args.Time * 1000;
        }

        public override void OnUpdateFrame(FrameEventArgs args)
        {
       
        }
    }
}
