shader_type spatial;
render_mode specular_disabled;

uniform vec4 star_color : source_color;
uniform float glow_power : hint_range(0.01, 0.1) = 0.1;
uniform float corona : hint_range(0.05, 0.2) = 0.1;
uniform float fade : hint_range(-1, 0) = -0.2;
uniform float scale : hint_range(1.0, 4.0) = 2.0;
uniform float vert_scale: hint_range(0.5, 4.0) = 1.0;
uniform float diffusion_hardness : hint_range(0.1, 5.0) = 0.6;

float star(vec2 uv, float flare)
{
    float d = length(uv) * 2.0;
    float m = corona / d;

    float rays = max(0.0, 1.0 - abs(uv.x * uv.y * 500.0));
    m += rays * flare;

    rays = max(0.0, 1.0 - abs(uv.x * uv.y * 500.0));
    m += rays * 0.3 * flare;

    m *= smoothstep(1.0, 0.2, d);
    return m;
}

void vertex()
{
	MODELVIEW_MATRIX = (
		VIEW_MATRIX * mat4(
			INV_VIEW_MATRIX[0],
			INV_VIEW_MATRIX[1],
			INV_VIEW_MATRIX[2],
			MODEL_MATRIX[3]
		)
	);
	
	VERTEX *= vert_scale;
}

void fragment()
{
    vec3 col = vec3(0);

	// Offset UV to center the starburst.
	vec2 uv = 0.5 - fract(UV);

	// diffusion spikes
	float flare = smoothstep(0.9, 1.0, 1.0) * diffusion_hardness * -fade;

    col += star(uv, flare);
	EMISSION = col * star_color.rgb * glow_power;
	ALPHA = min(col.r + col.g + col.b, 1.0);
}
