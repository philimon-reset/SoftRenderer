//-----------------------------------------------------------------------
// <copyright file="RayTracer.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine.RayTracer
{
    using SoftRenderer.Engine.Render;

    /// <summary>
    /// RayTracer Render Port.
    /// </summary>
    public class RayTracer : Render.RenderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RayTracer"/> class.
        /// Raytracing Render constructor.
        /// </summary>
        /// <param name="renderBaseArgs">handle for form.</param>
        public RayTracer(IRenderBaseArgs renderBaseArgs)
    : base(renderBaseArgs)
        {
        }

        /// <summary>
        /// dispose function for ray tracer.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
