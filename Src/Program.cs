using Raylib_cs;

public class Program
{
    public static Resources rc = new();

    public static void Main(String[] args)
    {
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(Raylib.GetMonitorWidth(0), Raylib.GetMonitorHeight(0), "Confronting Yourself");
        Raylib.InitAudioDevice();
        Raylib.SetTargetFPS(60);

        LoadResources();

        Character.LoadFromJSON("./Assets/characters/sonic/states/sonic.json");

        Game game = new Game();
        while (!Raylib.WindowShouldClose())
        {
            game.Input();
            game.Update(Raylib.GetFrameTime());

            Raylib.UpdateMusicStream(rc.Inst);
            Raylib.UpdateMusicStream(rc.Voices);

            Raylib.BeginDrawing();
            // Raylib.ClearBackground(Color.Red);
            game.Draw();

            Raylib.DrawFPS(0, Raylib.GetScreenHeight() / 2);

            Raylib.EndDrawing();
        }

        Raylib.CloseAudioDevice();
        Raylib.CloseWindow();
    }

    public static void LoadResources()
    {
        rc.Act1 = Raylib.LoadTexture("./Assets/title-card/act1.png");
        rc.Confronting = Raylib.LoadTexture("./Assets/title-card/confronting.png");
        rc.RedCircle = Raylib.LoadTexture("./Assets/title-card/redcircle.png");
        rc.Yourself = Raylib.LoadTexture("./Assets/title-card/yourself.png");
        rc.Aiback = Raylib.LoadTexture("./Assets/bg/ai/aiback.png");
        rc.Aifloor = Raylib.LoadTexture("./Assets/bg/ai/aifloor.png");
        rc.Aigreen = Raylib.LoadTexture("./Assets/bg/ai/aigreen.png");

        rc.Inst = Raylib.LoadMusicStream("./Assets/songs/Inst.ogg");
        rc.Voices = Raylib.LoadMusicStream("./Assets/songs/Voices.ogg");

        rc.Sonic = Raylib.LoadFont("./Assets/fonts/sonic.ttf");
        rc.Sonicdebugfont = Raylib.LoadFont("./Assets/fonts/sonicdebugfont.ttf");
        rc.Sonic = Raylib.LoadFont("./Assets/fonts/sonichud.ttf");
    }
}

public class Resources
{
    public Texture2D Act1,
        Confronting,
        RedCircle,
        Yourself,
        Aiback,
        Aifloor,
        Aigreen;

    public Music Inst,
        Voices;

    public Font Sonic,
        Sonicdebugfont,
        Sonichud;

    ~Resources()
    {
        Raylib.UnloadTexture(Act1);
        Raylib.UnloadTexture(Confronting);
        Raylib.UnloadTexture(RedCircle);
        Raylib.UnloadTexture(Yourself);
        Raylib.UnloadTexture(Aiback);
        Raylib.UnloadTexture(Aifloor);
        Raylib.UnloadTexture(Aigreen);

        Raylib.UnloadMusicStream(Inst);
        Raylib.UnloadMusicStream(Voices);

        Raylib.UnloadFont(Sonic);
        Raylib.UnloadFont(Sonicdebugfont);
        Raylib.UnloadFont(Sonichud);
    }
}

public class RunOnceAttribute : Attribute
{
    private static readonly Dictionary<string, bool> _hasRunCache = new Dictionary<string, bool>();

    public static void Execute(Action action, object instance, string methodName)
    {
        string key = $"{instance.GetHashCode()}_{methodName}";

        lock (_hasRunCache)
        {
            if (!_hasRunCache.ContainsKey(key))
            {
                _hasRunCache[key] = true;
                action.Invoke();
            }
        }
    }
}

public static class RunOnceExtensions
{
    public static void RunOnce(this object instance, Action action, [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
    {
        RunOnceAttribute.Execute(action, instance, methodName);
    }
}

public static class Extensions
{
    public static Rectangle GetRectangle(this Texture2D source)
        => new(0, 0, source.Width, source.Height);

    public static float GetBottom(this Rectangle source)
        => source.Y + source.Height;

    public static float GetRight(this Rectangle source)
        => source.X + source.Width;
}
