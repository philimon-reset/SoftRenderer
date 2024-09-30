//-----------------------------------------------------------------------
// <copyright file="IRenderBase.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine
{
    using System;

    /// <summary>
    /// Interface for RenderBase.
    /// </summary>
    public interface IRenderBase : IDisposable
    {
        /// <summary>
        /// Gets Handle for the windows form.
        /// </summary>
        IntPtr HostHandle { get; }

        /// <summary>
        /// Initalize common rendering steps.
        /// </summary>
        void Render();

        /// <summary>
        /// Render the current frame for current rendering techinque.
        /// </summary>
        abstract void RenderInternal();
    }
}
