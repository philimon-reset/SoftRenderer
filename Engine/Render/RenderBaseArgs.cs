// <copyright file="RenderBaseArgs.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>

namespace SoftRenderer.Engine.Render
{
    using System;
    using SoftRenderer.Engine.Input;

    /// <summary>
    /// Class to define RenderBaseConstructor Args
    /// Try to use records in the future if possible.
    /// Data creation argument holder for <see cref="IRenderBase"/>.
    /// </summary>.
    public record RenderBaseArgs : IRenderBaseArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderBaseArgs"/> class.
        /// </summary>
        /// <param name="hostHandle"> hosthandle for form.</param>
        /// <param name="input"> input handle for form. </param>
        public RenderBaseArgs(IntPtr hostHandle, IInput input)
        {
            this.HostHandle = hostHandle;
            this.Input = input;
        }

        /// <inheritdoc cref="IRenderBase.HostHandle"/>
        public IntPtr HostHandle { get; set; }

        /// <inheritdoc cref="IInput"/>
        public IInput Input { get; set; }
    }
}
