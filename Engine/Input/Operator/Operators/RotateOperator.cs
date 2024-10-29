namespace SoftRenderer.Engine.Input.Operator.Operators
{
    using System;
    using System.Windows.Forms;
    using SoftRenderer.Engine.Camera;
    using SoftRenderer.Engine.Render;
    using SoftRenderer.Math;

    /// <summary>
    /// Rotate operator.
    /// </summary>
    public class RotateOperator : Operator
    {
        private static bool _leftClicked = false;

        private static bool _rightClicked = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RotateOperator"/> class.
        /// </summary>
        /// <param name="renderBase">render base instance.</param>
        public RotateOperator(RenderBase renderBase)
            : base(renderBase)
        {
            this.Yaw = -Math.PI / 2;
            this.Pitch = 0f;
        }

        private CameraInfo? CameraInfo;

        private Vector3 MousePositionView { get; set; }

        private double Yaw { get; set; }

        private double Pitch { get; set; }

        /// <inheritdoc/>
        protected override void ControlMousePress(object sender, MouseEventArgs e)
        {
            base.ControlMousePress(sender, e);

            switch (e.Button)
            {
                case MouseButtons.Left:
                    _leftClicked = true;

                    // get camera info.
                    this.CameraInfo = this.RenderBase.MyCameraInfo;

                    // get mousePosition
                    this.MousePositionView = Matrix.TransformPoint(this.CameraInfo.Materalization.ClientViewMatrixInverse, new Vector3(e.X, e.Y, 0));
                    break;
                case MouseButtons.Right:
                    _rightClicked = true;

                    // get camera info.
                    this.CameraInfo = this.RenderBase.MyCameraInfo;

                    // get mousePosition
                    this.MousePositionView = Matrix.TransformPoint(this.CameraInfo.Materalization.ClientViewMatrixInverse, new Vector3(e.X, e.Y, 0));
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void ControlMouseRelease(object sender, MouseEventArgs e)
        {
            base.ControlMouseRelease(sender, e);

            switch (e.Button)
            {
                case MouseButtons.Left:
                    _leftClicked = false;
                    this.MousePositionView = default;
                    this.CameraInfo = default;
                    break;
                case MouseButtons.Right:
                    _rightClicked = false;
                    this.MousePositionView = default;
                    this.CameraInfo = default;
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void ControlMouseMove(object sender, MouseEventArgs e)
        {
            base.ControlMouseMove(sender, e);
            if (this.CameraInfo is not null)
            {
                // save camera info.
                var cameraInfo = this.RenderBase.MyCameraInfo;

                // get mouse offset.
                var mousePosition = Matrix.TransformPoint(this.CameraInfo.Materalization.ClientViewMatrixInverse, new Vector3(e.X, e.Y, 0));
                var offset = mousePosition - this.MousePositionView;

                // update mouse position.
                this.MousePositionView = mousePosition;

                // update yaw and pitch.
                this.Yaw += offset.X;
                this.Pitch += -offset.Y;

                // cap pitch value.
                this.Pitch = Math.Max(-1f, Math.Min(1f, this.Pitch));

                // get new direction vector.
                Vector3 direction = new Vector3(Math.Cos(this.Yaw) * Math.Cos(this.Pitch), Math.Sin(this.Pitch), Math.Sin(this.Yaw) * Math.Cos(this.Pitch)).GetNormalized;
                if (_leftClicked)
                {
                    cameraInfo.Eye = direction + cameraInfo.Eye.GetNormalized;
                }

                if (_rightClicked)
                {
                    // get distance from camera to target
                    double distance = (cameraInfo.Target - cameraInfo.Eye).Length;
                    cameraInfo.Target = cameraInfo.Eye + (direction * distance);
                }

                this.RenderBase.MyCameraInfo = new CameraInfo(cameraInfo, this.CameraInfo.Projection);
            }
        }
    }
}
