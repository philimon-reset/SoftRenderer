//-----------------------------------------------------------------------
// <copyright file="RenderBase.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine.Render
{
    using System;
    using System.Drawing;
    using SoftRenderer.Client.FPSCounter;

    /// <summary>
    /// Represents a vector in three-dimensional space.
    /// </summary>
    /// <remarks>
    /// This class provides properties and methods to perform vector operations.
    /// </remarks>
    public abstract class RenderBase : IRenderBase
    {

        /// <summary>
        /// Initialize the <see cref="RenderBase"/> class.
        /// </summary>
        /// <param name="hosthandle"> Given host handle. </param>
        public RenderBase(IntPtr hosthandle)
        {
            this.HostHandle = hosthandle;
            this.RendererFps = new FPSCounter(TimeSpan.FromSeconds(0.5));
            this.GraphicsHandle = Graphics.FromHwndInternal(this.HostHandle);
            this.FpsFont = new Font("Arial", 12);
        }

        /// <summary>
        /// Gets Handle for the windows form.
        /// </summary>
        public IntPtr HostHandle { get; private set; }

        /// <summary>
        /// Gets total fps data.
        /// </summary>
        public FPSCounter RendererFps { get; private set; }

        /// <summary>
        /// Gets graphics instance handle.
        /// </summary>
        protected Graphics GraphicsHandle { get; set; }

        private Font FpsFont { get; set; }

        /// <summary>
        /// Sets host handle as default.
        /// </summary>
        public virtual void Dispose()
        {
            this.GraphicsHandle.Dispose();
            this.RendererFps.Dispose();
            this.GraphicsHandle = default;
            this.RendererFps = default;
            this.HostHandle = default;
        }


        /// <summary>
        /// Render the current frame for current rendering techinque.
        /// </summary>
        public void Render()
        {
            this.RendererFps.StartFrame();
            this.RenderInternal();
            this.RendererFps.EndFrame();
        }

        /// <summary>
        /// Initalize common rendering steps.
        /// </summary>
        public void RenderInternal()
        {
            this.GraphicsHandle.Clear(Color.Black);
            this.GraphicsHandle.DrawString(this.RendererFps.ToString(), this.FpsFont, Brushes.OrangeRed, 0, 0);
        }

        /// <summary>
        /// checks if form is active.
        /// </summary>
        /// <returns>true if host handle for form is not set to default.</returns>
        public bool IsActive()
        {
            return this.HostHandle != IntPtr.Zero;
        }
    }
}
