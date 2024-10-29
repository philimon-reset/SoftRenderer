// <copyright file="Ray.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SoftRenderer.Utility
{
    using SoftRenderer.Math;

    /// <summary>
    /// Initalize Ray struct.
    /// </summary>
    /// <param name="direction">direction of ray.</param>
    /// <param name="source">source of ray.</param>
    public struct Ray(Vector3 direction, Vector3 source)
    {
        /// <summary>
        /// Source of Ray.
        /// </summary>
        public Vector3 Source = source;

        /// <summary>
        /// Direction of Ray.
        /// </summary>
        public Vector3 Direction = direction;
    }
}
