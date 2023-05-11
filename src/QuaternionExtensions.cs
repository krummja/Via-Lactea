using Godot;
using System;
using System.Collections.Generic;


static class QuaternionExtensions
{
    public static Tuple<Vector3, float> ToAxisAngle(this Quaternion q)
    {
        if (q.W > 1) q = q.Normalized();

        float angle = 2 * Mathf.Acos(q.W);
        float x;
        float y;
        float z;

        double s = Math.Sqrt(1 - q.W * q.W);

        if (s < 0.001) {
            x = q.X;
            y = q.Y;
            z = q.Z;
        } else {
            x = (float) (q.X / s);
            y = (float) (q.Y / s);
            z = (float) (q.Z / s);
        }

        return new Tuple<Vector3, float>(new Vector3(x, y, z), angle);
    }
}
