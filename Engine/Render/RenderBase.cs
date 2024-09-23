//-----------------------------------------------------------------------
// <copyright file="RenderBase.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine.Render
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using SoftRenderer.Client.FPSCounter;
    using SoftRenderer.Engine.Input;
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
        /// Initializes a new instance of the <see cref="RenderBase"/> class.
        /// </summary>
        /// <param name="renderBaseArgs"> Given host handle. </param>
        public RenderBase(IRenderBaseArgs renderBaseArgs)
        {
            this.HostHandle = renderBaseArgs.HostHandle;
            this.HostInput = renderBaseArgs.Input;
            this.FormControl = Util.GetForm(this.HostHandle);
            this.RendererFps = new FPSCounter(TimeSpan.FromSeconds(0.5));
            this.GraphicsHandle = Graphics.FromHwndInternal(this.HostHandle);
            this.ClientRectange = new Rectangle(Point.Empty, this.FormControl.Size);
            this.CurrentBuffer = BufferedGraphicsManager.Current.Allocate(this.GraphicsHandle, this.ClientRectange);
            this.FpsFont = new Font("Arial", 12);
        }

        /// <summary>
        /// Gets Handle for the windows form.
        /// </summary>
        public IntPtr HostHandle { get; private set; }

        /// <summary>
        /// Gets input handle for form.
        /// </summary>
        public IInput HostInput { get; private set; }

        /// <summary>
        /// Gets form control from host handle.
        /// </summary>
        public Control FormControl { get; private set; }

        /// <summary>
        /// Gets total fps data.
        /// </summary>
        public FPSCounter RendererFps { get; private set; }

        /// <summary>
        /// Gets or sets graphics instance handle.
        /// </summary>
        public Graphics GraphicsHandle { get; set; }

        private Font FpsFont { get; set; }

        /// <summary>
        /// Gets or sets double buffer handle.
        /// </summary>
        private BufferedGraphics CurrentBuffer { get; set; }

        private Rectangle ClientRectange { get; set; }

        /// <summary>
        /// Sets host handle as default.
        /// </summary>
        public virtual void Dispose()
        {
            this.GraphicsHandle.Dispose();
            this.CurrentBuffer.Dispose();
            this.RendererFps.Dispose();
            this.FpsFont.Dispose();
            this.HostInput.Dispose();
            this.FpsFont = default;
            this.GraphicsHandle = default;
            this.CurrentBuffer = default;
            this.RendererFps = default;
            this.HostHandle = default;
            this.HostInput = default;
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
