#version 430 core
layout (location = 0) in vec3 Position;
layout (location = 1) in float Size;

void main()
{
    gl_Position = vec4(Position, 1.0);
    gl_PointSize = Size;
}