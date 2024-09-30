//-----------------------------------------------------------------------
// <copyright file="Rasterizer.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SoftRenderer.Engine.Render.Technique.Rasterizer
{
    using System;
    using System.Drawing;

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
        }

        /// <inheritdoc/>
        public override void RenderInternal()
        {
            Graphics graphics = this.DrawGraphics;
            int stride = this.DrawBufferByteArraySize / this.DrawBufferSize.Height;
            var rand = new Random();

            // according to the 24bpp format, each pixel is 24 bit so we step by 3.
            for (int x = 0; x < stride; x += 3)
            {
                for (int y = 0; y < this.DrawBufferSize.Height; y++)
                {
                    this.SetRandomColor(x, y, rand);
                }
            }

            this.MoveToDrawBuffer();

            graphics.DrawString(this.RendererFps.ToString(), this.FpsFont, Brushes.Yellow, 0, 0);
            graphics.DrawString($"ViewPort: {this.ViewportSize.Width}, {this.ViewportSize.Height}", this.FpsFont, Brushes.Aquamarine, 0, 20);
            graphics.DrawString($"DrawBuffer: {this.DrawBufferSize.Width}, {this.DrawBufferSize.Height}", this.FpsFont, Brushes.MediumSlateBlue, 0, 40);

            // Draw bitmap to buffer
            this.ViewPortBuffer.Graphics.DrawImage(this.DrawBuffer, new RectangleF(Point.Empty, this.ViewportSize), new RectangleF(Point.Empty, this.DrawBufferSize), GraphicsUnit.Pixel);
            this.ViewPortBuffer.Render(this.ViewPortBufferHandle);
        }

        private void SetRandomColor(int x, int y, Random rnd)
        {
            this.DrawBufferBytesArray[(x * this.DrawBufferSize.Height) + y] = (byte)rnd.Next(0, 255);
            this.DrawBufferBytesArray[((x + 1) * this.DrawBufferSize.Height) + y] = (byte)rnd.Next(0, 255);
            this.DrawBufferBytesArray[((x + 2) * this.DrawBufferSize.Height) + y] = (byte)rnd.Next(0, 255);
        }
    }
}
