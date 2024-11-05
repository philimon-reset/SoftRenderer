namespace SoftRenderer.Engine.Geometry.Primitives
{
    /// <summary>
    /// Current Primitive behaviour.
    /// </summary>
    public interface ICurrentPrimitiveBehaviour
    {
        /// <inheritdoc cref="PrimitiveBehaviour"/>
        PrimitiveBehaviour PrimitiveBehaviour { get; }
    }
}
