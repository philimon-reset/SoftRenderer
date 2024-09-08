//-----------------------------------------------------------------------
// <copyright file="Rasterizer.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine.Rasterizer
{
    using System;

    /// <summary>
    /// Rasterizer Render Port.
    /// </summary>
    public class Rasterizer : Engine.Render.RenderBase
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
        /// Sets graphics handle to defualt.
        /// </summary>
        public override void Dispose()
        {
            this.GraphicsHandle.Dispose();
            this.GraphicsHandle = default;
        }
    }
}
