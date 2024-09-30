//-----------------------------------------------------------------------
// <copyright file="Canvas.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SoftRenderer.Engine.Render.Technique.Canvas
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using SoftRenderer.Engine.Render;

    /// <summary>
    /// Canvas Render Port.
    /// </summary>
    public class Canvas : Render.RenderBase
    {
        // should this be a struct??
        /// <summary>
        /// Draw setting that change based on event.
        /// </summary>
        /// <param name="clicked">whether the mouse was clicked or not.</param>
        /// <param name="currentBrush">current brush color.</param>
        /// <param name="rnd">random number for next brush color.</param>
        record struct DrawArgs(bool clicked, Brush currentBrush, Random rnd, int thickness);
        
        private DrawArgs CurrentDrawArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// General tomfoolery canvas.
        /// </summary>
        /// <param name="renderBaseArgs">dto that hold form handles.</param>
        public Canvas(IRenderBaseArgs renderBaseArgs)
            : base(renderBaseArgs)
        {
            // Double buffer for rendering setup.
            this.GraphicsHandle = Graphics.FromHwndInternal(this.HostHandle);
            this.CurrentDrawArgs = new DrawArgs(false, Brushes.Aqua, new Random(), 1);
            this.HostInput.MouseMove += this.Draw;
            this.HostInput.MousePress += this.Click;
            this.HostInput.MouseRelease += this.Click;
            this.HostInput.KeyDown += this.HandleKey;
            this.HostInput.MouseScroll += this.HandleThickness;
            this.DrawColorFont = new Font("Arial", 12);
        }

        private Graphics GraphicsHandle { get; set; }

        /// <inheritdoc/>
        public override void Dispose()
        {
            base.Dispose();
            this.HostInput.MouseMove -= this.Draw;
            this.HostInput.MousePress -= this.Click;
            this.HostInput.MouseRelease -= this.Click;
            this.HostInput.MouseScroll -= this.HandleThickness;
        }

        public override void Render()
        {
          
        }

        public override void RenderInternal()
        {
            
        }

        private Font DrawColorFont { get; set; }

        /// <summary>
        /// Draw When Mouse is moved
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">mouse event arg.</param>
        private void Draw(object sender, MouseEventArgs e)
        {
            if (this.CurrentDrawArgs.clicked)
            {
                this.GraphicsHandle.FillRectangle(this.CurrentDrawArgs.currentBrush, e.X, e.Y, this.CurrentDrawArgs.thickness, this.CurrentDrawArgs.thickness);
            }
            // this.CurrentBuffer.Render();
        }

        private void Click(object sender, MouseEventArgs e)
        {
            this.CurrentDrawArgs.clicked = !this.CurrentDrawArgs.clicked;
        }

        private void HandleKey(object sender, KeyEventArgs e)
        {
            Random rnd = this.CurrentDrawArgs.rnd;
            Brush brush = new SolidBrush(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
            switch (e.KeyData)
            {
                case Keys.Left:
                    this.CurrentDrawArgs.currentBrush = brush;
                    break;
                case Keys.Right:
                    this.CurrentDrawArgs.currentBrush = brush;
                    break;
                case Keys.C:
                    this.GraphicsHandle.Clear(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
                    break;
            }
        }

        private void HandleThickness(object sender, MouseEventArgs e)
        {
            this.CurrentDrawArgs.thickness += this.CurrentDrawArgs.thickness + e.Delta;
            this.CurrentDrawArgs.thickness = Math.Abs(this.CurrentDrawArgs.thickness) % 10;
        }
    }
}