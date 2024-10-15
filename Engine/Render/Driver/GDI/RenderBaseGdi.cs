// <copyright file="RenderBaseGdi.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>

namespace SoftRenderer.Engine.Render.Driver.GDI
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
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
            this.CreateClientBufferGraphics();
        }

        /// <summary>
        /// Gets handle for view port buffer.
        /// </summary>
        protected IntPtr ClientViewBufferHandle { get; private set; }

        /// <summary>
        /// Gets font shown for fps counter.
        /// </summary>
        protected Font FpsFont { get; private set; }

        /// <summary>
        /// Gets double buffer handle.
        /// </summary>
        protected BufferedGraphics ClientBufferedGraphics { get; private set; }

        /// <summary>
        /// Gets or sets graphics handle for client buffer.
        /// </summary>
        private Graphics GraphicsHandle { get; set; }


        /// <inheritdoc/>
        public override void Dispose()
        {
            base.Dispose();
            this.DestroyClientBufferGraphics();

            this.RendererFps.Dispose();
            this.FpsFont.Dispose();
            this.GraphicsHandle.Dispose();

            this.RendererFps = default;
            this.FpsFont = default;
            this.GraphicsHandle = default;
        }

        public override void Render()
        {
            base.Render();
            var graphics = this.ClientBufferedGraphics.Graphics;
            Point clientBufferPoint = new Point(this.ClientBuffer.X, this.ClientBuffer.Y);
            graphics.DrawString(this.RendererFps.ToString(), this.FpsFont, Brushes.Yellow, clientBufferPoint);
            graphics.DrawString($"FormSize: {this.FormSize.Width}, {this.FormSize.Height}", this.FpsFont, Brushes.MediumSlateBlue, clientBufferPoint.X, clientBufferPoint.Y + 20);
            graphics.DrawString($"ClientView: {this.ClientBuffer.Width}, {this.ClientBuffer.Height}", this.FpsFont, Brushes.MediumSlateBlue, clientBufferPoint.X, clientBufferPoint.Y + 40);
            graphics.DrawString($"DrawBuffer: {this.DrawBuffer.Width}, {this.DrawBuffer.Height}", this.FpsFont, Brushes.MediumSlateBlue, clientBufferPoint.X, clientBufferPoint.Y + 60);

            // Draw bitmap to buffer
            this.ClientBufferedGraphics.Render(this.ClientViewBufferHandle);
        }

        /// <inheritdoc/>
        protected override void ResizeClientBuffer(Size argsNewSize)
        {
            base.ResizeClientBuffer(argsNewSize);
            this.DestroyClientBufferGraphics();
            this.CreateClientBufferGraphics();
        }

        /// <summary>
        /// Dispose of ClientBuffer.
        /// </summary>
        private void DestroyClientBufferGraphics()
        {
            this.ClientBufferedGraphics.Dispose();
            this.ClientBufferedGraphics = default;
        }

        /// <summary>
        /// Allocate new client view graphics handle.
        /// </summary>
        private void CreateClientBufferGraphics()
        {
            this.GraphicsHandle = Graphics.FromHwndInternal(this.HostHandle);
            this.GraphicsHandle.Clear(Color.Black);
            this.ClientViewBufferHandle = this.GraphicsHandle.GetHdc();
            this.ClientBufferedGraphics = BufferedGraphicsManager.Current.Allocate(this.ClientViewBufferHandle, this.ClientBuffer.ClientRectangle);
            this.ClientBufferedGraphics.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }
    }
}
