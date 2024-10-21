//-----------------------------------------------------------------------
// <copyright file="Rasterizer.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SoftRenderer.Engine.Render.Technique.Rasterizer
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Windows.Media.Media3D;
    using Driver.GDI;
    using Math;
    using RCi.Tutorials.Gfx.Mathematics;

    /// <summary>
    ///     Rasterizer Render Port.
    /// </summary>
    public class Rasterizer : RenderBaseGdi
    {
        private Dictionary<char, double> Axis;
        /// <summary>
        ///     Initializes a new instance of the <see cref="Rasterizer" /> class.
        ///     Rasterization Render constructor.
        /// </summary>
        /// <param name="renderBaseArgs">dto that hold form handles.</param>
        public Rasterizer(IRenderBaseArgs renderBaseArgs)
            : base(renderBaseArgs)
        {
            this.Axis = new Dictionary<char, double>() { {'X', 1}, {'Y', 0}, {'Z', 0}};
            this.RunGame = true;
            this.SleepTime = 0;
            this.HostInput.KeyDown += this.HandleKeyPress;
        }

        private bool RunGame { get; set; }

        private int SleepTime { get; set; }

        /// <inheritdoc />
        public override void RenderInternal()
        {
            Graphics graphics = this.DrawBuffer.Graphics;
            if (this.RunGame)
            {
                graphics.Clear(Color.Black);
                if (this.SleepTime > 0)
                {
                    Thread.Sleep(this.SleepTime);
                }
                this.DrawWorldAxis();
                // this.DrawGeometry();

                // this.DrawBuffer.MoveToDrawBuffer();

                #region commented

                // asynchronous safe render (move buffer)
                // Parallel.For(0, this.DrawBuffer.Width, (x) =>
                // {
                //     for (int y = 0; y < this.DrawBuffer.Height; y++)
                //     {
                //         this.SetRandomColor(x, y);
                //     }
                // });
                // asynchronous safe render (int buffer)
                // Parallel.For(0, this.DrawBuffer.DrawBufferBytesArray.Length, this.SetRandomColorIdx);

                // // synchronous safe render
                // for (int idx = 0; idx < this.DrawBuffer.DrawBufferBytesArray.Length; idx++)
                // {
                //     this.SetRandomColor(idx);
                // }

                // asynchronous unsafe render
                // not possible at the moment
                // unsafe
                // {
                //     int* ptr = (int*)this.DrawBuffer.DrawBufferPtr.ToPointer();
                //     Parallel.For(0, this.DrawBuffer.DrawBufferBytesArray.Length, (idx) =>
                //     {
                //         *ptr++ = this.SetRandomColor(idx);
                //     });
                // }

                // synchronous unsafe render
                // unsafe
                // {
                //     int* ptr = (int*)this.DrawBuffer.DrawBufferPtr.ToPointer();
                //     for (int idx = 0; idx < this.DrawBuffer.DrawBufferBytesArray.Length; idx++)
                //     {
                //         *ptr++ = this.SetRandomColor(idx);
                //     }
                // }

                // this.DrawBuffer.MoveToDrawBuffer();

                #endregion

            }
            else
            {
                graphics.Clear(Color.Black);
            }

            // Draw bitmap to buffer
            Point clientBufferPoint = new Point(this.ClientBuffer.X, this.ClientBuffer.Y);
            graphics.DrawString($"X: {this.Axis['X']}", this.FpsFont, Brushes.Yellow, new PointF(clientBufferPoint.X + (this.ClientBuffer.Width / 2), clientBufferPoint.Y));
            graphics.DrawString($"Y: {this.Axis['Y']}", this.FpsFont, Brushes.Yellow, new PointF(clientBufferPoint.X + (this.ClientBuffer.Width / 2), clientBufferPoint.Y + 20));
            graphics.DrawString($"Z: {this.Axis['Z']}", this.FpsFont, Brushes.Yellow, new PointF(clientBufferPoint.X + (this.ClientBuffer.Width / 2), clientBufferPoint.Y + 40));
            this.ClientBufferedGraphics.Graphics.DrawImage(this.DrawBuffer.BitMap, new RectangleF(Point.Empty, this.ClientBuffer.Size), new RectangleF(new PointF(-1F, -1F), this.DrawBuffer.Size), GraphicsUnit.Pixel);
        }

        private void DrawWorldAxis()
        {
            Vector3[] xAxis = {new Vector3(0, 0, 0), new Vector3(1, 0, 0) };
            Vector3[] yAxis = {new Vector3(0, 0, 0), new Vector3(0, 1, 0) };
            Vector3[] zAxis = {new Vector3(0, 0, 0), new Vector3(0, 0, 1) };
            this.DrawPolyline(xAxis, Space.World, new Pen(Color.Aqua));
            this.DrawPolyline(yAxis, Space.World, new Pen(Color.Red));
            this.DrawPolyline(zAxis, Space.World, new Pen(Color.Blue));
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();
            this.HostInput.KeyDown -= this.HandleKeyPress;
        }

        private void DrawPolyline(IEnumerable<Vector3> points, Space space, Pen pen)
        {
            Matrix clientView = Matrix.NdcToScreenCoordinates(this.ClientBuffer);
            switch (space)
            {
                case Space.World:
                    DateTime utcNow = DateTime.UtcNow;
                    var t = GetAnimationTime(new TimeSpan(0, 0, 0, 10), utcNow);
                    var angle = t * Math.PI * 2;
                    var radius = 2;
                    // View Matrix
                    Vector3 camera = new Vector3(0.5, Math.Cos(angle) * radius, 1);
                    Vector3 up = Vector3.YAxis;
                    Vector3 target = new Vector3();
                    Matrix viewMatrix = Matrix.ViewMatrix(camera, target, up);

                    // Projection Matrix
                    double fov = Math.PI * 0.5;
                    double znear = 0.001;
                    double zfar = 1000;
                    Matrix projectionMatrix = Matrix.PerspectiveMatrix(fov, (double)this.DrawBuffer.Height / this.DrawBuffer.Width, znear, zfar);
                    Matrix transMatrix = viewMatrix * projectionMatrix * clientView;
                    var finalViews = Matrix.TransformPoints(transMatrix, points);
                    this.DrawPolylineScreenSpace(finalViews, pen);
                    break;
                case Space.View:
                    var viewPoints = Matrix.TransformPoints(clientView, points);
                    this.DrawPolylineScreenSpace(viewPoints, pen);
                    break;

                case Space.Screen:
                    this.DrawPolylineScreenSpace(points, pen);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(space), space, null);
            }
        }

        private void DrawPolylineScreenSpace(IEnumerable<Vector3> pointsScreen, Pen pen)
        {
            var from = default(Vector3?);
            foreach (Vector3 pointScreen in pointsScreen)
            {
                if (from != null)
                {
                    this.DrawBuffer.Graphics.DrawLine(pen, (float)from.X, (float)from.Y, (float)pointScreen.X, (float)pointScreen.Y);
                }

                from = pointScreen;
            }
        }

        private void TestTransformations()
        {
            // raw coordinates for arrow in screen space
            var pointsArrowScreen = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(40, 0, 0),
                new Vector3(35, 10, 0),
                new Vector3(50, 0, 0),
                new Vector3(35, -10, 0),
                new Vector3(40, 0, 0),
            };

            // raw coordinates for arrow in view space
            var pointsArrowView = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(0.08, 0, 0),
                new Vector3(0.07, 0.02, 0),
                new Vector3(0.1, 0, 0),
                new Vector3(0.07, -0.02, 0),
                new Vector3(0.08, 0, 0),
            };

            // draw default
            this.DrawPolyline(pointsArrowScreen, Space.Screen, Pens.Yellow);
            this.DrawPolyline(pointsArrowView, Space.View, Pens.Cyan);

            // get animation params
            var periodDuration = new TimeSpan(0, 0, 0, 5, 0);
            DateTime utcNow = DateTime.UtcNow;
            double t = GetAnimationTime(periodDuration, utcNow);
            var sinT = Math.Sin(t * Math.PI * 2);
            
            // translate
            Matrix transform1 = Matrix.Translate(sinT * 40.0, 0.0, 0.0) * Matrix.Translate(50, 100, 0);
            Matrix transform2 = Matrix.Translate(sinT * 0.1, 0, 0) * Matrix.Translate(-0.8, 0, 0);
            var transformPoint1 = Matrix.TransformPoints(transform1, pointsArrowScreen);
            var transformPoint2 = Matrix.TransformPoints(transform2, pointsArrowView);
            this.DrawPolyline(transformPoint1, Space.Screen, Pens.White);
            this.DrawPolyline(transformPoint2, Space.View, Pens.Red);

            // scale
            Matrix transform3 = Matrix.Scale(t * 2, t * 2, 1) * Matrix.Translate(150, 100, 0);
            Matrix transform4 = Matrix.Scale(t * 2, t * 2, 1) * Matrix.Translate(-0.6, 0, 0);
            transformPoint1 = Matrix.TransformPoints(transform3, pointsArrowScreen);
            transformPoint2 = Matrix.TransformPoints(transform4, pointsArrowView);
            this.DrawPolyline(transformPoint1, Space.Screen, Pens.White);
            this.DrawPolyline(transformPoint2, Space.View, Pens.Red);

            // // // rotate
            Matrix transform5 = Matrix.Rotate(t * Math.PI * 2, new Vector3(0, 0, 1)) * Matrix.Translate(300, 100, 0);
            Matrix transform6 = Matrix.Rotate(t * Math.PI * 2, new Vector3(0, 0, 1)) * Matrix.Translate(-0.2, 0, 0);
            transformPoint1 = Matrix.TransformPoints(transform5, pointsArrowScreen);
            transformPoint2 = Matrix.TransformPoints(transform6, pointsArrowView);
            this.DrawPolyline(transformPoint1, Space.Screen, Pens.White);
            this.DrawPolyline(transformPoint2, Space.View, Pens.Red);

            // rotate * translate
            Matrix transform7 = Matrix.Rotate(t * Math.PI * 2, new Vector3(0, 0, 1)) * Matrix.Translate(0, sinT * 40, 0) * Matrix.Translate(400, 100, 0);
            Matrix transform8 = Matrix.Rotate(t * Math.PI * 2, new Vector3(0, 0, 1)) * Matrix.Translate(0, sinT * 0.2, 0) * Matrix.Translate(0, 0, 0);
            transformPoint1 = Matrix.TransformPoints(transform7, pointsArrowScreen);
            transformPoint2 = Matrix.TransformPoints(transform8, pointsArrowView);

            this.DrawPolyline(transformPoint1, Space.Screen, Pens.White);
            this.DrawPolyline(transformPoint2, Space.View, Pens.Red);

            // translate * rotate
            Matrix transform9 = Matrix.Translate(0, sinT * 40, 0) * Matrix.Rotate(t * Math.PI * 2, new Vector3(0, 0, 1)) * Matrix.Translate(500, 100, 0);
            Matrix transform10 = Matrix.Translate(0, sinT * 0.2, 0) * Matrix.Rotate(t * Math.PI * 2, new Vector3(0, 0, 1)) * Matrix.Translate(0.4, 0, 0);
            transformPoint1 = Matrix.TransformPoints(transform9, pointsArrowScreen);
            transformPoint2 = Matrix.TransformPoints(transform10, pointsArrowView);

            this.DrawPolyline(transformPoint1, Space.Screen, Pens.White);
            this.DrawPolyline(transformPoint2, Space.View, Pens.Red);
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.RunGame = true;
                    break;
                case Keys.Right:
                    this.SleepTime += 100;
                    break;
                case Keys.X:
                    if (this.Axis['X'] < 2)
                    {
                        this.Axis['X'] += 0.1;
                    }
                    else
                    {
                        this.Axis['X'] = 0;
                    }
                    Console.WriteLine($"X: {this.Axis['X']}");
                    break;
                case Keys.Y:
                    if (this.Axis['Y'] < 2)
                    {
                        this.Axis['Y'] += 0.1;
                    }
                    else
                    {
                        this.Axis['Y'] = 0;
                    }
                    Console.WriteLine($"Y: {this.Axis['Y']}");
                    break;
                case Keys.Z:
                    if (this.Axis['Z'] < 2)
                    {
                        this.Axis['Z'] += 0.1;
                    }
                    else
                    {
                        this.Axis['Z'] = 0;
                    }
                    Console.WriteLine($"Z: {this.Axis['Z']}");
                    break;
                case Keys.Left:
                    this.SleepTime -= this.SleepTime > 0 ? 100 : 0;
                    break;
                case Keys.C:
                    this.RunGame = false;
                    this.SleepTime = 0;
                    break;
                case Keys.Escape:
                    this.RunGame = false;
                    this.SleepTime = 0;
                    break;
            }
        }
        private static readonly Vector3[][] CubePolylines = new[]
        {
            new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 0, 0),
            },
            new[]
            {
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 1),
                new Vector3(1, 1, 1),
                new Vector3(0, 1, 1),
                new Vector3(0, 0, 1),
            },
            new[] { new Vector3(0, 0, 0), new Vector3(0, 0, 1), },
            new[] { new Vector3(1, 0, 0), new Vector3(1, 0, 1), },
            new[] { new Vector3(1, 1, 0), new Vector3(1, 1, 1), },
            new[] { new Vector3(0, 1, 0), new Vector3(0, 1, 1), },
        }.Select(cubePolyline =>
        {
            var transform = Matrix.Translate(-0.5, -0.5, -0.5);
            var transformed = Matrix.TransformPoints(transform, cubePolyline);
            return transformed.ToArray();
        }).ToArray();

        private void DrawGeometry()
        {
            // bigger cube
            var angle = GetDeltaTime(new TimeSpan(0, 0, 0, 5)) * Math.PI * 2;
            var matrixModel =
                Matrix.Scale(0.5) *
                Matrix.Rotate(angle, new Vector3(1, 0, 0)) *
                Matrix.Translate(1, 0, 0);

            foreach (var cubePolyline in CubePolylines)
            {
                DrawPolyline(Matrix.TransformPoints(matrixModel, cubePolyline), Space.World, Pens.White);
            }

            // smaller cube
            angle = GetDeltaTime(new TimeSpan(0, 0, 0, 1)) * Math.PI * 2;
            matrixModel =
                Matrix.Scale(0.5) *
                Matrix.Rotate(angle, new Vector3(0, 1, 0)) *
                Matrix.Translate(0, 1, 0) *
                matrixModel;

            foreach (var cubePolyline in CubePolylines)
            {
                DrawPolyline(Matrix.TransformPoints(matrixModel, cubePolyline), Space.World, Pens.Yellow);
            }
        }
        private double GetDeltaTime(TimeSpan periodDuration)
        {
            DateTime utcNow = DateTime.UtcNow;
            return GetAnimationTime(periodDuration, utcNow);
        }
        
        private void SetRandomColor(int x, int y)
        {
            // Combine boundary checks into one statement for clarity
            double t = DateTime.UtcNow.Millisecond / 1000.0;
            int pixelIdx = y * this.DrawBuffer.Stride + x * 4;
            // int y = pixelIdx / this.DrawBuffer.Width;
            // int x = pixelIdx - (y * this.DrawBuffer.Width);
            // this.DrawBuffer.DrawBufferBytesArray[pixelIdx] = this.GetColor(x, y).ToArgb();
            this.DrawBuffer.DrawBufferBytesArray[pixelIdx] = (byte)(Math.Sin(t * Math.PI) * byte.MaxValue); // Blue
            this.DrawBuffer.DrawBufferBytesArray[pixelIdx + 1] = (byte)((double)y / this.DrawBuffer.Height * byte.MaxValue); // Green
            this.DrawBuffer.DrawBufferBytesArray[pixelIdx + 2] = (byte)((double)x / this.DrawBuffer.Width * byte.MaxValue); // Red
            this.DrawBuffer.DrawBufferBytesArray[pixelIdx + 3] = byte.MaxValue; // Transparency
        }
    }
}
