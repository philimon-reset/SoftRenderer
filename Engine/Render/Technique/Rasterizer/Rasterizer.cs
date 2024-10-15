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
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Windows.Media.Media3D;
    using Math;

    /// <summary>
    /// Rasterizer Render Port.
    /// </summary>
    public class Rasterizer : Driver.GDI.RenderBaseGdi
    {
        private Dictionary<char, int> Axis;
        /// <summary>
        /// Initializes a new instance of the <see cref="Rasterizer"/> class.
        /// Rasterization Render constructor.
        /// </summary>
        /// <param name="renderBaseArgs">dto that hold form handles.</param>
        public Rasterizer(IRenderBaseArgs renderBaseArgs)
            : base(renderBaseArgs)
        {
            this.RunGame = false;
            this.SleepTime = 0;
            this.Axis = new Dictionary<char, int>() { {'X', 1}, {'Y', 0}, {'Z', 0}};
            this.HostInput.KeyDown += this.HandleKeyPress;
        }

        private bool RunGame { get; set; }

        private int SleepTime { get; set; }

        /// <inheritdoc/>
        public override void RenderInternal()
        {
            Graphics graphics = this.DrawBuffer.Graphics;
            if (this.RunGame)
            {
                if (this.SleepTime > 0)
                {
                    Thread.Sleep(this.SleepTime);
                }

                // asynchronous safe render (byte buffer)
                Parallel.For(0, this.DrawBuffer.Width, (x) =>
                {
                    for (int y = 0; y < this.DrawBuffer.Height; y++)
                    {
                        this.SetRandomColor(x, y);
                    }
                });

                this.DrawBuffer.MoveToDrawBuffer();

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
                graphics.Clear(Color.Black);
            }

            var pen = new Pen(Color.Bisque, 2);
            Point clientBufferOffset = new Point(this.ClientBuffer.X, this.ClientBuffer.Y);
            var Scaled = ((float, float, float) vector) =>
            {
                return Vector3.ToScreenSpaceScaled(new Vector3(vector.Item1, vector.Item2, vector.Item3), this.ClientBuffer.Size, clientBufferOffset);
            };
            Point[] pointsScaled =
            [
                Scaled((-0.3F, 0.3F, 0)),
                Scaled((0.3F, 0.3F, 0)),
                Scaled((0.3F, -0.3F, 0)),
                Scaled((-0.3F, -0.3F, 0)),
                Scaled((-0.3F, 0.3F, 0)),
            ];
            PointF[] points =
            [
                new PointF(-0.4F, 0.4F),
                new PointF(0.4F, 0.4F),
                new PointF(0.4F, -0.4F),
                new PointF(-0.4F, -0.4F),
                new PointF(-0.4F, 0.4F),
            ];
            double t = DateTime.UtcNow.Millisecond / 1000.0;
            Matrix rotMat = Matrix.Rotate(t * 360, new Vector3(this.Axis['X'], this.Axis['Y'], this.Axis['Z']));
            Point[] newpoints = points.Select((e) =>
            {
                PointF pointF= (rotMat * (Vector4)e);
                return (Point)Scaled((pointF.X, pointF.Y, 0));
            }).ToArray();
            this.SetLine(graphics, pen, newpoints);


            // for (int i = 0; i < 360; i += 10)
            // {
            //     Matrix rotMat = Matrix.Rotate(i, new Vector3(1, 0, 0));
            //     pointsScaled = pointsScaled.Select((e) =>
            //     {
            //         Point result = (Vector3)e * rotMat;
            //         return result;
            //     }).ToArray();
            // }

            // Draw bitmap to buffer
            Point clientBufferPoint = new Point(this.ClientBuffer.X, this.ClientBuffer.Y);
            graphics.DrawString($"X: {this.Axis['X']}", this.FpsFont, Brushes.Yellow, new PointF(clientBufferPoint.X + (this.ClientBuffer.Width / 2), clientBufferPoint.Y));
            graphics.DrawString($"Y: {this.Axis['Y']}", this.FpsFont, Brushes.Yellow, new PointF(clientBufferPoint.X + (this.ClientBuffer.Width / 2), clientBufferPoint.Y + 20));
            graphics.DrawString($"Z: {this.Axis['Z']}", this.FpsFont, Brushes.Yellow, new PointF(clientBufferPoint.X + (this.ClientBuffer.Width / 2), clientBufferPoint.Y + 40));
            this.ClientBufferedGraphics.Graphics.DrawImage(this.DrawBuffer.BitMap, new RectangleF(Point.Empty, this.ClientBuffer.Size), new RectangleF(new PointF(-1F, -1F), this.DrawBuffer.Size), GraphicsUnit.Pixel);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();
            this.HostInput.KeyDown -= this.HandleKeyPress;
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
                case Keys.X:
                    this.Axis['X'] = this.Axis['X'] == 1 ? 0 : 1;
                    break;
                case Keys.Y:
                    this.Axis['Y'] = this.Axis['Y'] == 1 ? 0 : 1;
                    break;
                case Keys.Z:
                    this.Axis['Z'] = this.Axis['Z'] == 1 ? 0 : 1;
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
            int pixelIdx = (y * this.DrawBuffer.Stride) + (x * 4);
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
            int x = pixelIdx - (y * this.DrawBuffer.Width);
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
