//-----------------------------------------------------------------------
// <copyright file="RayTracer.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SoftRenderer.Engine.Render.Technique.RayTracer
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using SoftRenderer.Client.FPSCounter;
    using SoftRenderer.Utility.Util;

    /// <summary>
    /// RayTracer Render Port.
    /// </summary>
    public class RayTracer : Render.Driver.GDI.RenderBaseGdi
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

        /// <inheritdoc/>
        public override void RenderInternal()
        {
            this.ClientBufferedGraphics.Graphics.Clear(Color.Black);
            this.ClientBufferedGraphics.Graphics.DrawString(this.RendererFps.ToString(), this.FpsFont, Brushes.Yellow, 0, 0);

            this.ClientBufferedGraphics.Render(this.ClientViewBufferHandle);
        }
    }
}
