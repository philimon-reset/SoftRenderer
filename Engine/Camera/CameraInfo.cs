namespace SoftRenderer.Engine.Camera
{
    using System.Drawing;
    using Buffers;
    using Math;

    public record CameraInfo : ICameraInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraInfo"/> class.
        /// </summary>
        /// <param name="eye">eye of the camera.</param>
        /// <param name="target">target of the camera.</param>
        /// <param name="up">up direction.</param>
        /// <param name="projection">projection instance.</param>
        /// <param name="bufferSize">buffer size.</param>
        /// <param name="clientBuffer">client buffer.</param>
        public CameraInfo(Vector3 eye, Vector3 target, Vector3 up, Projection projection, Size bufferSize, ClientBuffer clientBuffer)
        {
            this.Eye = eye;
            this.Target = target;
            this.Up = up;
            this.Projection = projection;
            this.BufferSize = bufferSize;
            this.ClientBuffer = clientBuffer;
            this.Materalization = new MateralizedCameraInfo(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraInfo"/> class.
        /// </summary>
        /// <param name="cameraInfo">camera info.</param>
        /// <param name="projection">projection instance.</param>
        /// <param name="bufferSize">buffer size.</param>
        /// <param name="clientBuffer">client buffer.</param>
        public CameraInfo(CameraInfo cameraInfo, Projection projection, Size bufferSize, ClientBuffer clientBuffer)
            : this(
                cameraInfo.Eye,
                cameraInfo.Target,
                cameraInfo.Up,
                projection,
                bufferSize,
                clientBuffer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraInfo"/> class.
        /// </summary>
        /// <param name="cameraInfo">camera info.</param>
        /// <param name="projection">projection instance.</param>
        public CameraInfo(CameraInfo cameraInfo, Projection projection)
            : this(
                cameraInfo.Eye,
                cameraInfo.Target,
                cameraInfo.Up,
                projection,
                cameraInfo.BufferSize,
                cameraInfo.ClientBuffer)
        {
        }

        /// <inheritdoc/>
        public Vector3 Eye { get; set; }

        /// <inheritdoc/>
        public Vector3 Target { get; }

        /// <inheritdoc/>
        public Vector3 Up { get; }

        /// <inheritdoc/>
        public Projection Projection { get; set; }

        /// <inheritdoc/>
        public Size BufferSize { get; }

        /// <inheritdoc/>
        public ClientBuffer ClientBuffer { get; set; }

        /// <inheritdoc/>
        public MateralizedCameraInfo Materalization { get; }
    }
}
