namespace Vector
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    /// <summary>
    /// A vector representation.
    /// </summary>
    /// <remarks>
    /// Based on this tutorial : https://www.codeproject.com/articles/17425/a-vector-type-for-c.
    /// </remarks>
    public class Vector
    {
        private readonly double x;
        private readonly double y;
        private readonly double z;

        /// <summary>
        /// Constructor for the vector
        /// </summary>
        /// <param name="x"> x corrdinate of vector</param>
        /// <param name="y"> y corrdinate of vector</param>
        /// <param name="z"> z corrdinate of vector</param>
        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

    }


}
