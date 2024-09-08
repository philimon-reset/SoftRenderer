//-----------------------------------------------------------------------
// <copyright file="SoftRendererForm.cs" company="SoftEngine">
// Copyright (c) SoftEngine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRendererForm
{
    using System;
    using System.Windows.Forms;
    using System.Windows.Threading;
    using SoftRenderer.Client.WindowFactory;
    using SoftRenderer.Engine.Rasterizer;
    using SoftRenderer.Engine.RayTracer;

    /// <summary>
    /// soft renderer form window.
    /// </summary>
    public partial class SoftRendererForm : Form, IDisposable
    {
#pragma warning disable SA1642
        /// <summary>
        /// Initializes a new instance of the<see cref="SoftRendererForm"/> class.
        /// </summary>
        public SoftRendererForm()
        {
            this.InitializeComponent();
            this.RasterBase = WindowFactory.RenderBaseSeed<Rasterizer>(0);
            this.RayTraceBase = WindowFactory.RenderBaseSeed<RayTracer>(1);
            while (!Dispatcher.CurrentDispatcher.HasShutdownStarted)
            {
                this.Render();
                Application.DoEvents();
            }

            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].Name != "Menu")
                {
                    Application.OpenForms[i].Close();
                }
            }
        }

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
            if (this.RasterBase.IsActive())
            {
                this.RasterBase.Render();
            }

            if (this.RayTraceBase.IsActive())
            {
                this.RayTraceBase.Render();
            }
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

        private void SoftRendererForm_Load(object sender, EventArgs e)
        {
        }

        private void SoftRendererForm_Paint(object sender, PaintEventArgs e)
        {
            // Graphics g = e.Graphics;
            // Brush b = new SolidBrush(Color.Cyan);
            // for (int i = 0; i < 100; i++)
            // {
            //     g.FillRectangle(b, i, i, 1, 1);
            // }
        }
    }
}