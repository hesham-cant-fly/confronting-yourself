using Raylib_cs;
using Math;

namespace States;

public class IntroState : State
{
    private static readonly int TargetX = 350;
    private static readonly int TargetY = 100;
    public int BgAlpha = 255;
    public LinearTransform ConfrontingTimer = new(
        new(-2000, TargetY),
        new(TargetX, TargetY),
        500.0f
    );
    public LinearTransform RedcircleTimer = new(
        new(2000, TargetY),
        new(TargetX, TargetY),
        500.0f
    );
    public LinearTransform YourselfTimer = new(
        new(-2000, TargetY),
        new(TargetX, TargetY),
        500.0f
    );
    public LinearTransform Act1Timer = new(
        new(2000, TargetY),
        new(TargetX, TargetY),
        500.0f
    );

    public IntroState(Game game) : base(game)
    {
        double t = Raylib.GetTime() * 1000;
        ConfrontingTimer.Begin(t);
        RedcircleTimer.Begin(t);
    }

    public void StartGamePlay() {
        this.RunOnce(() => {
            Game.PrependState(new GamePlay(Game));
        });
    }

    public void PrepareFinishing() {
        this.RunOnce(() => {
            double t = Raylib.GetTime() * 1000;
            ConfrontingTimer = new(
                new(TargetX, TargetY),
                new(-2000, TargetY),
                500.0
            );
            RedcircleTimer = new(
                new(TargetX, TargetY),
                new(2000, TargetY),
                500.0
            );
            YourselfTimer = new(
                new(TargetX, TargetY),
                new(-2000, TargetY),
                500.0
            );
            Act1Timer = new(
                new(TargetX, TargetY),
                new(2000, TargetY),
                500.0
            );
            ConfrontingTimer.Begin(t);
            RedcircleTimer.Begin(t);
            YourselfTimer.Begin(t);
            Act1Timer.Begin(t);
        });
    }

    public override void Update(float dt)
    {
        double t = Raylib.GetTime() * 1000;
        if (t >= 400.0f && !YourselfTimer.IsStarted) {
            YourselfTimer.Begin(t);
        }
        if (t >= 700.0f && !Act1Timer.IsStarted) {
            Act1Timer.Begin(t);
        }
        if (t >= 3500.0) {
            PrepareFinishing();
            if (ConfrontingTimer.IsCompleted)
                ExitState();
        }
        if (t >= 2000.0f) {
            StartGamePlay();
            BgAlpha -= (int)(255 * dt * 2);
            if (BgAlpha < 0) {
                BgAlpha = 0;
            }
        }
        ConfrontingTimer.Update(t);
        RedcircleTimer.Update(t);
        YourselfTimer.Update(t);
        Act1Timer.Update(t);
    }

    public override void Draw()
    {
        Raylib.DrawRectangle(
            0, 0,
            Raylib.GetScreenWidth(),
            Raylib.GetScreenHeight(),
            new(0, 0, 0, BgAlpha)
        );
        Raylib.DrawTextureEx(
            Program.rc.RedCircle,
            RedcircleTimer.Current,
            0.0f,
            4.0f,
            Color.White
        );
        Raylib.DrawTextureEx(
            Program.rc.Confronting,
            ConfrontingTimer.Current,
            0.0f,
            4.0f,
            Color.White
        );
        Raylib.DrawTextureEx(
            Program.rc.Yourself,
            YourselfTimer.Current,
            0.0f,
            4.0f,
            Color.White
        );
        Raylib.DrawTextureEx(
            Program.rc.Act1,
            Act1Timer.Current,
            0.0f,
            4.0f,
            Color.White
        );
    }
}
