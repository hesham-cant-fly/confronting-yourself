using Raylib_cs;
using States;

public class Game
{
    public List<State> States = new();

    public Game()
    {
        AppendState(new IntroState(this));
        Raylib.PlayMusicStream(Program.rc.Inst);
        Raylib.PlayMusicStream(Program.rc.Voices);
        Raylib.SetMusicVolume(Program.rc.Inst, 0);
    }

    public void AppendState(State state)
    {
        state.Game = this;
        States.Add(state);
    }

    public void PrependState(State state)
    {
        state.Game = this;
        States.Insert(0, state);
    }

    public void Update(float dt = 0)
    {
        for (int i = 0; i < States.Count(); i++)
        {
            States[i].Update(dt);
        }
    }

    public void Input()
    {
        for (int i = 0; i < States.Count(); i++)
        {
            States[i].Input();
        }
    }

    public void Draw()
    {
        for (int i = 0; i < States.Count(); i++)
        {
            States[i].Draw();
        }
    }
}
