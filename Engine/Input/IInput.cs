// <copyright file="IInput.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>
namespace SoftRenderer.Engine.Input
{
    using System;
    using System.Windows.Forms;
    using SoftRenderer.Engine.Input.EventArgs;

    /// <summary>
    /// Input Class to handle user events.
    /// </summary>
    public interface IInput : IDisposable
    {
        /// <summary>
        /// Delegate to handle form size change.
        /// </summary>
        /// <param name="sender">sender arg.</param>
        /// <param name="args">event specific arg.</param>
        public delegate void SizeChangedEventHandler(object sender, ISizeChangeArgs args);

        /// <summary>
        /// Event when form size changes.
        /// </summary>
        public event SizeChangedEventHandler SizeChanged;

        /// <summary>
        /// Event when MouseMove.
        /// </summary>
        event MouseEventHandler MouseMove;

        /// <summary>
        /// Event when MousePress.
        /// </summary>
        event MouseEventHandler MousePress;

        /// <summary>
        /// Event when MouseRelease.
        /// </summary>
        event MouseEventHandler MouseRelease;

        /// <summary>
        /// Event when MouseScroll.
        /// </summary>
        event MouseEventHandler MouseScroll;

        /// <summary>
        /// Event when KeyDown.
        /// </summary>
        event KeyEventHandler KeyDown;

        /// <summary>
        /// Event when KeyUp.
        /// </summary>
        event KeyEventHandler KeyUp;

        /// <summary>
        /// Gets size of form.
        /// </summary>
        public System.Drawing.Size Size { get; }
    }
}
