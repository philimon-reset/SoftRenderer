//-----------------------------------------------------------------------
// <copyright file="WindowFactory.cs" company="SoftEngine">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Client.WindowFactory
{
    using System;
    using System.Windows.Forms;
    using SoftRenderer.Engine;
    using SoftRenderer.Engine.Rasterizer;
    using SoftRenderer.Engine.RayTracer;

    /// <summary>
    /// Creates windows for renderbase.
    /// </summary>
    public static class WindowFactory
    {
        /// <summary>
        /// create a viewport for a specific rendering technique.
        /// </summary>
        /// <param name="type">technique type for rendering.</param>
        /// <typeparam name="T">which rendering techinque return type to use.</typeparam>
        /// <returns> a renderbase (viewport). </returns>
        public static T RenderBaseSeed<T>(int type)
        {
            var size = new System.Drawing.Size(720, 480);
            IRenderBase renderBase;
            if (type == 0)
            {
                renderBase = CreateRenderBaseForm(size, "Rasterizer", (x) => new Rasterizer(x));
            }
            else
            {
                // TODO: Create ray tracer viewport
                renderBase = CreateRenderBaseForm(size, "RayTracer", (x) => new RayTracer(x));
            }

            return (T)Convert.ChangeType(renderBase, typeof(T));
        }

        /// <summary>
        /// Create a windows form and set it to a renderbase.
        /// </summary>
        /// <param name="size">size of form.</param>
        /// <param name="title">title of form.</param>
        private static T CreateRenderBaseForm<T>(System.Drawing.Size size, string title, Func<IntPtr, T> createRenderBase)
        {
            var window = new Form
            {
                Size = size,
                Text = title,
            };

            var hostControl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.MediumPurple,
                ForeColor = System.Drawing.Color.Transparent,
            };
            window.Controls.Add(hostControl);
            hostControl.MouseEnter += OnMouseEnter(window, hostControl);
            window.Closed += OnWindowClosed();
            window.Show();
            return createRenderBase(hostControl.Handle);
        }

        private static EventHandler OnWindowClosed()
        {
            return (sender, args) =>
            {
                System.Windows.Application.Current?.Shutdown();
            };
        }

        private static EventHandler OnMouseEnter(Form window, Panel hostControl)
        {
            return (sender, args) =>
            {
                if (Form.ActiveForm != window)
                {
                    window.Activate();
                }

                if (!hostControl.Focused)
                {
                    hostControl.Focus();
                }
            };
        }
    }
}
