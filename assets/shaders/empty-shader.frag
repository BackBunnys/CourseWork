#version 430 core

out vec4 FragColor;

in vec2 vTexCoords;

uniform sampler2D texture0;

void main()
{   
    float depthValue = texture(screenTexture, vTexCoords).r;
    FragColor = vec4(vec3(depthValue), 1.0);
}

