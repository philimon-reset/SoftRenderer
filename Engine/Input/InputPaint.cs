// <copyright file="InputControl.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>

using System;

namespace SoftRenderer.Engine.Input;
using System.Windows.Forms;
using EventArgs;

/// <summary>
/// Class to control form input.
/// </summary>
public class InputPaint : InputControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InputPaint"/> class.
    /// </summary>
    /// <param name="formControl">Control point of form.</param>
    public InputPaint(Control formControl)
        : base(formControl)
    {
        this.FormControl = formControl;
        this.MouseMove += this.PaintMouseMove;
        this.MousePress += this.PaintMouseDown;
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        this.MouseMove -= this.PaintMouseMove;
        this.MousePress -= this.PaintMouseDown;
        this.FormControl = default;
    }


    /// <inheritdoc cref="MouseMove" />
    private void PaintMouseMove(object sender, MouseEventArgs e)
    {
        Console.WriteLine($"Mouse move: {e.X}, {e.Y}");
    }

    /// <inheritdoc cref="MouseDown" />
    private void PaintMouseDown(object sender, MouseEventArgs e)
    {
        Console.WriteLine($"Mouse down: {e.X}, {e.Y}");
    }
}