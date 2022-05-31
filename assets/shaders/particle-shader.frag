#version 430 core                                                                           
            
out vec4 FragColor;

in vec2 gTexCoords;
in vec2 gNextTexCoords;
in vec3 vNormal;
in vec3 vPosition;
in float vVisibility;
in float gLifetime;


uniform vec4 color = vec4(1);

uniform sampler2D texture0;
uniform float colorMixing = 0.9;

void main()                                                                         
{                
    vec4 spriteColor = mix(texture(texture0, gTexCoords), texture(texture0, gNextTexCoords), fract(gLifetime));

    FragColor = mix(spriteColor, color, colorMixing);                                                               
}  