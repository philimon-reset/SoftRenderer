namespace SoftRenderer.Engine.Input.Operator.Operators
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Camera;
    using Math;
    using Render;

    /// <summary>
    /// Pan operator.
    /// </summary>
    public class PanOperator : Operator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PanOperator"/> class.
        /// </summary>
        /// <param name="renderBase">render base instance.</param>
        public PanOperator(RenderBase renderBase)
            : base(renderBase)
        {
        }

        private static bool clicked = false;
        
        private Vector3 MousePosition { get; set; }

        /// <inheritdoc/>
        protected override void ControlMousePress(object sender, MouseEventArgs e)
        {
            this.MousePosition = new Vector3(e.X, 0, e.Y);
            clicked = true;
        }

        /// <inheritdoc/>
        protected override void ControlMouseRelease(object sender, MouseEventArgs e)
        {
            clicked = false;
        }

        /// <inheritdoc/>
        protected override void ControlMouseMove(object sender, MouseEventArgs e)
        {
            if (clicked)
            {
                const double scale = 0.05;
                var cameraInfo = this.RenderBase.MyCameraInfo;
                var currentPosition = new Vector3(e.X, e.Y, 0);
                var viewSpace = Vector3.ToViewSpaceScaled(currentPosition - this.MousePosition, RenderBase.FormSize, new Point());
                var offset = new Vector3(viewSpace.X, 0, viewSpace.Y);
                Console.WriteLine(offset);
                Console.WriteLine(viewSpace);
                // zoom
                cameraInfo.Eye += offset;
                this.RenderBase.MyCameraInfo = new CameraInfo(cameraInfo, cameraInfo.Projection);
            }
        }
    }
}
