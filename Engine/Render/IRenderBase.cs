//-----------------------------------------------------------------------
// <copyright file="IRenderBase.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Engine.Render
{
    using System;
    using System.Drawing;
    using Camera;
    using Client;
    using Input;

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
        /// Gets total fps data.
        /// </summary>
        FpsCounter RendererFps { get; }

        /// <summary>
        /// Gets start of the render.
        /// </summary>
        DateTime FrameStart { get; }

        /// <summary>
        /// Gets input handle for form.
        /// </summary>
        IInput HostInput { get; }

        /// <summary>
        /// Gets size of the entire form.
        /// </summary>
        Size FormSize { get; }

        /// <summary>
        /// Gets camera info alongside buffer info.
        /// </summary>
        CameraInfo MyCameraInfo { get; }

        /// <summary>
        /// Initialize common rendering steps.
        /// </summary>
        void Render();

        /// <summary>
        /// Render the current frame for current rendering techinque.
        /// </summary>
        abstract void RenderInternal();
    }
}
