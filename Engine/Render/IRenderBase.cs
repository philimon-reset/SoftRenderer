//-----------------------------------------------------------------------
// <copyright file="IRenderBase.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine
{
    using System;

    /// <summary>
    /// Interface for RenderBase.
    /// </summary>
    public interface IRenderBase : IDisposable
    {
        /// <summary>
        /// Gets Handle for the windows form.
        /// </summary>
        IntPtr HostHandle { get; }
    }
}
