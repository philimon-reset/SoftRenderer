namespace SoftRenderer.Engine.Geometry.Primitives
{
    using Engine.Materials;

    /// <inheritdoc cref="IPrimitive"/>
    public abstract class Primitive :
        IPrimitive
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Primitive"/> class.
        /// <param name="primitiveBehaviour">primitiveBehaviour.</param>
        /// <param name="primitiveTopology"> current primitive topology of the vertexes. undefined if no vertexes are present.</param>
        /// </summary>
        public Primitive(PrimitiveBehaviour primitiveBehaviour, PrimitiveTopology primitiveTopology)
        {
            this.PrimitiveBehaviour = primitiveBehaviour;
            this.PrimitiveTopology = primitiveTopology;
        }

        /// <inheritdoc />
        public PrimitiveBehaviour PrimitiveBehaviour { get; }

        /// <inheritdoc cref="IPrimitive"/>
        public PrimitiveTopology PrimitiveTopology { get; }
    }
}
