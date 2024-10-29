namespace SoftRenderer.Utility
{
    using System;
    using System.Data.SqlTypes;
    using Math;

    /// <summary>
    /// Class to construct a plane.
    /// </summary>
    /// <remarks> gotten from https://github.com/mathnet/mathnet-spatial/blob/master/src/Spatial/Euclidean/Plane.cs#L272.</remarks>
    public struct Plane
    {
        /// <summary>
        /// The normal vector of the Plane.
        /// </summary>
        public readonly Vector3 Normal;

        /// <summary>
        /// The distance to the Plane along its normal from the origin.
        /// </summary>
        public readonly double OffSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> struct.
        /// Constructs a Plane from the X, Y, and Z components of its normal, and its distance from the origin on that normal.
        /// </summary>
        /// <param name="x">The X-component of the normal.</param>
        /// <param name="y">The Y-component of the normal.</param>
        /// <param name="z">The Z-component of the normal.</param>
        /// <param name="d">The distance of the Plane along its normal from the origin.</param>
        public Plane(double x, double y, double z, double d)
            : this(new Vector3(x, y, z), -d)
        {
        }

        /// <summary>
        /// Gets the point on the plane closest to origin.
        /// </summary>
        public Vector3 RootPoint
        {
            get => -this.OffSet * this.Normal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> struct.
        /// Constructs a Plane from the given normal and distance along the normal from the origin.
        /// </summary>
        /// <param name="normal">The Plane's normal vector.</param>
        /// <param name="offset">The Plane's distance from the origin along its normal vector.</param>
        public Plane(Vector3 normal, double offset = 0)
        {
            this.Normal = normal;
            this.OffSet = -offset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> struct.
        /// Constructs a Plane from the given normal and distance along the normal from the origin.
        /// </summary>
        /// <param name="normal">The Plane's normal vector.</param>
        /// <param name="rootPoint">A point in the plane.</param>
        public Plane(Vector3 rootPoint, Vector3 normal)
            : this(normal, normal.Dot(rootPoint))
        {
        }

        /// <summary>
        /// Get the distance to the point along the <see cref="Normal"/>.
        /// </summary>
        /// <param name="point">The <see cref="Vector3"/>.</param>
        /// <returns>The distance.</returns>
        public double SignedDistanceTo(Vector3 point)
        {
            var p = this.Project(point);
            var v = point - p;
            return v.Dot(this.Normal);
        }

        /// <summary>
        /// Get the distance to the point.
        /// </summary>
        /// <param name="point">The <see cref="Vector3"/>.</param>
        /// <returns>The distance.</returns>
        public double AbsoluteDistanceTo(Vector3 point)
        {
            return Math.Abs(this.SignedDistanceTo(point));
        }

        /// <summary>
        /// Projects a point onto the plane.
        /// </summary>
        /// <param name="p">A point.</param>
        /// <param name="projectionDirection">The direction of projection.</param>
        /// <returns>a projected point.</returns>
        public Vector3 Project(Vector3 p, Vector3? projectionDirection = null)
        {
            var direction = projectionDirection ?? this.Normal;
            var distance = (this.RootPoint - p).Dot(this.Normal) / direction.Dot(this.Normal);
            return p + (distance * direction);
        }

        /// <summary>
        /// http://www.cs.princeton.edu/courses/archive/fall00/cs426/lectures/raycast/sld017.htm.
        /// </summary>
        /// <param name="ray">A ray.</param>
        /// <param name="tolerance">A tolerance (epsilon) to account for floating point error.</param>
        /// <returns>The point of intersection.</returns>
        public Vector3 IntersectionWith(Ray ray, double tolerance = float.Epsilon)
        {
            var d = this.SignedDistanceTo(ray.Source);
            var t = -1 * d / ray.Direction.Dot(this.Normal);
            return ray.Source + (t * ray.Direction);
        }
    }
}
