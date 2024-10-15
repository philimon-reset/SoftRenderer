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
    using System.Windows;
    using System.Windows.Media.Media3D;
    using Point = System.Drawing.Point;
    using Size = System.Drawing.Size;

    /// <summary>
    /// Represents a vector in Four-dimensional space.
    /// </summary>
    /// <remarks>
    /// This class provides properties and methods to perform vector operations.
    /// </remarks>
    public class Vector4
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> class.
        /// </summary>
        public Vector4()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.W = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> class with the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <param name="w">The W coordinate.</param>
        public Vector4(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> class from an array.
        /// </summary>
        /// <param name="array">Array of values.</param>
        public Vector4(double[] array)
        {
            if (array.Length != 4)
            {
                throw new Exception("Size of array does not match with vector4");
            }

            this.X = array[0];
            this.Y = array[1];
            this.Z = array[2];
            this.W = array[3];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> class.
        /// </summary>
        /// <param name="vector">vector3.</param>
        /// <param name="w">w axis.</param>
        public Vector4(Vector3 vector, double w)
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
            this.W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> class with the coordinates of another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        public Vector4(Vector4 other)
        {
            this.X = other.X;
            this.Y = other.Y;
            this.Z = other.Z;
            this.W = other.W;
        }
        #endregion

        #region properties

        /// <summary>
        /// Gets the X-axis vector.
        /// </summary>
        public static Vector4 XAxis
        {
            get => new Vector4(1, 0, 0, 0);
        }

        /// <summary>
        /// Gets the Y-axis vector.
        /// </summary>
        public static Vector4 YAxis
        {
            get => new Vector4(0, 1, 0, 0);
        }

        /// <summary>
        /// Gets the Z-axis vector.
        /// </summary>
        public static Vector4 ZAxis
        {
            get => new Vector4(0, 0, 1, 0);
        }

        /// <summary>
        /// Gets the W-axis vector.
        /// </summary>
        public static Vector4 WAxis
        {
            get => new Vector4(0, 0, 0, 1);
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
        /// Gets or sets the Z coordinate of the vector.
        /// </summary>
        public double W { get; set; }

        #endregion

        #region operator_overloads

        /// <summary>
        /// Inverses the sign of a vector.
        /// </summary>
        /// <param name="vec">Vector to inverse the sign of.</param>
        /// <returns>The sign inverted vector.</returns>
        public static Vector4 operator -(Vector4 vec) => VectorOperation(vec, '*', -1);

        /// <summary>
        /// Adds a vector and a scalar.
        /// </summary>
        /// <param name="vec">Vector to add to.</param>
        /// <param name="scalar">Scalar to add to the vector.</param>
        /// <returns>The added vector.</returns>
        public static Vector4 operator +(Vector4 vec, double scalar) => VectorOperation(vec, '+', scalar);

        /// <summary>
        /// Adds a vector and a scalar.
        /// </summary>
        /// <param name="vec">Vector to add to.</param>
        /// <param name="scalar">Scalar to add to the vector.</param>
        /// <returns>The added vector.</returns>
        public static Vector4 operator +(double scalar, Vector4 vec) => vec + scalar;

        /// <summary>
        /// Substracts a vector and a scalar.
        /// </summary>
        /// <param name="vec">Vector to substract to.</param>
        /// <param name="scalar">Scalar to substract to the vector.</param>
        /// <returns>The substracted vector.</returns>
        public static Vector4 operator -(Vector4 vec, double scalar) => VectorOperation(vec, '-', scalar);

        /// <summary>
        /// Substracts a vector and a scalar.
        /// </summary>
        /// <param name="vec">Vector to substract to.</param>
        /// <param name="scalar">Scalar to substract to the vector.</param>
        /// <returns>The substracted vector.</returns>
        public static Vector4 operator -(double scalar, Vector4 vec) => vec - scalar;

        /// <summary>
        /// Multiplies a vector and a scalar.
        /// </summary>
        /// <param name="vec">Vector to multiply.</param>
        /// <param name="scalar">Scalar to multiply with the vector.</param>
        /// <returns>The multiplied vector.</returns>
        public static Vector4 operator *(Vector4 vec, double scalar) => VectorOperation(vec, '*', scalar);

        /// <summary>
        /// Multiplies a vector and a scalar.
        /// </summary>
        /// <param name="vec">Vector to multiply.</param>
        /// <param name="scalar">Scalar to multiply with the vector.</param>
        /// <returns>The multiplied vector.</returns>
        public static Vector4 operator *(double scalar, Vector4 vec) => vec * scalar;

        /// <summary>
        /// Multiplies a vector and another vector.
        /// </summary>
        /// <param name="vec">Vector to multiply.</param>
        /// <param name="vec2">second vector to multiply.</param>
        /// <returns>The resulting vector.</returns>
        public static Vector4 operator *(Vector4 vec, Vector4 vec2)
        {
            Vector4 newVec = new Vector4();
            newVec.X = vec.X * vec2.X;
            newVec.Y = vec.Y * vec2.Y;
            newVec.Z = vec.Z * vec2.Z;
            newVec.W = vec.W * vec2.W;
            return newVec;
        }

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="vec">Vector to divide.</param>
        /// <param name="scalar">Scalar to divide the vector with.</param>
        /// <returns>The divided vector.</returns>
        public static Vector4 operator /(Vector4 vec, double scalar) => VectorOperation(vec, '/', scalar);

        /// <summary>
        /// Multiplies a vector with a matrix (row major)
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The product of the matrix and vector.</returns>
        public static Vector4 operator *(Matrix matrix, Vector4 vector)
        {
            Vector4 newVec = new Vector4();

            for (int i = 0; i < 4; i++)
            {
                var multipliedVector = matrix[i] * vector;
                newVec[i] = Vector4.DotProduct(multipliedVector);
            }
            for (int i = 0; i < 4; i++)
            {
                newVec[i] /= newVec.W;
            }
            return newVec;
        }

        /// <summary>
        /// Multiplies a vector with a matrix
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The product of the matrix and vector.</returns>
        public static Vector4 operator *(Vector4 vector, Matrix matrix) => matrix * vector;

        #endregion

        #region overloads

        public static implicit operator PointF(Vector4 vector4)
        {
            return new PointF((float)vector4.X, (float)vector4.Y);
        }

        public static explicit operator Vector4(PointF point)
        {
            return new Vector4(point.X, point.Y, 0, 1);
        }

        public static implicit operator Vector3(Vector4 vector)
        {
            return new Vector3(vector);
        }
        
        public static implicit operator Quaternion(Vector4 vector)
        {
            return new Quaternion(vector.X, vector.Y, vector.Z, vector.W);
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
                else if (i == 3)
                {
                    return this.W;
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
                else if (i == 3)
                {
                    this.W = value;
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
            return $"[{this.X}, {this.Y}, {this.Z} {this.W}]";
        }
        #endregion

        #region screenSpaceConversions

        /// <summary>
        /// Convert Vector from screen space coordinate to view space coordinate.
        /// </summary>
        /// <param name="vector4">vector to convert.</param>
        /// <param name="vector"></param>
        /// <param name="size">size of buffer.</param>
        /// <param name="offsetPoint">point where buffer is initialized.</param>
        /// <returns>new vector position.</returns>
        public static Vector4 ToViewSpaceScaled(Vector4 vector, Size size, Point offsetPoint)
        {
            double vx = (vector.X - offsetPoint.X) * 2 / size.Width;
            double vy = (vector.Y + offsetPoint.Y) * 2 / size.Height;
            return new Vector4(vx, vy, vector.Z, vector.W);
        }

        /// <summary>
        /// Convert Vector from view space coordinate to screen space coordinate.
        /// </summary>
        /// <param name="vector">vector to convert.</param>
        /// <param name="size">size of buffer.</param>
        /// <param name="offsetPoint">point where buffer is initialized.</param>
        /// <returns>new vector position.</returns>
        public static Vector4 ToScreenSpaceScaled(Vector4 vector, Size size, Point offsetPoint)
        {
            double cx = (((vector.X + 1) * 0.5) * size.Width) + offsetPoint.X;
            double cy = (((1 - vector.Y) * 0.5) * size.Height) + offsetPoint.Y;
            return new Vector4(cx, cy, vector.Z, vector.W);
        }

        /// <summary>
        /// Convert Vector from screen space coordinate to view space coordinate.
        /// </summary>
        /// <param name="vector">vector to convert.</param>
        /// <param name="size">size of buffer.</param>
        /// <param name="offsetPoint">point where buffer is initialized.</param>
        /// <returns>new vector position.</returns>
        public static Vector4 ToViewSpace(Vector4 vector, Size size, Point offsetPoint)
        {
            double vx = Math.Abs(vector.X - (size.Width / 2) - offsetPoint.X);
            double vy = Math.Abs(vector.Y - (size.Height / 2) - offsetPoint.Y);
            return new Vector4(vx, vy, vector.Z, vector.W);
        }

        /// <summary>
        /// Convert Vector from view space coordinate to screen space coordinate.
        /// </summary>
        /// <param name="vector">vector to convert.</param>
        /// <param name="size">size of buffer.</param>
        /// <param name="offsetPoint">point where buffer is initialized.</param>
        /// <returns>new vector position.</returns>
        public static Vector4 ToScreenSpace(Vector4 vector, Size size, Point offsetPoint)
        {
            double cx = (size.Width * 0.5) + vector.X + offsetPoint.X;
            double cy = (size.Height * 0.5) - vector.Y + offsetPoint.Y;
            return new Vector4(cx, cy, vector.Z, vector.W);
        }
        #endregion

        #region vectorOperations

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public static double DotProduct(Vector4 a, Vector4 b)
        {
            return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z) + (a.W * b.W);
        }

        /// <summary>
        /// Calculates the dot product of a single vector
        /// </summary>
        /// <param name="a">The vector.</param>
        /// <returns>added vector.</returns>
        public static double DotProduct(Vector4 a)
        {
            Vector4 b = new Vector4(1, 1, 1, 1);
            return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z) + (a.W * b.W);
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
            this.W /= len;
            return true;
        }

        private static Vector4 VectorOperation(Vector4 vec, char operation, double scalar)
        {
            Vector4 newVec = new Vector4();
            switch (operation)
            {
                case '+':
                    newVec.X = vec.X + scalar;
                    newVec.Y = vec.Y + scalar;
                    newVec.Z = vec.Z + scalar;
                    newVec.W = vec.W + scalar;
                    break;
                case '-':
                    newVec.X = vec.X - scalar;
                    newVec.Y = vec.Y - scalar;
                    newVec.Z = vec.Z - scalar;
                    newVec.W = vec.W - scalar;
                    break;
                case '*':
                    newVec.X = vec.X * scalar;
                    newVec.Y = vec.Y * scalar;
                    newVec.Z = vec.Z * scalar;
                    newVec.W = vec.W * scalar;
                    break;
                case '/':
                    newVec.X = vec.X / scalar;
                    newVec.Y = vec.Y / scalar;
                    newVec.Z = vec.Z / scalar;
                    newVec.W = vec.W / scalar;
                    break;
            }

            return newVec;
        }

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        private double GetLength()
        {
            double sql = (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W);
            double len = Math.Sqrt(sql);
            return len;
        }
        #endregion
    }
}
