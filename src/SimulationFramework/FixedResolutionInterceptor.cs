using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using SimulationFramework.Messaging;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SimulationFramework;

/// <summary>
/// Provides the functionality to <see cref="Simulation.SetFixedResolution(int, int, SimulationFramework.Color, bool, bool, bool)"/> via a component.
/// </summary>
public sealed class FixedResolutionInterceptor : ISimulationComponent
{
    /// <summary>
    /// The width of the off-screen texture.
    /// </summary>
    public int Width { get; private set; }

    /// <summary>
    /// The height of the off-screen texture.
    /// </summary>
    public int Height { get; private set; }

    /// <summary>
    /// The color to fill the window area not covered by the texture.
    /// </summary>
    public Color BackgroundColor { get; set; }

    /// <summary>
    /// Whether the framebuffer should be rendered to the window with transparency.
    /// </summary>
    public bool Transparent { get; set; }

    /// <summary>
    /// Whether the off-screen texture should be stretched to fit the window.
    /// </summary>
    public bool StretchToFit { get; set; }

    /// <summary>
    /// Whether resolution dependent input values (such as <see cref="Mouse.Position"/>) should report values more precise than one pixel (when possible).
    /// </summary>
    public bool SubpixelInput { get => mouseProvider!.SubpixelInput; set => mouseProvider!.SubpixelInput = value; }

    /// <summary>
    /// The off-screen texture.
    /// </summary>
    public ITexture FrameBuffer { get; private set; }


    private FixedResolutionMouseProvider? mouseProvider;
    private FixedResolutionWindowProvider? windowProvider;

    /// <summary>
    /// Creates a new instance of the <see cref="FixedResolutionInterceptor"/> class.
    /// </summary>
    public FixedResolutionInterceptor(int width, int height, Color backgroundColor, bool transparent, bool subpixelInput, bool stretchToFit)
    {
        this.BackgroundColor = backgroundColor;
        this.Transparent = transparent;
        this.StretchToFit = stretchToFit;

        Resize(width, height);
        Application.InterceptComponent<IMouseProvider>(mouseProvider => this.mouseProvider = new FixedResolutionMouseProvider(mouseProvider, subpixelInput));
        Application.InterceptComponent<IWindowProvider>(windowProvider => this.windowProvider = new FixedResolutionWindowProvider(windowProvider, new(width, height)));
    }

    private void AfterEvents(AfterEventsMessage message)
    {
        var outputCanvas = Application.GetComponent<IGraphicsProvider>().GetFrameCanvas();
        outputCanvas.ResetState();

        this.mouseProvider!.transform
            .Reset()
            .Translate(outputCanvas.Width / 2f, outputCanvas.Height / 2f)
            .Scale(GetScale(outputCanvas.Width, outputCanvas.Height))
            .Translate(-FrameBuffer.Width / 2f, -FrameBuffer.Height / 2f);
    }

    internal void AfterRender()
    {
        var outputCanvas = Application.GetComponent<IGraphicsProvider>().GetFrameCanvas();
        outputCanvas.Clear(this.BackgroundColor);
        outputCanvas.Translate(outputCanvas.Width / 2f, outputCanvas.Height / 2f);
        outputCanvas.Scale(GetScale(outputCanvas.Width, outputCanvas.Height));
        if (!Transparent)
        {
            outputCanvas.Fill(Color.Black);
            outputCanvas.DrawRect(0, 0, FrameBuffer.Width, FrameBuffer.Height, Alignment.Center);
        }
        outputCanvas.DrawTexture(FrameBuffer, Alignment.Center);
        outputCanvas.Flush();
    }

    /// <inheritdoc/>
    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<AfterEventsMessage>(AfterEvents);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        FrameBuffer.Dispose();
    }

    /// <summary>
    /// Resizes the off-screen texture.
    /// </summary>
    /// <param name="width">The new width of the texture.</param>
    /// <param name="height">The new height of the texture.</param>
    [MemberNotNull(nameof(FrameBuffer))]
    public void Resize(int width, int height)
    {
        FrameBuffer?.Dispose();
        FrameBuffer = Graphics.CreateTexture(width, height);
        this.Width = width;
        this.Height = height;
        if (this.windowProvider is not null)
        {
            this.windowProvider.FixedSize = new(width, height);
        }
    }

    private Vector2 GetScale(int outputWidth, int outputHeight)
    {
        Vector2 scale = new(outputWidth / (float)FrameBuffer.Width, outputHeight / (float)FrameBuffer.Height);

        if (!StretchToFit)
        {
            float min = MathF.Min(scale.X, scale.Y);
            scale = new(min, min);
        }

        return scale;
    }

    internal void Render()
    {
        var outputCanvas = Graphics.GetOutputCanvas();
        outputCanvas.Clear(this.BackgroundColor);
        outputCanvas.Translate(outputCanvas.Width / 2f, outputCanvas.Height / 2f);
        outputCanvas.Scale(GetScale(outputCanvas.Width, outputCanvas.Height));
        if (!Transparent)
        {
            outputCanvas.Fill(Color.Black);
            outputCanvas.DrawRect(0, 0, FrameBuffer.Width, FrameBuffer.Height, Alignment.Center);
        }
        outputCanvas.DrawTexture(FrameBuffer, Alignment.Center);
        outputCanvas.Flush();
    }

    // hijacks mouse position to be accurate to fake framebuffer
    private class FixedResolutionMouseProvider : IMouseProvider
    {
        private readonly IMouseProvider original;
        public MatrixBuilder transform = new();
        public bool SubpixelInput { get; set; }

        private Vector2 mousePosition;
        private Vector2? lastMousePosition;

        public FixedResolutionMouseProvider(IMouseProvider original, bool subpixelInput)
        {
            this.original = original;
            this.SubpixelInput = subpixelInput;
        }

        public Vector2 Position
        {
            get => mousePosition;
            set
            {
                var result = Vector2.Transform(value, transform.Matrix);
                original.Position = new(MathF.Floor(result.X), MathF.Floor(result.Y));
                RecalculateMousePosition();
            }
        }
        public Vector2 DeltaPosition
        {
            get
            {
                return mousePosition - lastMousePosition!.Value;
            }
        }

        public float ScrollWheelDelta => original.ScrollWheelDelta;
        public bool Visible
        {
            get => original.Visible;
            set => original.Visible = value;
        }
        public IEnumerable<MouseButton> HeldButtons => original.HeldButtons;
        public IEnumerable<MouseButton> PressedButtons => original.PressedButtons;
        public IEnumerable<MouseButton> ReleasedButtons => original.ReleasedButtons;

        public event MouseButtonEvent ButtonPressed
        {
            add => original.ButtonPressed += value;
            remove => original.ButtonPressed -= value;
        }
        public event MouseButtonEvent ButtonReleased
        {
            add => original.ButtonReleased += value;
            remove => original.ButtonReleased -= value;
        }

        public void Dispose()
        {
            original.Dispose();
        }

        public void Initialize(MessageDispatcher dispatcher)
        {
            dispatcher.Subscribe<AfterEventsMessage>(m =>
            {
                if (lastMousePosition is not null)
                {
                    lastMousePosition = mousePosition;
                }
                RecalculateMousePosition();
                lastMousePosition ??= mousePosition;
            });
        }

        private void RecalculateMousePosition()
        {
            mousePosition = Vector2.Transform(original.Position, transform.InverseMatrix);
            if (!SubpixelInput)
                mousePosition = new(MathF.Floor(mousePosition.X), MathF.Floor(mousePosition.Y));
        }

        public void SetCursor(ReadOnlySpan<Color> cursor, int width, int height, int anchorX, int anchorY)
        {
            original.SetCursor(cursor, width, height, anchorX, anchorY);
        }

        public void SetCursor(SystemCursor cursor)
        {
            original.SetCursor(cursor);
        }
    }

    // hijacks mouse position to be accurate to fake framebuffer
    private class FixedResolutionWindowProvider : IWindowProvider
    {
        public string Title { get => baseProvider.Title; set => baseProvider.Title = value; }
        public IDisplay Display { get => baseProvider.Display; }
        public Vector2 Size { get => fixedSize; }
        public Vector2 Position { get => baseProvider.Position; }
        public bool IsUserResizable { get => baseProvider.IsUserResizable; set => baseProvider.IsUserResizable = value; }
        public bool ShowSystemMenu { get => baseProvider.ShowSystemMenu; set => baseProvider.ShowSystemMenu = false; }
        public bool IsMinimized => baseProvider.IsMinimized;
        public bool IsMaximized => baseProvider.IsMaximized;

        private readonly IWindowProvider baseProvider;
        internal Vector2 fixedSize;

        public FixedResolutionWindowProvider(IWindowProvider baseProvider, Vector2 fixedSize)
        {
            this.baseProvider = baseProvider;
            this.fixedSize = fixedSize;

        public void Dispose()
        {
            baseProvider.Dispose();
        }

        public ITexture GetBackBuffer()
        {
            return baseProvider.GetBackBuffer();
        }

        public void Initialize(MessageDispatcher dispatcher)
        {
            baseProvider.Initialize(dispatcher);
        }

        public void Maximize()
        {
            baseProvider.Maximize();
        }

        public void Minimize()
        {
            baseProvider.Minimize();
        }

        public void Resize(Vector2 size)
        {
            baseProvider.Resize(size);
        }

        public void Restore()
        {
            baseProvider.Restore();
        }

        public void SetIcon(ReadOnlySpan<Color> icon, int width, int height)
        {
            baseProvider.SetIcon(icon, width, height);
        }

        public void SetPosition(Vector2 position)
        {
            baseProvider.SetPosition(position);
        }
    }
}