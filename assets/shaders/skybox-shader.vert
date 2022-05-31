#version 430 core
layout(location = 0) in vec3 aPosition;

uniform mat4 view;
uniform mat4 projection;

out vec3 vTexCoords;

void main()
{
    vec4 position = projection * mat4(mat3(view)) * vec4(aPosition, 1.0);
    gl_Position = position.xyww;
    vTexCoords = vec3(aPosition.x, aPosition.y, aPosition.z);
}