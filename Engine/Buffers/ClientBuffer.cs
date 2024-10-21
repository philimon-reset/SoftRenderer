namespace SoftRenderer.Engine.Buffers
{
    using System;
    using System.Drawing;

    public record struct ClientBuffer
    {
        /// <summary>
        /// Gets the X-coordinate offset.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the Y-coordinate offset.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Gets or sets the minimum Z value.
        /// </summary>
        public double MinZ { get; set; }

        /// <summary>
        /// Gets or sets the maximum Z value.
        /// </summary>
        public double MaxZ { get; set; }

        /// <summary>
        /// Gets the height dimension.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the width dimension.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the size, which is a combination of width and height.
        /// </summary>
        public Size Size
        {
            get => new Size(this.Width, this.Height);
        }

        public Rectangle ClientRectangle
        {
            get => new Rectangle(this.Location, this.Size);
        }

        /// <summary>
        /// Gets the location, which is a combination of the X and Y coordinates.
        /// </summary>
        public Point Location
        {
            get => new Point(this.X, this.Y);
        }

        /// <summary>
        /// Gets the aspect ratio, which is the ratio of the width to the height.
        /// </summary>
        public double AspectRatio
        {
            get => (double)this.Height / this.Width;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBuffer"/> class.
        /// </summary>
        /// <param name="x">x offset.</param>
        /// <param name="y">y offset.</param>
        /// <param name="minZ">minZ.</param>
        /// <param name="maxZ">maxZ.</param>
        /// <param name="width">width of client buffer.</param>
        /// <param name="height">height of client buffer.</param>
        public ClientBuffer(int x, int y, double minZ, double maxZ, int width, int height)
        {
            this.MinZ = minZ;
            this.MaxZ = maxZ;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBuffer"/> class.
        /// </summary>
        /// <param name="size">size of client buffer.</param>
        /// <param name="x">x offset.</param>
        /// <param name="y">y offset.</param>
        /// <param name="minZ">minZ.</param>
        /// <param name="maxZ">maxZ.</param>
        public ClientBuffer(Size size, int x = 0, int y = 0, double minZ = 0.0, double maxZ = 1.0)
            : this(x, y, minZ, maxZ, size.Width, size.Height)
        {
        }
    }
}
