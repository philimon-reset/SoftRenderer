namespace SoftRenderer.Engine.Camera
{
    using Math;
    using Utility;

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

        /// <summary>
        /// get a ray to where the mouse is pointing from the camera.
        /// </summary>
        /// <param name="cameraPosition">camera position.</param>
        /// <param name="mousePosition">mouse position in world coordinates.</param>
        /// <returns>ray.</returns>
        Ray GetMouseRay(Vector3 cameraPosition, Vector3 mousePosition);
    }
}
