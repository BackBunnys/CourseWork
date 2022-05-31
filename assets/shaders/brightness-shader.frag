#version 430 core

in vec2 vTexCoords;

out vec4 FragColor;

uniform sampler2D screenTexture;

void main()
{
    vec4 color = texture(screenTexture, vTexCoords);

    float brightness = dot(color.rgb, vec3(0.2126, 0.7152, 0.0722));
    if(brightness > 1)
        FragColor = vec4(color.rgb, 1.0);
    else
        FragColor = vec4(0.0, 0.0, 0.0, 1.0);
}