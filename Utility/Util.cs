//-----------------------------------------------------------------------
// <copyright file="Utility.cs" company="SoftEngine">
// Copyright (c) SoftEngine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Utility.Util
{
    using System;
    using System.Windows;
    using SoftRenderer.Client.Win32.DLL;

    public static class Util
    {
        /// <summary>
        /// Get client rectangle.
        /// </summary>
        /// <param name="handle">form handle.</param>
        /// <returns> a new rectangle representing the given form handle.</returns>
        public static System.Drawing.Rectangle GetClientRectangle(IntPtr handle)
        {
            DLL.ClientToScreen(handle, out Point point);
            DLL.GetClientRect(handle, out Rect rect);
            return new System.Drawing.Rectangle((int)point.X, (int)point.Y, (int)(rect.Right - rect.Left), (int)(rect.Bottom - rect.Top));
        }
    }
}
