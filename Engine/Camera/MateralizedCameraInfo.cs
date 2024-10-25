namespace SoftRenderer.Engine.Camera
{
    using Math;

    public record struct MateralizedCameraInfo
    {
        /// <summary>
        /// Gets View Matrix. (world coordinates -> camera coordinates)
        /// </summary>
        public Matrix ViewMatrix { get; }

        /// <summary>
        /// Gets Inverse: View Matrix. (camera coordinates -> world coordinates)
        /// </summary>
        public Matrix ViewMatrixInverse { get; }

        /// <summary>
        /// Gets PerspectiveProj Matrix. (camera coordinates -> clip space coordinates)
        /// </summary>
        public Matrix PerspectiveProjMatrix { get; }

        /// <summary>
        /// Gets Inverse: PerspectiveProj Matrix. (clip space coordinates -> camera coordinates)
        /// </summary>
        public Matrix PerspectiveProjMatrixInverse { get; }

        /// <summary>
        /// Gets ClientView Matrix. (clip space coordinates -> client buffer coordinates)
        /// </summary>
        public Matrix ClientViewMatrix { get; }

        /// <summary>
        /// Gets Inverse: ClientView Matrix. (client buffer coordinates -> clip space coordinates)
        /// </summary>
        public Matrix ClientViewMatrixInverse { get; }

        /// <summary>
        /// Gets ViewPerspectiveProj Matrix. (world coordinates -> camera coordinates -> clip space coordinates)
        /// </summary>
        public Matrix ViewPerspectiveProjMatrix { get; }

        /// <summary>
        /// Gets Inverse: ViewPerspectiveProj Matrix. (clip space coordinates -> camera coordinates -> world coordinates)
        /// </summary>
        public Matrix ViewPerspectiveProjMatrixInverse { get; }

        /// <summary>
        /// Gets ViewPerspectiveProjClient Matrix. (world coordinates -> camera coordinates -> clip space coordinates -> client buffer coordinates)
        /// </summary>
        public Matrix ViewPerspectiveProjClientMatrix { get; }

        /// <summary>
        /// Gets Inverse: ViewPerspectiveProjClient Matrix. (client buffer coordinates -> clip space coordinates -> camera coordinates -> world coordinates)
        /// </summary>
        public Matrix ViewPerspectiveProjClientMatrixInverse { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MateralizedCameraInfo"/> class.
        /// </summary>
        /// <param name="cameraInfo">camera info.</param>
        public MateralizedCameraInfo(ICameraInfo cameraInfo)
        {
            this.ViewMatrix = Matrix.ViewMatrix(cameraInfo.Eye, cameraInfo.Target, cameraInfo.Up);
            this.ViewMatrixInverse = this.ViewMatrix.GetInverted;

            this.PerspectiveProjMatrix = cameraInfo.Projection.GetPerspectiveProjectionMatrix();
            this.PerspectiveProjMatrixInverse = this.PerspectiveProjMatrix.GetInverted;

            this.ClientViewMatrix = Matrix.NdcToScreenCoordinates(cameraInfo.ClientBuffer);
            this.ClientViewMatrixInverse = this.ClientViewMatrix.GetInverted;

            this.ViewPerspectiveProjMatrix = this.ViewMatrix * this.PerspectiveProjMatrix;
            this.ViewPerspectiveProjMatrixInverse = this.ViewPerspectiveProjMatrix.GetInverted;

            this.ViewPerspectiveProjClientMatrix = this.ViewPerspectiveProjMatrix * this.ClientViewMatrix;
            this.ViewPerspectiveProjClientMatrixInverse = this.ViewPerspectiveProjClientMatrix.GetInverted;
        }
    }
}
