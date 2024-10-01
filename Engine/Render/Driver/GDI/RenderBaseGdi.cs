// <copyright file="RenderBaseGdi.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>

namespace SoftRenderer.Engine.Render.Driver.GDI
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using SoftRenderer.Client.FPSCounter;
    using SoftRenderer.Utility.Util;

    /// <summary>
    /// RenderBase abstraction for gdi driver.
    /// </summary>
    public abstract class RenderBaseGdi : Render.RenderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderBaseGdi"/> class.
        /// Initialize base GDI class.
        /// </summary>
        /// <param name="renderBaseArgs">dto that hold form handles.</param>
        /// <returns> RenderBaseGdi instance.</returns>
        protected RenderBaseGdi(IRenderBaseArgs renderBaseArgs)
    : base(renderBaseArgs)
        {
            // FPS counter.
            this.RendererFps = new FPSCounter(TimeSpan.FromSeconds(0.5));
            this.FpsFont = new Font("Arial", 12);

            // Form control of the form.
            this.FormControl ??= Util.GetForm(this.HostHandle);
            this.DrawBufferPixelFormat = PixelFormat.Format32bppArgb;

            // Double buffer for rendering setup.
            // Note: Buffer is the whole screen.
            this.GraphicsHandle = Graphics.FromHwndInternal(this.HostHandle);
            this.ViewPortBufferHandle = this.GraphicsHandle.GetHdc();
            this.CreateViewPort(this.ViewportSize);

            // Buffer size
            this.CreateDrawBuffer(this.DrawBufferSize);
        }

        /// <summary>
        /// Gets handle for view port buffer.
        /// </summary>
        protected IntPtr ViewPortBufferHandle { get; }

        /// <summary>
        /// Gets font shown for fps counter.
        /// </summary>
        protected Font FpsFont { get; private set; }

        /// <summary>
        /// Gets bitMap where rendering occurs.
        /// </summary>
        protected Bitmap DrawBuffer { get; private set; }

        /// <summary>
        /// Gets graphics Surface of the DrawBuffer.
        /// </summary>
        protected Graphics DrawGraphics { get; private set; }

        /// <summary>
        /// Gets double buffer handle.
        /// </summary>
        protected BufferedGraphics ViewPortBuffer { get; private set; }


        /// <summary>
        /// Gets or sets byte array that is manipulated and translated to the draw buffer.
        /// </summary>
        protected byte[] DrawBufferBytesArray { get; set; }

        /// <summary>
        /// Gets or sets size of the drawBuffer array.
        /// </summary>
        protected int DrawBufferByteArraySize { get; set; }

        /// <summary>
        /// Gets pixel Format of the draw buffer.
        /// </summary>
        protected PixelFormat DrawBufferPixelFormat { get; }

        /// <summary>
        /// Gets or sets graphics instance handle.
        /// </summary>
        private Graphics GraphicsHandle { get; set; }

        /// <summary>
        /// Gets or sets rectangle of the client form.
        /// </summary>
        private Rectangle ClientRectangle { get; set; }

        /// <summary>
        /// Gets or sets rectangle of the draw buffer.
        /// </summary>
        private Rectangle DrawBufferRectangle { get; set; }

        /// <inheritdoc/>
        public override void Dispose()
        {
            base.Dispose();
            this.DestroyViewPort();
            this.DestroyDrawBuffer();
            this.GraphicsHandle.Dispose();
            this.RendererFps.Dispose();
            this.FpsFont.Dispose();
            this.FpsFont = default;
            this.GraphicsHandle = default;
            this.RendererFps = default;
        }

        /// <inheritdoc/>
        protected override void ResizeBuffer(Size argsNewSize)
        {
            base.ResizeBuffer(argsNewSize);
            this.DestroyDrawBuffer();
            this.CreateDrawBuffer(argsNewSize);
        }

        /// <inheritdoc/>
        protected override void ResizeViewPort(Size argsNewSize)
        {
            base.ResizeViewPort(argsNewSize);
            this.DestroyViewPort();
            this.CreateViewPort(argsNewSize);
        }

        /// <summary>
        /// Move data in bytes array to draw buffer.
        /// </summary>
        protected void MoveToDrawBuffer()
        {
            BitmapData drawBufferData = this.DrawBuffer.LockBits(this.DrawBufferRectangle, ImageLockMode.WriteOnly, this.DrawBufferPixelFormat);
            IntPtr drawBufferPtr = drawBufferData.Scan0;
            Marshal.Copy(this.DrawBufferBytesArray, 0, drawBufferPtr, this.DrawBufferByteArraySize);
            this.DrawBuffer.UnlockBits(drawBufferData);
            Array.Clear(this.DrawBufferBytesArray, 0, this.DrawBufferByteArraySize);
        }

        /// <summary>
        /// Create bytes array that is later translated to the bitmaps.
        /// Also, Save size of array for easier reference.
        /// </summary>
        /// <param name="size">size of the draw buffer.</param>
        /// <returns>byte array.</returns>
        private byte[] CreateDrawBufferBytesArray(Size size)
        {
            int bitsPerPixel = Image.GetPixelFormatSize(this.DrawBufferPixelFormat);
            int widthInBits = size.Width * bitsPerPixel;
            int stride = ((widthInBits + 31) / 32) * 4;
            this.DrawBufferByteArraySize = Math.Abs(stride) * size.Height;
            return new byte[this.DrawBufferByteArraySize];
        }

        /// <summary>
        /// Dispose of viewport.
        /// </summary>
        private void DestroyViewPort()
        {
            this.ViewPortBuffer.Dispose();
            this.ViewPortBuffer = default;
        }

        /// <summary>
        /// Allocate new view port buffer.
        /// </summary>
        /// <param name="size">size of viewport.</param>
        private void CreateViewPort(Size size)
        {
            this.ClientRectangle = new Rectangle(Point.Empty, size);
            this.ViewPortBuffer = BufferedGraphicsManager.Current.Allocate(this.ViewPortBufferHandle, this.ClientRectangle);
            this.ViewPortBuffer.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        /// <summary>
        /// Dispose of bitmap(draw buffer).
        /// </summary>
        private void DestroyDrawBuffer()
        {
            this.DrawGraphics.Dispose();
            this.DrawBuffer.Dispose();
            this.DrawBufferBytesArray = default;
            this.DrawGraphics = default;
            this.DrawBuffer = default;
        }

        /// <summary>
        /// Allocate new draw buffer (bitmap) and graphics surface for draw buffer.
        /// </summary>
        /// <param name="size">size of draw buffer.</param>
        private void CreateDrawBuffer(Size size)
        {
            // Initiate draw buffer and byte array we make changes to.
            this.DrawBufferRectangle = new Rectangle(Point.Empty, this.DrawBufferSize);
            this.DrawBuffer = new Bitmap(size.Width, size.Height, this.DrawBufferPixelFormat);
            this.DrawBufferBytesArray = this.CreateDrawBufferBytesArray(size);
            this.DrawGraphics = Graphics.FromImage(this.DrawBuffer);
        }

        public void Print2DArray()
        {
            int stride = this.DrawBufferByteArraySize / this.DrawBufferSize.Height;
            for (int i = 0; i < stride; i += 3)
            {
                for (int j = 0; j < this.DrawBufferSize.Height; j++)
                {
                    int pixelIdx = (j * stride) + i;
                    int B = this.DrawBufferBytesArray[pixelIdx];
                    int G = this.DrawBufferBytesArray[pixelIdx + 1];
                    int R = this.DrawBufferBytesArray[pixelIdx + 2];
                    Console.Write($"{R},{B},{G})"); }
                Console.WriteLine();
            }
            Console.WriteLine("====================");
        }
    }
}
