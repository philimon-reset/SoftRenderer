namespace SoftRenderer.Engine.Geometry.Primitives
{
    using SoftRenderer.Engine.Materials;

    /// <inheritdoc cref="IPrimitive{TMaterial}"/>
    public abstract class Primitive<TMaterial> : Primitive, IPrimitive<TMaterial>
        where TMaterial : IMaterial
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Primitive{TMaterial}"/> class.
        /// </summary>
        /// <param name="primitiveBehaviour">primitiveBehaviour.</param>
        /// <param name="primitiveTopology"> current primitive topology of the vertexes. undefined if no vertexes are present.</param>
        /// <param name="material">material type of primitive.</param>
        protected Primitive(PrimitiveBehaviour primitiveBehaviour, PrimitiveTopology primitiveTopology, TMaterial material)
            : base(primitiveBehaviour, primitiveTopology) => this.Material = material;

        /// <inheritdoc/>
        public TMaterial Material { get; }
    }
}
