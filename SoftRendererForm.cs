//-----------------------------------------------------------------------
// <copyright file="SoftRendererForm.cs" company="SoftEngine">
// Copyright (c) SoftEngine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRendererForm
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using SoftRenderer.Client;
    using SoftRenderer.Engine;

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
            this.RenderBases = WindowFactory.RenderBaseSeed(2);
        }

        /// <summary>
        /// Gets or sets renderBase list.
        /// </summary>
        private IReadOnlyList<IRenderBase> RenderBases { get; set; }

        /// <summary>
        /// Dispose RenderBases and set RenderBases list as default.
        /// </summary>
        public new void Dispose()
        {
            foreach (var item in this.RenderBases)
            {
                item.Dispose();
            }

            this.RenderBases = default;
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