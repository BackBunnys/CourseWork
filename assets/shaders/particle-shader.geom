#version 430 core
layout (points) in;
layout (triangle_strip) out;
layout (max_vertices = 4) out;

uniform mat4 view;
uniform mat4 projection;
uniform vec3 viewPosition;

uniform ivec2 spriteSetSize = ivec2(8);

in float vLifetime[];
in float vType[];
 
out vec2 gTexCoords;
out vec2 gNextTexCoords;
out vec3 vNormal;
out vec3 vPosition;
out float vVisibility; 
out float gLifetime;

const float density = 0.03; //move ?
const float gradient = 2.0; //move ?

vec4 calculateSpriteBounds(int number) {
    vec2 spriteSize = 1.0f / spriteSetSize;
    vec2 spriteIndex = vec2(number % spriteSetSize.y, number / spriteSetSize.y); 
    vec2 spritePosition = spriteSize * spriteIndex; 
    return vec4(spritePosition, spritePosition + spriteSize);
}

void main() {
    if(vLifetime[0] <= 0) return;

	vec3 position = gl_in[0].gl_Position.xyz;
    float size = gl_in[0].gl_PointSize;
	vec3 direction = normalize(viewPosition - position);
	vec3 up = vec3(0.0, 1.0, 0.0);
	vec3 right = cross(direction, up) * size;
	
    mat4 viewProjection = projection * view;

    vNormal = direction;
    vPosition = (viewProjection * vec4(position, 1.0)).xyz;
    vec4 cameraRelativePosition = view * vec4(position, 1.0f);

    gLifetime = vLifetime[0];

    float type = vType[0];
    int spriteNumber = int(floor(type));
    vec4 spriteBounds = calculateSpriteBounds(spriteNumber);
    vec4 nextSpriteBounds = calculateSpriteBounds(spriteNumber + 1);

    position.y -= size / 2;
    position -= (right * 0.5);
    gl_Position = viewProjection * vec4(position, 1.0);
    gTexCoords = spriteBounds.xy;
    gNextTexCoords = nextSpriteBounds.xy;
    EmitVertex();
  
    position.y += size;
    gl_Position = viewProjection * vec4(position, 1.0);
    gTexCoords = spriteBounds.xw;
    gNextTexCoords = nextSpriteBounds.xw;
    EmitVertex();
 
    position.y -= size;
    position += right;
    gl_Position = viewProjection * vec4(position, 1.0);
    gTexCoords = spriteBounds.zy;
    gNextTexCoords = nextSpriteBounds.zy;
    EmitVertex();
 
    position.y += size;
    gl_Position = viewProjection * vec4(position, 1.0);
    gTexCoords = spriteBounds.zw;
    gNextTexCoords = nextSpriteBounds.zw;
    EmitVertex();
 
    EndPrimitive();
}
