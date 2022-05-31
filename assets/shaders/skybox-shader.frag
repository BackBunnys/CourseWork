#version 430 core

in vec4 vColor;
in vec3 vTexCoords;

out vec4 FragColor;

uniform samplerCube skybox;
uniform vec3 fogColor = vec3(93 / 255.0f, 95 / 255.0f, 110 / 255.0f); //move

const float lowerLimit = 0.0;
const float upperLimit = 0.03;


void main()
{
    FragColor = texture(skybox, vTexCoords);

    float factor = (vTexCoords.y - lowerLimit) / (upperLimit - lowerLimit);
    factor = clamp(factor, 0.0, 1.0);
    FragColor = mix(vec4(fogColor, 1), FragColor, factor);
}