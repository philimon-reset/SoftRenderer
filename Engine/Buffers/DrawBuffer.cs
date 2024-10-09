namespace SoftRenderer.Engine.Buffers
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Class to represent data use for the drawBuffer.
    /// </summary>
    public class DrawBuffer : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawBuffer"/> class.
        /// </summary>
        /// <param name="size">size of draw buffer.</param>
        public DrawBuffer(Size size)
        {
            this.DrawBufferRectangle = new Rectangle(Point.Empty, size);
            this.DrawBufferBytesArray = new int[size.Width * size.Height];
            this.DrawBufferHandle = GCHandle.Alloc(this.DrawBufferBytesArray, GCHandleType.Pinned);
            this.BitMap = new Bitmap(size.Width, size.Height, size.Width * 4, PixelFormat.Format32bppArgb, this.DrawBufferHandle.AddrOfPinnedObject());
            this.Graphics = Graphics.FromImage(this.BitMap);
        }

        /// <summary>
        /// Gets bitMap where rendering occurs.
        /// </summary>
        public Bitmap BitMap { get; private set; }

        /// <summary>
        /// Gets graphics Surface of the DrawBuffer.
        /// </summary>
        public Graphics Graphics { get; private set; }

        /// <summary>
        /// Gets or sets rectangle of the client form.
        /// </summary>
        public Rectangle DrawBufferRectangle { get; set; }

        /// <summary>
        /// Gets or sets byte array that is manipulated and translated to the draw buffer.
        /// </summary>
        public int[] DrawBufferBytesArray { get; set; }

        /// <summary>
        /// Gets or sets gC handle for the draw buffer to improve performace.
        /// </summary>
        private GCHandle DrawBufferHandle { get; set;  }

        /// <summary>
        /// Dispose of draw buffer instance.
        /// </summary>
        public void Dispose()
        {
            this.DestroyDrawBuffer();
        }

        /// <summary>
        /// Dispose of bitmap(draw buffer).
        /// </summary>
        private void DestroyDrawBuffer()
        {
            this.Graphics.Dispose();
            this.Graphics = default;

            this.DrawBufferHandle.Free();
            this.DrawBufferHandle = default;

            this.BitMap.Dispose();
            this.BitMap = default;

            this.DrawBufferRectangle = default;
            this.DrawBufferBytesArray = default;
        }
    }
}
