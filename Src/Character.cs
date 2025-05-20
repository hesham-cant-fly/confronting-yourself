using Raylib_cs;
using System.Xml;
using Newtonsoft.Json;
using Math;

public enum AnimationKind {
    Idle,
    SingDown,
    SingUp,
    SingRight,
    SingLeft

}

public class Character
{
    public Texture2D Texture { get; private set; }
    public Dictionary<string, List<Rectangle>> Frames { get; private set; } = new();
    public Dictionary<AnimationKind, Animation> Animations { get; private set; } = new();
    public AnimationKind CurrentAnimation { get; set; } = AnimationKind.Idle;
    public Vec2 Pos = Vec2.Zero;
    public Vec2 Scale = Vec2.One;
    public bool FlipX = false;
    public bool FlipY = false;

    ~Character()
    {
        Raylib.UnloadTexture(Texture);
    }

    public Rectangle CurrentFrame
    {
        get
        {
            var frame = Frames[Animations[CurrentAnimation].Name][Animations[CurrentAnimation].GetCurrentIndex()];
            frame.Width *= FlipX ? -1 : 1;
            frame.Height *= FlipY ? -1 : 1;
            return frame;
        }
    }

    public Rectangle CurrentRect
    {
        get
        {
            var dist = CurrentFrame;
            dist.X = (float)Pos.X;
            dist.Y = (float)Pos.Y;
            dist.Width = (float)(System.Math.Abs(dist.Width) * Scale.X);
            dist.Height = (float)(System.Math.Abs(dist.Height) * Scale.Y);
            return dist;
        }
    }

    public float Bottom
    {
        get => CurrentRect.GetBottom();
        set
        {
            Pos.Y = value - CurrentRect.Height;
        }
    }
    public float Right
    {
        get => CurrentRect.GetRight();
        set
        {
            Pos.X = value - CurrentRect.Width;
        }
    }

    public void Update()
    {
        Animations[CurrentAnimation].GetCurrentIndex();
    }

    public void Draw()
    {
        var index = Animations[CurrentAnimation].GetCurrentIndex();
        var frame = CurrentFrame;
        var dist = CurrentRect;
        Raylib.DrawTexturePro(
            Texture,
            frame,
            dist, Vec2.Zero,
            0f, Color.White
        );
    }

    public void PlayAnimation(AnimationKind name, bool reset = true)
    {
        if (Animations.ContainsKey(name) && name != CurrentAnimation) {
            CurrentAnimation = name;
            if (reset) {
                Animations[name].Reset();
            }
        }
    }

    public static Character LoadFromJSON(string path)
    {
        var root_path = Path.GetDirectoryName(path);
        CharacterJson j;
        using (StreamReader reader = new(path)) {
            string jsonContent = reader.ReadToEnd();
            CharacterJson? jj = JsonConvert.DeserializeObject<CharacterJson>(jsonContent);
            if (jj == null) {
                throw new Exception("");
            }
            j = jj;
        }
        var result = LoadFromXML(
            Path.Join(
                root_path, "..", j.image + ".xml"
            )
        );
        foreach (var anim in j.animations) {
            Animation animation = new(anim.name);
            animation.Loop = anim.loop;
            animation.Fps = anim.fps;
            if (anim.indices.Count() == 0) {
                var frames = result.Frames[anim.name];
                var keys = new List<int>();
                int i = 0;
                foreach (var _ in frames) keys.Add(i++);
                animation.SetIndices(keys);
            } else {
                animation.SetIndices(anim.indices);
            }
            result.Animations[FromString(anim.anim)] = animation;
        }
        return result;
    }

    private static Character LoadFromXML(string path)
    {
        var result = new Character();
        var root_path = Path.GetDirectoryName(path);
        var doc = new XmlDocument();
        doc.Load(path);
        var atlas = doc.SelectSingleNode("TextureAtlas");
        if (atlas == null) throw new Exception("");

        var image_path = Path.Join(root_path, atlas.Attributes?["imagePath"]?.InnerText);
        result.Texture = Raylib.LoadTexture(image_path);

        var subTextures = atlas.SelectNodes("SubTexture");
        if (subTextures == null) throw new Exception("");

        foreach (XmlNode st in subTextures) {
            if (st.Attributes == null) throw new Exception("");
            var attributes = st.Attributes;
            int x = int.Parse(attributes["x"]?.InnerText ?? "0"),
                y = int.Parse(attributes["y"]?.InnerText ?? "0"),
                width = int.Parse(attributes["width"]?.InnerText ?? "0"),
                height = int.Parse(attributes["height"]?.InnerText ?? "0");
            string name;
            int id;
            {
                var nameAttr = st.Attributes["name"]?.InnerText;
                if (nameAttr == null) throw new Exception("");
                name = nameAttr[..(nameAttr.Count() - 4)];
                id = int.Parse(nameAttr[(nameAttr.Count() - 4)..]);
            }

            Rectangle rect = new(
                x, y,
                width, height
            );
            if (!result.Frames.ContainsKey(name)) {
                result.Frames.Add(name, new());
            }

            if (result.Frames[name].Count() > id) {
                for (int i = 0; i < id; i++) {
                    result.Frames[name].Add(new());
                }
                result.Frames[name][id] = rect;
            } else if (result.Frames[name].Count() != id) {
                result.Frames[name][id] = rect;
            } else {
                result.Frames[name].Add(rect);
            }
        }

        return result;
    }

    public static AnimationKind FromString(string kind)
    {
        switch (kind) {
            case "idle":
                return AnimationKind.Idle;
            case "singDOWN":
                return AnimationKind.SingDown;
            case "singUP":
                return AnimationKind.SingUp;
            case "singRIGHT":
                return AnimationKind.SingRight;
            case "singLEFT":
                return AnimationKind.SingLeft;
            default:
                throw new Exception("");
        }
    }
}

record CharacterJson
{
    public string image = "";
    public List<AnimationJson> animations = new();

    public record AnimationJson
    {
        public bool loop = true;
        public int fps = 24;
        public string anim = "";
        public List<int> indices = new();
        public string name = "";
    }
}

public class Animation(string name)
{
    public bool Loop { get; set; } = true;
    public string Name { get; set; } = name;
    private float FrameDuration = 1.0f / 24;
    private float LastUpdate = 0.0f;
    private int CurrentIndex = 0;
    private List<int> IndicesList = new();
    public int Fps {
        get => (int)(1.0 / FrameDuration);
        set {
            FrameDuration = (float)(1.0 / System.Math.Max(1, value));
        }
    }

    public void Reset()
    {
        LastUpdate = (float)Raylib.GetTime();
        CurrentIndex = 0;
    }

    public void SetIndices(List<int> indices)
    {
        IndicesList = indices;
        Reset();
    }

    public int GetCurrentIndex()
    {
        double t = Raylib.GetTime();
        double elapsed = t - LastUpdate;
        int framesPassed = (int)(elapsed / FrameDuration);

        if (framesPassed > 0) {
            LastUpdate += framesPassed * FrameDuration;
            if (Loop)
            {
                CurrentIndex = IndicesList[(CurrentIndex + framesPassed) % IndicesList.Count()];
            }
            else
            {
                CurrentIndex = IndicesList[CurrentIndex + framesPassed < IndicesList.Count() ? CurrentIndex + framesPassed : IndicesList.Count() - 1];
            }
        }

        return CurrentIndex;
    }
}
