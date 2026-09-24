using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Resources;
using System.Text;

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

    private bool _dowloading;

    private Button _button;

    public static SpriteFont Font;

    public const ushort SCREEN_WIDTH = 800;
    public const ushort SCREEN_HEIGHT = 600;

    private const byte FPS = 45;

    private Texture2D _background;
    private Rectangle _sourceRectangle;

    RenderTarget2D sceneRT, rtA, rtB;
    Effect blur;

    public Application()
    {
        Content.RootDirectory = "source/resources";

        Window.Title = "Dreadborne Launcher | " + "0.0.1a";
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
        base.Initialize();

        // TODO: Add your initialization logic here
        _version = "";
        _serverVersion = "";
        _path = @"Game\";
        _exeName = "Client.exe";
        _url = "https://github.com/SunRise-d3v/Dreadborne";

        _dowloading = false;

        _sourceRectangle = new(0, 0, 734, 600);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        Font = Content.Load<SpriteFont>("fonts/default");

        _button = new(Content, "textures/button", new(100, 100));
        _background = Content.Load<Texture2D>("textures/background");

        string dir = System.IO.Path.Combine(AppContext.BaseDirectory, "Source", "Resources", "Shaders");
        System.Diagnostics.Debug.WriteLine("dir exists: " + System.IO.Directory.Exists(dir));
        if (System.IO.Directory.Exists(dir))
            foreach (var f in System.IO.Directory.GetFiles(dir))
                System.Diagnostics.Debug.WriteLine(f);

        blur = Content.Load<Effect>("shaders/blur");

        var pp = GraphicsDevice.PresentationParameters;
        int w = pp.BackBufferWidth, h = pp.BackBufferHeight;

        sceneRT = new RenderTarget2D(GraphicsDevice, w, h);
        // Половинный размер: быстрее и размытие сильнее
        rtA = new RenderTarget2D(GraphicsDevice, w / 2, h / 2);
        rtB = new RenderTarget2D(GraphicsDevice, w / 2, h / 2);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboard = Keyboard.GetState();

        if (keyboard.IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        float strength = 0.7f;

        GraphicsDevice.Clear(Color.Black);

        GraphicsDevice.SetRenderTarget(sceneRT);
        _spriteBatch.Begin();

        _spriteBatch.Draw(_background, new Vector2(_sourceRectangle.Width + 19, _sourceRectangle.Height - 15) * 0.5f, _sourceRectangle, Color.White, 0f, new Vector2(_sourceRectangle.Width, _sourceRectangle.Height) * 0.5f, 1.15f, SpriteEffects.None, 0.1f);

        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(rtA);
        blur.Parameters["Direction"].SetValue(new Vector2(1f / sceneRT.Width, 0));
        blur.Parameters["Weights"].SetValue(new float[] { 0.2270270270f, 0.3162162162f, 0.0702702703f });
        blur.Parameters["Offsets"].SetValue(new float[] { 0f, 1.3846153846f * strength, 3.2307692308f * strength });
        
        _spriteBatch.Begin(effect: blur, samplerState: SamplerState.LinearClamp);
        _spriteBatch.Draw(sceneRT, rtA.Bounds, Color.White);
        _spriteBatch.End();

        // 3. Вертикальный проход
        GraphicsDevice.SetRenderTarget(rtB);
        blur.Parameters["Direction"].SetValue(new Vector2(0, 1f / rtA.Height));
        blur.Parameters["Weights"].SetValue(new float[] { 0.2270270270f, 0.3162162162f, 0.0702702703f });
        blur.Parameters["Offsets"].SetValue(new float[] { 0f, 1.3846153846f * strength, 3.2307692308f * strength });
        
        _spriteBatch.Begin(effect: blur, samplerState: SamplerState.LinearClamp);
        _spriteBatch.Draw(rtA, rtB.Bounds, Color.White);
        _spriteBatch.End();

        // 4. Выводим на экран
        GraphicsDevice.SetRenderTarget(null);
        
        _spriteBatch.Begin(SpriteSortMode.FrontToBack,samplerState: SamplerState.PointClamp);
        _button.Draw(_spriteBatch);
        _spriteBatch.Draw(rtB, GraphicsDevice.Viewport.Bounds, Color.White);
        _spriteBatch.End();

        // TODO: Add your drawing code here
        base.Draw(gameTime);
    }

    private void Download()
    {
        Directory.CreateDirectory(Path.GetFullPath(_path));
        if (!File.Exists(Path.GetFullPath(_path + _exeName)))
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey("Dreadborne");
            key.SetValue("Version", "none");
            key.Close();

            _version = "none";
        }
        else
        {
            _version = (string)Registry.CurrentUser.OpenSubKey("Dreadborne").GetValue("Version");
        }

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url + "file");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        if (response.StatusCode == HttpStatusCode.OK)
        {
            Stream reciveSteam = response.GetResponseStream();
            StreamReader readerStream = null;

            if (response.CharacterSet == null)
            {
                readerStream = new(reciveSteam);
            }
            else
            {
                readerStream = new(reciveSteam, Encoding.GetEncoding(response.CharacterSet));
            }

            string data = readerStream.ReadToEnd();
            _serverVersion = data;

            response.Close();
            readerStream.Close();
        }
        else
        {
            MessageBox.Show("Error", "An error occurred while uploading the file.", ["OK"]);
        }

        Window.Title = _version + '/' + _serverVersion;

        if (_version != _serverVersion)
        {
            if (File.Exists(Path.GetFullPath(_path + _exeName)))
            {
                //Button.text = "Update
                //Button.Click += UpdateGame
            }
            else
            {
                //Button.text = "Download
                //Button.Click += UpdateGame
            }
        }
        else
        {
            //Button.text = "Play/Launch/Start
            //Button.Click += Play
        }
    }

    private void UpdateGame()
    {
        _dowloading = false;
        //Button.Enable = false

        DirectoryInfo directory = new(Path.GetFullPath(_path));
        foreach (FileInfo item in directory.GetFiles())
        {
            item.Delete();
        }

        foreach (DirectoryInfo item in directory.GetDirectories())
        {
            item.Delete(true);
        }

        using (WebClient client = new())
        {
            client.DownloadProgressChanged += (s, g) =>
            {
                if (g.ProgressPercentage == 100)
                {
                    if (!File.Exists(Path.GetFullPath(_path + _exeName)))
                    {
                        ZipFile.ExtractToDirectory("Game.zip", _path);

                        RegistryKey key = Registry.CurrentUser.CreateSubKey("Dreadborne");
                        key.SetValue("Version", _serverVersion);
                        key.Close();

                        _version = _serverVersion;

                        Window.Title = _version + '/' + _serverVersion;

                        //Button.text = "Play"
                        //Button.Click -= UpdateGame
                        //Button.Click -= Play
                    }

                    //Button.Enable = true
                    _dowloading = false;
                }
            };

            client.DownloadFileAsync(new Uri(_url + "Dreadborne.zip"), "Game.zip");
        }
        ;
    }

    private void Play()
    {
        Process.Start(_path + _exeName);
        Exit();
    }
}