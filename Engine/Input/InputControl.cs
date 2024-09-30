// <copyright file="InputControl.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>

namespace SoftRenderer.Engine.Input;
using System.Windows.Forms;
using EventArgs;

/// <summary>
/// Class to control form input.
/// </summary>
public class InputControl : Input
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InputControl"/> class.
    /// </summary>
    /// <param name="formControl">Control point of form.</param>
    public InputControl(Control formControl)
    {
        this.FormControl = formControl;
        this.FormControl.SizeChanged += this.ControlSizeChanged;
        this.FormControl.MouseMove += this.ControlMouseMove;
        this.FormControl.MouseDown += this.ControlMouseDown;
        this.FormControl.MouseUp += this.ControlMouseUp;
        this.FormControl.MouseWheel += this.ControlMouseWheel;
        this.FormControl.KeyDown += this.ControlKeyUp;
        this.FormControl.KeyUp += this.ControlKeyDown;
    }

    /// <inheritdoc/>
    public override event IInput.SizeChangedEventHandler SizeChanged;

    /// <inheritdoc/>
    public override event MouseEventHandler MouseMove;

    /// <inheritdoc/>
    public override event MouseEventHandler MousePress;

    /// <inheritdoc/>
    public override event MouseEventHandler MouseRelease;

    /// <inheritdoc/>
    public override event MouseEventHandler MouseScroll;

    /// <inheritdoc/>
    public override event KeyEventHandler KeyDown;

    /// <inheritdoc/>
    public override event KeyEventHandler KeyUp;

    /// <inheritdoc/>
    public override System.Drawing.Size Size
    {
        get => this.FormControl.Size;
    }

    /// <summary>
    /// Gets or sets control for form.
    /// </summary>
    protected Control FormControl { get; set; }

    /// <inheritdoc/>
    public override void Dispose()
    {
        this.FormControl.SizeChanged -= this.ControlSizeChanged;
        this.FormControl.MouseMove -= this.ControlMouseMove;
        this.FormControl.MouseDown -= this.ControlMouseDown;
        this.FormControl.MouseUp -= this.ControlMouseUp;
        this.FormControl.MouseWheel -= this.ControlMouseWheel;
        this.FormControl.KeyDown -= this.ControlKeyUp;
        this.FormControl.KeyUp -= this.ControlKeyDown;
        this.FormControl = default;
    }

    /// <inheritdoc cref="SizeChanged" />
    protected void ControlSizeChanged(object sender, System.EventArgs e) => this.SizeChanged?.Invoke(sender, new SizeChangeArgs(this.FormControl.Size));

    /// <inheritdoc cref="MouseMove" />
    protected void ControlMouseMove(object sender, MouseEventArgs e) => this.MouseMove?.Invoke(sender, e);

    /// <inheritdoc cref="MousePress" />
    protected void ControlMouseDown(object sender, MouseEventArgs e) => this.MousePress?.Invoke(sender, e);

    /// <inheritdoc cref="MouseRelease" />
    protected void ControlMouseUp(object sender, MouseEventArgs e) => this.MouseRelease?.Invoke(sender, e);

    /// <inheritdoc cref="MouseScroll" />
    protected void ControlMouseWheel(object sender, MouseEventArgs e) => this.MouseScroll?.Invoke(sender, e);

    /// <inheritdoc cref="KeyDown" />
    protected void ControlKeyDown(object sender, KeyEventArgs e) => this.KeyDown?.Invoke(sender, e);

    /// <inheritdoc cref="KeyUp" />
    protected void ControlKeyUp(object sender, KeyEventArgs e) => this.KeyUp?.Invoke(sender, e);
}