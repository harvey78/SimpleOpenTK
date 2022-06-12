using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTk_Test.GamObj
{
    internal class Player : genObj
    {
        SKPaint? TestBrush;
        Vector2 Direction;
        float Speed;
        bool Keys_Down;
        bool Keys_Up;
        bool Keys_Left;
        bool Keys_Right;


        public Player(float x, float y):base( x,  y)
        {
            this.Direction = new Vector2(0, -1);
            this.Speed = 0;
            TestBrush = new SKPaint
            {
                Color = new SKColor(118, 118, 118) ,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Center,
                TextSize = 24
            };
        }



        public override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Down) Keys_Down = true;
            if (e.Key == Keys.Up) Keys_Up = true;
            if (e.Key == Keys.Left) Keys_Left = true;
            if (e.Key == Keys.Right) Keys_Right = true;

            if (e.Key == Keys.Space)
                Game.ObjToInsert.Enqueue(new GamObj.Bullet(x, y, Direction, Speed + 20));

        }

        public override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Down) Keys_Down = false;
            if (e.Key == Keys.Up) Keys_Up = false;
            if (e.Key == Keys.Left) Keys_Left = false;
            if (e.Key == Keys.Right) Keys_Right = false;
        }

        public override void OnRenderFrame(FrameEventArgs args, SKCanvas canvas)
        {

            canvas.DrawCircle(x, y, 5, TestBrush);
            canvas.DrawLine(x, y, x - Direction.X * 10, y - Direction.Y * 10, TestBrush);

        }

        public override void OnUpdateFrame(FrameEventArgs args)
        {
            if (Keys_Up && Speed < 10)
                Speed += .02f;
            else if (!Keys_Up && Speed > 0)
                Speed -= .001f;

            if (Keys_Down && Speed > -10)
                Speed -= .1f;
            else if (!Keys_Down && Speed < 0)
                Speed += .001f;

            if (Keys_Right)
                //Direction += new Vector2(0.001f, 0);
                Direction = Vector2.Transform(Direction, Quaternion.FromEulerAngles(0, 0, 0.0001f));
            if (Keys_Left)
                Direction = Vector2.Transform(Direction, Quaternion.FromEulerAngles(0, 0, -0.0001f)); ;


            Direction.Normalize();
            this.y += Speed * Direction.Y / 10000;
            this.x += Speed * Direction.X / 10000;

        }


    }
}
