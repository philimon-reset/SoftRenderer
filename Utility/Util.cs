//-----------------------------------------------------------------------
// <copyright file="Utility.cs" company="SoftEngine">
// Copyright (c) SoftEngine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Utility.Util
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using SoftRenderer.Utility.Win32;

    /// <summary>
    /// Utility functions for managing the view ports.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Get client rectangle.
        /// </summary>
        /// <param name="handle">form handle.</param>
        /// <returns> a new rectangle representing the given form handle.</returns>
        public static Rectangle GetClientRectangle(IntPtr handle)
        {
            DLL.ClientToScreen(handle, out Point point);
            DLL.GetClientRect(handle, out Rectangle rect);
            return new Rectangle(point.X, point.Y, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        /// <summary>
        /// get handler for form.
        /// </summary>
        /// <param name="window">window form control.</param>
        /// <returns>handle of form.</returns>
        public static IntPtr Handle(System.Windows.Forms.Control window)
        {
            return window.IsDisposed ? default : window.Handle;
        }

        /// <summary>
        /// Get form from handle.
        /// </summary>
        /// <param name="handle">handle of form.</param>
        /// <returns>form instance.</returns>
        public static Control GetForm(IntPtr handle)
        {
            return handle == IntPtr.Zero ? default : Control.FromHandle(handle);
        }
    }
}
