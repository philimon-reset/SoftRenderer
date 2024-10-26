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

        private static bool leftClicked = false;
        private static bool rightClicked = false;

        private Vector3 MousePosition { get; set; }

        /// <inheritdoc/>
        protected override void ControlMousePress(object sender, MouseEventArgs e)
        {
            this.MousePosition = new Vector3(e.X, e.Y, 0);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    leftClicked = true;
                    break;
                case MouseButtons.Right:
                    rightClicked = true;
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void ControlMouseRelease(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    leftClicked = false;
                    break;
                case MouseButtons.Right:
                    rightClicked = false;
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void ControlMouseMove(object sender, MouseEventArgs e)
        {
            double yaw = 90f;
            double pitch = 90f;
            double scale = 0.01f;
            if (rightClicked)
            {
                Vector3 direction = new Vector3();
                Vector3 currentPosition = new Vector3((e.X - this.MousePosition.X) * scale, (this.MousePosition.Y - e.Y) * scale, 0);
                yaw += currentPosition.X;
                pitch += currentPosition.Y;
                if (pitch > 89.0f)
                {
                    pitch = 89.0f;
                }

                if (pitch < -89.0f)
                {
                    pitch = -89.0f;
                }

                direction.X = Math.Cos(yaw) * Math.Cos(pitch);
                direction.Y = Math.Sin(pitch);
                direction.Z = Math.Sin(yaw) * Math.Cos(pitch);
                Vector3 cameraFront = currentPosition.GetNormalized;
                var cameraInfo = this.RenderBase.MyCameraInfo;
                cameraInfo.Eye += cameraFront;
                this.RenderBase.MyCameraInfo = new CameraInfo(cameraInfo, cameraInfo.Projection);
            }
        }

        /// <inheritdoc/>
        protected override void ControlKeyDown(object sender, KeyEventArgs e)
        {
            double scale = 0.05;
            var cameraMove = new Vector3(0, 0, -1.0f);
            var cameraInfo = this.RenderBase.MyCameraInfo;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    cameraInfo.Eye += cameraMove * scale;
                    break;
                case Keys.Down:
                    cameraInfo.Eye -= cameraMove * scale;
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
