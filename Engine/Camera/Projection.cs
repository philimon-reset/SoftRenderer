namespace SoftRenderer.Engine.Camera
{
    using SoftRenderer.Math;

    public record Projection(double FovY, double AspectRatio, double Znear, double Zfar)
        : IProjection
    {
        /// <inheritdoc/>
        public Matrix GetPerspectiveProjectionMatrix()
        {
            return Matrix.PerspectiveMatrix(this.FovY, this.AspectRatio, this.Znear, this.Zfar);
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
