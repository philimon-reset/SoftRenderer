//-----------------------------------------------------------------------
// <copyright file="FPSCounter.cs" company="SoftRenderer">
// Copyright (c) SoftRenderer. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Client
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// class to count frames.
    /// </summary>
    public class FpsCounter : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FpsCounter"/> class.
        /// Constructor to count frames.
        /// </summary>
        /// <param name="frameRate">chosen frame rate.</param>
        public FpsCounter(TimeSpan frameRate)
        {
            this.FrameRate = frameRate;
            this.SWFrame = new Stopwatch();
            this.SWUpdate = new Stopwatch();
            this.SWUpdate.Start();
            this.Elapsed = TimeSpan.Zero;
            this.Frames = 0;
        }

        /// <summary>
        /// GETs render loop average fps counter.
        /// </summary>
        public double LocalFPS { get; private set; }

        /// <summary>
        /// Gets total render process fps counter.
        /// </summary>
        public double GlobalFPS { get; private set; }

        private TimeSpan FrameRate { get; set; }

        private int Frames { get; set; }

        private TimeSpan Elapsed { get; set; }

        private Stopwatch SWFrame { get; set; }

        private Stopwatch SWUpdate { get; set; }

        /// <summary>
        /// Start of render loop.
        /// </summary>
        public void StartFrame()
        {
            this.SWFrame.Restart();
        }

        /// <summary>
        /// End of render loop.
        /// </summary>
        public void EndFrame()
        {
            this.SWFrame.Stop();
            this.Elapsed += this.SWFrame.Elapsed;
            this.Frames++;
            TimeSpan totalElapsed = this.SWUpdate.Elapsed;
            if (totalElapsed >= this.FrameRate)
            {
                this.LocalFPS = this.Frames / this.Elapsed.TotalSeconds;
                this.GlobalFPS = this.Frames / totalElapsed.TotalSeconds;
                this.SWUpdate.Restart();
                this.Elapsed = TimeSpan.Zero;
                this.Frames = 0;
            }
        }

        /// <summary>
        /// dispose function for rasterizer.
        /// </summary>
        public void Dispose()
        {
            this.SWUpdate.Stop();
            this.SWFrame.Stop();
            this.SWUpdate = default;
            this.SWFrame = default;
        }

        /// <summary>
        /// String representation of fps data.
        /// </summary>
        /// <returns>local fps and global fps data.</returns>
        public override string ToString()
        {
            return $"{this.LocalFPS:F3} ({this.GlobalFPS:F3})";
        }
    }
}
