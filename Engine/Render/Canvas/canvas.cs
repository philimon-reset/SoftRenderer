//-----------------------------------------------------------------------
// <copyright file="Canvas.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine.Render.Canvas
{
    using SoftRenderer.Engine.Render;
    using System;

    /// <summary>
    /// Canvas Render Port.
    /// </summary>
    public class Canvas : Render.RenderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// General tomfoolery canvas.
        /// </summary>
        /// <param name="renderBaseArgs">dto that hold form handles.</param>
        public Canvas(IRenderBaseArgs renderBaseArgs)
            : base(renderBaseArgs)
        {
        }
    }
}