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
        private readonly int resizeFactor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderBase"/> class.
        /// </summary>
        /// <param name="renderBaseArgs"> Given host handle. </param>
        protected RenderBase(IRenderBaseArgs renderBaseArgs)
        {
            // Renerbase arg values.
            this.resizeFactor = 1;
            this.HostHandle = renderBaseArgs.HostHandle;
            this.HostInput = renderBaseArgs.Input;

            // Form control of the form.
            this.FormControl = Util.GetForm(this.HostHandle);

            // Set size of viewport and buffer.
            this.DrawBufferSize = new Size(this.FormControl.Size.Width / this.resizeFactor, this.FormControl.Size.Height / this.resizeFactor);
            this.ClientBufferSize = this.FormControl.Size;

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
        /// Gets or sets size of the buffer where rendering will take place.
        /// </summary>
        protected Size DrawBufferSize { get; set; }

        /// <summary>
        /// Gets size of the screen. Output will be scaled to this size.
        /// </summary>
        protected Size ClientBufferSize { get; private set; }

        /// <summary>
        /// Gets or sets total fps data.
        /// </summary>
        protected FPSCounter RendererFps { get; set; }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            // Dispose Properties.
            this.HostInput.Dispose();

            // Set properties to default.
            this.HostHandle = default;
            this.HostInput = default;

            // Event dispose
            this.HostInput!.SizeChanged -= this.ScreenResize;
        }

        /// <inheritdoc/>
        public virtual void Render()
        {
            this.RendererFps.StartFrame();
            this.RenderInternal();
            this.RendererFps.EndFrame();
        }

        /// <inheritdoc/>
        public abstract void RenderInternal();

        /// <summary>
        /// Resize Client Buffer size.
        /// </summary>
        /// <param name="argsNewSize">New size.</param>
        protected virtual void ResizeClientBuffer(Size argsNewSize)
        {
            this.ClientBufferSize = argsNewSize;
        }

        /// <summary>
        /// Resize draw buffer size.
        /// </summary>
        /// <param name="argsNewSize">New size.</param>
        protected virtual void ResizeBuffer(Size argsNewSize)
        {
            this.DrawBufferSize = argsNewSize;
        }

        private void ScreenResize(object sender, ISizeChangeArgs args)
        {
            Size size = args.NewSize;
            if (size.Width < 1 || size.Height < 1)
            {
                size = new Size(1, 1);
            }

            this.ResizeBuffer(new Size(size.Width / this.resizeFactor, size.Height / this.resizeFactor));
            this.ResizeClientBuffer(size);
        }
    }
}
