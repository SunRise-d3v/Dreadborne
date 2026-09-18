namespace Dreadborne.Graphics;

public sealed class TextureAtlas
{
    private Dictionary<string, TextureRegion> _regions;
    private Dictionary<string, Animation> _animations;

    public Texture2D Texture { get; set; }

    public TextureAtlas()
    {
        _regions = new Dictionary<string, TextureRegion>();
        _animations = new Dictionary<string, Animation>();
    }

    public TextureAtlas(Texture2D texture)
    {
        Texture = texture;
        _regions = new Dictionary<string, TextureRegion>();
        _animations = new Dictionary<string, Animation>();
    }

    #region Texture
    public void AddRegion(string name, int x, int y, int width, int height)
    {
        TextureRegion region = new TextureRegion(Texture, x, y, width, height);
        _regions.Add(name, region);
    }

    public TextureRegion GetRegion(string name)
    {
        return _regions[name];
    }

    public bool RemoveRegion(string name)
    {
        return _regions.Remove(name);
    }

    public void Clear()
    {
        _regions.Clear();
    }
    #endregion

    #region Sprite
    public Sprite CreateSprite(string regionName)
    {
        TextureRegion region = GetRegion(regionName);
        return new Sprite(region);
    }
    #endregion

    #region Animation
    public void AddAnimation(string animationName, Animation animation)
    {
        _animations.Add(animationName, animation);
    }

    public Animation GetAnimation(string animationName)
    {
        return _animations[animationName];
    }

    public bool RemoveAnimation(string animationName)
    {
        return _animations.Remove(animationName);
    }

    public AnimatedSprite CreateAnimatedSprite(string animationName)
    {
        Animation animation = GetAnimation(animationName);
        return new AnimatedSprite(animation);
    }
    #endregion

    public static TextureAtlas FromFile(ContentManager content, string fileName)
    {
        TextureAtlas atlas = new TextureAtlas();

        string filePath = Path.Combine(content.RootDirectory, fileName);

        using (Stream stream = TitleContainer.OpenStream(filePath + ".xml"))
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XDocument doc = XDocument.Load(reader);
                XElement root = doc.Root;

                string texturePath = root.Element("Texture").Value;
                atlas.Texture = content.Load<Texture2D>(texturePath);

                var regions = root.Element("Regions")?.Elements("Region");

                if (regions != null)
                {
                    foreach (var region in regions)
                    {
                        string name = region.Attribute("name")?.Value;

                        int x = ParseIntValue(region.Attribute("x")?.Value, 0);
                        int y = ParseIntValue(region.Attribute("y")?.Value, 0);
                        int width = ParseIntValue(region.Attribute("width")?.Value, 0);
                        int height = ParseIntValue(region.Attribute("height")?.Value, 0);

                        if (!string.IsNullOrEmpty(name))
                        {
                            atlas.AddRegion(name, x, y, width, height);
                        }
                    }
                }

                var animationElements = root.Element("Animations")?.Elements("Animation");

                if (animationElements != null)
                {
                    foreach (var animationElement in animationElements)
                    {
                        string name = animationElement.Attribute("name")?.Value;
                        float delayInMilliseconds = float.Parse(animationElement.Attribute("delay")?.Value ?? "0");
                        TimeSpan delay = TimeSpan.FromMilliseconds(delayInMilliseconds);

                        List<TextureRegion> frames = new List<TextureRegion>();

                        var frameElements = animationElement.Elements("Frame");

                        if (frameElements != null)
                        {
                            foreach (var frameElement in frameElements)
                            {
                                string regionName = frameElement.Attribute("region").Value;
                                TextureRegion region = atlas.GetRegion(regionName);
                                frames.Add(region);
                            }
                        }

                        Animation animation = new Animation(frames, delay);
                        atlas.AddAnimation(name, animation);
                    }
                }

                return atlas;
            }
        }
    }

    private static int ParseIntValue(string value, int defaultValue)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        if (int.TryParse(value, out int result))
        {
            return result;
        }

        int dotIndex = value.LastIndexOf('.');

        if (dotIndex <= 0 || dotIndex == value.Length - 1)
        {
            throw new FormatException($"Не удалось разобрать значение '{value}' как число или ссылку вида Class.Member.");
        }

        string typeName = value.Substring(0, dotIndex);
        string memberName = value.Substring(dotIndex + 1);

        Type type = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null); }
            })
            .FirstOrDefault(t => t.Name == typeName);

        if (type == null)
        {
            throw new InvalidOperationException($"Не найден тип '{typeName}' для значения '{value}'.");
        }

        FieldInfo field = type.GetField(memberName, BindingFlags.Public | BindingFlags.Static);
        if (field != null)
        {
            return Convert.ToInt32(field.GetValue(null));
        }

        PropertyInfo property = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.Static);
        if (property != null)
        {
            return Convert.ToInt32(property.GetValue(null));
        }

        throw new InvalidOperationException($"Не найдено статическое поле или свойство '{memberName}' в типе '{typeName}'.");
    }
}