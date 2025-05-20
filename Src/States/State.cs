namespace States;

public abstract class State(Game game)
{
    public Game Game = game;
    public State? Parent = null;
    public List<State> Children = new();

    public void ExitState()
    {
        Game.States.Remove(this);
    }

    public void AddChild(State state)
    {
        state.Parent = this;
        Children.Add(state);
    }

    public virtual void Update(float dt)
    {
        for (int i = 0; i < Children.Count(); i++) {
            Children[i].Update(dt);
        }
    }

    public virtual void Input()
    {
        for (int i = 0; i < Children.Count(); i++) {
            Children[i].Input();
        }
    }

    public virtual void Draw()
    {
        for (int i = 0; i < Children.Count(); i++) {
            Children[i].Draw();
        }
    }
}
