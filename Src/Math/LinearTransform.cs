
namespace Math;

public class LinearTransform(Vec2 start, Vec2 end, double duration)
{
    public Vec2 Start = start;
    public Vec2 End = end;
    public Vec2 Current = start;
    public double Duration = duration;
    public double StartTime = 0;
    public bool IsCompleted = false;
    public bool IsStarted = false;

    public void Begin(double timer)
    {
        StartTime = timer;
        IsCompleted = false;
        IsStarted = true;
        Current = Start;
    }

    public Tuple<Vec2, double> Update(double timer)
    {
        if (!IsStarted)
            return new(Start, 0.0);
        if (IsCompleted)
            return new(End, 1.0);

        var elapsed = timer - StartTime;
        var t = System.Math.Min(elapsed / Duration, 1.0);

        // Current = Start + (End - Start) * t;
        Current = new(
            Start.X + (End.X - Start.X) * t,
            Start.Y + (End.Y - Start.Y) * t
        );
        if (t >= 1.0)
            IsCompleted = true;
        return new(Current, t);
    }
}
