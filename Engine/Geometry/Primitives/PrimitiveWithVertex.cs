namespace SoftRenderer.Engine.Geometry.Primitives
{
    using System.Collections.Generic;
    using SoftRenderer.Engine.Geometry.Vertex;
    using SoftRenderer.Engine.Materials;

    /// <inheritdoc cref="IPrimitive{TMaterial,TVertex}"/>
    public abstract class Primitive<TMaterial, TVertex> : Primitive<TMaterial>, IPrimitive<TMaterial, TVertex>
        where TMaterial : IMaterial
        where TVertex : IVertex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Primitive{TMaterial, TVertex}"/> class.
        /// </summary>
        /// <param name="primitiveBehaviour">primitiveBehaviour.</param>
        /// <param name="primitiveTopology"> current primitive topology of the vertexes. undefined if no vertexes are present.</param>
        /// <param name="material">material type of primitive.</param>
        /// <param name="vertexes">set of vertexes for the primitive.</param>
        protected Primitive(PrimitiveBehaviour primitiveBehaviour, PrimitiveTopology primitiveTopology, TMaterial material, IReadOnlyList<TVertex> vertexes) : base(primitiveBehaviour, primitiveTopology, material)
        {
            this.Vertexes = vertexes;
        }

        /// <inheritdoc cref="IPrimitive"/>
        public IReadOnlyList<TVertex> Vertexes { get; }
    }
}
