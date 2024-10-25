namespace SoftRenderer.Engine.Camera
{
    using System.Drawing;
    using SoftRenderer.Engine.Buffers;
    using SoftRenderer.Math;

    /// <summary>
    /// Interface for the camera info class
    /// </summary>
    public interface ICameraInfo
    {
        /// <summary>
        /// Gets Current eye position of the camera.
        /// </summary>
        Vector3 Eye { get; }

        /// <summary>
        /// Gets target Position.
        /// </summary>
        Vector3 Target { get; }

        /// <summary>
        /// Gets up Direction.
        /// </summary>
        Vector3 Up { get; }

        /// <summary>
        /// Gets projection Matrix.
        /// </summary>
        Projection Projection { get; }

        /// <summary>
        /// Gets Draw Buffer Size.
        /// </summary>
        Size BufferSize { get; }

        /// <summary>
        /// Gets Current client buffer.
        /// </summary>
        ClientBuffer ClientBuffer { get; }

        /// <summary>
        /// Gets Cached transformation and camera info.
        /// </summary>
        MateralizedCameraInfo Materalization { get; }
    }
}
