﻿//-----------------------------------------------------------------------
// <copyright file="SoftRendererForm.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace SoftRenderer.Client
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using SoftRenderer.Client;
    using SoftRenderer.Engine;
    using SoftRenderer.Engine.Rasterizer;
    using SoftRenderer.Engine.RayTracer;
    using SoftRenderer.Engine.Render.Canvas;
    using SoftRenderer.Utility.Win32;

    /// <summary>
    /// soft renderer form window.
    /// </summary>
    public class SoftRendererForm : Form, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoftRendererForm"/> class.
        /// </summary>
        public SoftRendererForm()
        {
            // this.RasterBase = WindowFactory.RenderBaseSeed<Rasterizer>(0);
            // this.RayTraceBase = WindowFactory.RenderBaseSeed<RayTracer>(1);
            this.CanvasBase = WindowFactory.RenderBaseSeed<Canvas>(2);
            this.RenderBases = new IRenderBase[] { this.CanvasBase };
            while (Application.OpenForms.Count >= 1)
            {
                this.Render();
                Application.DoEvents();
            }
        }

        private IEnumerable<IRenderBase> RenderBases { get; set; }

        /// <summary>
        /// Gets or sets raster port.
        /// </summary>
        private Rasterizer RasterBase { get; set; }

        /// <summary>
        /// Gets or sets raytracer port.
        /// </summary>
        private RayTracer RayTraceBase { get; set; }

        /// <summary>
        /// Gets or sets Canvas port.
        /// </summary>
        private Canvas CanvasBase { get; set; }

        /// <summary>
        /// Render all the available bases.
        /// </summary>
        public void Render()
        {
            foreach (var renderBase in this.RenderBases)
            {
                if (DLL.IsWindow(renderBase.HostHandle))
                {
                    renderBase.Render();
                }
            }
        }

        /// <summary>
        /// Dispose RenderBases and set RenderBases list as default.
        /// </summary>
        public new void Dispose()
        {
            this.RasterBase.Dispose();
            this.RayTraceBase.Dispose();
            this.CanvasBase.Dispose();
            this.RayTraceBase = default;
            this.RasterBase = default;
            this.CanvasBase = default;
        }
    }
}