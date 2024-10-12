// <copyright file="Input.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>
namespace SoftRenderer.Engine.Input
{
    using System;
    using System.Windows.Forms;

    /// <inheritdoc/>
    public abstract class Input : IInput
    {
        /// <inheritdoc/>
        public abstract event IInput.SizeChangedEventHandler SizeChanged;

        /// <inheritdoc/>
        public abstract event MouseEventHandler MouseMove;

        /// <inheritdoc/>
        public abstract event MouseEventHandler MousePress;

        /// <inheritdoc/>
        public abstract event MouseEventHandler MouseRelease;

        /// <inheritdoc/>
        public abstract event MouseEventHandler MouseScroll;

        /// <inheritdoc/>
        public abstract event KeyEventHandler KeyDown;

        /// <inheritdoc/>
        public abstract event KeyEventHandler KeyUp;

        /// <inheritdoc/>
        public abstract System.Drawing.Size Size { get; }

        /// <summary>
        /// Dispose Input instance.
        /// </summary>
        public abstract void Dispose();
    }
}
