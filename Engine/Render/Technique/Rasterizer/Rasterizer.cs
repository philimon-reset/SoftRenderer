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

        /// <summary>
        ///     Initializes a new instance of the <see cref="Rasterizer" /> class.
        ///     Rasterization Render constructor.
        /// </summary>
        /// <param name="renderBaseArgs">dto that hold form handles.</param>
        public Rasterizer(IRenderBaseArgs renderBaseArgs)
            : base(renderBaseArgs)
        {
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
                graphics.Clear(Color.Gray);
                if (this.SleepTime > 0)
                {
                    Thread.Sleep(this.SleepTime);
                }
                var periodDuration = new TimeSpan(0, 0, 0, 5, 0);
                DateTime utcNow = DateTime.UtcNow;
                double t = GetAnimationTime(periodDuration, utcNow);
                var sinT = Math.Sin(t * Math.PI * 2);
                // asynchronous safe render (byte buffer)
                TestTransformations();


                // this.DrawBuffer.MoveToDrawBuffer();

                #region commented

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
                graphics.Clear(Color.Gray);
            }

            // Draw bitmap to buffer
            this.ClientBufferedGraphics.Graphics.DrawImage(this.DrawBuffer.BitMap, new RectangleF(Point.Empty, this.ClientBuffer.Size),
                new RectangleF(new PointF(-1F, -1F), this.DrawBuffer.Size), GraphicsUnit.Pixel);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();
            this.HostInput.KeyDown -= this.HandleKeyPress;
        }

        private void DrawPolyline(IEnumerable<Vector3> points, Space space, Pen pen)
        {
            Matrix clientView = Matrix.ClientViewTransform(this.ClientBuffer);
            switch (space)
            {
                case Space.World:
                    throw new NotSupportedException();

                case Space.View:
                    var viewPoints = points.Select(e => clientView * e);
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
            var transformPoint1 = pointsArrowScreen.Select(e => transform1 * e);
            var transformPoint2 = pointsArrowView.Select(e => transform2 * e);
            this.DrawPolyline(transformPoint1, Space.Screen, Pens.White);
            this.DrawPolyline(transformPoint2, Space.View, Pens.Black);
            
            // scale
            Matrix transform3 =  Matrix.Translate(150, 100, 0) * Matrix.Scale(t * 2, t * 2, 1);
            Matrix transform4 =  Matrix.Translate(-0.6, 0, 0) * Matrix.Scale(t * 2, t * 2, 1);
            transformPoint1 = pointsArrowScreen.Select(e => transform3 * e);
            transformPoint2 = pointsArrowView.Select(e => transform4 * e);
            this.DrawPolyline(transformPoint1, Space.Screen, Pens.White);
            this.DrawPolyline(transformPoint2, Space.View, Pens.Black);

            // // rotate
            Matrix transform5 = Matrix.Translate(300, 100, 0) * Matrix.Rotate(t * 360, new Vector3(0, 0, 1));
            Matrix transform6 = Matrix.Translate(-0.2, 0, 0) * Matrix.Rotate(t * 360, new Vector3(0, 0, 1));
            transformPoint1 = pointsArrowScreen.Select(e => transform5 * e);
            transformPoint2 = pointsArrowView.Select(e => transform6 * e);
            this.DrawPolyline(transformPoint1, Space.Screen, Pens.White);
            this.DrawPolyline(transformPoint2, Space.View, Pens.Black);

            // rotate * translate
            Matrix transform7 = Matrix.Translate(400, 100, 0) * Matrix.Translate(0, sinT * 40, 0) * Matrix.Rotate(t * 360, new Vector3(0, 0, 1));
            Matrix transform8 = Matrix.Translate(0, 0, 0) * Matrix.Translate(0, sinT * 0.2, 0) * Matrix.Rotate(t * 360, new Vector3(0, 0, 1));
            transformPoint1 = pointsArrowScreen.Select(e => transform7 * e);
            transformPoint2 = pointsArrowView.Select(e => transform8 * e);

            this.DrawPolyline(transformPoint1, Space.Screen, Pens.White);
            this.DrawPolyline(transformPoint2, Space.View, Pens.Black);

            // translate * rotate
            Matrix transform9 = Matrix.Translate(500, 100, 0) * Matrix.Rotate(t * 360, new Vector3(0, 0, 1)) * Matrix.Translate(0, sinT * 40, 0);
            Matrix transform10 = Matrix.Translate(0.4, 0, 0) * Matrix.Rotate(t * 360, new Vector3(0, 0, 1)) * Matrix.Translate(0, sinT * 0.2, 0);
            transformPoint1 = pointsArrowScreen.Select(e => transform9 * e);
            transformPoint2 = pointsArrowView.Select(e => transform10 * e);

            this.DrawPolyline(transformPoint1, Space.Screen, Pens.White);
            this.DrawPolyline(transformPoint2, Space.View, Pens.Black);
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

        private void SetRandomColorIdx(int pixelIdx)
        {
            // Combine boundary checks into one statement for clarity
            double t = DateTime.UtcNow.Millisecond / 1000.0;
            int y = pixelIdx / this.DrawBuffer.Width;
            int x = pixelIdx - y * this.DrawBuffer.Width;
            // this.DrawBuffer.DrawBufferBytesArray[pixelIdx] = this.GetColor(x, y).ToArgb();
            this.DrawBuffer.DrawBufferBytesArray[pixelIdx] = (byte)(Math.Sin(t * Math.PI) * byte.MaxValue); // Blue
            this.DrawBuffer.DrawBufferBytesArray[pixelIdx + 1] = (byte)((double)y / this.DrawBuffer.Height * byte.MaxValue); // Green
            this.DrawBuffer.DrawBufferBytesArray[pixelIdx + 2] = (byte)((double)x / this.DrawBuffer.Width * byte.MaxValue); // Red
            this.DrawBuffer.DrawBufferBytesArray[pixelIdx + 3] = byte.MaxValue; // Transparency
        }

        private Color GetColor(int x, int y)
        {
            double t = DateTime.UtcNow.Millisecond / 1000.0;
            return Color.FromArgb(
                byte.MaxValue,
                (byte)((double)x / this.DrawBuffer.Width * byte.MaxValue),
                (byte)((double)y / this.DrawBuffer.Height * byte.MaxValue),
                (byte)(Math.Sin(t * Math.PI) * byte.MaxValue));
        }

        private void SetLine(Graphics graphics, Pen pen, Point[] points)
        {
            graphics.DrawLines(pen, points);
        }
    }
}
