//-----------------------------------------------------------------------
// <copyright file="RenderBase.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine.Render
{
    using System;

    /// <summary>
    /// Represents a vector in three-dimensional space.
    /// </summary>
    /// <remarks>
    /// This class provides properties and methods to perform vector operations.
    /// </remarks>
    public class RenderBase : IRenderBase
    {
        /// <summary>
        /// Constructor for RenderBase.
        /// </summary>
        /// <param name="hosthandle"> Given host handle </param>
        public RenderBase(IntPtr hosthandle)
        {
            this.HostHandle = hosthandle;
        }

        /// <summary>
        /// Gets Handle for the windows form.
        /// </summary>
        public IntPtr HostHandle { get; private set; }

        /// <summary>
        /// Sets host handle as default.
        /// </summary>
        public void Dispose()
        {
            this.HostHandle = default;
        }
    }
}
