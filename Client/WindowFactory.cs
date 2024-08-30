//-----------------------------------------------------------------------
// <copyright file="WindowFactory.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Client
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using SoftRenderer.Engine;
    using SoftRenderer.Engine.Render;

    /// <summary>
    /// Creates windows for renderbase.
    /// </summary>
    public static class WindowFactory
    {
        /// <summary>
        /// create a list of renderbases each defined to forms.
        /// </summary>
        /// <returns>
        /// List of renderbases.
        /// </returns>
        public static IReadOnlyList<IRenderBase> RenderBaseSeed()
        {
            var size = new System.Drawing.Size(720, 480);
            var renderHost = new[]
            {
                CreateRenderBaseForm(size, "Rasterizer"),
            };
            return renderHost;
        }

        /// <summary>
        /// Create a windows form and set it to a renderbase.
        /// </summary>
        /// <param name="size">size of form.</param>
        /// <param name="title">title of form.</param>
        private static IRenderBase CreateRenderBaseForm(System.Drawing.Size size, string title)
        {
            var window = new System.Windows.Forms.Form
            {
                Size = size,
                Text = title,
            };

            var hostControl = new System.Windows.Forms.Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.Transparent,
                ForeColor = System.Drawing.Color.Transparent,
            };
            hostControl.MouseEnter += (sender, args) =>
            {
                if (System.Windows.Forms.Form.ActiveForm != window) { window.Activate(); };
                if (!hostControl.Focused) { hostControl.Focus(); };
            };
            window.FormClosed += (sender, args) =>
            {
                System.Windows.Application.Current.Shutdown();
            };
            window.Show();
            return new RenderBase(hostControl.Handle);
        }
    }
}
