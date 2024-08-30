//-----------------------------------------------------------------------
// <copyright file="vector.cs" company="SoftEngine">
// Copyright (c) SoftEngine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Math
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a vector in three-dimensional space.
    /// </summary>
    /// <remarks>
    /// This class provides properties and methods to perform vector operations.
    /// </remarks>
    public class Vector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        public Vector()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class with the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        public Vector(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class with the coordinates of another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        public Vector(Vector other)
        {
            this.X = other.X;
            this.Y = other.Y;
            this.Z = other.Z;
        }

        /// <summary>
        /// Gets the X-axis vector.
        /// </summary>
        public static Vector XAxis
        {
            get => new Vector(1, 0, 0);
        }

        /// <summary>
        /// Gets the Y-axis vector.
        /// </summary>
        public static Vector YAxis
        {
            get => new Vector(0, 1, 0);
        }

        /// <summary>
        /// Gets the Z-axis vector.
        /// </summary>
        public static Vector ZAxis
        {
            get => new Vector(0, 0, 1);
        }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        public double Length
        {
            get
            {
                return this.GetLength();
            }
        }

        /// <summary>
        /// Gets or sets the X coordinate of the vector.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the vector.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the Z coordinate of the vector.
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <param name="i">The index.</param>
        /// <returns>The value at the specified index.</returns>
        public double this[int i]
        {
            get
            {
                if (i == 0)
                {
                    return this.X;
                }
                else if (i == 1)
                {
                    return this.Y;
                }
                else if (i == 2)
                {
                    return this.Z;
                }

                throw new Exception();
            }

            set
            {
                if (i == 0)
                {
                    this.X = value;
                }
                else if (i == 1)
                {
                    this.Y = value;
                }
                else if (i == 2)
                {
                    this.Z = value;
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// Calculates the sum of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static Vector Addition(Vector a, Vector b)
        {
            double newX = a.X + b.X;
            double newY = a.Y + b.Y;
            double newZ = a.Z + b.Z;
            Vector v = new Vector(newX, newY, newZ);
            return v;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public static double DotProduct(Vector a, Vector b)
        {
            return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The cross product of the two vectors.</returns>
        public static Vector CrossProduct(Vector a, Vector b)
        {
            double x = (a.Y * b.Z) - (a.Z * b.Y);
            double y = (a.Z * b.X) - (a.X * b.Z);
            double z = (a.X * b.Y) - (a.Y * b.X);
            return new Vector(x, y, z);
        }

        /// <summary>
        /// Returns a string representation of the vector.
        /// </summary>
        /// <returns>A string representation of the vector.</returns>
        public override string ToString()
        {
            return $"[{this.X}, {this.Y}, {this.Z}]";
        }

        /// <summary>
        /// Reverses the direction of the vector.
        /// </summary>
        public void Reverse()
        {
            this.X = -this.X;
            this.Y = -this.Y;
            this.Z = -this.Z;
        }

        /// <summary>
        /// Scales the vector by the specified factor.
        /// </summary>
        /// <param name="factor">The scaling factor.</param>
        public void Scale(double factor)
        {
            this.X *= factor;
            this.Y *= factor;
            this.Z *= factor;
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        /// <returns>True if the vector was successfully converted into a unit vector, otherwise false.</returns>
        public bool Unitize()
        {
            double len = this.GetLength();
            if (len <= 0)
            {
                return false;
            }

            this.X /= len;
            this.Y /= len;
            this.Z /= len;
            return true;
        }

        /// <summary>
        /// Adds another vector to this vector.
        /// </summary>
        /// <param name="other">The vector to add.</param>
        public void Add(Vector other)
        {
            this.X += other.X;
            this.Y += other.Y;
            this.Z += other.Z;
        }

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        private double GetLength()
        {
            double sql = (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z);
            double len = Math.Sqrt(sql);
            return len;
        }
    }
}
