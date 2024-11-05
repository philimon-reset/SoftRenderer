namespace SoftRenderer.Utility
{
    /// <summary>
    /// Graphics context space.
    /// </summary>
    public enum Space
    {
        /// <summary>
        /// World space. Operating in world units.
        /// </summary>
        World,

        /// <summary>
        /// View space or NDC (Normalized Device Coordinates). Operating in NDC units (usually [-1..1]).
        /// </summary>
        View,

        /// <summary>
        /// Screen space. Operating in pixel units.
        /// </summary>
        Screen,
    }
}
