#[compute]
#version 450

// #define delta ( 1.0 / 30.0 )

layout(local_size_x = 2, local_size_y = 1, local_size_z = 1) in;

layout(set = 0, binding = 0, std430) restrict buffer DataBuffer {
    float data[];
}

data_buffer;

void main()
{
    data_buffer.data[gl_GlobalInvocationID.x] *= 2.0;
    // vec2 uv = gl_FragCoord.xy / resolution.xy;
    // vec4 tmpPos = texture2D( texturePosition, uv );
    // vec3 pos = tmpPos.xyz;

    // vec4 tmpVel = texture2D( textureVelocity, uv );
    // vec3 vel = tmpVel.xyz;

    // // Dynamics
    // if (pos.x != 0.0 && pos.y != 0.0 && pos.z != 0.0) {
    //     pos += vel * delta;
    // }

    // gl_FragColor = vec4( pos, tmpPos.w );
}
