// <copyright file="SizeChangeArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace SoftRenderer.Engine.Input.EventArgs;
using System.Drawing;
public record struct SizeChangeArgs : ISizeChangeArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SizeChangeArgs"/> class.
    /// </summary>
    /// <param name="newSize">new size of form.</param>
    public SizeChangeArgs(Size newSize) => this.NewSize = newSize;
    /// <inheritdoc/>
    public Size NewSize { get; }
}
