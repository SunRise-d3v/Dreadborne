using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Win32;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Launcher;

public class Application : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private string _version;
    private string _serverVersion;
    private string _path;
    private string _exeName;
    private string _url;
    private string _downloadUrl;

    private bool _dowloading;

    private Button _button;
    private MouseState _previousMouseState;

    public static SpriteFont Font;

    public const ushort SCREEN_WIDTH = 800;
    public const ushort SCREEN_HEIGHT = 600;

    private const byte FPS = 60;

    private Texture2D _background;
    private Rectangle _sourceRectangle;

    private RenderTarget2D _renderTarget, _RTHorizontal, _RTVertical;
    private Effect _blur;

    private Texture2D _pixel;

    private static readonly HttpClient _httpClient = new();

    static Application()
    {
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("DreadborneLauncher/1.0");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
    }

    public Application()
    {
        Content.RootDirectory = "source/resources";

        Window.Title = "Dreadborne Launcher | " + _version;
        _graphics = new(this)
        {
            PreferredBackBufferWidth = SCREEN_WIDTH,
            PreferredBackBufferHeight = SCREEN_HEIGHT
        };

        IsFixedTimeStep = true;
        _graphics.SynchronizeWithVerticalRetrace = true;

        TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / FPS);

        IsMouseVisible = true;

        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        _version = "";
        _serverVersion = "";
        _path = @"Game\";
        _exeName = "Client.exe";
        _url = "https://github.com/SunRise-d3v/Dreadborne";

        _dowloading = false;

        _sourceRectangle = new(0, 0, 734, 600);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Font = Content.Load<SpriteFont>("fonts/default");

        _button = new(Content, "textures/button", new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT) * 0.5f)
        {
            Text = "Checking...",
            Enable = false
        };

        _background = Content.Load<Texture2D>("textures/background");

        _blur = Content.Load<Effect>("shaders/blur");

        PresentationParameters parameters = GraphicsDevice.PresentationParameters;
        int width = parameters.BackBufferWidth, height = parameters.BackBufferHeight;

        _renderTarget = new RenderTarget2D(GraphicsDevice, width, height);

        _RTHorizontal = new RenderTarget2D(GraphicsDevice, width / 2, height / 2);
        _RTVertical = new RenderTarget2D(GraphicsDevice, width / 2, height / 2);

        _pixel = new(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _ = CheckForUpdatesAsync();
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboard = Keyboard.GetState();
        MouseState mouse = Mouse.GetState();

        if (keyboard.IsKeyDown(Keys.Escape))
            Exit();

        bool clicked = mouse.LeftButton == ButtonState.Released
            && _previousMouseState.LeftButton == ButtonState.Pressed
            && _button is not null
            && _button.Enable
            && _button.Contains(mouse.Position);

        if (clicked)
            _button.OnClick();

        _previousMouseState = mouse;

        base.Update(gameTime);
    }

    #region Application Render
    protected override void Draw(GameTime gameTime)
    {
        float strength = 0.725f;

        GraphicsDevice.Clear(Color.Black);

        GraphicsDevice.SetRenderTarget(_renderTarget);
        _spriteBatch.Begin();

        _spriteBatch.Draw(_background, new Vector2(_sourceRectangle.Width + 19, _sourceRectangle.Height - 15) * 0.5f, _sourceRectangle, Color.White, 0f, new Vector2(_sourceRectangle.Width, _sourceRectangle.Height) * 0.5f, 1.15f, SpriteEffects.None, 0.1f);

        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(_RTHorizontal);
        _blur.Parameters["Direction"].SetValue(new Vector2(1f / _renderTarget.Width, 0));
        _blur.Parameters["Weights"].SetValue([0.2270270270f, 0.3162162162f, 0.0702702703f]);
        _blur.Parameters["Offsets"].SetValue([0f, 1.3846153846f * strength, 3.2307692308f * strength]);

        _spriteBatch.Begin(effect: _blur, samplerState: SamplerState.LinearClamp);
        _spriteBatch.Draw(_renderTarget, _RTHorizontal.Bounds, Color.White);
        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(_RTVertical);
        _blur.Parameters["Direction"].SetValue(new Vector2(0, 1f / _RTHorizontal.Height));
        _blur.Parameters["Weights"].SetValue([0.2270270270f, 0.3162162162f, 0.0702702703f]);
        _blur.Parameters["Offsets"].SetValue([0f, 1.3846153846f * strength, 3.2307692308f * strength]);

        _spriteBatch.Begin(effect: _blur, samplerState: SamplerState.LinearClamp);
        _spriteBatch.Draw(_RTHorizontal, _RTVertical.Bounds, Color.White);
        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);

        _spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_RTVertical, GraphicsDevice.Viewport.Bounds, Color.White);
        _spriteBatch.Draw(_pixel, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.Black * 0.7f, 0f, Vector2.Zero, SpriteEffects.None, 0.02f);

        DrawUI();

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawUI()
    {
        _button.Draw(_spriteBatch, 0.03f, Color.White, Color.White);
    }
    #endregion

    #region Application Logic
    private async Task CheckForUpdatesAsync()
    {
        try
        {
            _version = GetLocalVersion();

            (string latestVersion, string downloadUrl) = await GetLatestReleaseAsync();
            _serverVersion = latestVersion;
            _downloadUrl = downloadUrl;

            Window.Title = $"Dreadborne Launcher | {_version}/{_serverVersion}";

            bool installed = File.Exists(Path.GetFullPath(_path + _exeName));

            if (_version != _serverVersion)
            {
                _button.Text = installed ? "Update" : "Download";
                _button.Click = UpdateGame;
            }
            else
            {
                _button.Text = "Play";
                _button.Click = Play;
            }
        }
        catch (Exception ex)
        {
            _button.Text = "Retry";
            _button.Click = () => _ = CheckForUpdatesAsync();
            MessageBox.Show("Error", "Failed to check for updates: " + ex.Message, ["OK"]);
        }
        finally
        {
            _button.Enable = true;
        }
    }

    private static string GetLocalVersion()
    {
        using RegistryKey key = Registry.CurrentUser.CreateSubKey("Dreadborne");

        if (key.GetValue("Version") is not string version)
        {
            version = "none";
            key.SetValue("Version", version);
        }

        return version;
    }

    private string GetReleasesApiUrl()
    {
        string repoPath = _url
            .Replace("https://github.com/", string.Empty)
            .Replace("http://github.com/", string.Empty)
            .Trim('/');

        return $"https://api.github.com/repos/{repoPath}/releases";
    }

    private async Task<(string version, string downloadUrl)> GetLatestReleaseAsync()
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(GetReleasesApiUrl());
        response.EnsureSuccessStatusCode();

        await using Stream stream = await response.Content.ReadAsStreamAsync();
        using JsonDocument document = await JsonDocument.ParseAsync(stream);

        JsonElement releases = document.RootElement;
        if (releases.GetArrayLength() == 0)
            throw new InvalidOperationException("The repository has no published releases yet.");

        JsonElement latest = releases[0];
        string tagName = latest.GetProperty("tag_name").GetString();

        string assetUrl = null;
        foreach (JsonElement asset in latest.GetProperty("assets").EnumerateArray())
        {
            string name = asset.GetProperty("name").GetString();
            if (name is not null && name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                assetUrl = asset.GetProperty("browser_download_url").GetString();
                break;
            }
        }

        if (assetUrl is null)
            throw new InvalidOperationException("The latest GitHub release has no .zip asset attached.");

        return (tagName, assetUrl);
    }

    private async void UpdateGame()
    {
        if (_dowloading)
            return;

        _dowloading = true;
        _button.Enable = false;
        _button.Text = "Downloading... 0%";

        string zipPath = Path.Combine(Path.GetTempPath(), "Dreadborne.zip");

        try
        {
            string fullPath = Path.GetFullPath(_path);
            Directory.CreateDirectory(fullPath);

            DirectoryInfo directory = new(fullPath);
            foreach (FileInfo item in directory.GetFiles())
                item.Delete();
            foreach (DirectoryInfo item in directory.GetDirectories())
                item.Delete(true);

            using (HttpResponseMessage response = await _httpClient.GetAsync(_downloadUrl, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                long? totalBytes = response.Content.Headers.ContentLength;

                await using Stream contentStream = await response.Content.ReadAsStreamAsync();
                await using FileStream fileStream = new(zipPath, FileMode.Create, FileAccess.Write, FileShare.None);

                byte[] buffer = new byte[81920];
                long totalRead = 0;
                int bytesRead;

                while ((bytesRead = await contentStream.ReadAsync(buffer)) > 0)
                {
                    await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
                    totalRead += bytesRead;

                    if (totalBytes.HasValue && totalBytes.Value > 0)
                    {
                        int percent = (int)(totalRead * 100 / totalBytes.Value);
                        _button.Text = $"Downloading... {percent}%";
                    }
                }
            }

            ZipFile.ExtractToDirectory(zipPath, fullPath, overwriteFiles: true);

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Dreadborne"))
            {
                key.SetValue("Version", _serverVersion);
            }

            _version = _serverVersion;
            Window.Title = $"Dreadborne Launcher | {_version}/{_serverVersion}";

            _button.Text = "Play";
            _button.Click = Play;
        }
        catch (Exception ex)
        {
            _button.Text = "Retry";
            _button.Click = UpdateGame;
            MessageBox.Show("Error", "Failed to download the update: " + ex.Message, ["OK"]);
        }
        finally
        {
            if (File.Exists(zipPath))
            {
                try { File.Delete(zipPath); } catch { /* best effort cleanup */ }
            }

            _dowloading = false;
            _button.Enable = true;
        }
    }

    private void Play()
    {
        string exePath = Path.GetFullPath(Path.Combine(_path, _exeName));

        ProcessStartInfo startInfo = new(exePath)
        {
            WorkingDirectory = Path.GetFullPath(_path),
            UseShellExecute = true
        };

        Process.Start(startInfo);
        Exit();
    }
    #endregion
}