// <copyright file="IRenderBaseArgs.cs" company="SoftRenderer">
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
    public interface IRenderBaseArgs
    {
        /// <inheritdoc cref="IRenderBase.HostHandle"/>
        IntPtr HostHandle { get; set; }
        /// <inheritdoc cref="IRenderBase.HostInput"/>
        IInput Input { get; set; }
    }
}
