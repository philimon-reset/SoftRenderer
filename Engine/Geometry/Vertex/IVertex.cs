namespace SoftRenderer.Engine.Geometry.Vertex
{
    using SoftRenderer.Math;

    /// <summary>
    /// Vertex instance.
    /// </summary>
    public interface IVertex
    {
        /// <summary>
        /// Gets position Of vertex.
        /// </summary>
        Vector3 position { get; }
    }
}
