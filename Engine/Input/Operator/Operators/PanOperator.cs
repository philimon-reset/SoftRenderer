namespace SoftRenderer.Engine.Input.Operator.Operators
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using SoftRenderer.Engine.Camera;
    using SoftRenderer.Engine.Input.Operator;
    using SoftRenderer.Engine.Render;
    using SoftRenderer.Math;
    using SoftRenderer.Utility;

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

        private static bool rightClicked = false;
        private CameraInfo? CameraInfo;
        private Plane CameraPlane;

        private Vector3 MousePositionPlane { get; set; }

        private Vector3 MousePosition { get; set; }

        /// <inheritdoc/>
        protected override void ControlMousePress(object sender, MouseEventArgs e)
        {
            base.ControlMousePress(sender, e);
            switch (e.Button)
            {
                case MouseButtons.Right:
                    rightClicked = true;
                    // get camera info.
                    this.CameraInfo = this.RenderBase.MyCameraInfo;

                    // get plane info.
                    var planeOrigin = this.GetPlaneOrigin();
                    var planeDirection = this.CameraInfo.GetEyeVectorDirection();
                    this.CameraPlane = new Plane(planeOrigin, planeDirection);

                    // get mouse info, change from screen coordinate to world coordinates.
                    this.MousePosition = Matrix.TransformPoints(this.CameraInfo.Materalization.ViewPerspectiveProjClientMatrixInverse, [new Vector3(e.X, e.Y, 0)]).First();
                    var mouseRay = this.CameraInfo.GetMouseRay(this.MousePosition);

                    // get plane and mouse position intersection.
                    this.MousePositionPlane = this.CameraPlane.IntersectionWith(mouseRay);
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void ControlMouseRelease(object sender, MouseEventArgs e)
        {
            base.ControlMouseRelease(sender, e);
            switch (e.Button)
            {
                case MouseButtons.Right:
                    rightClicked = false;
                    this.MousePositionPlane = default;
                    this.MousePosition = default;
                    this.CameraPlane = default;
                    this.CameraInfo = default;
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void ControlMouseMove(object sender, MouseEventArgs e)
        {
            base.ControlMouseMove(sender, e);
            if (rightClicked && this.CameraInfo is not null)
            {
                // get mouse info, change from screen coordinate to world coordinates.
                this.MousePosition = Matrix.TransformPoints(this.CameraInfo.Materalization.ViewPerspectiveProjClientMatrixInverse, [new Vector3(e.X, e.Y, 0)]).First();
                var mouseRay = this.CameraInfo.GetMouseRay(this.MousePosition);

                // get plane and mouse position intersection.
                var mouseIntersection = this.CameraPlane.IntersectionWith(mouseRay);
                var offset = (mouseIntersection - this.MousePositionPlane) * 100;
                var cameraInfo = this.RenderBase.MyCameraInfo;
                cameraInfo.Eye += offset;
                cameraInfo.Target += offset;
                this.RenderBase.MyCameraInfo = new CameraInfo(this.CameraInfo, this.CameraInfo.Projection);
            }
        }

        private Vector3 GetPlaneOrigin()
        {
            return this.RenderBase.MyCameraInfo.Target;
        }
    }
}
