using Raylib_cs;
using Math;

namespace States;

public class GamePlay : State
{
    public int Score = 0;
    public int Misses = 0;
    public static Character Sonic = Character.LoadFromJSON("./Assets/characters/sonic/states/sonic.json");
    public static Character SonicLegs = Character.LoadFromJSON("./Assets/characters/sonic/states/soniclegs.json");
    public List<NoteKind> CurrentPressed = new();

    public GamePlay(Game game) : base(game)
    {
        AddChild(new IntroSection(game));
        AddChild(new UiState(game));

        Sonic.FlipX = true;
        Sonic.Scale *= 4;
        SonicLegs.Scale *= 4;
        SonicLegs.FlipX = true;
    }

    public override void Update(float dt)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Left)) {
            CurrentPressed.Add(NoteKind.Left);
        } else if (Raylib.IsKeyReleased(KeyboardKey.Left)) {
            CurrentPressed.Remove(NoteKind.Left);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Down)) {
            CurrentPressed.Add(NoteKind.Down);
        } else if (Raylib.IsKeyReleased(KeyboardKey.Down)) {
            CurrentPressed.Remove(NoteKind.Down);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Up)) {
            CurrentPressed.Add(NoteKind.Up);
        } else if (Raylib.IsKeyReleased(KeyboardKey.Up)) {
            CurrentPressed.Remove(NoteKind.Up);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Right)) {
            CurrentPressed.Add(NoteKind.Right);
        } else if (Raylib.IsKeyReleased(KeyboardKey.Right)) {
            CurrentPressed.Remove(NoteKind.Right);
        }
        base.Update(dt);
    }

    public override void Draw()
    {
        base.Draw();
    }
}

public class UiState : State
{
    public UiState(Game game) : base(game)
    {}

    public void PrintYellowText(string text, Vec2 pos)
    {
        Raylib.DrawTextEx(
            Program.rc.Sonic,
            text,
            pos + 3,
            70, 0, Color.Black
        );
        Raylib.DrawTextEx(
            Program.rc.Sonic,
            text,
            pos,
            70, 0, Color.Yellow
        );
    }

    public void PrintWhiteText(string text, Vec2 pos)
    {
        Raylib.DrawTextEx(
            Program.rc.Sonic,
            text,
            pos + 3,
            70, 0, Color.Black
        );
        Raylib.DrawTextEx(
            Program.rc.Sonic,
            text,
            pos,
            70, 0, Color.White
        );
    }

    public override void Draw()
    {
        if (Parent == null) throw new Exception("bad");
        PrintYellowText("SCORE", new(120.0, 50.0));
        PrintWhiteText(string.Format("{0,8:d}", ((GamePlay)Parent).Score), new(300, 50));
        PrintYellowText("TIME", new(120.0, 100.0));
        PrintWhiteText(
            string.Format("{0}", FormatTime(Raylib.GetMusicTimePlayed(Program.rc.Inst))),
            new(300, 100)
        );
        PrintYellowText("MISSES", new(120.0, 150.0));
        PrintWhiteText(string.Format("{0,4:d}", ((GamePlay)Parent).Misses), new(300, 150));
    }

    private static string FormatTime(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return $"{(int)time.TotalMinutes}:{time.Seconds:00}";
    }
}
