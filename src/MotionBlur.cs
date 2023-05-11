using Godot;


public partial class MotionBlur : MeshInstance3D
{
    [Export]
    public Label LinearVelocity;

    [Export]
    public Label AngularVelocity;

    private Vector3 cameraPosPrev;
    private Quaternion cameraRotPrev;

    public override void _Process(double delta)
    {
        ShaderMaterial mat = MaterialOverride as ShaderMaterial;
        Camera3D cam = GetParent<Camera3D>();

        Vector3 velocity = cam.GlobalTransform.Origin - cameraPosPrev;

        Quaternion camRot = new Quaternion(cam.GlobalTransform.Basis);
        Quaternion camRotDiff = camRot - cameraRotPrev;

        if (camRot.Dot(cameraRotPrev) < 0.0) {
            camRotDiff = -camRotDiff;
        }

        Quaternion camRotConj = Conjugate(camRot);

        Quaternion angularVelocity = (camRotDiff * 2.0f) * camRotConj;
        Vector3 angular = new Vector3(
            angularVelocity.X,
            angularVelocity.Y,
            angularVelocity.Z
        );

        mat.SetShaderParameter("linear_velocity", velocity);
        mat.SetShaderParameter("angular_velocity", angular);

        Variant matLinVel = mat.GetShaderParameter("linear_velocity");
        Variant matAngVel = mat.GetShaderParameter("angular_velocity");

        LinearVelocity.Text = $"Linear: {matLinVel}";
        AngularVelocity.Text = $"Angular: {matAngVel}";

        cameraPosPrev = cam.GlobalTransform.Origin;
        cameraRotPrev = new Quaternion(cam.GlobalTransform.Basis);
    }

    private Quaternion Conjugate(Quaternion quat)
    {
        return new Quaternion(-quat.X, -quat.Y, -quat.Z, quat.W);
    }
}
