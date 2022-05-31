#version 430 core
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoords;
layout(location = 2) in vec3 aNormal;
layout(location = 3) in vec3 aTangent;
layout(location = 4) in mat4 aInstanceTransform;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection; 

out vec2 vTexCoords;
out vec3 vNormal;
out vec3 vPosition;
out float vVisibility;
out mat3 vTBN;

const float density = 0.03; //move ?
const float gradient = 2.0; //move ?

void main()
{
    mat4 instanceTransform = aInstanceTransform * model;
    vec4 worldPosition = instanceTransform * vec4(aPosition, 1.0f);
    vec4 cameraRelativePosition = view * worldPosition;
    gl_Position = projection * cameraRelativePosition;
    vTexCoords = aTexCoords;
    
    vec3 T = normalize(vec3(model * vec4(aTangent,   0.0)));
    vec3 N = normalize(vec3(model * vec4(aNormal,    0.0)));
    T = normalize(T - dot(T, N) * N);
    vec3 B = cross(N, T);
    vTBN = mat3(T, B, N);
    vNormal =  mat3(transpose(inverse(instanceTransform))) * aNormal;
    vPosition = worldPosition.xyz;
    float distance = length(cameraRelativePosition.xyz);
    vVisibility = exp(-pow((distance * density), gradient));
    vVisibility = clamp(vVisibility, 0.0, 1.0);
}