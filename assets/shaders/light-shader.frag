#version 430 core

out vec4 FragColor;

uniform vec4 color = vec4(1.0f);
uniform float intensity;

void main()
{
    FragColor = color * intensity * 2;
}