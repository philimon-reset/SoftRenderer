//-----------------------------------------------------------------------
// <copyright file="RenderBase.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SoftRenderer.Engine.Render
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using Buffers;
    using SoftRenderer.Client.FPSCounter;
    using SoftRenderer.Engine.Input;
    using SoftRenderer.Engine.Input.EventArgs;
    using SoftRenderer.Utility.Util;

    /// <summary>
    /// Represents a vector in three-dimensional space.
    /// </summary>
    /// <remarks>
    /// This class provides properties and methods to perform vector operations.
    /// </remarks>
    public abstract class RenderBase : IRenderBase
    {
        /// <summary>
        /// Factor by which we shrink view port to draw buffer.
        /// </summary>
        private const int ResizeFactor = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderBase"/> class.
        /// </summary>
        /// <param name="renderBaseArgs"> Given host handle. </param>
        protected RenderBase(IRenderBaseArgs renderBaseArgs)
        {
            // Renerbase arg values.
            this.HostHandle = renderBaseArgs.HostHandle;
            this.HostInput = renderBaseArgs.Input;

            // Form control of the form.
            this.FormControl = Util.GetForm(this.HostHandle);
            this.FormSize = this.FormControl.Size;

            // set client buffer.
            Size clientBufferSize = this.FormSize;
            this.ClientBuffer = new ClientBuffer(clientBufferSize, 0, 0);

            // Set size of draw buffer.
            this.DrawBuffer = new DrawBuffer(new Size(this.ClientBuffer.Size.Width / ResizeFactor, this.ClientBuffer.Size.Height / ResizeFactor));

            // Event Hooking.
            this.HostInput.SizeChanged += this.ScreenResize;
        }

        /// <summary>
        /// Gets Handle for the windows form.
        /// </summary>
        public IntPtr HostHandle { get; private set; }

        /// <summary>
        /// Gets input handle for form.
        /// </summary>
        protected IInput HostInput { get; private set; }

        /// <summary>
        /// Gets or sets form control.
        /// </summary>
        protected Control FormControl { get; set; }


        /// <summary>
        /// Gets size of the entire form.
        /// </summary>
        protected Size FormSize { get; private set; }

        /// <summary>
        /// Gets client Buffer struct
        /// </summary>
        protected ClientBuffer ClientBuffer { get; private set; }

        /// <summary>
        /// Gets draw Buffer instance to work with bitmap.
        /// </summary>
        protected DrawBuffer DrawBuffer { get; private set; }

        /// <summary>
        /// Gets or sets total fps data.
        /// </summary>
        protected FPSCounter RendererFps { get; set; }

        /// <summary>
        /// Gets start of the render.
        /// </summary>
        protected DateTime FrameStart { get; private set; }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            // Dispose Properties.
            this.HostInput.Dispose();
            this.DestroyDrawBuffer();

            // Set properties to default.
            this.HostHandle = default;
            this.FormSize = default;
            this.HostInput = default;
            this.ClientBuffer = default;

            // Event dispose
            this.HostInput!.SizeChanged -= this.ScreenResize;
        }

        /// <inheritdoc/>
        public virtual void Render()
        {
            this.RendererFps.StartFrame();
            this.FrameStart = DateTime.Now;
            this.RenderInternal();
            this.RendererFps.EndFrame();
        }

        /// <inheritdoc/>
        public abstract void RenderInternal();

        /// <summary>
        /// Get constant periodic animation variable change.
        /// </summary>
        /// <param name="periodDuration">Duration each frame should last.</param>
        /// <param name="utcNow">Current Datetime as seed.</param>
        /// <returns>Variable used to mimic periodic time changes.</returns>
        protected static double GetAnimationTime(TimeSpan periodDuration, DateTime utcNow)
        {
            return (((utcNow.Second * 1000) + utcNow.Millisecond) % periodDuration.TotalMilliseconds) / periodDuration.TotalMilliseconds;
        }

        /// <summary>
        /// Resize Client Buffer size.
        /// </summary>
        /// <param name="argsNewSize">New size.</param>
        protected virtual void ResizeClientBuffer(Size argsNewSize)
        {
            int x = (this.FormSize.Width - argsNewSize.Width) / 2;
            int y = (this.FormSize.Height - argsNewSize.Height) / 2;
            this.ClientBuffer = new ClientBuffer(argsNewSize, x, y);
        }

        /// <summary>
        /// Resize draw buffer size.
        /// </summary>
        /// <param name="argsNewSize">New size.</param>
        protected virtual void ResizeBuffer(Size argsNewSize)
        {
            this.DestroyDrawBuffer();
            this.DrawBuffer = new DrawBuffer(argsNewSize);
        }

        private void ScreenResize(object sender, ISizeChangeArgs args)
        {
            this.FormSize = args.NewSize;
            Size size = args.NewSize;
            if (size.Width < 1 || size.Height < 1)
            {
                size = new Size(1, 1);
            }

            this.ResizeClientBuffer(size);
            this.ResizeBuffer(new Size(size.Width / ResizeFactor, size.Height / ResizeFactor));
        }

        /// <summary>
        /// Dispose of bitmap(draw buffer).
        /// </summary>
        private void DestroyDrawBuffer()
        {
            this.DrawBuffer.Dispose();
            this.DrawBuffer = default;
        }
    }
}
