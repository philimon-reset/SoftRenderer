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

    /// <summary>
    /// General Render Port.
    /// </summary>
    public class General : Render.Driver.GDI.RenderBaseGdi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="General"/> class.
        /// General Render constructor.
        /// </summary>
        /// <param name="renderBaseArgs">dto that hold form handles.</param>
        public General(IRenderBaseArgs renderBaseArgs)
            : base(renderBaseArgs)
        {
            this.Stride = this.DrawBufferByteArraySize / this.DrawBufferSize.Height;
            this.NextGenerationArray = new byte[this.DrawBufferByteArraySize];
            this.RunGame = false;
            this.SleepTime = 100;
            this.HostInput.KeyDown += this.HandleKeyPress;
            this.HostInput.MousePress += this.HandleMousePress;
        }

        private void HandleMousePress(object sender, MouseEventArgs e)
        {
            int x = (int)Math.Ceiling(e.X / (double)this.ResizeFactor) - 1;
            int y = (int)Math.Ceiling(e.Y / (double)this.ResizeFactor) - 1;
            this.SetAlive(x, y);

            // Console.WriteLine($"X: {x}, Y: {y}");
            // Console.WriteLine($"eX: {e.X}, eY: {e.Y}");
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            var graphics = this.DrawGraphics;
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.RunGame = true;
                    break;
                case Keys.Right:
                    this.SleepTime += 10;
                    break;
                case Keys.Left:
                    this.SleepTime -= this.SleepTime > 0 ? 10 : 0;
                    break;
                case Keys.C:
                    this.RunGame = false;
                    this.SleepTime = 100;
                    Array.Clear(this.DrawBufferBytesArray, 0, this.DrawBufferByteArraySize);
                    graphics.Clear(Color.Black);
                    break;
                case Keys.Escape:
                    this.RunGame = false;
                    this.SleepTime = 100;
                    break;
            }
        }

        private int Stride { get; set; }

        private bool RunGame { get; set; }

        private byte[] NextGenerationArray { get; set; }

        private int SleepTime { get; set; }

        /// <inheritdoc/>
        public override void Dispose()
        {
            base.Dispose();
            this.HostInput.KeyDown -= this.HandleKeyPress;
            this.HostInput.MousePress -= this.HandleMousePress;
        }

        /// <inheritdoc/>
        public override void RenderInternal()
        {
            // Array.Clear(this.DrawBufferBytesArray, 0, this.DrawBufferByteArraySize);
            if (this.RunGame)
            {
                if (this.SleepTime > 0)
                {
                    Thread.Sleep(this.SleepTime);
                }

                Array.Clear(this.NextGenerationArray, 0, this.NextGenerationArray.Length);
                this.NextGrid();
                Array.Clear(this.DrawBufferBytesArray, 0, this.DrawBufferByteArraySize);
                Array.Copy(this.NextGenerationArray, this.DrawBufferBytesArray, this.NextGenerationArray.Length);
            }

            this.MoveToDrawBuffer();

            // Draw bitmap to buffer
            this.ViewPortBuffer.Graphics.DrawImage(this.DrawBuffer, new RectangleF(Point.Empty, this.ViewportSize), new RectangleF(new PointF(-0.5F, -0.5F), this.DrawBufferSize), GraphicsUnit.Pixel);
            this.ViewPortBuffer.Render(this.ViewPortBufferHandle);
        }

        /// <inheritdoc/>
        protected override void ResizeBuffer(Size argsNewSize)
        {
            base.ResizeBuffer(argsNewSize);
            this.NextGenerationArray = new byte[this.DrawBufferByteArraySize];
            int bitsPerPixel = Image.GetPixelFormatSize(this.DrawBufferPixelFormat);
            int widthInBits = argsNewSize.Width * bitsPerPixel;
            this.Stride = ((widthInBits + 31) / 32) * 4;
        }

        private bool IsAlive(int x, int y)
        {
            // Combine boundary checks into one statement for clarity
            if (x < 0 || x >= this.DrawBufferSize.Width || y < 0 || y >= this.DrawBufferSize.Height)
            {
                return false;
            }

            int pixelIdx = (y * this.Stride) + (x * 4);

            // Simplify pixel comparison to one condition
            return this.DrawBufferBytesArray[pixelIdx] == 255 &&
                   this.DrawBufferBytesArray[pixelIdx + 1] == 255 &&
                   this.DrawBufferBytesArray[pixelIdx + 2] == 255 &&
                   this.DrawBufferBytesArray[pixelIdx + 3] == 255;
        }

        private void NextGrid()
        {
            for (int x = 0; x < this.DrawBufferSize.Width; x++)
            {
                for (int y = 0; y < this.DrawBufferSize.Height; y++)
                {
                    if (this.NextGeneration(x, y))
                    {
                        this.SetNextGenerationAlive(x, y);
                    }
                    else
                    {
                        this.SetNextGenerationDead(x, y);
                    }
                }
            }
        }

        private bool NextGeneration(int x, int y)
        {
            bool cellAlive = this.IsAlive(x, y);
            int aliveNeighbors = 0;
            for (int nx = x - 1; nx <= x + 1; nx++)
            {
                for (int ny = y - 1; ny <= y + 1; ny++)
                {
                    if (nx != x || ny != y)
                    {
                        aliveNeighbors += this.IsAlive(nx, ny) ? 1 : 0;
                    }
                }
            }

            if (!cellAlive)
            {
                return aliveNeighbors == 3;
            }

            switch (aliveNeighbors)
            {
                case < 2 or > 3:
                    return false;
                case 2 or 3:
                    return true;
            }
        }

        private void SetAlive(int x, int y)
        {
            // Combine boundary checks into one statement for clarity
            if (x < 0 || x >= this.Stride || y < 0 || y >= this.DrawBufferSize.Height)
            {
                return;
            }

            int pixelIdx = (y * this.Stride) + (x * 4);
            this.DrawBufferBytesArray[pixelIdx] = 255; // Blue
            this.DrawBufferBytesArray[pixelIdx + 1] = 255; // Green
            this.DrawBufferBytesArray[pixelIdx + 2] = 255; // Red
            this.DrawBufferBytesArray[pixelIdx + 3] = 255; // Transparency
        }

        private void SetNextGenerationAlive(int x, int y)
        {
            // Combine boundary checks into one statement for clarity
            if (x < 0 || x >= this.DrawBufferSize.Width || y < 0 || y >= this.DrawBufferSize.Height)
            {
                return;
            }

            int pixelIdx = (y * this.Stride) + (x * 4);
            this.NextGenerationArray[pixelIdx] = byte.MaxValue; // Blue
            this.NextGenerationArray[pixelIdx + 1] = byte.MaxValue; // Green
            this.NextGenerationArray[pixelIdx + 2] = byte.MaxValue; // Red
            this.NextGenerationArray[pixelIdx + 3] = byte.MaxValue; // Transparency
        }

        private void SetNextGenerationDead(int x, int y)
        {
            // Combine boundary checks into one statement for clarity
            if (x < 0 || x >= this.DrawBufferSize.Width || y < 0 || y >= this.DrawBufferSize.Height)
            {
                return;
            }

            int pixelIdx = (y * this.Stride) + (x * 4);
            this.NextGenerationArray[pixelIdx] = byte.MinValue; // Blue
            this.NextGenerationArray[pixelIdx + 1] = byte.MinValue; // Green
            this.NextGenerationArray[pixelIdx + 2] = byte.MinValue; // Red
            this.NextGenerationArray[pixelIdx + 3] = byte.MaxValue; // Transparency
        }

        private void SetRandomColor(int x, int y)
        {
            int pixelIdx = (y * this.Stride) + x;
            double t = DateTime.UtcNow.Millisecond / 1000.0;
            this.DrawBufferBytesArray[pixelIdx] = (byte)(Math.Cos(t * Math.PI) * byte.MaxValue); // Blue
            this.DrawBufferBytesArray[pixelIdx + 1] = (byte)((double)y / this.DrawBufferSize.Height * byte.MaxValue); // Green
            this.DrawBufferBytesArray[pixelIdx + 2] = (byte)((double)x / this.Stride * byte.MaxValue); // Red
            this.DrawBufferBytesArray[pixelIdx + 3] = byte.MaxValue; // Transparency
        }
    }
}
