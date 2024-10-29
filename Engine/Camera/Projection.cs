namespace SoftRenderer.Engine.Camera
{
    using SoftRenderer.Math;
    using Utility;

    public record Projection(double FovY, double AspectRatio, double Znear, double Zfar)
        : IProjection
    {
        /// <inheritdoc/>
        public Matrix GetPerspectiveProjectionMatrix()
        {
            return Matrix.PerspectiveMatrix(this.FovY, this.AspectRatio, this.Znear, this.Zfar);
        }

        /// <inheritdoc/>
        public Ray GetMouseRay(Vector3 cameraPosition, Vector3 mousePosition)
        {
            return new Ray(mousePosition, (mousePosition - cameraPosition).GetNormalized);
        }

        /// <inheritdoc/>
        public double Znear { get; } = Znear;

        /// <inheritdoc/>
        public double Zfar { get; } = Zfar;

        /// <inheritdoc/>
        public double FovY { get; set; } = FovY;

        /// <inheritdoc/>
        public double AspectRatio { get; set; } = AspectRatio;
    };
}
