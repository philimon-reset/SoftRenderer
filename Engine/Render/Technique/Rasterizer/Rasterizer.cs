//-----------------------------------------------------------------------
// <copyright file="Rasterizer.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SoftRenderer.Engine.Render.Technique.Rasterizer
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// Rasterizer Render Port.
    /// </summary>
    public class Rasterizer : Driver.GDI.RenderBaseGdi
    {
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
            this.HostInput.KeyDown += this.HandleKeyPress;
            this.HostInput.MousePress += this.HandleMousePress;
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

                Parallel.For(0, this.DrawBuffer.DrawBufferBytesArray.Length, this.SetRandomColor);
            }
            else
            {
                graphics.Clear(Color.Black);
            }

            var pen = new Pen(Color.Bisque, 2);
            Point[] points = [new Point(100, 200), new Point(200, 200), new Point(100, 400), new Point(100, 200)];

            this.SetLine(graphics, pen, points);

            graphics.DrawString(this.RendererFps.ToString(), this.FpsFont, Brushes.Yellow, 0, 0);
            graphics.DrawString($"Running: {this.RunGame}", this.FpsFont, Brushes.MediumSlateBlue, 0, 20);
            graphics.DrawString($"ClientView: {this.ClientBufferSize.Width}, {this.ClientBufferSize.Height}", this.FpsFont, Brushes.MediumSlateBlue, 0, 40);
            graphics.DrawString($"DrawBuffer: {this.DrawBufferSize.Width}, {this.DrawBufferSize.Height}", this.FpsFont, Brushes.MediumSlateBlue, 0, 60);

            // Draw bitmap to buffer
            this.ClientBuffer.Graphics.DrawImage(this.DrawBuffer.BitMap, new RectangleF(Point.Empty, this.ClientBufferSize), new RectangleF(new PointF(-1F, -1F), this.DrawBufferSize), GraphicsUnit.Pixel);
            this.ClientBuffer.Render(this.ClientViewBufferHandle);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();
            this.HostInput.KeyDown -= this.HandleKeyPress;
            this.HostInput.MousePress -= this.HandleMousePress;
        }

        private void HandleMousePress(object sender, MouseEventArgs e)
        {
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

        private void SetRandomColor(int pixelIdx)
        {
            int y = pixelIdx / this.DrawBufferSize.Width;
            int x = pixelIdx - (y * this.DrawBufferSize.Width);
            this.DrawBuffer.DrawBufferBytesArray[pixelIdx] = this.GetColor(x, y).ToArgb();

            // this.DrawBufferBytesArray[pixelIdx] = (byte)(Math.Sin(t * Math.PI) * byte.MaxValue); // Blue
            // this.DrawBufferBytesArray[pixelIdx + 1] = (byte)((double)y / this.DrawBufferSize.Height * byte.MaxValue); // Green
            // this.DrawBufferBytesArray[pixelIdx + 2] = (byte)((double)x / this.DrawBufferSize.Width * byte.MaxValue); // Red
            // this.DrawBufferBytesArray[pixelIdx + 3] = byte.MaxValue; // Transparency
        }

        private Color GetColor(int x, int y)
        {
            double t = DateTime.UtcNow.Millisecond / 1000.0;
            return Color.FromArgb(
                byte.MaxValue,
                (byte)((double)x / this.DrawBufferSize.Width * byte.MaxValue),
                (byte)((double)y / this.DrawBufferSize.Height * byte.MaxValue),
                (byte)(Math.Sin(t * Math.PI) * byte.MaxValue));
        }

        private void SetLine(Graphics graphics, Pen pen, Point[] points)
        {
            graphics.DrawLines(pen, points);
        }
    }
}
