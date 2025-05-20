using System.Drawing;
using System.Numerics;

namespace Math;

public struct Vec2(double x, double y) {
    public double X { get; set; } = x;
    public double Y { get; set; } = y;
    public double Length2 {
        get => X * X + Y * Y;
        set => this.Length = System.Math.Sqrt(value);
    }
    public double Length {
        get => System.Math.Sqrt(this.Length2);
        set
        {
            double angle = this.Angle;
            this.X = System.Math.Cos(angle) * value;
            this.Y = System.Math.Sin(angle) * value;
        }
    }
    public double Angle {
        get => System.Math.Atan2(Y, X);
        set {
            double length = this.Length;
            this.X = System.Math.Cos(value) * length;
            this.Y = System.Math.Sin(value) * length;
        }
    }

    public static Vec2 FromPolar(double length, double angle = 0)
        => new Vec2(
            length * System.Math.Cos(angle),
            length * System.Math.Sin(angle)
        );

    public double DistanceTo2(double vx, double vy) {
        vx -= this.X;
        vy -= this.Y;
        return (vx * vx + vy * vy);
    }

    public double DistanceTo2(Vec2 v) {
        double vx = v.X - this.X,
              vy = v.Y - this.Y;
        return (vx * vx + vy * vy);
    }

    public double DistanceTo(Vec2 v)             => System.Math.Sqrt(DistanceTo2(v));
    public double DistanceTo(double vx, double vy) => System.Math.Sqrt(DistanceTo2(vx, vy));

    public double AngleTo(Vec2 other) {
        var v = other - this;
        return v.Angle;
    }

    public void Normalize() {
        double length = this.Length;
        X /= length;
        Y /= length;
    }

    public Vec2 ToNormalize() {
        double length = this.Length;
        return new Vec2(
            X / length,
            Y / length
        );
    }

    public double Dot(double vx, double vy) => (X * vx) + (Y * vy);
    public double Dot(Vec2 v) => (v.X * X) + (v.Y * Y);
    public double Cross(double vx, double vy) => (vx * Y) - (vy * X);

    public static Vec2 operator +(Vec2 self) => new Vec2(self.X, self.Y);
    public static Vec2 operator -(Vec2 self) => new Vec2(-self.X, -self.Y);
    public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.X + b.X, a.Y + b.Y);
    public static Vec2 operator +(Vec2 a, double b) => new Vec2(a.X + b, a.Y + b);
    public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.X - b.X, a.Y - b.Y);
    public static Vec2 operator -(Vec2 a, double b) => new Vec2(a.X - b, a.Y - b);
    public static Vec2 operator /(Vec2 a, Vec2 b) => new Vec2(a.X / b.X, a.Y / b.Y);
    public static Vec2 operator /(Vec2 a, double divider) => new Vec2(a.X / divider, a.Y / divider);
    public static Vec2 operator *(Vec2 a, Vec2 b) => new Vec2(a.X * b.X, a.Y * b.Y);
    public static Vec2 operator *(double scaleFactor, Vec2 b) => new Vec2(scaleFactor * b.X, scaleFactor * b.Y);
    public static Vec2 operator *(Vec2 a, double scaleFactor) => new Vec2(a.X * scaleFactor, a.Y * scaleFactor);
    public static bool operator ==(Vec2 a, Vec2 b) => a.X == b.X && b.Y == b.Y;
    public static bool operator !=(Vec2 a, Vec2 b) => a.X != b.X && b.Y != b.Y;
    public static implicit operator Vector2(Vec2 self) => new Vector2((float)self.X, (float)self.Y);
    public static implicit operator Vec2(System.Numerics.Vector2 a) => new Vec2(a.X, a.Y);
    public static implicit operator PointF(Vec2 self) => new PointF((float)self.X, (float)self.Y);

    public static implicit operator double(Vec2 v)
    {
        throw new NotImplementedException();
    }

    public static Vec2 Lerp(Vec2 a, Vec2 b, double amount) => a * (1 - amount) + b * amount;
    public static Vec2 Zero { get => new Vec2(0, 0); }
    public static Vec2 One { get => new Vec2(1, 1); }
    public static Vec2 UnitX { get => new Vec2(1, 0); }
    public static Vec2 UnitY { get => new Vec2(0, 1); }

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object? obj) {
        double x, y;
        if (obj == null) return false;
        if (obj is Vec2) {
            x = ((Vec2)obj).X;
            y = ((Vec2)obj).Y;
        } else if (obj is System.Numerics.Vector2) {
            x = ((Vector2)obj).X;
            y = ((Vector2)obj).Y;
        } else {
            return false;
        }
        return (X == x) && (y == Y);
    }

    public override string ToString() => $"{{ X: {X}, Y: {Y} }}";
}
