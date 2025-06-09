using Raylib_cs;

namespace States;

class IntroSection : State
{
    public BackgroundManager BM;

    public IntroSection(Game game) : base(game)
    {
        GamePlay.Sonic.PlayAnimation(AnimationKind.Idle);
        GamePlay.Sonic.Pos.X = -1000;
        GamePlay.SonicLegs.PlayAnimation(AnimationKind.Idle);

        const int SCALE = 5;
        var bgs = new List<Background>();
        {
            var bg1 = new Background(
                Program.rc.Aiback,
                new(0, -250),
                new(300, 0),
                new(0, 0, Program.rc.Aiback.Width * SCALE, Program.rc.Aiback.Height * SCALE)
            );
            var bg2 = new Background(
                Program.rc.Aigreen,
                new(0, bg1.Rect.Item1.GetBottom()),
                new(1500, 0),
                new(0, 0, Program.rc.Aigreen.Width * SCALE, Program.rc.Aigreen.Height * SCALE)
            );
            var bg3 = new Background(
                Program.rc.Aifloor,
                new(0, -200),
                new(4000, 0),
                new(0, 0, Program.rc.Aifloor.Width * SCALE, Program.rc.Aifloor.Height * SCALE)
            );

            bgs.AddRange([bg1, bg2, bg3]);
        }
        this.BM = new(bgs);
    }

    public override void Update(float dt)
    {
        BM.Update();
        GamePlay.Sonic.Update();
        var targetX = Raylib.GetScreenWidth() / 2 - GamePlay.Sonic.CurrentRect.Width / 2;
        if (GamePlay.Sonic.Pos.X < targetX) {
            GamePlay.Sonic.Pos.X += 2000 * Raylib.GetFrameTime();
        } else {
            GamePlay.Sonic.Pos.X = Raylib.GetScreenWidth() / 2 - GamePlay.Sonic.CurrentRect.Width / 2;
        }
        GamePlay.Sonic.Pos.Y = Raylib.GetScreenHeight() / 2 - GamePlay.Sonic.CurrentRect.Height / 2 + 20;
        if (GamePlay.Sonic.CurrentAnimation != AnimationKind.Idle)
        {
            GamePlay.SonicLegs.Pos = GamePlay.Sonic.Pos;
            GamePlay.SonicLegs.Bottom = GamePlay.Sonic.Bottom;
            GamePlay.SonicLegs.Update();
        }
        if (Parent == null) throw new Exception("");
        var parent = (GamePlay)Parent;
        if (parent.IsActive(NoteKind.Left))
            GamePlay.Sonic.PlayAnimation(AnimationKind.SingLeft);
        if (parent.IsActive(NoteKind.Down))
            GamePlay.Sonic.PlayAnimation(AnimationKind.SingDown);
        if (parent.IsActive(NoteKind.Up))
            GamePlay.Sonic.PlayAnimation(AnimationKind.SingUp);
        if (parent.IsActive(NoteKind.Right))
            GamePlay.Sonic.PlayAnimation(AnimationKind.SingRight);
        if (parent.CurrentActive.Count() == 0)
            GamePlay.Sonic.PlayAnimation(AnimationKind.Idle);
    }

    public override void Draw()
    {
        BM.Draw();
        if (GamePlay.Sonic.CurrentAnimation != AnimationKind.Idle)
            GamePlay.SonicLegs.Draw();
        GamePlay.Sonic.Draw();
    }
}
