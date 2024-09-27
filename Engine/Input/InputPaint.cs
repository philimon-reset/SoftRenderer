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
        this.MouseRelease += this.PaintMouseUp;
        this.MouseScroll += this.PaintMouseScroll;
        this.KeyUp += this.PaintKeyUp;
        this.KeyDown += this.PaintKeyDown;
    }

    private void PaintMouseScroll(object sender, MouseEventArgs e)
    {
        Console.WriteLine($@"Scroll {e.Delta}");
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        this.MouseMove -= this.PaintMouseMove;
        this.MousePress -= this.PaintMouseDown;
        this.MouseRelease -= this.PaintMouseUp;
        this.KeyUp -= this.PaintKeyUp;
        this.KeyDown -= this.PaintKeyDown;
        this.FormControl = default;
    }

    /// <inheritdoc cref="PaintKeyDown"/>
    private void PaintKeyDown(object sender, KeyEventArgs e)
    {
        Console.WriteLine($@"KeyDown: {e.KeyCode} {e.KeyData}");
    }

    /// <inheritdoc cref="PaintKeyUp"/>
    private void PaintKeyUp(object sender, KeyEventArgs e)
    {
        Console.WriteLine($@"KeyDown: {e.KeyCode} {e.KeyData}");
    }

    /// <inheritdoc cref="MouseMove" />
    private void PaintMouseMove(object sender, MouseEventArgs e)
    {
        Console.WriteLine($@"Mouse move: {e.X}, {e.Y}");
    }

    /// <inheritdoc cref="MouseDown" />
    private void PaintMouseDown(object sender, MouseEventArgs e)
    {
        Console.WriteLine($@"Mouse down: {e.X}, {e.Y}");
    }
    
    /// <inheritdoc cref="MouseDown" />
    private void PaintMouseUp(object sender, MouseEventArgs e)
    {
        Console.WriteLine($@"Mouse up: {e.X}, {e.Y}");
    }
}