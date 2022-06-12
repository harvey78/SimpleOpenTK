using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using SkiaSharp;
using System.Drawing;

namespace OpenTk_Test
{
    public class Game : GameWindow
    {
        GRGlInterface? grgInterface;
        GRContext? grContext;
        SKSurface? surface;
        SKCanvas? canvas;
        GRBackendRenderTarget? renderTarget;
        SKPaint? TestBrush;
        Random r = new Random();
        List<GamObj.genObj> objList = new List<GamObj.genObj>();
        List<GamObj.Rock> RockList = new List<GamObj.Rock>();
        List<GamObj.Bullet> BulletList = new List<GamObj.Bullet>();
        static public Queue<GamObj.genObj> ObjToInsert = new Queue<GamObj.genObj>();
        GamObj.Player Player;
        float MousePos_X;
        float MousePos_Y;
        
        public Game(string title, int width, int height) : base(new GameWindowSettings
        {
            RenderFrequency = 60.0
        },
        new NativeWindowSettings
        {
            Title = title,
            Flags = ContextFlags.ForwardCompatible | ContextFlags.Debug,
            Profile = ContextProfile.Core,
            StartFocused = true,
            WindowBorder = WindowBorder.Fixed,
            Size = new Vector2i(width, height),
            //WindowState  =  WindowState.Fullscreen
        })
        {
            VSync = VSyncMode.Off;
            Player = new GamObj.Player(width / 2, height / 2);
            objList.Add(Player);

            Random r= new Random();
            for (int i = 0; i < 100; i++)
                RockList.Add(new GamObj.Rock(r.Next(width), r.Next(height)));

            objList.AddRange(RockList);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            //Context.MakeCurrent();
            grgInterface = GRGlInterface.Create();
            grContext = GRContext.CreateGl(grgInterface);
            renderTarget = new GRBackendRenderTarget(ClientSize.X, ClientSize.Y, 0, 8, new GRGlFramebufferInfo(0, (uint)SizedInternalFormat.Rgba8));
            surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            canvas = surface.Canvas;

            TestBrush = new SKPaint
            {
                Color = SKColors.White,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Center,
                TextSize = 24
            };


        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            MousePos_X = e.X;
            MousePos_Y = e.Y;
            base.OnMouseMove(e);
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Random r = new Random();
            for (int i = 0; i < 500; i++)
            {
                Vector2 Direction = new Vector2((float)r.NextDouble() - .5f, (float)r.NextDouble() - .5f);
                Direction.Normalize();
                objList.Add(new GamObj.Bullet(MousePos_X, MousePos_Y, Direction, (float)r.NextDouble()*60));

            }

            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Escape)
                Close();

            foreach (var obj in objList)
                obj.OnKeyDown(e);


            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            foreach (var obj in objList)
                obj.OnKeyUp(e);

            base.OnKeyUp(e);
        }

        protected override void OnUnload()
        {
            TestBrush!.Dispose();
            surface!.Dispose();
            renderTarget!.Dispose();
            grContext!.Dispose();
            grgInterface!.Dispose();
            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            foreach (var obj in objList)
                obj.OnUpdateFrame(args);

            while (ObjToInsert.Count > 0)
            {
                GamObj.genObj o = ObjToInsert.Dequeue();
                objList.Add(o);
                if(o is GamObj.Bullet )
                    BulletList.Add((GamObj.Bullet)o);

            }


            objList.RemoveAll(obj => obj.inDestroy);
            BulletList.RemoveAll(obj => obj.inDestroy);
            RockList.RemoveAll(obj => obj.inDestroy);


            base.OnUpdateFrame(args);
        }

        double time = 0;
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            time += args.Time;
            canvas!.Clear(new  SKColor(11, 16, 38));

            //TestBrush!.Color = SKColors.Wheat;
            //canvas.DrawRoundRect(new SKRoundRect(new SKRect(0, 0, 256, 256), (float)Math.Max(Math.Sin(-time) * 128.0f, 0)), TestBrush);


            //TestBrush!.Color = SKColors.DarkRed;
            //canvas.DrawCircle(1, 1, 10, TestBrush);
            bool Collision=false;
            RockList.ForEach(n => Collision = Collision || n.CircleCollision(Player.x, Player.y, 10));

            foreach (var b in BulletList)
                foreach (var r in RockList)
                    if (r.CircleCollision(b.x, b.y, 2))
                    {
                        r.inDestroy = true;
                        b.inDestroy = true;
                        Random rand = new Random();
                        for (int i = 0; i < 100; i++)
                            objList.Add(new GamObj.Bullet(r.x,r.y,new Vector2((float )rand.NextDouble()-0.5f, (float)rand.NextDouble() - 0.5f).Normalized(), rand.Next(20,50)));
                    }
            


            TestBrush!.Color = SKColors.White;
            canvas.DrawText("RPS " + (1 / args.Time).ToString("0") + "  " + time.ToString("0") + "s", 400, 30, TestBrush);
            if(Collision)
            canvas.DrawText("Collision", 400, 50, TestBrush);

            foreach (var obj in objList)
                obj.OnRenderFrame(args, canvas);

            canvas.Flush();
            SwapBuffers();
        }




        //private bool CircleCollision(Point p1, double r1, Point p2, double r2)
        //{
        //    var radius = r1 + r2;
        //    var deltaX = p1.X - p2.X;
        //    var deltaY = p1.Y - p2.Y;
        //    return deltaX * deltaX + deltaY * deltaY <= radius * radius;
        //}
    }
}
