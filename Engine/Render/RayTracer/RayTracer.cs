//-----------------------------------------------------------------------
// <copyright file="RayTracer.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine.RayTracer
{
    using System;
    using System.Drawing;

    /// <summary>
    /// RayTracer Render Port.
    /// </summary>
    public class RayTracer : Engine.Render.RenderBase
    {
        /// <summary>
        /// Raytracing Render constructor.
        /// </summary>
        /// <param name="hosthandle">handle for form.</param>
        public RayTracer(IntPtr hosthandle)
    : base(hosthandle)
        {
            this.GraphicsHandle = Graphics.FromHwndInternal(this.HostHandle);
        }

        private Graphics GraphicsHandle { get; set; }

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
