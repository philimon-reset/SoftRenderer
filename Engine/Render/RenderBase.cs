//-----------------------------------------------------------------------
// <copyright file="RenderBase.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine.Render
{
    using System;
    using System.Drawing;
    using SoftRenderer.Client.FPSCounter;
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
        /// Initialize the <see cref="RenderBase"/> class.
        /// </summary>
        /// <param name="hosthandle"> Given host handle. </param>
        public RenderBase(IntPtr hosthandle)
        {
            this.HostHandle = hosthandle;
            this.RendererFps = new FPSCounter(TimeSpan.FromSeconds(0.5));
            this.GraphicsHandle = Graphics.FromHwndInternal(this.HostHandle);
            this.CurrentBuffer = BufferedGraphicsManager.Current.Allocate(this.GraphicsHandle, new Rectangle(Point.Empty, Util.GetClientRectangle(this.HostHandle).Size));
            this.FpsFont = new Font("Arial", 12);
        }

        /// <summary>
        /// gets Double buffer handle.
        /// </summary>
        private BufferedGraphics CurrentBuffer { get; set; }

        /// <summary>
        /// Gets Handle for the windows form.
        /// </summary>
        public IntPtr HostHandle { get; private set; }

        /// <summary>
        /// Gets total fps data.
        /// </summary>
        public FPSCounter RendererFps { get; private set; }

        /// <summary>
        /// gets graphics instance handle.
        /// </summary>
        public Graphics GraphicsHandle { get; set; }

        private Font FpsFont { get; set; }

        /// <summary>
        /// Sets host handle as default.
        /// </summary>
        public virtual void Dispose()
        {
            this.GraphicsHandle.Dispose();
            this.CurrentBuffer.Dispose();
            this.RendererFps.Dispose();
            this.FpsFont.Dispose();
            this.FpsFont = default;
            this.GraphicsHandle = default;
            this.CurrentBuffer = default;
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
            this.CurrentBuffer.Graphics.Clear(Color.Black);
            this.CurrentBuffer.Graphics.DrawString(this.RendererFps.ToString(), this.FpsFont, Brushes.OrangeRed, 0, 0);

            this.CurrentBuffer.Render();
        }
    }
}
