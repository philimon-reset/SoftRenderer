namespace SoftRenderer.Engine
{
    using System;

    /// <summary>
    /// Interface for RenderBase.
    /// </summary>
    internal interface IRenderBase : IDisposable
    {
        /// <summary>
        /// Gets Handle for the windows form.
        /// </summary>
        IntPtr HostHandle { get; }
    }
}
