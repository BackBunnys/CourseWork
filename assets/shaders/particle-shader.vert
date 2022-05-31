#version 430 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in float aLifetime;
layout (location = 2) in float aSize;
layout (location = 3) in float aType;
 
out float vLifetime;
out float vType;
 
void main()
{
    vLifetime = aLifetime;
    vType = aType;
    gl_Position = vec4(aPosition, 1.0);
    gl_PointSize = aSize;
}