using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenTk_Test.GamObj
{
    internal class Rock : genObj
    {
        SKPoint pCenter;
        SKPoint pImage;
        SKImage image;
        float rotation;
        int size;
        SKPaint TestBrush;

        public Rock(float x, float y) : base(x, y)
        {
           
            
            Random r = new Random();

            this.size = r.Next(20, 60);
            this.pCenter = new SKPoint(x , y );
            this.pImage = new SKPoint(x - size, y- size);
            SKImage inImage;
            using (Stream stream = new FileStream(@"Image\asteroid.png", FileMode.Open))
            {
                inImage = SKImage.FromEncodedData(stream);
            }
            //image.ScalePixels(new SKPixmap(new SKImageInfo(40, 40), this.image.Handle), SKFilterQuality.Medium );
            SKImageInfo info = new SKImageInfo(size * 2, size * 2, SKColorType.Bgra8888);
            image = SKImage.Create(info);
            inImage.ScalePixels(image.PeekPixels(), SKFilterQuality.None);
            rotation = r.Next(360);

            TestBrush = new SKPaint
            {
                Color = SKColors.Red,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                TextAlign = SKTextAlign.Center,
                TextSize = 24
            };
        }


        public override void OnKeyDown(KeyboardKeyEventArgs e) { }

        public override void OnKeyUp(KeyboardKeyEventArgs e) { }

        public override void OnRenderFrame(FrameEventArgs args, SKCanvas canvas)
        {
            rotation += 0.2f;
            //canvas.DrawPath(SKPath.ParseSvgPathData("M 21,13 26,11 18,5 11,3 10,9 6,16 14,19 23,19 19,15 Z"), TestBrush);
            //canvas.DrawCircle(pCenter.X, pCenter.Y, size, TestBrush);
            canvas.Save();
            canvas.RotateDegrees(rotation, pCenter.X, pCenter.Y);
            canvas.DrawImage(image, pImage);
            canvas.Restore();

        }

        public override void OnUpdateFrame(FrameEventArgs args)
        { }

        public bool CircleCollision(double p1_X, double p1_Y, double r1)
        {
            var radius = r1 + size;
            var deltaX = p1_X - pCenter.X;
            var deltaY = p1_Y - pCenter.Y;
            return deltaX * deltaX + deltaY * deltaY <= radius * radius;
        }
    }
}
