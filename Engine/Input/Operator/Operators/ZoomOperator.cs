namespace SoftRenderer.Engine.Input.Operator.Operators
{
    using System;
    using System.Windows.Forms;
    using Camera;
    using Render;

    /// <summary>
    /// Zoom operator.
    /// </summary>
    public class ZoomOperator : Operator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomOperator"/> class.
        /// </summary>
        /// <param name="renderBase">render base instance.</param>
        public ZoomOperator(RenderBase renderBase)
            : base(renderBase)
        {
        }

        /// <inheritdoc/>
        protected override void ControlMouseScroll(object sender, MouseEventArgs e)
        {
            base.ControlMouseScroll(sender, e);

            var cameraInfo = this.RenderBase.MyCameraInfo;

            // default scaling
            const double scale = 0.05;
            const double scaleForward = 1.0 + scale;
            const double scaleBackwards = 1 - scale;

            // calculate how much to zoom
            var scaleCurrent = e.Delta > 0 ? scaleForward : scaleBackwards;
            var eyeVector = cameraInfo.Eye - cameraInfo.Target;
            var offset = eyeVector - (eyeVector * scaleCurrent);

            // zoom
            cameraInfo.Eye += offset;
            this.RenderBase.MyCameraInfo = new CameraInfo(cameraInfo, cameraInfo.Projection);
        }
    }
}
