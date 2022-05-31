#version 430 core

in vec2 gTexCoords;
in vec3 vNormal;
in vec3 vPosition;
in float vVisibility;

out vec4 FragColor;

uniform vec4 color = vec4(1.0f);

uniform sampler2D texture0;
uniform vec3 textureTiling = vec3(1);

uniform float colorMixing = 0;


void main()
{
    vec4 fcolor = texture(texture0, gTexCoords * textureTiling.xy);
    if(fcolor.a < 0.1f) {
        discard;
    }

    FragColor = mix(fcolor, color, colorMixing);
}
