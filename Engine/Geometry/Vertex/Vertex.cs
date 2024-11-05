namespace SoftRenderer.Engine.Geometry.Vertex
{
    using SoftRenderer.Math;

    /// <summary>
    /// Vertex instance.
    /// </summary>
    /// <param name="position">position of vertex.</param>
    public record struct Vertex(Vector3 position) : IVertex
    {
        /// <inheritdoc cref="IVertex"/>>
        public Vector3 Position = position;
    }
}
