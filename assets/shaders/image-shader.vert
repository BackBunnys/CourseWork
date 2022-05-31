#version 430 core
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoords;
layout(location = 2) in vec3 aNormal;
layout(location = 3) in vec3 aTangent;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec2 vTexCoords;
out float vVisibility;

const float density = 0.03; //move ?
const float gradient = 2.0; //move ?

void main()
{

    mat4 viewModel = view * model;

    viewModel[0][0] = 1;    
    viewModel[0][1] = 0;
    viewModel[0][2] = 0;
    viewModel[2][0] = 0;
    viewModel[2][1] = 0;
    viewModel[2][2] = 1;

    vec4 worldPosition = model * vec4(aPosition, 1.0f);
    vec4 cameraRelativePosition = viewModel * vec4(aPosition, 1.0f);
    gl_Position = projection * cameraRelativePosition;
    vTexCoords = aTexCoords;

    float distance = length(cameraRelativePosition.xyz);
    vVisibility = exp(-pow((distance * density), gradient));
    vVisibility = clamp(vVisibility, 0.0, 1.0);
}