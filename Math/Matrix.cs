namespace SoftRenderer.Math
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media.Media3D;
    using Engine.Buffers;

    public record struct Matrix
    {
        #region Constants

        /// <summary>
        ///     Size of the matrix.
        /// </summary>
        public const int Size = 4;

        /// <summary>
        ///     The identity matrix.
        /// </summary>
        public static readonly Matrix Identity = new Matrix(new double[,]
        {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 },
        });

        /// <summary>
        ///     Matrix for projection onto the x = 0 plane.
        /// </summary>
        public static readonly Matrix ProjectionOrthoX = new Matrix(new double[,]
        {
            { 0, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 },
        });

        /// <summary>
        ///     Matrix for projection onto the y = 0 plane.
        /// </summary>
        public static readonly Matrix ProjectionOrthoY = new Matrix(new double[,]
        {
            { 1, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 },
        });

        /// <summary>
        ///     Matrix for projection onto the z = 0 plane.
        /// </summary>
        public static readonly Matrix ProjectionOrthoZ = new Matrix(new double[,]
        {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 1 },
        });

        /// <summary>
        ///     The zero matrix.
        /// </summary>
        public static readonly Matrix Zero = new Matrix(new double[,]
        {
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
        });

        #endregion

        #region ctro

        /// <summary>
        ///     Initializes a new instance of the <see cref="Matrix" /> class.
        /// </summary>
        /// <param name="mValues">Values of the matrix. Should be 4x4.</param>
        public Matrix(double[,] mValues)
        {
            this.mValues = new Vector4[Size]
            {
                new Vector4(mValues[0, 0], mValues[0, 1], mValues[0, 2], mValues[0, 3]),
                new Vector4(mValues[1, 0], mValues[1, 1], mValues[1, 2], mValues[1, 3]),
                new Vector4(mValues[2, 0], mValues[2, 1], mValues[2, 2], mValues[2, 3]),
                new Vector4(mValues[3, 0], mValues[3, 1], mValues[3, 2], mValues[3, 3]),
            };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Matrix" /> class.
        /// </summary>
        /// <param name="mValues">values of doubles.</param>
        /// <param name="isColums">Tells if the vector is a column major vector or a row major vector.</param>
        public Matrix(double[] mValues, bool isColums = false)
        {
            if (mValues.Length != Size * Size)
            {
                throw new Exception($"There should be exactly {Size * Size} elements.");
            }

            if (isColums)
            {
                Vector4[] cols = [new Vector4(), new Vector4(), new Vector4(), new Vector4()];
                for (int x = 0; x <= 3; x++)
                {
                    for (int y = 0; y <= 3; y++)
                    {
                        cols[y][x] = mValues[(4 * x) + y];
                    }

                    this.mValues[x] = cols[x];
                }
            }
            else
            {
                for (int x = 0; x <= 3; x++)
                {
                    Vector4 row = new Vector4();
                    for (int y = 0; y <= 3; y++)
                    {
                        row[y] = mValues[(4 * x) + y];
                    }

                    this.mValues[x] = row;
                }
 
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Matrix" /> class.
        /// </summary>
        /// <param name="matrix">Matrix struct to take the mValues from.</param>
        public Matrix(Matrix matrix)
            : this(matrix.mValues)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Matrix" /> class.
        /// </summary>
        /// <param name="vectors">vectors.</param>
        public Matrix(Vector4[] vectors)
        {
            if (vectors.Length != Size)
            {
                throw new Exception($"There should be exactly {Size} vectors.");
            }

            for (int x = 0; x <= 3; x++)
            {
                this.mValues[x] = vectors[x];
            }
        }

        #endregion Constructors

        #region Properties

        private readonly Vector4[] mValues = new Vector4[Size];

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int x = 0; x <= 3; x++)
            {
                for (int y = 0; y <= 3; y++)
                {
                    builder.Append($"|*{this[y, x]}*|");
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        /// <summary>
        ///     Gets the values of the matrix.
        /// </summary>
        public double[,] Values
        {
            get
            {
                return new double[Size, Size]
                {
                    { this.mValues[0][0], this.mValues[0][1], this.mValues[0][2], this.mValues[0][3] },
                    { this.mValues[1][0], this.mValues[1][1], this.mValues[1][2], this.mValues[1][3] },
                    { this.mValues[2][0], this.mValues[2][1], this.mValues[2][2], this.mValues[2][3] },
                    { this.mValues[3][0], this.mValues[3][1], this.mValues[3][2], this.mValues[3][3] },
                };
            }
        }

        /// <summary>
        ///     Gets the value at 0, 0 on the matrix.
        /// </summary>
        public double V00
        {
            get => this.mValues[0][0];
        }

        /// <summary>
        ///     Gets the value at 0, 1 on the matrix.
        /// </summary>
        public double V01
        {
            get => this.mValues[0][1];
        }

        /// <summary>
        ///     Gets the value at 0, 2 on the matrix.
        /// </summary>
        public double V02
        {
            get => this.mValues[0][2];
        }

        /// <summary>
        ///     Gets the value at 0, 3 on the matrix.
        /// </summary>
        public double V03
        {
            get => this.mValues[0][3];
        }

        /// <summary>
        ///     Gets the value at 1, 0 on the matrix.
        /// </summary>
        public double V10
        {
            get => this.mValues[1][0];
        }

        /// <summary>
        ///     Gets the value at 1, 1 on the matrix.
        /// </summary>
        public double V11
        {
            get => this.mValues[1][1];
        }

        /// <summary>
        ///     Gets the value at 1, 2 on the matrix.
        /// </summary>
        public double V12
        {
            get => this.mValues[1][2];
        }

        /// <summary>
        ///     Gets the value at 1, 3 on the matrix.
        /// </summary>
        public double V13
        {
            get => this.mValues[1][3];
        }

        /// <summary>
        ///     Gets the value at 2, 0 on the matrix.
        /// </summary>
        public double V20
        {
            get => this.mValues[2][0];
        }

        /// <summary>
        ///     Gets the value at 2, 1 on the matrix.
        /// </summary>
        public double V21
        {
            get => this.mValues[2][1];
        }

        /// <summary>
        ///     Gets the value at 2, 2 on the matrix.
        /// </summary>
        public double V22
        {
            get => this.mValues[2][2];
        }

        /// <summary>
        ///     Gets the value at 2, 3 on the matrix.
        /// </summary>
        public double V23
        {
            get => this.mValues[2][3];
        }

        /// <summary>
        ///     Gets the value at 3, 0 on the matrix.
        /// </summary>
        public double V30
        {
            get => this.mValues[3][0];
        }

        /// <summary>
        ///     Gets the value at 3, 1 on the matrix.
        /// </summary>
        public double V31
        {
            get => this.mValues[3][1];
        }

        /// <summary>
        ///     Gets the value at 3, 2 on the matrix.
        /// </summary>
        public double V32
        {
            get => this.mValues[3][2];
        }

        /// <summary>
        /// Gets the value at 3, 3 on the matrix.
        /// </summary>
        public double V33
        {
            get => this.mValues[3][3];
        }

        /// <summary>
        ///     Gets the determinant of the matrix.
        /// </summary>
        public double Determinant
        {
            get => (this.V00 * this.V11 * this.V22 * this.V33) -
                   (this.V00 * this.V11 * this.V23 * this.V32) +
                   (this.V00 * this.V12 * this.V23 * this.V31) -
                   (this.V00 * this.V12 * this.V21 * this.V33) +
                   (this.V00 * this.V13 * this.V21 * this.V32) -
                   (this.V00 * this.V13 * this.V22 * this.V31) -
                   (this.V01 * this.V12 * this.V23 * this.V30) +
                   (this.V01 * this.V12 * this.V20 * this.V33) -
                   (this.V01 * this.V13 * this.V20 * this.V32) +
                   (this.V01 * this.V13 * this.V22 * this.V30) -
                   (this.V01 * this.V10 * this.V22 * this.V33) +
                   (this.V01 * this.V10 * this.V23 * this.V32) +
                   (this.V02 * this.V13 * this.V20 * this.V31) -
                   (this.V02 * this.V13 * this.V21 * this.V30) +
                   (this.V02 * this.V10 * this.V21 * this.V33) -
                   (this.V02 * this.V10 * this.V23 * this.V31) +
                   (this.V02 * this.V11 * this.V23 * this.V30) +
                   (this.V02 * this.V11 * this.V20 * this.V33) -
                   (this.V03 * this.V10 * this.V21 * this.V32) +
                   (this.V03 * this.V10 * this.V22 * this.V31) -
                   (this.V03 * this.V11 * this.V22 * this.V30) +
                   (this.V03 * this.V11 * this.V20 * this.V32) -
                   (this.V03 * this.V12 * this.V20 * this.V31) +
                   (this.V03 * this.V12 * this.V21 * this.V30);
        }

        /// <summary>
        /// Computes the transpose of the given matrix.
        /// </summary>
        /// <returns>The transpose of the given matrix.</returns>
        public Matrix Transpose
        {
            get => TransposeOperation(this);
        }

        /// <summary>
        ///     Gets the trace of the matrix, the sum of the values along the diagonal axis.
        /// </summary>
        public double Trace
        {
            get => this.V00 + this.V11 + this.V22 + this.V33;
        }

        /// <summary>
        /// Gets get Normalized Matrix
        /// </summary>
        public Matrix GetNormalized
        {
            get => Normalize(this);
        }

        /// <summary>
        /// Gets Inverted Matrix.
        /// </summary>
        public Matrix GetInverted
        {
            get => Invert(this);
        }

        /// <summary>
        /// Gets the value at the specified index.
        /// </summary>
        /// <param name="i">The index.</param>
        /// <returns>The value at the specified index.</returns>
        public Vector4 this[int i]
        {
            get
            {
                if (i is > 3 or < 0)
                {
                    throw new Exception("out of index range.");
                }

                return this.mValues[i];
            }
        }

        #endregion Properties

        #region  matrixOperations

        /// <summary>
        /// Get a column in the matrix.
        /// </summary>
        /// <param name="idx">col index.</param>
        /// <param name="mat">matrix to slice.</param>
        /// <returns>vector representing the col.</returns>
        public static Vector4 GetCol(int idx, Matrix mat)
        {
            double[] colVector = new double[4];
            for (int i = 0; i < 4; i++)
            {
                colVector[i] = mat[i, idx];
            }

            return new Vector4(colVector);
        }

        /// <summary>
        ///     Divides each element in the Matrix by the <see cref="Determinant" />.
        /// </summary>
        /// <param name="mat">Matrix to normalize.</param>
        /// <returns>The normalized matrix.</returns>

        public static Matrix Normalize(Matrix mat)
        {
            double determinant = mat.Determinant;
            return mat / determinant;
        }

        /// <summary>
        /// Get inverted matrix.
        /// </summary>
        /// <param name="mat">matrix to invert.</param>
        /// <returns>an inverted matrix</returns>
        /// <exception cref="InvalidOperationException">inversion not possible.</exception>
        /// <remarks> Taken from here : https://github.com/opentk/opentk/blob/master/src/OpenTK.Mathematics/Matrix/Matrix4d.cs .</remarks>
        public static Matrix Invert(Matrix mat)
        {
            double a = mat[0, 0], b = mat[1, 0], c = mat[2, 0], d = mat[3, 0];
            double e = mat[0, 1], f = mat[1, 1], g = mat[2, 1], h = mat[3, 1];
            double i = mat[0, 2], j = mat[1, 2], k = mat[2, 2], l = mat[3, 2];
            double m = mat[0, 3], n = mat[1, 3], o = mat[2, 3], p = mat[3, 3];

            double kplo = (k * p) - (l * o);
            double jpln = (j * p) - (l * n);
            double jokn = (j * o) - (k * n);
            double iplm = (i * p) - (l * m);
            double iokm = (i * o) - (k * m);
            double injm = (i * n) - (j * m);

            double a11 = +((f * kplo) - (g * jpln) + (h * jokn));
            double a12 = -((e * kplo) - (g * iplm) + (h * iokm));
            double a13 = +((e * jpln) - (f * iplm) + (h * injm));
            double a14 = -((e * jokn) - (f * iokm) + (g * injm));

            double det = (a * a11) + (b * a12) + (c * a13) + (d * a14);

            if (Math.Abs(det) < double.Epsilon)
            {
                throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
            }

            double invDet = 1.0f / det;

            Vector4 row0 = new Vector4(a11, a12, a13, a14) * invDet;

            Vector4 row1 = new Vector4(
                -((b * kplo) - (c * jpln) + (d * jokn)),
                +((a * kplo) - (c * iplm) + (d * iokm)),
                -((a * jpln) - (b * iplm) + (d * injm)),
                +((a * jokn) - (b * iokm) + (c * injm))) * invDet;

            double gpho = (g * p) - (h * o);
            double fphn = (f * p) - (h * n);
            double fogn = (f * o) - (g * n);
            double ephm = (e * p) - (h * m);
            double eogm = (e * o) - (g * m);
            double enfm = (e * n) - (f * m);

            Vector4 row2 = new Vector4(
                +((b * gpho) - (c * fphn) + (d * fogn)),
                -((a * gpho) - (c * ephm) + (d * eogm)),
                +((a * fphn) - (b * ephm) + (d * enfm)),
                -((a * fogn) - (b * eogm) + (c * enfm))) * invDet;

            double glhk = (g * l) - (h * k);
            double flhj = (f * l) - (h * j);
            double fkgj = (f * k) - (g * j);
            double elhi = (e * l) - (h * i);
            double ekgi = (e * k) - (g * i);
            double ejfi = (e * j) - (f * i);

            Vector4 row3 = new Vector4(
                -((b * glhk) - (c * flhj) + (d * fkgj)),
                +((a * glhk) - (c * elhi) + (d * ekgi)),
                -((a * flhj) - (b * elhi) + (d * ejfi)),
                +((a * fkgj) - (b * ekgi) + (c * ejfi))) * invDet;
            return new Matrix([row0, row1, row2, row3]);
        }

        
        /// <summary>
        /// Computes the transpose of the given matrix.
        /// </summary>
        /// <param name="mat">Matrix to compute the transpose of.</param>
        /// <returns>The transpose of the given matrix.</returns>
        public static Matrix TransposeOperation(Matrix mat)
        {
            double[,] newMat = new double[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    newMat[j, i] = mat[i, j];
                }
            }

            return new(newMat);
        }

        private static Matrix MatrixOperation(Matrix mat, char operation, double scalar)
        {
            double[,] newMatrix = new double[Size, Size];
            for (int x = 0; x <= 3; x++)
            {
                for (int y = 0; y <= 3; y++)
                {
                    switch (operation)
                    {
                        case '+':
                            newMatrix[x, y] = mat[x, y] + scalar;
                            break;
                        case '-':
                            newMatrix[x, y] = mat[x, y] - scalar;
                            break;
                        case '*':
                            newMatrix[x, y] = mat[x, y] * scalar;
                            break;
                        case '/':
                            newMatrix[x, y] = mat[x, y] / scalar;
                            break;
                    }
                }
            }

            return new Matrix(newMatrix);
        }

        private static Matrix MatrixOperation(Matrix mat, Matrix secondMat, char operation)
        {
            double[,] newMatrix = new double[Size, Size];
            for (int x = 0; x <= 3; x++)
            {
                for (int y = 0; y <= 3; y++)
                {
                    switch (operation)
                    {
                        case '+':
                            newMatrix[x, y] = mat[x, y] + secondMat[x, y];
                            break;
                        case '-':
                            newMatrix[x, y] = mat[x, y] - secondMat[x, y];
                            break;
                        case '*':
                            newMatrix[x, y] = mat[x, y] * secondMat[x, y];
                            break;
                        case '/':
                            newMatrix[x, y] = mat[x, y] / secondMat[x, y];
                            break;
                    }
                }
            }

            return new Matrix(newMatrix);
        }
        #endregion

        #region operations


        /// <summary>
        /// Generate translation matrix.
        /// </summary>
        /// <param name="vector">vector to translate by.</param>
        /// <returns>translation matrix.</returns>
        public static Matrix Translate(Vector3 vector)
        {
            double[,] mValues = new double[Size, Size]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { vector.X, vector.Y, vector.Z, 1 },
            };
            return new Matrix(mValues);
        }

        /// <summary>
        /// Generate translation matrix.
        /// </summary>
        /// <param name="x">x coordinate.</param>
        /// <param name="y">y coordinate.</param>
        /// <param name="z">z coordinate.</param>
        /// <returns>translation matrix.</returns>
        public static Matrix Translate(double x, double y, double z) => Translate(new Vector3(x, y, z));

        /// <summary>
        ///  Generate Scale matrix.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <param name="z">z.</param>
        /// <returns>Scale matrix.</returns>
        public static Matrix Scale(double x, double y, double z)
        {
            double[,] mValues = new double[Size, Size]
            {
                { x, 0, 0, 0 },
                { 0, y, 0, 0 },
                { 0, 0, z, 0 },
                { 0, 0, 0, 1 },
            };
            return new Matrix(mValues);
        }

        /// <summary>
        ///  Generate Scale matrix.
        /// </summary>
        /// <param name="vector">vector3.</param>
        /// <returns>Scale matrix.</returns>
        public static Matrix Scale(Vector3 vector) => Scale(vector.X, vector.Y, vector.Z);

        /// <summary>
        ///  Generate Scale matrix.
        /// </summary>
        /// <param name="factor"factor.></param>
        /// <returns>Scale matrix.</returns>
        public static Matrix Scale(double factor) => Scale(new Vector3(factor, factor, factor));

        /// <summary>
        ///     Generate rotation matrix about an axis.
        /// </summary>
        /// <param name="angle">angle of rotation.</param>
        /// <param name="axis">axis of rotation.</param>
        /// <returns>rotation matrix.</returns>
        public static Matrix Rotate(double angle, Vector3 axis)
        {
            var rotQ = new Quaternion(Vector4.Normalize(axis), (angle * -180) / Math.PI);
            double xx = rotQ.X * rotQ.X;
            double yy = rotQ.Y * rotQ.Y;
            double zz = rotQ.Z * rotQ.Z;
            double xy = rotQ.X * rotQ.Y;
            double xz = rotQ.X * rotQ.Z;
            double yz = rotQ.Y * rotQ.Z;
            double xw = rotQ.X * rotQ.W;
            double wy = rotQ.W * rotQ.Y;
            double wz = rotQ.W * rotQ.Z;
            double m00 = 1 - (2 * (yy + zz));
            double m01 = 2 * (xy - wz);
            double m02 = 2 * (xz + wy);
            double m10 = 2 * (xy + wz);
            double m11 = 1 - (2 * (xx + zz));
            double m12 = 2 * (yz - xw);
            double m20 = 2 * (xz - wy);
            double m21 = 2 * (yz + xw);
            double m22 = 1 - (2 * (xx + yy));
            return new Matrix([
                m00,
                m01,
                m02,
                0,
                m10,
                m11,
                m12,
                0,
                m20,
                m21,
                m22,
                0,
                0, 0, 0, 1,
            ]);
        }

        /// <summary>
        /// Get transformation around a point that isn't the origin.
        /// </summary>
        /// <param name="transformation">transformation matrix.</param>
        /// <param name="transOrigin">new origin point.</param>
        /// <returns>new transformation matrix.</returns>
        public static Matrix TransformAround(Matrix transformation, Vector3 transOrigin)
        {
            Matrix translateToPoint = Translate(transOrigin);
            return translateToPoint.GetInverted * transformation * translateToPoint;
        }

        /// <summary>
        /// Transform coordinates from NDC to Screen Space Coordinates.
        /// </summary>
        /// <param name="clientBuffer">client buffer.</param>
        /// <returns>Final matrix.</returns>
        public static Matrix NdcToScreenCoordinates(ClientBuffer clientBuffer)
        {
            double[,] mValues = new double[Size, Size]
            {
                { clientBuffer.Width * 0.5, 0, 0, 0 },
                { 0, -clientBuffer.Height * 0.5, 0, 0 },
                { 0, 0, clientBuffer.MaxZ - clientBuffer.MinZ, 0 },
                { clientBuffer.X + (clientBuffer.Width * 0.5), clientBuffer.Y + (clientBuffer.Height * 0.5), clientBuffer.MinZ, 1 },
            };
            return new Matrix(mValues);
        }

        /// TODO: Understand the transformation matrix...
        /// <summary>
        /// Build a world space to camera space matrix
        /// </summary>
        /// <param name="eye">Eye (camera) position in world space.</param>
        /// <param name="target">Target position in world .</param>
        /// <param name="up">Up vector in world space (should not be parallel to the camera direction, that is target - eye)</param>
        /// <returns>A Matrix that transforms world space to camera .</returns>
        /// <remarks>Taken from OpenTK.Mathematics/Matrix/Matrix4d.</remarks>
        public static Matrix ViewMatrix(Vector3 eye, Vector3 target, Vector3 up)
        {
            Vector3 z = Vector3.Normalize(eye - target);
            Vector3 x = up.Cross(z);
            Vector3 y = z.Cross(x);

            Matrix rot = new Matrix(
               [new Vector4(x.X, y.X, z.X, 0),
                new Vector4(x.Y, y.Y, z.Y, 0),
                new Vector4(x.Z, y.Z, z.Z, 0),
                new Vector4(-x.Dot(eye), -y.Dot(eye), -z.Dot(eye), 1)]);

            return rot;
        }

        /// <summary>
        /// Creates a perspective projection matrix.
        /// </summary>
        /// <param name="fovy">Angle of the field of view in the y direction (in radians).</param>
        /// <param name="aspect">Aspect ratio of the view (width / height).</param>
        /// <param name="depthNear">Distance to the near clip plane.</param>
        /// <param name="depthFar">Distance to the far clip plane.</param>
        /// <returns>A perspective projection matrix.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown under the following conditions:
        ///  <list type="bullet">
        ///  <item>fovy is zero, less than zero or larger than Math.PI</item>
        ///  <item>aspect is negative or zero</item>
        ///  <item>depthNear is negative or zero</item>
        ///  <item>depthFar is negative or zero</item>
        ///  <item>depthNear is larger than depthFar</item>
        ///  </list>
        /// </exception>
        public static Matrix PerspectiveMatrix(double fovy, double aspect, double depthNear, double depthFar)
        {
            if (fovy <= 0 || fovy > Math.PI)
            {
                throw new ArgumentOutOfRangeException(nameof(fovy));
            }

            if (aspect <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(aspect));
            }

            if (depthNear <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(depthNear));
            }

            if (depthFar <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(depthFar));
            }

            double fovScale = 1 / Math.Tan(fovy * 0.5);
            double x = fovScale * aspect;
            double y = fovScale;
            double zScale = (depthFar) / (depthNear - depthFar);
            double wScale =  (zScale * depthNear);

            return new Matrix([
                x, 0, 0, 0,
                0, y, 0, 0,
                0, 0, zScale, -1,
                0, 0, wScale, 0]);
        }

        /// <summary>
        /// Transform each point according to the transformation matrix.
        /// </summary>
        /// <param name="transformationMatrix">transformation matrix.</param>
        /// <param name="vectors">vectors to be transformed.</param>
        /// <returns>array of vectors after transformations.</returns>
        public static IEnumerable<Vector3> TransformPoints(Matrix transformationMatrix, IEnumerable<Vector3> vectors)
        {
            return vectors.Select<Vector3, Vector3>((vector) =>
            {
                Vector4 multipliedVector = transformationMatrix * vector;
                for (int i = 0; i < 4; i++)
                {
                    multipliedVector[i] /= multipliedVector.W;
                }
                return multipliedVector;
            });
        }

        #endregion

        #region operator_overloads

        /// <summary>
        ///     Gets the value at the specified row and column.
        /// </summary>
        /// <param name="rowIdx">The index of the row.</param>
        /// <param name="colIdx">The index of the column.</param>
        /// <returns>The element at the given row and column index.</returns>
        public double this[int rowIdx, int colIdx]
        {
            get => this.mValues[rowIdx][colIdx];
        }

        /// <summary>
        ///     Adds a matrix and a scalar.
        /// </summary>
        /// <param name="mat">Matrix to add to.</param>
        /// <param name="scalar">Scalar to add to the matrix.</param>
        /// <returns>The added matrix.</returns>
        public static Matrix operator +(Matrix mat, double scalar)
        {
            return MatrixOperation(mat, '+', scalar);
        }

        /// <summary>
        ///     Adds a matrix and a scalar.
        /// </summary>
        /// <param name="mat">Matrix to add to.</param>
        /// <param name="scalar">Scalar to add to the matrix.</param>
        /// <returns>The added matrix.</returns>
        public static Matrix operator +(double scalar, Matrix mat)
        {
            return mat + scalar;
        }

        /// <summary>
        ///     Substracts a matrix and a scalar.
        /// </summary>
        /// <param name="mat">Matrix to substract to.</param>
        /// <param name="scalar">Scalar to substract to the matrix.</param>
        /// <returns>The substracted matrix.</returns>
        public static Matrix operator -(Matrix mat, double scalar)
        {
            return MatrixOperation(mat, '-', scalar);
        }

        /// <summary>
        ///     Substracts a matrix and a scalar.
        /// </summary>
        /// <param name="mat">Matrix to substract to.</param>
        /// <param name="scalar">Scalar to substract to the matrix.</param>
        /// <returns>The substracted matrix.</returns>
        public static Matrix operator -(double scalar, Matrix mat)
        {
            return mat - scalar;
        }

        /// <summary>
        ///     Multiplies a matrix and a scalar.
        /// </summary>
        /// <param name="mat">Matrix to multiply.</param>
        /// <param name="scalar">Scalar to multiply with the matrix.</param>
        /// <returns>The multiplied matrix.</returns>
        public static Matrix operator *(Matrix mat, double scalar)
        {
            return MatrixOperation(mat, '*', scalar);
        }

        /// <summary>
        ///     Multiplies a matrix and a scalar.
        /// </summary>
        /// <param name="mat">Matrix to multiply.</param>
        /// <param name="scalar">Scalar to multiply with the matrix.</param>
        /// <returns>The multiplied matrix.</returns>
        public static Matrix operator *(double scalar, Matrix mat)
        {
            return mat * scalar;
        }

        /// <summary>
        ///     Divides a matrix by a scalar.
        /// </summary>
        /// <param name="mat">Matrix to divide.</param>
        /// <param name="scalar">Scalar to divide the matrix with.</param>
        /// <returns>The divided matrix.</returns>
        public static Matrix operator /(Matrix mat, double scalar)
        {
            return MatrixOperation(mat, '/', scalar);
        }

        /// <summary>
        ///     Inverses the sign of a matrix.
        /// </summary>
        /// <param name="mat">Matrix to inverse the sign of.</param>
        /// <returns>The sign inverted matrix.</returns>
        public static Matrix operator -(Matrix mat)
        {
            return MatrixOperation(mat, '*', -1);
        }

        /// <summary>
        ///     Adds two matrices.
        /// </summary>
        /// <param name="left">The matrix on the left.</param>
        /// <param name="right">The matrix on the right.</param>
        /// <returns>The added matrix.</returns>
        public static Matrix operator +(Matrix left, Matrix right)
        {
            return MatrixOperation(left, right, '+');
        }

        /// <summary>
        ///     Substracts two matrices.
        /// </summary>
        /// <param name="left">The matrix on the left.</param>
        /// <param name="right">The matrix on the right.</param>
        /// <returns>The subtracted matrix.</returns>
        public static Matrix operator -(Matrix left, Matrix right)
        {
            return MatrixOperation(left, right, '-');
        }

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="left">The left matrix.</param>
        /// <param name="right">The right matrix.</param>
        /// <returns>The product of the matrices.</returns>
        public static Matrix operator *(Matrix left, Matrix right)
        {
            double[,] newMat = new double[Size, Size];

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    newMat[r, c] = Vector4.DotProduct(left[r], GetCol(c, right));
                }
            }

            return new Matrix(newMat);
        }

        /// <summary>
        /// Multiplies a vector with a matrix (col major)
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The product of the matrix and vector.</returns>
        public static Vector4 operator *(Matrix matrix, Vector4 vector)
        {
            Vector4 newVec = new Vector4();

            for (int i = 0; i < 4; i++)
            {
                var multipliedVector = Matrix.GetCol(i, matrix) * vector;
                newVec[i] = Vector4.DotProduct(multipliedVector);
            }
            return newVec;
        }

        /// <summary>
        /// Multiplies a vector with a matrix (row major)
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="vector">The vector3.</param>
        /// <returns>The product of the matrix and vector.</returns>
        public static Vector4 operator *(Matrix matrix, Vector3 vector) => matrix * new Vector4(vector, 1);

        #endregion Operators
    }
}
