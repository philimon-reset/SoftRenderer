namespace SoftRenderer.Engine.Input.Operator
{
    using System.Windows.Forms;
    using SoftRenderer.Engine.Input.EventArgs;
    using SoftRenderer.Engine.Render;

    /// <inheritdoc/>
    public abstract class Operator : IOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Operator"/> class.
        /// </summary>
        /// <param name="renderBase">render base instance.</param>
        protected Operator(RenderBase renderBase)
        {
            this.RenderBase = renderBase;
            this.Input = renderBase.HostInput;

            this.Input.SizeChanged += this.ControlSizeChanged;
            this.Input.MouseMove += this.ControlMouseMove;
            this.Input.MousePress += this.ControlMousePress;
            this.Input.MouseRelease += this.ControlMouseRelease;
            this.Input.MouseScroll += this.ControlMouseScroll;
            this.Input.KeyDown += this.ControlKeyUp;
            this.Input.KeyUp += this.ControlKeyDown;
        }

        /// <inheritdoc/>
        public RenderBase RenderBase { get; private set; }

        /// <inheritdoc/>
        public IInput Input { get; private set; }

        /// <summary>
        /// Dispose of instance.
        /// </summary>
        public void Dispose()
        {
            this.Input.SizeChanged -= this.ControlSizeChanged;
            this.Input.MouseMove -= this.ControlMouseMove;
            this.Input.MouseRelease -= this.ControlMouseRelease;
            this.Input.MousePress -= this.ControlMousePress;
            this.Input.MouseScroll -= this.ControlMouseScroll;
            this.Input.KeyDown -= this.ControlKeyUp;
            this.Input.KeyUp -= this.ControlKeyDown;

            this.RenderBase?.Dispose();
            this.Input?.Dispose();

            this.RenderBase = default;
            this.Input = default;
        }

        /// <summary>
        /// Event handler for SizeChanged.
        /// <param name="sender">sender of the event.</param>
        /// <param name="e">info related to the event.</param>
        /// </summary>
        protected virtual void ControlSizeChanged(object sender, ISizeChangeArgs e)
        {
            
        }

        /// <summary>
        /// Event handler for MouseMove.
        /// <param name="sender">sender of the event.</param>
        /// <param name="e">info related to the event.</param>
        /// </summary>
        protected virtual void ControlMouseMove(object sender, MouseEventArgs e)
        {
            
        }

        /// <summary>
        /// Event handler for MouseDown.
        /// <param name="sender">sender of the event.</param>
        /// <param name="e">info related to the event.</param>
        /// </summary>
        protected virtual void ControlMousePress(object sender, MouseEventArgs e)
        {
            
        }

        /// <summary>
        /// Event handler for MouseUp.
        /// <param name="sender">sender of the event.</param>
        /// <param name="e">info related to the event.</param>
        /// </summary>
        protected virtual void ControlMouseRelease(object sender, MouseEventArgs e)
        {
            
        }

        /// <summary>
        /// Event handler for MouseWheel.
        /// <param name="sender">sender of the event.</param>
        /// <param name="e">info related to the event.</param>
        /// </summary>
        protected virtual void ControlMouseScroll(object sender, MouseEventArgs e)
        {
            
        }

        /// <summary>
        /// Event handler for KeyDown.
        /// <param name="sender">sender of the event.</param>
        /// <param name="e">info related to the event.</param>
        /// </summary>
        protected virtual void ControlKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        /// <summary>
        /// Event handler for KeyUp.
        /// <param name="sender">sender of the event.</param>
        /// <param name="e">info related to the event.</param>
        /// </summary>
        protected virtual void ControlKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            
        }
    }
}
