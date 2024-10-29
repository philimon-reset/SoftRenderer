namespace SoftRenderer.Engine.Input.Operator.Operators
{
    using System.Windows.Forms;
    using Camera;
    using Math;
    using Render;

    public class MoveOperator : Operator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveOperator"/> class.
        /// </summary>
        /// <param name="renderBase">render base instance.</param>
        public MoveOperator(RenderBase renderBase)
            : base(renderBase)
        {
        }

        /// <inheritdoc/>
        protected override void ControlKeyDown(object sender, KeyEventArgs e)
        {
            base.ControlKeyDown(sender, e);
            double scale = 0.05;
            var cameraMove = new Vector3(0, -1.0f, 0);
            var cameraInfo = this.RenderBase.MyCameraInfo;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    cameraInfo.Eye -= cameraMove * scale;
                    break;
                case Keys.Down:
                    cameraInfo.Eye += cameraMove * scale;
                    break;
                case Keys.Left:
                    cameraInfo.Eye -= Vector3.Normalize(cameraInfo.Eye.Cross(cameraMove)) * scale;
                    break;
                case Keys.Right:
                    cameraInfo.Eye -= Vector3.Normalize(cameraMove.Cross(cameraInfo.Eye)) * scale;
                    break;
                case Keys.W:
                    // default scaling
                    double scaleForward = 1.0 + scale;
                    var forwardVector = cameraInfo.Eye - cameraInfo.Target;
                    cameraInfo.Eye += forwardVector - (forwardVector * scaleForward);
                    break;
                case Keys.S:
                    double scaleBackwards = 1 - scale;
                    var backwardVector = cameraInfo.Eye - cameraInfo.Target;
                    cameraInfo.Eye += backwardVector - (backwardVector * scaleBackwards);
                    break;
            }

            this.RenderBase.MyCameraInfo = new CameraInfo(cameraInfo, cameraInfo.Projection);
        }
    }
}
