#version 430 core
layout (points) in;
layout (triangle_strip) out;
layout (max_vertices = 4) out;

uniform mat4 view;
uniform mat4 projection;
uniform vec3 viewPosition;
 
out vec2 vTexCoords;
out vec3 vNormal;
out vec3 vPosition;
out float vVisibility; 

const float density = 0.03; //move ?
const float gradient = 2.0; //move ?

void main() {
	vec3 position = gl_in[0].gl_Position.xyz;
    float size = gl_in[0].gl_PointSize;
	vec3 direction = normalize(viewPosition - position);
	vec3 up = vec3(0.0, 1.0, 0.0);
	vec3 right = cross(direction, up);
	
    mat4 viewProjection = projection * view;

    vNormal = direction;
    vPosition = (viewProjection * vec4(position, 1.0)).xyz;
    vec4 cameraRelativePosition = view * vec4(position, 1.0f);

    float distance = length(cameraRelativePosition.xyz);
    vVisibility = exp(-pow((distance * density), gradient));
    vVisibility = clamp(vVisibility, 0.0, 1.0);

    position -= (right * 0.5 * size);
    gl_Position = viewProjection * vec4(position, 1.0);
    vTexCoords = vec2(0.0, 0.0);
    EmitVertex();
  
    position.y += 1.0 * size;
    gl_Position = viewProjection * vec4(position, 1.0);
    vTexCoords = vec2(0.0, 1.0);
    EmitVertex();
 
    position.y -= 1.0 * size;
    position += right * size;
    gl_Position = viewProjection * vec4(position, 1.0);
    vTexCoords = vec2(1.0, 0.0);
    EmitVertex();
 
    position.y += 1.0 * size;
    gl_Position = viewProjection * vec4(position, 1.0);
    vTexCoords = vec2(1.0, 1.0);
    EmitVertex();
 
    EndPrimitive();
}
