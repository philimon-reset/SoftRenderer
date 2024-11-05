namespace SoftRenderer.Engine.Geometry.Materials
{
    using SoftRenderer.Engine.Materials;

    /// <summary>
    /// Current Material.
    /// </summary>
    /// <typeparam name="TMaterial">material type.</typeparam>
    public interface ICurrentMaterial<out TMaterial>
        where TMaterial : IMaterial
    {
        /// <summary>
        /// Gets current Material.
        /// </summary>
        TMaterial Material { get; }
    }
}
