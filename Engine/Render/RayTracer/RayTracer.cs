//-----------------------------------------------------------------------
// <copyright file="RayTracer.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine.RayTracer
{
    using System;

    /// <summary>
    /// RayTracer Render Port.
    /// </summary>
    public class RayTracer : Render.RenderBase
    {
        /// <summary>
        /// Raytracing Render constructor.
        /// </summary>
        /// <param name="hosthandle">handle for form.</param>
        public RayTracer(IntPtr hosthandle)
    : base(hosthandle)
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
