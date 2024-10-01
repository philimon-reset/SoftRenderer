//-----------------------------------------------------------------------
// <copyright file="Rasterizer.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SoftRenderer.Engine.Render.Technique.Rasterizer
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// Rasterizer Render Port.
    /// </summary>
    public class Rasterizer : Render.Driver.GDI.RenderBaseGdi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rasterizer"/> class.
        /// Rasterization Render constructor.
        /// </summary>
        /// <param name="renderBaseArgs">dto that hold form handles.</param>
        public Rasterizer(IRenderBaseArgs renderBaseArgs)
            : base(renderBaseArgs)
        {
            // this.HostInput.MouseMove += this.CaptureCoordinates;
            this.Stride = this.DrawBufferByteArraySize / this.DrawBufferSize.Height;
        }
        
        private int Stride { get; set; }

        private void CaptureCoordinates(object sender, MouseEventArgs e)
        {
            int pixelIdx = (e.Y * this.Stride) + e.X;
            Console.WriteLine($"{e.X} {e.Y}");
            int B = this.DrawBufferBytesArray[pixelIdx];
            int G = this.DrawBufferBytesArray[pixelIdx + 1];
            int R = this.DrawBufferBytesArray[pixelIdx + 2];
            Console.WriteLine($"{R} {G} {B}");
        }

        /// <inheritdoc/>
        public override void RenderInternal()
        {
            Array.Clear(this.DrawBufferBytesArray, 0, this.DrawBufferByteArraySize);
            this.MoveToDrawBuffer();
            int bitsPerPixel = Image.GetPixelFormatSize(this.DrawBufferPixelFormat) / 8;
            Parallel.For(0, this.Stride / 4, (x) =>
            {
                int rX = x * 4;
                for (int y = 0; y < this.DrawBufferSize.Height; y++)
                {
                    this.SetRandomColor(rX, y);
                }
            });

            this.MoveToDrawBuffer();

            // Graphics graphics = this.DrawGraphics;
            // graphics.DrawString(this.RendererFps.ToString(), this.FpsFont, Brushes.Yellow, 0, 0);
            // graphics.DrawString($"ViewPort: {this.ViewportSize.Width}, {this.ViewportSize.Height}", this.FpsFont, Brushes.MediumSlateBlue, 0, 20);
            // graphics.DrawString($"DrawBuffer: {this.DrawBufferSize.Width}, {this.DrawBufferSize.Height}", this.FpsFont, Brushes.MediumSlateBlue, 0, 40);

            // Draw bitmap to buffer
            this.ViewPortBuffer.Graphics.DrawImage(this.DrawBuffer, new RectangleF(Point.Empty, this.ViewportSize), new RectangleF(Point.Empty, this.DrawBufferSize), GraphicsUnit.Pixel);
            this.ViewPortBuffer.Render(this.ViewPortBufferHandle);
        }

        public override void Dispose()
        {
            base.Dispose();
            // this.HostInput.MouseMove -= this.CaptureCoordinates;
        }

        private void SetRandomColor(int x, int y)
        {
            int pixelIdx = (y * this.Stride) + x;
            double t = DateTime.UtcNow.Millisecond / 1000.0;
            this.DrawBufferBytesArray[pixelIdx] = (byte)(Math.Sin(t * Math.PI) * byte.MaxValue); // Blue
            this.DrawBufferBytesArray[pixelIdx + 1] = (byte)((double)y / this.DrawBufferSize.Height * byte.MaxValue); // Green
            this.DrawBufferBytesArray[pixelIdx + 2] = (byte)((double)x / this.Stride * byte.MaxValue); // Red
            this.DrawBufferBytesArray[pixelIdx + 3] = byte.MaxValue; // Transparency
            // this.DrawBufferBytesArray[pixelIdx] = (byte)(Math.Sin(t * Math.PI) * byte.MaxValue); // Blue
            // this.DrawBufferBytesArray[pixelIdx + 1] = 0; // Green
            // this.DrawBufferBytesArray[pixelIdx + 2] = 0; // Red
            // this.DrawBufferBytesArray[pixelIdx + 3] = byte.MaxValue; // Transparency
        }
    }
}
