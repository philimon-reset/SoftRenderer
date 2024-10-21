//-----------------------------------------------------------------------
// <copyright file="vector.cs" company="SoftEngine">
// Copyright (c) SoftEngine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace SoftRenderer.Math
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// Represents a vector in three-dimensional space.
    /// </summary>
    /// <remarks>
    /// This class provides properties and methods to perform vector operations.
    /// </remarks>
    public class Vector3
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> class.
        /// </summary>
        public Vector3()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> class with the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        public Vector3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> class from an array.
        /// </summary>
        /// <param name="array">Array of values.</param>
        public Vector3(double[] array)
        {
            if (array.Length != 3)
            {
                throw new Exception("Size of array does not match with vector4");
            }

            this.X = array[0];
            this.Y = array[1];
            this.Z = array[2];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> class with the coordinates of another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        public Vector3(Vector3 other)
        {
            this.X = other.X;
            this.Y = other.Y;
            this.Z = other.Z;
        }

        /// <summary>
        /// Gets the normalized matrix.
        /// </summary>
        public Vector3 GetNormalized
        {
            get => Normalize(this);
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets the X-axis vector.
        /// </summary>
        public static Vector3 XAxis
        {
            get => new Vector3(1, 0, 0);
        }

        /// <summary>
        /// Gets the Y-axis vector.
        /// </summary>
        public static Vector3 YAxis
        {
            get => new Vector3(0, 1, 0);
        }

        /// <summary>
        /// Gets the Z-axis vector.
        /// </summary>
        public static Vector3 ZAxis
        {
            get => new Vector3(0, 0, 1);
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
        #endregion

        #region operator_overload

        /// <summary>
        /// Inverses the sign of a vector.
        /// </summary>
        /// <param name="vec">Vector to inverse the sign of.</param>
        /// <returns>The sign inverted vector.</returns>
        public static Vector3 operator -(Vector3 vec) => VectorOperation(vec, '*', -1);

        /// <summary>
        /// Adds a vector and a scalar.
        /// </summary>
        /// <param name="vec">Vector to add to.</param>
        /// <param name="scalar">Scalar to add to the vector.</param>
        /// <returns>The added vector.</returns>
        public static Vector3 operator +(Vector3 vec, double scalar) => VectorOperation(vec, '+', scalar);

        /// <summary>
        /// Adds a vector and a scalar.
        /// </summary>
        /// <param name="vec">Vector to add to.</param>
        /// <param name="scalar">Scalar to add to the vector.</param>
        /// <returns>The added vector.</returns>
        public static Vector3 operator +(double scalar, Vector3 vec) => vec + scalar;

        /// <summary>
        /// Substracts a vector and a scalar.
        /// </summary>
        /// <param name="vec">Vector to substract to.</param>
        /// <param name="scalar">Scalar to substract to the vector.</param>
        /// <returns>The substracted vector.</returns>
        public static Vector3 operator -(Vector3 vec, double scalar) => VectorOperation(vec, '-', scalar);

        /// <summary>
        /// Substracts a vector and a scalar.
        /// </summary>
        /// <param name="vec">Vector to substract to.</param>
        /// <param name="scalar">Scalar to substract to the vector.</param>
        /// <returns>The substracted vector.</returns>
        public static Vector3 operator -(double scalar, Vector3 vec) => vec - scalar;

        /// <summary>
        /// Multiplies a vector and a scalar.
        /// </summary>
        /// <param name="vec">Vector to multiply.</param>
        /// <param name="scalar">Scalar to multiply with the vector.</param>
        /// <returns>The multiplied vector.</returns>
        public static Vector3 operator *(Vector3 vec, double scalar) => VectorOperation(vec, '*', scalar);

        /// <summary>
        /// Multiplies a vector and a scalar.
        /// </summary>
        /// <param name="vec">Vector to multiply.</param>
        /// <param name="scalar">Scalar to multiply with the vector.</param>
        /// <returns>The multiplied vector.</returns>
        public static Vector3 operator *(double scalar, Vector3 vec) => vec * scalar;

        /// <summary>
        /// Adds a vector to another vector.
        /// </summary>
        /// <param name="vec">First vector.</param>
        /// <param name="vec2">second vector.</param>
        /// <returns>The resulting vector.</returns>
        public static Vector3 operator +(Vector3 vec, Vector3 vec2)
        {
            Vector3 newVec = new Vector3();
            newVec.X = vec.X + vec2.X;
            newVec.Y = vec.Y + vec2.Y;
            newVec.Z = vec.Z + vec2.Z;
            return newVec;
        }

        /// <summary>
        /// Subtracts a vector to another vector.
        /// </summary>
        /// <param name="vec">First vector.</param>
        /// <param name="vec2">second vector.</param>
        /// <returns>The resulting vector.</returns>
        public static Vector3 operator -(Vector3 vec, Vector3 vec2)
        {
            Vector3 newVec = new Vector3();
            newVec.X = vec.X - vec2.X;
            newVec.Y = vec.Y - vec2.Y;
            newVec.Z = vec.Z - vec2.Z;
            return newVec;
        }

        /// <summary>
        /// Divides a vector to another vector.
        /// </summary>
        /// <param name="vec">First vector.</param>
        /// <param name="vec2">second vector.</param>
        /// <returns>The resulting vector.</returns>
        public static Vector3 operator /(Vector3 vec, Vector3 vec2)
        {
            Vector3 newVec = new Vector3();
            newVec.X = vec.X / vec2.X;
            newVec.Y = vec.Y / vec2.Y;
            newVec.Z = vec.Z / vec2.Z;
            return newVec;
        }

        /// <summary>
        /// Multiplies a vector to another vector.
        /// </summary>
        /// <param name="vec">Vector to multiply.</param>
        /// <param name="vec2">second vector to multiply.</param>
        /// <returns>The resulting vector.</returns>
        public static Vector3 operator *(Vector3 vec, Vector3 vec2)
        {
            Vector3 newVec = new Vector3();
            newVec.X = vec.X * vec2.X;
            newVec.Y = vec.Y * vec2.Y;
            newVec.Z = vec.Z * vec2.Z;
            return newVec;
        }

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="vec">Vector to divide.</param>
        /// <param name="scalar">Scalar to divide the vector with.</param>
        /// <returns>The divided vector.</returns>
        public static Vector3 operator /(Vector3 vec, double scalar) => VectorOperation(vec, '/', scalar);

        #endregion

        #region overloads

        public static implicit operator PointF(Vector3 vector3)
        {
            return new PointF((float)vector3.X, (float)vector3.Y);
        }

        public static implicit operator Vector3D(Vector3 vector3)
        {
            return new Vector3D(vector3.X, vector3.Y, vector3.Z);
        }

        public static implicit operator Point(Vector3 vector3)
        {
            return new Point((int)vector3.X, (int)vector3.Y);
        }

        public static implicit operator Vector3(Point point)
        {
            return new Vector3((double)point.X, (double)point.Y, 0);
        }

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

                throw new Exception("out of index range.");
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
                    throw new Exception("out of index range.");
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the vector.
        /// </summary>
        /// <returns>A string representation of the vector.</returns>
        public override string ToString()
        {
            return $"[{this.X}, {this.Y}, {this.Z}]";
        }
        #endregion

        #region screenSpaceConversions 

        /// <summary>
        /// Convert Vector from screen space coordinate to view space coordinate.
        /// </summary>
        /// <param name="vector3">vector to convert.</param>
        /// <param name="size">size of buffer.</param>
        /// <param name="offsetPoint">point where buffer is initialized.</param>
        /// <returns>new vector position.</returns>
        public static Vector3 ToViewSpaceScaled(Vector3 vector3, Size size, Point offsetPoint)
        {
            double vx = (vector3.X - offsetPoint.X) * 2 / size.Width;
            double vy = (vector3.Y + offsetPoint.Y) * 2 / size.Height;
            return new Vector3(vx, vy, vector3.Z);
        }

        /// <summary>
        /// Convert Vector from view space coordinate to screen space coordinate.
        /// </summary>
        /// <param name="vector3">vector to convert.</param>
        /// <param name="size">size of buffer.</param>
        /// <param name="offsetPoint">point where buffer is initialized.</param>
        /// <returns>new vector position.</returns>
        public static Vector3 ToScreenSpaceScaled(Vector3 vector3, Size size, Point offsetPoint)
        {
            double cx = (((vector3.X + 1) * 0.5) * size.Width) + offsetPoint.X;
            double cy = (((1 - vector3.Y) * 0.5) * size.Height) + offsetPoint.Y;
            return new Vector3(cx, cy, vector3.Z);
        }

        /// <summary>
        /// Convert Vector from screen space coordinate to view space coordinate.
        /// </summary>
        /// <param name="vector3">vector to convert.</param>
        /// <param name="size">size of buffer.</param>
        /// <param name="offsetPoint">point where buffer is initialized.</param>
        /// <returns>new vector position.</returns>
        public static Vector3 ToViewSpace(Vector3 vector3, Size size, Point offsetPoint)
        {
            double vx = Math.Abs(vector3.X - (size.Width / 2) - offsetPoint.X);
            double vy = Math.Abs(vector3.Y - (size.Height / 2) - offsetPoint.Y);
            return new Vector3(vx, vy, vector3.Z);
        }

        /// <summary>
        /// Convert Vector from view space coordinate to screen space coordinate.
        /// </summary>
        /// <param name="vector3">vector to convert.</param>
        /// <param name="size">size of buffer.</param>
        /// <param name="offsetPoint">point where buffer is initialized.</param>
        /// <returns>new vector position.</returns>
        public static Vector3 ToScreenSpace(Vector3 vector3, Size size, Point offsetPoint)
        {
            double cx = (size.Width * 0.5) + vector3.X + offsetPoint.X;
            double cy = (size.Height * 0.5) - vector3.Y + offsetPoint.Y;
            return new Vector3(cx, cy, vector3.Z);
        }
        #endregion

        #region vectorOperations
        
        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public static double DotProduct(Vector3 a, Vector3 b)
        {
            return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
        }

        /// <summary>
        /// Calculates the dot product of a single vector
        /// </summary>
        /// <param name="a">The vector.</param>
        /// <returns>added vector.</returns>
        public static double DotProduct(Vector3 a)
        {
            Vector3 b = new Vector3();
            return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
        }

        /// <summary>
        /// Determine the cross product of two Vectors.
        /// Determine the vector product.
        /// Determine the normal vector (Vector3 90° to the plane).
        /// </summary>
        /// <param name="v1">The vector to multiply.</param>
        /// <param name="v2">The vector to multiply by.</param>
        /// <returns>Vector3 representing the cross product of the two vectors.</returns>
        public static Vector3 CrossProduct(Vector3 v1, Vector3 v2) => new Vector3((v1.Y * v2.Z) - (v1.Z * v2.Y), (v1.Z * v2.X) - (v1.X * v2.Z), (v1.X * v2.Y) - (v1.Y * v2.X));

        /// <summary>
        /// Calculates the dot product of a single vector
        /// </summary>
        /// <param name="a">The vector.</param>
        /// <returns>added vector.</returns>
        public double Dot(Vector3 a) => DotProduct(this, a);

        /// <summary>
        /// Determine the cross product of two Vectors.
        /// Determine the vector product.
        /// Determine the normal vector (Vector3 90° to the plane).
        /// </summary>
        /// <param name="other">The vector to multiply by.</param>
        /// <returns>Vector3 representing the cross product of the two vectors.</returns>
        public Vector3 Cross(Vector3 other) => CrossProduct(this, other);


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
        public static Vector3 Normalize(Vector3 vector)
        {
            double len = vector.GetLength();
            if (len <= 0)
            {
                throw new Exception("To small.");
            }

            vector.X /= len;
            vector.Y /= len;
            vector.Z /= len;
            return vector;
        }

        private static Vector3 VectorOperation(Vector3 vec, char operation, double scalar)
        {
            Vector3 newVec = new Vector3();
            switch (operation)
            {
                case '+':
                    newVec.X = vec.X + scalar;
                    newVec.Y = vec.Y + scalar;
                    newVec.Z = vec.Z + scalar;
                    break;
                case '-':
                    newVec.X = vec.X - scalar;
                    newVec.Y = vec.Y - scalar;
                    newVec.Z = vec.Z - scalar;
                    break;
                case '*':
                    newVec.X = vec.X * scalar;
                    newVec.Y = vec.Y * scalar;
                    newVec.Z = vec.Z * scalar;
                    break;
                case '/':
                    newVec.X = vec.X / scalar;
                    newVec.Y = vec.Y / scalar;
                    newVec.Z = vec.Z / scalar;
                    break;
            }

            return newVec;
        }

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        public double GetLength()
        {
            double sql = (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z);
            double len = Math.Sqrt(sql);
            return len;
        }
        #endregion
    }
}
