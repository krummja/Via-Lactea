using Godot;
using System;
using System.Collections.Generic;


public partial class CameraController : Node3D
{
    [ExportGroup("Controller Dynamics")]


    [ExportSubgroup("Movement")]

    [Export] public float MaxImpulse = 5f;
    [Export] public float Acceleration = 10f;
    [Export] public float Damping = 15f;
    [Export] public float VerticalDamping = 5f;


    [ExportSubgroup("Zoom")]

    [Export] public float ZoomStep = 7.5f;
    [Export] public float ZoomDamping = 7.5f;
    [Export] public float ZoomSpeed = 2f;
    [Export] public float MinAltitude = 5f;
    [Export] public float MaxAltitude = 50f;


    [ExportSubgroup("Rotation")]

    [Export] public float MaxRotationSpeed = 0.01f;
    [Export] public float MaxKeyRotationSpeed = 0.1f;
    [Export] public float RotationDamping = 10f;
}
