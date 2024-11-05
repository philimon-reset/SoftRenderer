// <copyright file="IOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SoftRenderer.Engine.Input.Operator
{
    using System;
    using SoftRenderer.Engine.Input;
    using SoftRenderer.Engine.Render;

    /// <summary>
    /// Class used to inject functionality based on events called.
    /// </summary>
    public interface IOperator : IDisposable
    {
        /// <summary>
        /// Gets render base instance.
        /// </summary>
        RenderBase RenderBase { get; }

        /// <summary>
        /// Gets input control instance.
        /// </summary>
        IInput Input { get; }
    }
}
