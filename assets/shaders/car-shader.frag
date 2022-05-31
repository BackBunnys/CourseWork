#version 430 core

in vec2 vTexCoords;

out vec4 FragColor;

uniform sampler2D texture0;
uniform vec4 color = vec4(1.0f);

void main()
{
	FragColor = texture(texture0, vTexCoords) * color;
}