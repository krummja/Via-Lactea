using Godot;
using Godot.Collections;
using OctreeLib;
using System;


public partial class OctreeRoot : Node3D
{
    [Export]
    public float Width;

    [Export]
    public float Height;

    [Export]
    public float Depth;

    [Export]
    public int CubeCapacity;

    [Export]
    public bool Debug = false;

    public Octree Octree;

    public override void _Ready()
    {
        Cube boundary = new Cube(this.Position, Width, Height, Depth);
        Octree = new Octree(boundary, CubeCapacity);
        Octree.Parent = this;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey) {
            if (eventKey.Keycode == Key.Space) {
                Cube b = this.Octree.Boundary;

                Random rand = new Random();

                Vector3 p = new Vector3(
                    (float) rand.Next(
                        (int) (b.Center.X - b.Dimensions.X),
                        (int) (b.Center.X + b.Dimensions.X)
                    ),
                    (float) rand.Next(
                        (int) (b.Center.Y - b.Dimensions.Y),
                        (int) (b.Center.Y + b.Dimensions.Y)
                    ),
                    (float) rand.Next(
                        (int) (b.Center.Z - b.Dimensions.Z),
                        (int) (b.Center.Z + b.Dimensions.Z)
                    )
                );

                this.Octree.Insert(p);
            }
        }
    }

    public override void _Process(double delta)
    {
        if (Debug) {
            Octree.Debug = true;
            Octree.DebugDraw();
        } else {
            Octree.Debug = false;
        }
    }
}
