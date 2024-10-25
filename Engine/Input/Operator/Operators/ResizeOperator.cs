// <copyright file="ResizeOperator.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>

namespace SoftRenderer.Engine.Input.Operator.Operators
{
    using System;
    using System.Drawing;
    using SoftRenderer.Engine.Input.EventArgs;
    using SoftRenderer.Engine.Render;

    /// <summary>
    /// Constructor for ResizeOperator.
    /// </summary>
    /// <param name="renderBase">render base instance.</param>
    /// <param name="resizeHost">resize host delegate.</param>
    /// <param name="resizeClientBuffer">resize client buffer delegate.</param>
    public class ResizeOperator(RenderBase renderBase, Action<Size> resizeHost, Action<Size> resizeClientBuffer) : Operator(renderBase)
    {
        /// <summary>
        /// Resize factor for draw buffer to client view buffer.
        /// </summary>
        public const int ResizeFactor = 1;

        /// <summary>
        /// Resize client buffer and host size.
        /// </summary>
        /// <param name="renderBase">render base instance.</param>
        /// <param name="newSize">new size of host.</param>
        /// <param name="resizeHost">resize host delegate.</param>
        /// <param name="resizeClientBuffer">resize client buffer delegate.</param>
        public static void ResizeBuffers(IRenderBase renderBase, Size newSize, Action<Size> resizeHost, Action<Size> resizeClientBuffer)
        {
            if (newSize.Width < 1 || newSize.Height < 1)
            {
                newSize = new Size(1, 1);
            }

            resizeHost(newSize);
            resizeClientBuffer(newSize);
        }

        /// <inheritdoc/>
        protected override void ControlSizeChanged(object sender, ISizeChangeArgs e)
        {
            base.ControlSizeChanged(sender, e);
            ResizeBuffers(this.RenderBase, e.NewSize, resizeHost, resizeClientBuffer);
        }
    }
}
