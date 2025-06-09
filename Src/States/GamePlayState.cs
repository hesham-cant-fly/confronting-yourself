using Raylib_cs;
using Math;

namespace States;

public class GamePlay : State
{
    public int Score = 0;
    public int Misses = 0;
    public List<NoteKind> CurrentPressed = new();
    public List<NoteKind> CurrentActive = new();
    public ChartBoard ChartBoard = new();
    public static Character Sonic = Character.LoadFromJSON("./Assets/characters/sonic/states/sonic.json");
    public static Character SonicLegs = Character.LoadFromJSON("./Assets/characters/sonic/states/soniclegs.json");

    public GamePlay(Game game) : base(game)
    {
        LoadCharts();
        AddChild(new IntroSection(game));
        AddChild(new UiState(game));
        var arrows = Arrows.LoadXML("./Assets/sonicNotes.xml", game);
        arrows.Scale *= 0.65;
        arrows.Pos = new(Raylib.GetScreenWidth() - 500, 60);
        AddChild(arrows);

        Sonic.FlipX = true;
        Sonic.Scale *= 4;
        SonicLegs.Scale *= 4;
        SonicLegs.FlipX = true;
    }

    private void LoadCharts()
    {
        this.ChartBoard = ChartBoard.LoadFromJSON("./Assets/data/charts.json");
    }

    // private void LoadCharts()
    // {
    //     // this.ChartBoard.Speed = 100;
    //     this.ChartBoard.BPM = (int)(240 * 2);
    //     var rnd = new Random();
    //     // ChartBoard.Note? lastNote = null;
    //     var i = 0;
    //     while (i < 1000) {
    //         // var noteValue = rnd.Next(0, 2);
    //         var noteI = rnd.Next(0, 4);
    //         var note = new ChartBoard.Note(
    //             // noteI == 0 ? noteValue : 0,
    //             // noteI == 1 ? noteValue : 0,
    //             // noteI == 2 ? noteValue : 0,
    //             // noteI == 3 ? noteValue : 0
    //             noteI == 0 ? 1 : 0,
    //             noteI == 1 ? 1 : 0,
    //             noteI == 2 ? 1 : 0,
    //             noteI == 3 ? 1 : 0
    //         );
    //         ChartBoard.Notes.Add((new(), note));
    //         i++;
    //     }
    // }

    public void Activate(NoteKind kind)
    {
        if (!CurrentActive.Contains(kind))
            CurrentActive.Add(kind);
    }

    public void Deactivate(NoteKind kind)
    {
        CurrentActive.Remove(kind);
    }

    public bool IsPressed(NoteKind kind)
        => CurrentPressed.Contains(kind);
    public bool IsActive(NoteKind kind)
        => CurrentActive.Contains(kind);

    public override void Update(float dt)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Left) || Raylib.IsKeyPressed(KeyboardKey.A)) {
            CurrentPressed.Add(NoteKind.Left);
        } else if (Raylib.IsKeyReleased(KeyboardKey.Left) || Raylib.IsKeyReleased(KeyboardKey.A)) {
            CurrentPressed.Remove(NoteKind.Left);
            Deactivate(NoteKind.Left);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Down) || Raylib.IsKeyPressed(KeyboardKey.S)) {
            CurrentPressed.Add(NoteKind.Down);
        } else if (Raylib.IsKeyReleased(KeyboardKey.Down) || Raylib.IsKeyReleased(KeyboardKey.S)) {
            CurrentPressed.Remove(NoteKind.Down);
            Deactivate(NoteKind.Down);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Up) || Raylib.IsKeyPressed(KeyboardKey.L)) {
            CurrentPressed.Add(NoteKind.Up);
        } else if (Raylib.IsKeyReleased(KeyboardKey.Up) || Raylib.IsKeyReleased(KeyboardKey.L)) {
            CurrentPressed.Remove(NoteKind.Up);
            Deactivate(NoteKind.Up);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Right) || Raylib.IsKeyPressed(KeyboardKey.Semicolon)) {
            CurrentPressed.Add(NoteKind.Right);
        } else if (Raylib.IsKeyReleased(KeyboardKey.Right) || Raylib.IsKeyReleased(KeyboardKey.Semicolon)) {
            CurrentPressed.Remove(NoteKind.Right);
            Deactivate(NoteKind.Right);
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
        return $"{(int)time.TotalMinutes}:{time.Seconds:00}.{time.Milliseconds:000}";
    }
}
