#version 430 core
out vec4 FragColor;

in vec2 vTexCoords;

layout(binding = 0) uniform sampler2D texture0;

uniform float gamma = 2.2f;

void main()
{             
    vec4 color = texture(texture0, vTexCoords);
    vec3 result = vec3(1.0) - exp(-color.rgb * 1);

    result = pow(result, vec3(1.0 / gamma));
    FragColor = vec4(result, 1.0);
}