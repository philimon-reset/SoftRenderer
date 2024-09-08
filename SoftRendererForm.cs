﻿//-----------------------------------------------------------------------
// <copyright file="SoftRendererForm.cs" company="SoftEngine">
// Copyright (c) SoftEngine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRendererForm
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Windows.Threading;
    using SoftRenderer.Client.Win32.DLL;
    using SoftRenderer.Client.WindowFactory;
    using SoftRenderer.Engine;
    using SoftRenderer.Engine.Rasterizer;
    using SoftRenderer.Engine.RayTracer;

    /// <summary>
    /// soft renderer form window.
    /// </summary>
    public class SoftRendererForm : Form, IDisposable
    {
#pragma warning disable SA1642
        /// <summary>
        /// Initializes a new instance of the<see cref="SoftRendererForm"/> class.
        /// </summary>
        public SoftRendererForm()
        {
            //this.InitializeComponent();
            this.RasterBase = WindowFactory.RenderBaseSeed<Rasterizer>(0);
            //this.RayTraceBase = WindowFactory.RenderBaseSeed<RayTracer>(1);
            this.RenderBases = new IRenderBase[] { this.RasterBase, this.RayTraceBase };
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
        /// Render all the available bases.
        /// </summary>
        public void Render()
        {
            if (DLL.IsWindow(this.RasterBase.HostHandle))
            {
                this.RasterBase.Render();
            }

            //if (DLL.IsWindow(this.RayTraceBase.HostHandle))
            //{
            //    this.RayTraceBase.Render();
            //}
        }

        /// <summary>
        /// Dispose RenderBases and set RenderBases list as default.
        /// </summary>
        public new void Dispose()
        {
            this.RasterBase.Dispose();
            this.RayTraceBase.Dispose();
            this.RayTraceBase = default;
            this.RasterBase = default;
        }

        //private void SoftRendererForm_Paint(object sender, PaintEventArgs e)
        //{
        //    // Graphics g = e.Graphics;
        //    // Brush b = new SolidBrush(Color.Cyan);
        //    // for (int i = 0; i < 100; i++)
        //    // {
        //    //     g.FillRectangle(b, i, i, 1, 1);
        //    // }
        //}
    }
}