namespace SoftRenderer.Utility.Util
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Class to represent data use for the drawBuffer.
    /// TODO!!!
    /// </summary>
    public class DrawBuffer
    {
        public DrawBuffer(byte[] drawBufferBytesArray, Bitmap drawBuffer, Size size)
        {
            this.ClientRectangle = new Rectangle(Point.Empty, size);
            this.DrawBufferSize = size;
            this.DrawBufferBytesArray = drawBufferBytesArray;
            this.DrawBufferBitMap = drawBuffer;
        }

        /// <summary>
        /// Gets size of the buffer where rendering will take place.
        /// </summary>
        public Size DrawBufferSize { get; set; }

        /// <summary>
        /// Gets or sets bitMap where rendering occurs.
        /// </summary>
        public Bitmap DrawBufferBitMap { get; set; }

        /// <summary>
        /// Gets graphics Surface of the DrawBuffer.
        /// </summary>
        public Graphics DrawGraphics { get; set; }

        /// <summary>
        /// Gets or sets rectangle of the client form.
        /// </summary>
        public Rectangle ClientRectangle { get; set; }

        /// <summary>
        /// Gets or sets byte array that is manipulated and translated to the draw buffer.
        /// </summary>
        public byte[] DrawBufferBytesArray { get; set; }

        public int DrawBufferByteArraySize { get; set; }

        /// <summary>
        /// Move data in bytes array to draw buffer.
        /// </summary>
        public void MoveToDrawBuffer()
        {
            BitmapData drawBufferData = this.DrawBufferBitMap.LockBits(this.ClientRectangle, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            IntPtr drawBufferPtr = drawBufferData.Scan0;
            Marshal.Copy(this.DrawBufferBytesArray, 0, drawBufferPtr, this.DrawBufferByteArraySize);
            this.DrawBufferBitMap.UnlockBits(drawBufferData);
        }

        /// <summary>
        /// Create bytes array that is later translated to the bitmaps.
        /// Also, Save size of array for easier reference.
        /// </summary>
        /// <param name="size">size of the draw buffer.</param>
        /// <returns>byte array.</returns>
        public byte[] CreateDrawBufferBytesArray()
        {
            int bitsPerPixel = Image.GetPixelFormatSize(PixelFormat.Format24bppRgb);
            int widthInBits = this.DrawBufferSize.Width * bitsPerPixel;
            int stride = ((widthInBits + 31) / 32) * 4;
            this.DrawBufferByteArraySize = Math.Abs(stride) * this.DrawBufferSize.Height;
            return new byte[this.DrawBufferByteArraySize];
        }
    }
}
