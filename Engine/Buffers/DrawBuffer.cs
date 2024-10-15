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
            this.Size = size;
            this.Height = size.Height;
            this.Width = size.Width;
            this.DrawBufferRectangle = new Rectangle(Point.Empty, size);
            this.DrawBufferBytesArray = this.CreateDrawBufferArray(size);
            // this.DrawBufferHandle = GCHandle.Alloc(this.DrawBufferBytesArray, GCHandleType.Pinned);
            // this.DrawBufferPtr = this.DrawBufferHandle.AddrOfPinnedObject();
            // this.BitMap = new Bitmap(size.Width, size.Height, this.Stride, PixelFormat.Format32bppArgb,  this.DrawBufferPtr);
            this.BitMap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            this.Graphics = Graphics.FromImage(this.BitMap);
        }

        private byte[] CreateDrawBufferArray(Size size)
        {
            this.Stride = size.Width * 4;
            return new byte[this.Stride * size.Height];
        }

        public void MoveToDrawBuffer()
        {
            BitmapData drawBufferData = this.BitMap.LockBits(this.DrawBufferRectangle, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            IntPtr drawBufferPtr = drawBufferData.Scan0;
            Marshal.Copy(this.DrawBufferBytesArray, 0, drawBufferPtr, this.DrawBufferBytesArray.Length);
            this.BitMap.UnlockBits(drawBufferData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawBuffer"/> class.
        /// </summary>
        /// <param name="width">width.</param>
        /// <param name="height">height.</param>
        public DrawBuffer(int width, int height)
            : this(new Size(width, height))
        {
        }

        /// <summary>
        /// Gets bitMap where rendering occurs.
        /// </summary>
        public Bitmap BitMap { get; private set; }

        /// <summary>
        /// Int Ptr to Draw Buffer.
        /// </summary>
        // public IntPtr DrawBufferPtr { get; }

        public int Stride { get; private set; }

        /// <summary>
        /// Gets the height dimension.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the width dimension.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets size of draw buffer.
        /// </summary>
        public Size Size { get; }

        /// <summary>
        /// Gets graphics Surface of the DrawBuffer.
        /// </summary>
        public Graphics Graphics { get; private set; }

        /// <summary>
        /// Gets or sets rectangle of the client form.
        /// </summary>
        public Rectangle DrawBufferRectangle { get; set; }

        /// <summary>
        /// Gets byte array that is manipulated and translated to the draw buffer.
        /// </summary>
        public byte[] DrawBufferBytesArray { get; private set; }

        /// <summary>
        /// Gets or sets gC handle for the draw buffer to improve performace.
        /// </summary>
        // private GCHandle DrawBufferHandle { get; set;  }

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

            // this.DrawBufferHandle.Free();
            // this.DrawBufferHandle = default;

            this.BitMap.Dispose();
            this.BitMap = default;

            this.DrawBufferRectangle = default;
            this.DrawBufferBytesArray = default;
        }
    }
}
