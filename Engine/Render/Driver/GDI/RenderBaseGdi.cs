// <copyright file="RenderBaseGdi.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>

namespace SoftRenderer.Engine.Render.Driver.GDI
{
    using System;
    using System.Drawing;
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

            // Double buffer for rendering setup.
            // Note: Buffer is the whole screen.
            this.GraphicsHandle = Graphics.FromHwndInternal(this.HostHandle);
            this.ViewPortBufferHandle = this.GraphicsHandle.GetHdc();
            this.CreateViewPort(this.FormControl.Size);
            this.CreateDrawBuffer(this.FormControl.Size);
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
        /// Gets or sets graphics instance handle.
        /// </summary>
        private Graphics GraphicsHandle { get; set; }

        /// <summary>
        /// Gets or sets rectangle of the client form.
        /// </summary>
        private Rectangle ClientRectangle { get; set; }

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
            BitmapData drawBufferData = this.DrawBuffer.LockBits(this.ClientRectangle, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            IntPtr drawBufferPtr = drawBufferData.Scan0;
            Marshal.Copy(this.DrawBufferBytesArray, 0, drawBufferPtr, this.DrawBufferByteArraySize);
            this.DrawBuffer.UnlockBits(drawBufferData);
        }

        /// <summary>
        /// Create bytes array that is later translated to the bitmaps.
        /// Also, Save size of array for easier reference.
        /// </summary>
        /// <param name="size">size of the draw buffer.</param>
        /// <returns>byte array.</returns>
        private byte[] CreateDrawBufferBytesArray(Size size)
        {
            int bitsPerPixel = Image.GetPixelFormatSize(PixelFormat.Format24bppRgb);
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
            this.DrawBuffer = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb);
            this.DrawBufferBytesArray = this.CreateDrawBufferBytesArray(size);
            this.DrawGraphics = Graphics.FromImage(this.DrawBuffer);
        }
    }
}
