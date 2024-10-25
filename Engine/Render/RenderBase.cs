//-----------------------------------------------------------------------
// <copyright file="RenderBase.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SoftRenderer.Engine.Render
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using SoftRenderer.Client.FPSCounter;
    using SoftRenderer.Engine.Buffers;
    using SoftRenderer.Engine.Camera;
    using SoftRenderer.Engine.Input;
    using SoftRenderer.Engine.Input.EventArgs;
    using SoftRenderer.Engine.Input.Operator;
    using SoftRenderer.Engine.Input.Operator.Operators;
    using SoftRenderer.Math;

    /// <summary>
    /// Represents a vector in three-dimensional space.
    /// </summary>
    /// <remarks>
    /// This class provides properties and methods to perform vector operations.
    /// </remarks>
    public abstract class RenderBase : IRenderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderBase"/> class.
        /// </summary>
        /// <param name="renderBaseArgs"> Given host handle. </param>
        protected RenderBase(IRenderBaseArgs renderBaseArgs)
        {
            // Render base arg values.
            this.HostHandle = renderBaseArgs.HostHandle;
            this.HostInput = renderBaseArgs.Input;

            // Form control of the form.
            this.FormSize = this.HostInput.Size;

            // set Up client buffer
            Size clientBufferSize = this.FormSize;
            var clientBuffer = new ClientBuffer(clientBufferSize, 0, 0);

            // Set up draw buffer.
            this.DrawBuffer = new DrawBuffer(new Size(clientBuffer.Width / 1, clientBuffer.Height / 1));

            // View Matrix
            var eye = new Vector3(0.6, 0.6, 1);
            var target = new Vector3();

            // Projection Matrix
            double fov = Math.PI * 0.5;
            double znear = 0.001;
            double zfar = 1000;
            var projection = new Projection(fov, (double)this.DrawBuffer.Height / this.DrawBuffer.Width, znear, zfar);

            // camera info.
            this.MyCameraInfo = new CameraInfo(eye, target, Vector3.ZAxis, projection, this.DrawBuffer.Size, clientBuffer);

            // Event Hooking.
            this.Operators =[
                new ResizeOperator(this, this.ResizeHost, this.ResizeClientBuffer),
                new ZoomOperator(this),
                new PanOperator(this)
            ];

            // Initial Resize
            ResizeOperator.ResizeBuffers(this, this.FormSize, this.ResizeHost, this.ResizeClientBuffer);
        }

        /// <summary>
        /// Event handler for camera change.
        /// </summary>
        public event EventHandler<ICameraInfo> CameraInfoChanged;

        /// <summary>
        /// Gets or sets camera info alongside buffer info.
        /// </summary>
        public CameraInfo MyCameraInfo
        {
            get => this._cameraInfo;

            set
            {
                this._cameraInfo = value;
                this.CameraInfoChanged?.Invoke(this, this._cameraInfo);
            }
        }

        /// <summary>
        /// Gets Handle for the windows form.
        /// </summary>
        public IntPtr HostHandle { get; private set; }

        /// <summary>
        /// Gets or sets total fps data.
        /// </summary>
        public FPSCounter RendererFps { get; set; }

        /// <summary>
        /// Gets start of the render.
        /// </summary>
        public DateTime FrameStart { get; private set; }

        /// <summary>
        /// Gets input handle for form.
        /// </summary>
        public IInput HostInput { get; private set; }

        /// <summary>
        /// Gets size of the entire form.
        /// </summary>
        public Size FormSize { get; set; }

        /// <summary>
        /// Gets Resize operator.
        /// </summary>
        protected List<IOperator> Operators { get; private set; }

        /// <summary>
        /// Gets draw Buffer instance to work with bitmap.
        /// </summary>
        protected DrawBuffer DrawBuffer { get; private set; }

        private CameraInfo _cameraInfo { get; set; }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            // Dispose Properties.
            this.HostInput.Dispose();
            this.Operators.ForEach((x) => x.Dispose());
            this.DestroyDrawBuffer();

            // Set properties to default.
            this.HostHandle = default;
            this.FormSize = default;
            this.HostInput = default;
            this.MyCameraInfo = default;
            this.Operators = default;
        }

        /// <inheritdoc/>
        public virtual void Render()
        {
            this.EnsureBufferSize();
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
        /// Resize draw buffer size.
        /// </summary>
        /// <param name="argsNewSize">New size.</param>
        protected virtual void ResizeBuffer(Size argsNewSize)
        {
            this.DestroyDrawBuffer();
            this.DrawBuffer = new DrawBuffer(argsNewSize);
        }

        /// <summary>
        /// Resize Client Buffer size.
        /// </summary>
        /// <param name="argsNewSize">New size.</param>
        protected virtual void ResizeClientBuffer (Size argsNewSize)
        {
            int x = (this.FormSize.Width - argsNewSize.Width) / 2;
            int y = (this.FormSize.Height - argsNewSize.Height) / 2;
            var clientBuffer = new ClientBuffer(argsNewSize, x, y);
            var drawBufferSize = new Size(clientBuffer.Width / ResizeOperator.ResizeFactor, clientBuffer.Height / ResizeOperator.ResizeFactor);
            Projection projection = this.MyCameraInfo.Projection with { AspectRatio = (double)drawBufferSize.Height / drawBufferSize.Width };
            this.MyCameraInfo = new CameraInfo(this.MyCameraInfo, projection, drawBufferSize, clientBuffer);
        }

        /// <summary>
        /// Resize the host of the render base.
        /// </summary>
        /// <param name="newSize">new size.</param>
        protected virtual void ResizeHost(Size newSize)
        {
            this.FormSize = newSize;
        }

        private void EnsureBufferSize()
        {
            var size = new Size(this.MyCameraInfo.ClientBuffer.Width / ResizeOperator.ResizeFactor, this.MyCameraInfo.ClientBuffer.Height / ResizeOperator.ResizeFactor);
            if (this.DrawBuffer.Size != size)
            {
                this.ResizeBuffer(size);
            }
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
