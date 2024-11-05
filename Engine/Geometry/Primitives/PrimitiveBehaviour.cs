namespace SoftRenderer.Engine.Geometry.Primitives
{
    using SoftRenderer.Utility;

    public record struct PrimitiveBehaviour
    {
        /// <inheritdoc cref="Space"/>
        public Space Space { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveBehaviour"/> class.
        /// Defines how a primitive Reacts to ins current space.
        /// <param name="space">space current.</param>
        /// </summary>
        public PrimitiveBehaviour(Space space)
        {
            this.Space = space;
        }

        /// <summary>
        /// Get default primitive behaviour.
        /// </summary>
        public static PrimitiveBehaviour Default = new PrimitiveBehaviour(Space.World);

    }
}
