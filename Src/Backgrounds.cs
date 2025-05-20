using Raylib_cs;
using Math;

public sealed class Background(Texture2D texture)
{
    public readonly Texture2D Texture = texture;
    private (Vec2, Vec2) Positions = (Vec2.Zero, Vec2.Zero);
    public Vec2 Speed = Vec2.Zero;
    public Rectangle DistRect = texture.GetRectangle();
    public (Rectangle, Rectangle) Rect {
        get => (new Rectangle((float)Positions.Item1.X, (float)Positions.Item1.Y, DistRect.Width, DistRect.Height), new Rectangle((float)Positions.Item2.X, (float)Positions.Item2.Y, DistRect.Width, DistRect.Height));
    }

    public Background(Texture2D texture, Vec2 pos) : this(texture)
    {
        this.Positions = (pos, new(pos.X + DistRect.Width, pos.Y));
    }

    public Background(Texture2D texture, Vec2 pos, Vec2 speed) : this(texture, pos)
    {
        this.Speed = speed;
    }

    public Background(Texture2D texture, Vec2 pos, Vec2 speed, Rectangle distRect) : this(texture, pos, speed)
    {
        DistRect = distRect;
        this.Positions.Item2.X = pos.X + DistRect.Width;
    }

    public void Update()
    {
        float dt = Raylib.GetFrameTime();

        if (Positions.Item1.X + DistRect.Width < 0)
        {
            Positions.Item1.X = Positions.Item2.X + DistRect.Width;
        }
        else if (Positions.Item2.X + DistRect.Width < 0)
        {
            Positions.Item2.X = Positions.Item1.X + DistRect.Width;
        }

        Positions.Item1 -= Speed * dt;
        Positions.Item2 -= Speed * dt;
    }

    public void Draw()
    {
        Raylib.DrawTexturePro(
            Texture,
            Texture.GetRectangle(),
            Rect.Item1,
            Vec2.Zero,
            0f,
            Color.White
        );
        Raylib.DrawTexturePro(
            Texture,
            Texture.GetRectangle(),
            Rect.Item2,
            Vec2.Zero,
            0f,
            Color.White
        );
    }
}

public class BackgroundManager
{
    public List<Background> Bgs { get; private set; } = new();

    public BackgroundManager(List<Background> bgs)
    {
        this.Bgs = bgs;
    }

    public void Update()
    {
        foreach (var bg in Bgs)
        {
            bg.Update();
        }
    }

    public void Draw()
    {
        foreach (var bg in Bgs)
        {
            bg.Draw();
        }
    }
}
