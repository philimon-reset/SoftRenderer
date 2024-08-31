//-----------------------------------------------------------------------
// <copyright file="WindowFactory.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                CreateRenderBaseForm(size, "0: Form"),
            };
            return renderHost;
        }

        /// <summary>
        /// create a list of renderbases each defined to forms.
        /// </summary>
        /// <param name="amount">number of renderbases to seed.</param>
        /// <returns> List of renderbases. </returns>
        public static IReadOnlyList<IRenderBase> RenderBaseSeed(int amount)
        {
            var size = new System.Drawing.Size(720, 480);
            RenderBase[] renderBases = new RenderBase[amount];
            for (int i = 1; i <= amount; i++)
            {
                renderBases.Append(CreateRenderBaseForm(size, $"{i}: Form"));
            }

            return renderBases;
        }

        /// <summary>
        /// Create a windows form and set it to a renderbase.
        /// </summary>
        /// <param name="size">size of form.</param>
        /// <param name="title">title of form.</param>
        private static IRenderBase CreateRenderBaseForm(System.Drawing.Size size, string title)
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
            return new RenderBase(hostControl.Handle);
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
