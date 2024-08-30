//-----------------------------------------------------------------------
// <copyright file="SoftRendererForm.cs" company="SoftEngine">
// Copyright (c) SoftEngine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRendererForm
{
    using SoftRenderer.Client;
    using SoftRenderer.Engine;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// soft renderer form window.
    /// </summary>
    public partial class SoftRendererForm : Form
    {
        #pragma warning disable SA1642
        /// <summary>
        /// Initializes a new instance of the<see cref="SoftRendererForm"/> class.
        /// </summary>
        public SoftRendererForm()
        {
            this.InitializeComponent();
            //this.RenderBases = WindowFactory.RenderBaseSeed();
        }

        /// <summary>
        /// Gets or sets renderBase list.
        /// </summary>
        //public IReadOnlyList<IRenderBase> RenderBases { get; set; }

        private void SoftRendererForm_Load(object sender, EventArgs e)
        {
            new System.Windows.Window().Show();
        }

        private void SoftRendererForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush b = new SolidBrush(Color.Cyan);
            for (int i = 0; i < 100; i++)
            {
                g.FillRectangle(b, i, i, 1, 1);
            }
        }
    }
}