//-----------------------------------------------------------------------
// <copyright file="Rasterizer.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine.Rasterizer
{
    using SoftRenderer.Engine.Render;
    using System;

    /// <summary>
    /// Rasterizer Render Port.
    /// </summary>
    public class Rasterizer : Render.RenderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rasterizer"/> class.
        /// Rasterization Render constructor.
        /// </summary>
        /// <param name="renderBaseArgs">dto that hold form handles.</param>
        public Rasterizer(IRenderBaseArgs renderBaseArgs)
    : base(renderBaseArgs)
        {
        }
    }
}
