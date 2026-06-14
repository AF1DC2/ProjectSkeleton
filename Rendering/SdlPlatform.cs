// AI-generated
using Silk.NET.Maths;
using Silk.NET.SDL;
using TheAdventure.Input;

namespace TheAdventure.Rendering;

/// <summary> Owns every native SDL resource the game needs </summary>
public sealed unsafe class SdlPlatform : IDisposable
{
    private readonly Sdl _sdl;
    private readonly Window* _window;
    private readonly Renderer* _renderer;
    private readonly byte* _keyboardState;
    private bool _disposed;

    public SdlPlatform(string title, int width, int height)
    {
        Width = width;
        Height = height;

        _sdl = new Sdl(new SdlContext());

        if (_sdl.Init(Sdl.InitVideo | Sdl.InitEvents | Sdl.InitTimer) < 0)
        {
            throw new InvalidOperationException("Failed to initialise SDL.");
        }

        _window = _sdl.CreateWindow(
            title, Sdl.WindowposCentered, Sdl.WindowposCentered, width, height,
            (uint)WindowFlags.AllowHighdpi);
        if (_window is null)
        {
            throw (Exception?)_sdl.GetErrorAsException() ?? new InvalidOperationException("Failed to create window.");
        }

        _renderer = _sdl.CreateRenderer(_window, -1, (uint)RendererFlags.Accelerated);
        if (_renderer is null)
        {
            throw (Exception?)_sdl.GetErrorAsException() ?? new InvalidOperationException("Failed to create renderer.");
        }

        _sdl.RenderSetVSync(_renderer, 1);
        _keyboardState = _sdl.GetKeyboardState(null);
    }

    public int Width { get; }
    public int Height { get; }

    public void SetTitle(string title) => _sdl.SetWindowTitle(_window, title);

    public void PumpEvents(InputState input)
    {
        input.BeginFrame();

        var ev = new Event();
        while (_sdl.PollEvent(ref ev) != 0)
        {
            switch ((EventType)ev.Type)
            {
                case EventType.Quit:
                    input.QuitRequested = true;
                    break;
                case EventType.Keydown:
                    input.RegisterKeyDown((KeyCode)ev.Key.Keysym.Scancode);
                    break;
            }
        }
    }

    public void Clear(Color color)
    {
        SetDrawColor(color);
        _sdl.RenderClear(_renderer);
    }

    public void FillRect(int x, int y, int w, int h, Color color)
    {
        SetDrawColor(color);
        var rect = new Rectangle<int>(x, y, w, h);
        _sdl.RenderFillRect(_renderer, ref rect);
    }

    public void DrawRectOutline(int x, int y, int w, int h, Color color)
    {
        SetDrawColor(color);
        var rect = new Rectangle<int>(x, y, w, h);
        _sdl.RenderDrawRect(_renderer, ref rect);
    }

    public void Present() => _sdl.RenderPresent(_renderer);

    private void SetDrawColor(Color color) =>
        _sdl.SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        if (_renderer is not null)
        {
            _sdl.DestroyRenderer(_renderer);
        }

        if (_window is not null)
        {
            _sdl.DestroyWindow(_window);
        }

        _sdl.Quit();
        _sdl.Dispose();
        _disposed = true;
    }
}
// end AI-generated
