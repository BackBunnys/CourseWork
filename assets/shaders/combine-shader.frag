#version 430 core
out vec4 FragColor;

in vec2 vTexCoords;

layout(binding = 0) uniform sampler2D texture0;
layout(binding = 1) uniform sampler2D texture1;

void main()
{             
    vec3 hdrColor = texture(texture0, vTexCoords).rgb;      
    vec3 bloomColor = texture(texture1, vTexCoords).rgb;

    FragColor = vec4(hdrColor + bloomColor, 1.0f);
}