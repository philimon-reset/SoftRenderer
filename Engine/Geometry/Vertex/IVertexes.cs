namespace SoftRenderer.Engine.Geometry.Vertex
{
    using System.Collections.Generic;

    /// <summary>
    /// Set of vertexes.
    /// </summary>
    /// <typeparam name="TVertex">vertex type.</typeparam>
    public interface IVertexes<out TVertex>
        where TVertex : IVertex
    {
        /// <summary>
        /// Gets a list of vertexes.
        /// </summary>
        IReadOnlyList<TVertex> Vertexes
        {
            get;
        }
}
}
