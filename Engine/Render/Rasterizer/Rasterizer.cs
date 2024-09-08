//-----------------------------------------------------------------------
// <copyright file="Rasterizer.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine.Rasterizer
{
    using System;

    /// <summary>
    /// Rasterizer Render Port.
    /// </summary>
    public class Rasterizer : Render.RenderBase
    {
        /// <summary>
        /// Rasterization Render constructor.
        /// </summary>
        /// <param name="hosthandle">handle for form.</param>
        public Rasterizer(IntPtr hosthandle)
    : base(hosthandle)
        {
        }

        /// <summary>
        /// dispose function for rasterizer.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
