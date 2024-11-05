namespace SoftRenderer.Engine.Geometry.Primitives
{
    using Materials;
    using SoftRenderer.Engine.Geometry.Vertex;
    using SoftRenderer.Engine.Materials;

    /// <summary>
    /// Represents graphical primitive.
    /// </summary>
    public interface IPrimitive :
        ICurrentPrimitiveBehaviour
    {
        /// <summary>
        /// Gets Current Primitive Topology.
        /// </summary>
        PrimitiveTopology PrimitiveTopology { get; }
    }

    /// <summary>
    /// <see cref="IPrimitive"/> which knows about its <see cref="Material"/>.
    /// </summary>
    /// <typeparam name="TMaterial">material type of primitive.</typeparam>
    public interface IPrimitive<out TMaterial> :
        IPrimitive,
        ICurrentMaterial<TMaterial>
        where TMaterial : IMaterial
    {
    }

    /// <summary>
    /// <see cref="IPrimitive{TMaterial}"/> which knows about its type of <see cref="TVertex"/>.
    /// </summary>
    /// <typeparam name="TMaterial">material type of primitive.</typeparam>
    /// <typeparam name="TVertex">vertex type of primitive.</typeparam>
    public interface IPrimitive<out TMaterial, out TVertex> :
        IPrimitive<TMaterial>,
        IVertexes<TVertex>
        where TMaterial : IMaterial
        where TVertex : IVertex
    {
    }
}
