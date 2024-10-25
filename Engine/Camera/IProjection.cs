namespace SoftRenderer.Engine.Camera
{
    using Math;

    public interface IProjection
    {
        /// <summary>
        /// Gets near plane value.
        /// </summary>
        double Znear { get; }

        /// <summary>
        /// Gets far plane value.
        /// </summary>
        double Zfar { get; }

        /// <summary>
        /// Gets field of view angle (in radians).
        /// </summary>
        double FovY { get; }

        /// <summary>
        /// Gets aspect ratio of buffer.
        /// </summary>
        double AspectRatio { get; }

        /// <summary>
        /// Gets the appropriate projection matrix.
        /// </summary>
        /// <returns>a projection matrix using the information present.</returns>
        Matrix GetPerspectiveProjectionMatrix();
    }
}
