using System;
using SystemVector3 = System.Numerics.Vector3;
using Godot;


namespace GalaxyLib
{
    static class StarExtensions
    {
        public static Star Offset(this Star s, Vector3 offset)
        {
            s.Position += offset;
            return s;
        }

        public static Star Scale(this Star s, Vector3 scale)
        {
            s.Position *= scale;
            return s;
        }

        public static Star Swirl(this Star s, Vector3 axis, float amount)
        {
            float d = s.Position.Length();
            float a = (float) Math.Pow(d, 0.1f) * amount;

            Vector3 newPos = s.Position.Rotated(axis, a);
            s.Position = newPos;
            return s;
        }
    }
}
