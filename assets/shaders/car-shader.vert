#version 430 core
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec2 vTexCoords;

void main()
{
    vec4 worldPosition = model * vec4(aPosition, 1.0f);
    vec4 cameraRelativePosition = view * worldPosition;
    gl_Position = projection * cameraRelativePosition;
    vTexCoords = aTexCoords;
}