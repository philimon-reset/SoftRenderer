// <copyright file="ISizeChangeArgs.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>

namespace SoftRenderer.Engine.Input.EventArgs
{
    /// <summary>
    /// Event argument for size change.
    /// </summary>
    public interface ISizeChangeArgs
    {
        /// <summary>
        /// Gets new Size of form.
        /// </summary>
        System.Drawing.Size NewSize { get; }
    }
}
