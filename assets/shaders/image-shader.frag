#version 430 core

in vec2 vTexCoords;
in float vVisibility;

out vec4 FragColor;

uniform vec4 color = vec4(1.0f);
uniform vec3 viewPosition;

layout(binding = 0) uniform sampler2D texture0;
uniform vec3 textureTiling = vec3(1);

uniform int spotLightCount;

uniform float colorMixing = 0;
uniform vec3 fogColor = vec3(93 / 255.0f, 95 / 255.0f, 110 / 255.0f); //move

void main()
{
    vec2 texCoords = vTexCoords * textureTiling.xy;
    vec4 texColor = texture(texture0, texCoords);
    if(texColor.a < 0.1) discard;

    FragColor = mix(texColor, color, colorMixing);
    FragColor = mix(vec4(fogColor, 1), FragColor, vVisibility);
}