using System.Xml;
using Raylib_cs;
using Math;

namespace States;

public class Arrows : State
{
    private readonly Texture2D Texture;
    private const float Padding = 10;
    public Vec2 Pos = Vec2.Zero;
    public Vec2 Scale = Vec2.One;

    public Rectangle Left { get; private set; }
    public Rectangle Down { get; private set; }
    public Rectangle Up { get; private set; }
    public Rectangle Right { get; private set; }

    public Rectangle LeftActive { get; private set; }
    public Rectangle DownActive { get; private set;}
    public Rectangle UpActive { get; private set;}
    public Rectangle RightActive { get; private set;}

    public Rectangle LeftPress { get; private set;}
    public Rectangle DownPress { get; private set;}
    public Rectangle UpPress { get; private set;}
    public Rectangle RightPress { get; private set;}

    public Rectangle LeftHold { get; private set;}
    public Rectangle DownHold { get; private set;}
    public Rectangle UpHold { get; private set;}
    public Rectangle RightHold { get; private set;}

    public Rectangle LeftHoldTail { get; private set;}
    public Rectangle DownHoldTail { get; private set;}
    public Rectangle UpHoldTail { get; private set;}
    public Rectangle RightHoldTail { get; private set;}

    public double NoteHeight
    {
        get => Left.Height * Scale.Y;
    }

    public double NoteWidth
    {
        get => Left.Width * Scale.Y;
    }

    public Arrows(Game game, Texture2D texture) :  base(game)
    {
        this.Texture = texture;
    }

    ~Arrows()
    {
        Raylib.UnloadTexture(Texture);
    }

    private void CheckFromScores()
    {
        if (Parent == null) throw new Exception("");
        GamePlay parent = (GamePlay)Parent;
        var chartBoard = parent.ChartBoard;
        var notes = chartBoard.GetIntersects(
            (float)NoteHeight,
            (float)(-Raylib.GetScreenHeight() + Pos.Y),
            (float)(-Raylib.GetScreenHeight() + Pos.Y + NoteHeight)
        );
        int i = 0;
        float noteValues = 0;
        foreach (var (_, note) in notes)
        {
            if (parent.IsPressed(NoteKind.Left) && ((note.Left == 0.5 || note.Left == 0.25) || (note.Left != 0 && !parent.IsActive(NoteKind.Left))))
            {
                noteValues += note.Left;
                note.Left = 0;
                parent.Activate(NoteKind.Left);
            }
            if (parent.IsPressed(NoteKind.Down) && ((note.Down == 0.5 || note.Down == 0.25) || (note.Down != 0 && !parent.IsActive(NoteKind.Down))))
            {
                noteValues += note.Down;
                note.Down = 0;
                parent.Activate(NoteKind.Down);
            }
            if (parent.IsPressed(NoteKind.Up) && ((note.Up == 0.5 || note.Up == 0.25) || (note.Up != 0 && !parent.IsActive(NoteKind.Up))))
            {
                noteValues += note.Up;
                note.Up = 0;
                parent.Activate(NoteKind.Up);
            }
            if (parent.IsPressed(NoteKind.Right) && ((note.Right == 0.5 || note.Right == 0.25) || (note.Right != 0 && !parent.IsActive(NoteKind.Right))))
            {
                noteValues += note.Right;
                note.Right = 0;
                parent.Activate(NoteKind.Right);
            }
            i += 1;
        }
        parent.Score += (int)(noteValues * 100);
    }

    public void CheckForMisses()
    {
        if (Parent == null) throw new Exception("");
        GamePlay parent = (GamePlay)Parent;
        var chartBoard = parent.ChartBoard;
        var notes = chartBoard.GetIntersects(
            (float)NoteHeight,
            (float)(-Raylib.GetScreenHeight() - 100),
            (float)(-Raylib.GetScreenHeight())
        );
        int i = 0;
        foreach (var note in notes)
        {
            var i2 = note.Item2;
            if (i2.Left == 1)
            {
                i2.Left = 0;
                i++;
            }
            if (i2.Up == 1)
            {
                i2.Up = 0;
                i++;
            }
            if (i2.Down == 1)
            {
                i2.Down = 0;
                i++;
            }
            if (i2.Right == 1)
            {
                i2.Right = 0;
                i++;
            }
        }
        parent.Misses += i;
    }

    private void SetupCurrentY()
    {
    }

    public override void Update(float dt)
    {
        if (Parent == null) throw new Exception("");
        GamePlay parent = (GamePlay)Parent;
        var chartBoard = parent.ChartBoard;
        this.RunOnce(() => {
            chartBoard.CurrentY += (float)((chartBoard.BPM / 60) * chartBoard.Speed * Raylib.GetTime() * NoteHeight);
        });
        chartBoard.CurrentY += (float)((chartBoard.BPM / 60) * chartBoard.Speed * Raylib.GetFrameTime() * NoteHeight);
        CheckFromScores();
        CheckForMisses();
    }

    public override void Draw()
    {
        if (Parent == null) throw new Exception("");
        GamePlay parent = (GamePlay)Parent;
        var dist = new Rectangle((float)Pos.X, (float)Pos.Y, 0, 0);
        if (parent.IsActive(NoteKind.Left)) {
            dist.Width = (float)(LeftActive.Width * Scale.X);
            dist.Height = (float)(LeftActive.Height * Scale.Y);
            Raylib.DrawTexturePro(
                Texture,
                LeftActive,
                dist,
                Vec2.Zero,
                0, Color.White
            );
        } else if (parent.IsPressed(NoteKind.Left)) {
            dist.Width = (float)(LeftPress.Width * Scale.X);
            dist.Height = (float)(LeftPress.Height * Scale.Y);
            Raylib.DrawTexturePro(
                Texture,
                LeftPress,
                dist,
                Vec2.Zero,
                0, Color.White
            );
        } else {
            dist.Width = (float)(Left.Width * Scale.X);
            dist.Height = (float)(Left.Height * Scale.Y);
            Raylib.DrawTexturePro(
                Texture,
                Left,
                dist,
                Vec2.Zero,
                0, Color.White
            );
        }
        dist.X += Padding;
        if (parent.IsActive(NoteKind.Down)) {
            dist.Width = (float)(DownActive.Width * Scale.X);
            dist.Height = (float)(DownActive.Height * Scale.Y);
            dist.X += (float)(DownActive.Width * Scale.X);
            Raylib.DrawTexturePro(
                Texture,
                DownActive,
                dist,
                Vec2.Zero,
                0, Color.White
            );
        } else if (parent.IsPressed(NoteKind.Down)) {
            dist.Width = (float)(DownPress.Width * Scale.X);
            dist.Height = (float)(DownPress.Height * Scale.Y);
            dist.X += (float)(Right.Width * Scale.X);
            Raylib.DrawTexturePro(
                Texture,
                DownPress,
                dist,
                Vec2.Zero,
                0, Color.White
            );
        } else {
            dist.Width = (float)(Down.Width * Scale.X);
            dist.Height = (float)(Down.Height * Scale.Y);
            dist.X += (float)(Right.Width * Scale.X);
            Raylib.DrawTexturePro(
                Texture,
                Down,
                dist,
                Vec2.Zero,
                0, Color.White
            );
        }
        dist.X += Padding;
        if (parent.IsActive(NoteKind.Up)) {
            dist.Width = (float)(UpActive.Width * Scale.X);
            dist.Height = (float)(UpActive.Height * Scale.Y);
            dist.X += (float)(UpActive.Width * Scale.X);
            Raylib.DrawTexturePro(
                Texture,
                UpActive,
                dist,
                Vec2.Zero,
                0, Color.White
            );
        } else if (parent.IsPressed(NoteKind.Up)) {
            dist.Width = (float)(UpPress.Width * Scale.X);
            dist.Height = (float)(UpPress.Height * Scale.Y);
            dist.X += (float)(Right.Width * Scale.X);
            Raylib.DrawTexturePro(
                Texture,
                UpPress,
                dist,
                Vec2.Zero,
                0, Color.White
            );
        } else {
            dist.Width = (float)(Up.Width * Scale.X);
            dist.Height = (float)(Up.Height * Scale.Y);
            dist.X += (float)(Right.Width * Scale.X);
            Raylib.DrawTexturePro(
                Texture,
                Up,
                dist,
                Vec2.Zero,
                0, Color.White
            );
        }
        dist.X += Padding;
        if (parent.IsActive(NoteKind.Right)) {
            dist.Width = (float)(RightActive.Width * Scale.X);
            dist.Height = (float)(RightActive.Height * Scale.Y);
            dist.X += (float)(RightActive.Width * Scale.X);
            Raylib.DrawTexturePro(
                Texture,
                RightActive,
                dist,
                Vec2.Zero,
                0, Color.White
            );
        } else if (parent.CurrentPressed.Contains(NoteKind.Right)) {
            dist.Width = (float)(RightPress.Width * Scale.X);
            dist.Height = (float)(RightPress.Height * Scale.Y);
            dist.X += (float)(Right.Width * Scale.X);
            Raylib.DrawTexturePro(
                Texture,
                RightPress,
                dist,
                Vec2.Zero,
                0, Color.White
            );
        } else {
            dist.Width = (float)(Right.Width * Scale.X);
            dist.Height = (float)(Right.Height * Scale.Y);
            dist.X += (float)(Right.Width * Scale.X);
            Raylib.DrawTexturePro(
                Texture,
                Right,
                dist,
                Vec2.Zero,
                0, Color.White
            );
        }
        DrawCharts();
    }

    private void DrawCharts()
    {
        if (Parent == null) throw new Exception("");
        GamePlay parent = (GamePlay)Parent;
        var chartBoard = parent.ChartBoard;
        var range = chartBoard.GetCurrentRange((float)NoteHeight, -Raylib.GetScreenHeight() - 200, 200);
        List<(ChartBoard.Note, ChartBoard.Note)>? notes;
        if (range.Item1 == -1 && range.Item2 == -1) {
            return;
        } else if (range.Item1 == -1) {
            notes = chartBoard.Notes[(chartBoard.Notes.Count() - range.Item2)..range.Item2];
        } else if (range.Item2 == -1) {
            notes = chartBoard.Notes[range.Item1..];
        } else {
            notes = chartBoard.Notes[range.Item1..range.Item2];
        }

        var dist = new Rectangle((float)Pos.X, (float)Pos.Y , (float)NoteWidth, (float)NoteHeight);
        int i = range.Item2;
        notes.Reverse();
        foreach (var (_, note) in notes)
        {
            dist.X = (float)Pos.X;
            dist.Y = (float)(Raylib.GetScreenHeight() - chartBoard.CurrentY + NoteHeight * i);
            if (note.Left == 0.5)
            {
                var oldDist = dist;
                dist.Width = LeftHold.Width / 2;
                dist.Y -= dist.Height / 2;
                Raylib.DrawTexturePro(
                    Texture,
                    LeftHold,
                    dist,
                    new(-dist.Width, 0),
                    0, Color.White
                );
                dist.Y += dist.Height / 2;
                Raylib.DrawTexturePro(
                    Texture,
                    LeftHold,
                    dist,
                    new(-dist.Width, 0),
                    0, Color.White
                );
                dist = oldDist;
            }
            if (note.Left == 0.25)
            {
                var oldDist = dist;
                dist.Width = LeftHoldTail.Width / 2;
                dist.Y -= 15;
                Raylib.DrawTexturePro(
                    Texture,
                    LeftHoldTail,
                    dist,
                    new(-dist.Width, 0),
                    0, Color.White
                );
                dist = oldDist;
            }
            if (note.Left == 1)
            {
                Raylib.DrawTexturePro(
                    Texture,
                    LeftActive,
                    dist,
                    Vec2.Zero,
                    0, Color.White
                );
            }
            dist.X += Padding;
            dist.X += (float)NoteWidth;
            if (note.Down == 0.5)
            {
                var oldDist = dist;
                dist.Width = DownHold.Width / 2;
                dist.Y -= dist.Height / 2;
                Raylib.DrawTexturePro(
                    Texture,
                    DownHold,
                    dist,
                    new(-dist.Width, 0),
                    0, Color.White
                );
                dist.Y += dist.Height / 2;
                Raylib.DrawTexturePro(
                    Texture,
                    DownHold,
                    dist,
                    new(-dist.Width, 0),
                    0, Color.White
                );
                dist = oldDist;
            }
            if (note.Down == 0.25)
            {
                var oldDist = dist;
                dist.Width = DownHoldTail.Width / 2;
                dist.Y -= 15;
                Raylib.DrawTexturePro(
                    Texture,
                    DownHoldTail,
                    dist,
                    new(-dist.Width, 0),
                    0, Color.White
                );
                dist = oldDist;
            }
            if (note.Down == 1)
            {
                Raylib.DrawTexturePro(
                    Texture,
                    DownActive,
                    dist,
                    Vec2.Zero,
                    0, Color.White
                );
            }
            dist.X += Padding;
            dist.X += (float)NoteWidth;
            if (note.Up == 0.5)
            {
                var oldDist = dist;
                dist.Width = UpHold.Width / 2;
                dist.Y -= dist.Height / 2;
                Raylib.DrawTexturePro(
                    Texture,
                    UpHold,
                    dist,
                    new(-dist.Width, 0),
                    0, Color.White
                );
                dist.Y += dist.Height / 2;
                Raylib.DrawTexturePro(
                    Texture,
                    UpHold,
                    dist,
                    new(-dist.Width, 0),
                    0, Color.White
                );
                dist = oldDist;
            }
            if (note.Up == 0.25)
            {
                var oldDist = dist;
                dist.Width = UpHoldTail.Width / 2;
                dist.Y -= 15;
                Raylib.DrawTexturePro(
                    Texture,
                    UpHoldTail,
                    dist,
                    new(-dist.Width, 0),
                    0, Color.White
                );
                dist = oldDist;
            }
            if (note.Up == 1)
            {
                Raylib.DrawTexturePro(
                    Texture,
                    UpActive,
                    dist,
                    Vec2.Zero,
                    0, Color.White
                );
            }
            dist.X += Padding;
            dist.X += (float)NoteWidth;
            if (note.Right == 0.5)
            {
                var oldDist = dist;
                dist.Width = RightHold.Width / 2;
                dist.Y -= dist.Height / 2;
                Raylib.DrawTexturePro(
                    Texture,
                    RightHold,
                    dist,
                    new(-dist.Width, 0),
                    0, Color.White
                );
                dist.Y += dist.Height / 2;
                Raylib.DrawTexturePro(
                    Texture,
                    RightHold,
                    dist,
                    new(-dist.Width, 0),
                    0, Color.White
                );
                dist = oldDist;
            }
            if (note.Right == 0.25)
            {
                var oldDist = dist;
                dist.Width = RightHoldTail.Width / 2;
                dist.Y -= 15;
                Raylib.DrawTexturePro(
                    Texture,
                    RightHoldTail,
                    dist,
                    new(-dist.Width, 0),
                    0, Color.White
                );
                dist = oldDist;
            }
            if (note.Right == 1)
            {
                Raylib.DrawTexturePro(
                    Texture,
                    RightActive,
                    dist,
                    Vec2.Zero,
                    0, Color.White
                );
            }
            i -= 1;
        }
    }

    public static Arrows LoadXML(string path, Game game)
    {
        Arrows arrows;
        var root_path = Path.GetDirectoryName(path);
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        var atlas = xmlDoc.SelectSingleNode("TextureAtlas");
        if (atlas == null) throw new Exception("");

        var image_path = Path.Join(root_path, atlas.Attributes?["imagePath"]?.InnerText);
        arrows = new(game, Raylib.LoadTexture(image_path));

        arrows.Left = FromXMLNode(atlas, "Left");
        arrows.Down = FromXMLNode(atlas, "Down");
        arrows.Up = FromXMLNode(atlas, "Up");
        arrows.Right = FromXMLNode(atlas, "Right");

        arrows.LeftActive = FromXMLNode(atlas, "LeftActive");
        arrows.DownActive = FromXMLNode(atlas, "DownActive");
        arrows.UpActive = FromXMLNode(atlas, "UpActive");
        arrows.RightActive = FromXMLNode(atlas, "RightActive");

        arrows.LeftPress = FromXMLNode(atlas, "LeftPress");
        arrows.DownPress = FromXMLNode(atlas, "DownPress");
        arrows.UpPress = FromXMLNode(atlas, "UpPress");
        arrows.RightPress = FromXMLNode(atlas, "RightPress");

        arrows.LeftHold = FromXMLNode(atlas, "LeftHold");
        arrows.DownHold = FromXMLNode(atlas, "DownHold");
        arrows.UpHold = FromXMLNode(atlas, "UpHold");
        arrows.RightHold = FromXMLNode(atlas, "RightHold");

        arrows.LeftHoldTail = FromXMLNode(atlas, "LeftHoldTail");
        arrows.DownHoldTail = FromXMLNode(atlas, "DownHoldTail");
        arrows.UpHoldTail = FromXMLNode(atlas, "UpHoldTail");
        arrows.RightHoldTail = FromXMLNode(atlas, "RightHoldTail");

        return arrows;
    }

    private static Rectangle FromXMLNode(XmlNode? target_node, string key)
    {
        var node = target_node!.SelectSingleNode(key);
        if (node == null) throw new Exception("");
        if (node.Attributes == null) throw new Exception("");

        float x = float.Parse(node.Attributes["x"]!.InnerText);
        float y = float.Parse(node.Attributes["y"]!.InnerText);
        float width = float.Parse(node.Attributes["width"]!.InnerText);
        float height = float.Parse(node.Attributes["height"]!.InnerText);
        return new(x, y, width, height);
    }
}
