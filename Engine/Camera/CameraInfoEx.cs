namespace SoftRenderer.Engine.Camera
{
    using Math;
    using Utility;

    /// <summary>
    /// CameraInfo extensions.
    /// </summary>
    public static class CameraInfoEx
    {
        /// <summary>
        /// Get vector in the direction of the camera.
        /// </summary>
        /// <param name="cameraInfo">camera info.</param>
        /// <returns>vector3.</returns>
        public static Vector3 GetEyeVector(this CameraInfo cameraInfo) => cameraInfo.Target - cameraInfo.Eye;

        /// <summary>
        /// Get unit vector in the direction of the camera.
        /// </summary>
        /// <param name="cameraInfo">camera info.</param>
        /// <returns>vector3.</returns>
        public static Vector3 GetEyeVectorDirection(this CameraInfo cameraInfo) => cameraInfo.GetEyeVector().GetNormalized;

        /// <summary>
        /// get a ray to where the mouse is pointing from the camera.
        /// </summary>  
        /// <param name="cameraInfo">camera info.</param>
        /// <param name="mousePosition">mouse position in world coordinates.</param>
        /// <returns>ray.</returns>
        public static Ray GetMouseRay(this CameraInfo cameraInfo, Vector3 mousePosition) => cameraInfo.Projection.GetMouseRay(cameraInfo.Eye, mousePosition);
    }
}
