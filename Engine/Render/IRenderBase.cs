//-----------------------------------------------------------------------
// <copyright file="IRenderBase.cs" company="SoftEngine">
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

        /// <summary>
        /// Render the current frame for current rendering techinque.
        /// </summary>
        void Render();

        /// <summary>
        /// Initalize common rendering steps.
        /// </summary>
        void RenderInternal();
    }
}
