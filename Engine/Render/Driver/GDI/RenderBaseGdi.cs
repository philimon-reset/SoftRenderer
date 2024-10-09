// <copyright file="RenderBaseGdi.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>

namespace SoftRenderer.Engine.Render.Driver.GDI
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using Buffers;
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

            // Create DrawBuffer
            this.DrawBuffer = new DrawBuffer(this.DrawBufferSize);

            // Double buffer for rendering setup.
            // Note: Buffer is the whole screen.
            this.GraphicsHandle = Graphics.FromHwndInternal(this.HostHandle);
            this.ClientViewBufferHandle = this.GraphicsHandle.GetHdc();
            this.CreateClientBuffer(this.ClientBufferSize);

            // // Buffer size
            // this.CreateDrawBuffer(this.DrawBufferSize);
        }

        /// <summary>
        /// Gets or sets graphics handle for client buffer.
        /// </summary>
        private Graphics GraphicsHandle { get; set; }

        /// <summary>
        /// Gets handle for view port buffer.
        /// </summary>
        protected IntPtr ClientViewBufferHandle { get; }

        /// <summary>
        /// Gets or sets draw Buffer instance to work with bitmap.
        /// </summary>
        protected DrawBuffer DrawBuffer { get; set; }

        /// <summary>
        /// Gets font shown for fps counter.
        /// </summary>
        protected Font FpsFont { get; private set; }

        /// <summary>
        /// Gets double buffer handle.
        /// </summary>
        protected BufferedGraphics ClientBuffer { get; private set; }

        /// <summary>
        /// Gets or sets rectangle of the client form.
        /// </summary>
        private Rectangle ClientBufferRectangle { get; set; }

        /// <inheritdoc/>
        public override void Dispose()
        {
            base.Dispose();
            this.DestroyDrawBuffer();
            this.DestroyClientBuffer();

            this.RendererFps.Dispose();
            this.FpsFont.Dispose();
            this.GraphicsHandle.Dispose();

            this.RendererFps = default;
            this.FpsFont = default;
            this.GraphicsHandle = default;
        }

        /// <inheritdoc/>
        protected override void ResizeBuffer(Size argsNewSize)
        {
            base.ResizeBuffer(argsNewSize);
            this.DestroyDrawBuffer();
            this.CreateDrawBuffer(argsNewSize);
        }

        /// <inheritdoc/>
        protected override void ResizeClientBuffer(Size argsNewSize)
        {
            base.ResizeClientBuffer(argsNewSize);
            this.DestroyClientBuffer();
            this.CreateClientBuffer(argsNewSize);
        }

        /// <summary>
        /// Dispose of ClientBuffer.
        /// </summary>
        private void DestroyClientBuffer()
        {
            this.ClientBuffer.Dispose();
            this.ClientBuffer = default;
        }

        /// <summary>
        /// Allocate new view port buffer.
        /// </summary>
        /// <param name="size">size of ClientView.</param>
        private void CreateClientBuffer(Size size)
        {
            this.ClientBufferRectangle = new Rectangle(Point.Empty, size);
            this.ClientBuffer = BufferedGraphicsManager.Current.Allocate(this.ClientViewBufferHandle, this.ClientBufferRectangle);
            this.ClientBuffer.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        /// <summary>
        /// Dispose of bitmap(draw buffer).
        /// </summary>
        private void DestroyDrawBuffer()
        {
            this.DrawBuffer.Dispose();
            this.DrawBuffer = default;
        }

        /// <summary>
        /// Allocate new draw buffer (bitmap) and graphics surface for draw buffer.
        /// </summary>
        /// <param name="size">size of draw buffer.</param>
        private void CreateDrawBuffer(Size size)
        {
            this.DrawBuffer = new DrawBuffer(size);
        }
    }
}
