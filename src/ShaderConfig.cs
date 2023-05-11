using Godot;


public partial class ShaderConfig : Node3D
{
    public RenderingDevice RD;
    public Rid Shader;
    public Rid Buffer;
    public Rid UniformSet;
    public Rid Pipeline;
    public long ComputeList;

    public override void _Ready()
    {
        LoadShaderfile("res://resources/compute/computeTestShader.glsl");

        float[] input = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        SetupBuffer(input);
        SetupUniformSet();
        SetupPipeline();
        SetupComputeList();

        float[] output = GetComputeResult(input);

        GD.Print("Input: ", input.Stringify());
        GD.Print("Output: ", output.Stringify());
    }

    private void LoadShaderfile(string shaderPath)
    {
        RD = RenderingServer.CreateLocalRenderingDevice();
        RDShaderFile shaderFile = GD.Load<RDShaderFile>(shaderPath);
        RDShaderSpirV shaderBytecode = shaderFile.GetSpirV();
        Shader = RD.ShaderCreateFromSpirV(shaderBytecode);
    }

    private void SetupBuffer(float[] input)
    {
        byte[] inputBytes = new byte[input.Length * sizeof(float)];
        System.Buffer.BlockCopy(input, 0, inputBytes, 0, inputBytes.Length);
        Buffer = RD.StorageBufferCreate((uint) inputBytes.Length, inputBytes);
    }

    private void SetupUniformSet()
    {
        RDUniform uniform = new RDUniform
        {
            UniformType = RenderingDevice.UniformType.StorageBuffer,
            Binding = 0
        };

        uniform.AddId(Buffer);

        UniformSet = RD.UniformSetCreate(
            new Godot.Collections.Array<RDUniform> { uniform },
            Shader,
            0
        );
    }

    private void SetupPipeline()
    {
        Pipeline = RD.ComputePipelineCreate(Shader);
    }

    private void SetupComputeList()
    {
        ComputeList = RD.ComputeListBegin();
        RD.ComputeListBindComputePipeline(ComputeList, Pipeline);
        RD.ComputeListBindUniformSet(ComputeList, UniformSet, 0);
        RD.ComputeListDispatch(ComputeList, xGroups: 5, yGroups: 1, zGroups: 1);
        RD.ComputeListEnd();
    }

    private float[] GetComputeResult(float[] input)
    {
        RD.Submit();
        RD.Sync();

        byte[] outputBytes = RD.BufferGetData(Buffer);
        float[] output = new float[input.Length];
        System.Buffer.BlockCopy(outputBytes, 0, output, 0, outputBytes.Length);

        return output;
    }
}
