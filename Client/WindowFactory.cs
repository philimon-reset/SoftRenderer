//-----------------------------------------------------------------------
// <copyright file="WindowFactory.cs" company="SoftRenderer">
//     SoftRenderer.
// </copyright>
//-----------------------------------------------------------------------

using SoftRenderer.Engine.Render.Canvas;

namespace SoftRenderer.Client
{
    using System;
    using System.Windows.Forms;
    using SoftRenderer.Engine;
    using SoftRenderer.Engine.Input;
    using SoftRenderer.Engine.Rasterizer;
    using SoftRenderer.Engine.RayTracer;
    using SoftRenderer.Engine.Render;

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
                renderBase = CreateRenderBaseForm(size, "Rasterizer", (hostControl) => new Rasterizer(new RenderBaseArgs(hostControl.Handle, new InputControl(hostControl))));
            }
            else if (type == 1)
            {
                // TODO: Create ray tracer viewport
                renderBase = CreateRenderBaseForm(size, "RayTracer", (hostControl) => new RayTracer(new RenderBaseArgs(hostControl.Handle, new InputControl(hostControl))));
            }
            else
            {
                renderBase = CreateRenderBaseForm(size, "Canvas", (hostControl) => new Canvas(new RenderBaseArgs(hostControl.Handle, new InputPaint(hostControl))));
            }

            return (T)Convert.ChangeType(renderBase, typeof(T));
        }

        /// <summary>
        /// Create a windows form and set it to a renderbase.
        /// </summary>
        /// <param name="size">size of form.</param>
        /// <param name="title">title of form.</param>
        /// <param name="createRenderBase"> Helper func to instanciate renderbase.</param>
        private static T CreateRenderBaseForm<T>(System.Drawing.Size size, string title, Func<Control, T> createRenderBase)
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
            return createRenderBase(hostControl);
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
