#version 430 core

struct BaseLight {
  vec3 color;
  float ambientIntensity;
  float diffuseIntensity;
};

struct DirectionalLight {
  BaseLight base;
  vec3 direction;
};

struct Attenuation {
    float constant;
    float linear;
    float exp;
};

struct PointLight {
  Attenuation attenuation;
  BaseLight base;
  vec3 position;
};

struct SpotLight {
  PointLight base;
  vec3 direction;
  float cutoff;
};

in vec2 vTexCoords;
in vec3 vNormal;
in vec3 vPosition;
in float vVisibility;
in mat3 vTBN;
in vec3 vTangentViewPosition;
in vec3 vTangentFragPosition;

out vec4 FragColor;

uniform vec4 color = vec4(1.0f);
uniform vec3 viewPosition;

layout(binding = 0) uniform sampler2D texture0;
layout(binding = 1) uniform sampler2D normalTexture;
uniform vec3 textureTiling = vec3(1);
uniform int textureUnit1 = 0;

uniform DirectionalLight directionalLight[4];
uniform int directionalLightCount;

uniform PointLight pointLight[4];
uniform int pointLightCount;

uniform SpotLight spotLight[4];
uniform int spotLightCount;

uniform float colorMixing = 0;
uniform vec3 fogColor = vec3(93 / 255.0f, 95 / 255.0f, 110 / 255.0f); //move

float specularStrength = 1.f; //move

vec3 calculateBaseLight(BaseLight light, vec3 direction, vec3 normal) {
    vec3 ambientColor = light.color * light.ambientIntensity;
    float diffuseFactor = dot(normal, normalize(-direction));
    vec3 diffuseColor = max(light.color * light.diffuseIntensity * diffuseFactor, vec3(0));

    vec3 specularColor = vec3(0);
    
    if(diffuseFactor > 0.f) {
        vec3 viewDirection = normalize(viewPosition - vPosition);
        //vec3 halfwayDir = normalize(direction + viewDirection);
        vec3 reflectDirection =  normalize(reflect(direction, normal));  
        float specularFactor = pow(max(dot(viewDirection, reflectDirection), 0.0), 16);
        specularColor =  specularFactor * light.color;  
    }

    return ambientColor + diffuseColor + specularColor;
}

vec3 calculateDirectionalLight(DirectionalLight light, vec3 normal) {
    return calculateBaseLight(light.base, light.direction, normal);
}

vec3 calculatePointLight(PointLight light, vec3 normal) {
    vec3 direction = vPosition - light.position;
    float distance = length(direction);
    direction = normalize(direction);
 
    vec3 color = calculateBaseLight(light.base, direction, normal);
    float attenuation =  light.attenuation.constant +
                 light.attenuation.linear * distance +
                 light.attenuation.exp * distance * distance;
 
    return color / attenuation;
}

vec3 calculateSpotLight(SpotLight light, vec3 normal) {
    vec3 direction = normalize(vPosition - light.base.position);
    float factor = dot(direction, light.direction);
 
      if (factor > light.cutoff) {
        vec3 color = calculatePointLight(light.base, normal);
        return color * (1.0 - (1.0 - factor) * 1.0/(1.0 - light.cutoff));
      }
      else {
        return vec3(0);
      }
}

vec3 mapNormal(vec2 texCoords) {
   vec3 normal = texture(normalTexture, texCoords).rgb;
    normal = normalize(normal * 2.0 - 1.0);   
    normal = normalize(vTBN * normal);  
    return normal;
}

void main()
{
    vec2 texCoords = vTexCoords * textureTiling.xy;

    vec3 normal;
    if(textureUnit1 == 1) {
        normal = mapNormal(texCoords);
    } else {
        normal = normalize(vNormal);
    }

    vec3 totalLight = vec3(0);
    for(int i = 0; i < directionalLightCount; i++) {
        totalLight += calculateDirectionalLight(directionalLight[i], normal);
    }

    for(int i = 0; i < pointLightCount; i++) {
        totalLight += calculatePointLight(pointLight[i], normal);
    }

    for(int i = 0; i < spotLightCount; i++) {
        totalLight += calculateSpotLight(spotLight[i], normal);
    }

    FragColor = mix(texture(texture0, texCoords), color, colorMixing) * vec4(totalLight, 1.f);
    FragColor = mix(vec4(fogColor, 1), FragColor, vVisibility);
}



//
//const float PI = 3.14159265359;
//const float metallic = 0.5f;
//const float roughness = 0.2f;
//const float ao = 0.2f;
//
//
//vec3 fresnelSchlick(float cosTheta, vec3 F0)
//{
//    return F0 + (1.0 - F0) * pow(clamp(1.0 - cosTheta, 0.0, 1.0), 5.0);
//}  
//
//float DistributionGGX(vec3 N, vec3 H, float roughness)
//{
//    float a      = roughness*roughness;
//    float a2     = a*a;
//    float NdotH  = max(dot(N, H), 0.0);
//    float NdotH2 = NdotH*NdotH;
//	
//    float num   = a2;
//    float denom = (NdotH2 * (a2 - 1.0) + 1.0);
//    denom = PI * denom * denom;
//	
//    return num / max(denom, 0.00001);
//}
//
//float GeometrySchlickGGX(float NdotV, float roughness)
//{
//    float r = (roughness + 1.0);
//    float k = (r*r) / 8.0;
//
//    float num   = NdotV;
//    float denom = NdotV * (1.0 - k) + k;
//	
//    return num / max(denom, 0.00001);
//}
//float GeometrySmith(vec3 N, vec3 V, vec3 L, float roughness)
//{
//    float NdotV = max(dot(N, V), 0.0);
//    float NdotL = max(dot(N, L), 0.0);
//    float ggx2  = GeometrySchlickGGX(NdotV, roughness);
//    float ggx1  = GeometrySchlickGGX(NdotL, roughness);
//	
//    return ggx1 * ggx2;
//}
//
//void main()
//{
//    vec2 texCoords = vTexCoords * textureTiling.xy;
//    vec3 N;
//    if(textureUnit1) {
//        N = mapNormal(texCoords);
//    } else {
//        N = normalize(vNormal);
//    }
//    vec3 V = normalize(viewPosition - vPosition);
//
//    vec3 F0 = vec3(0.77, 0.78, 0.78); 
//    F0 = mix(F0, color.rgb, metallic);
//	           
//     reflectance equation
//    vec3 Lo = vec3(0.0);
//    for(int i = 0; i < directionalLightCount; ++i) 
//    {
//         calculate per-light radiance
//        vec3 L = normalize(-directionalLight[i].direction);
//        vec3 H = normalize(V + L);
//        float distance    = length(-directionalLight[i].direction - vPosition);
//        float attenuation = 1.0;
//        vec3 radiance     = directionalLight[i].base.color * attenuation;        
//        
//         cook-torrance brdf
//        float NDF = DistributionGGX(N, H, roughness);        
//        float G   = GeometrySmith(N, V, L, roughness);      
//        vec3 F    = fresnelSchlick(max(dot(H, V), 0.0), F0);       
//        
//        vec3 kS = F;
//        vec3 kD = vec3(1.0) - kS;
//        kD *= 1.0 - metallic;	  
//        
//        vec3 numerator    = NDF * G * F;
//        float denominator = 4.0 * max(dot(N, V), 0.0) * max(dot(N, L), 0.0) + 0.0001;
//        vec3 specular     = numerator / max(denominator, 0.00001);  
//            
//         add to outgoing radiance Lo
//        float NdotL = max(dot(N, L), 0.0);                
//        Lo += (kD * color.rgb / PI + specular) * radiance * NdotL; 
//    }   
//
//        for(int i = 0; i < pointLightCount; ++i) 
//    {
//         calculate per-light radiance
//        vec3 L = normalize(pointLight[i].position);
//        vec3 H = normalize(V + L);
//        float distance    = length(pointLight[i].position - vPosition);
//        float attenuation = 1.0 / (distance * distance);;
//        vec3 radiance     = pointLight[i].base.color * attenuation;        
//        
//         cook-torrance brdf
//        float NDF = DistributionGGX(N, H, roughness);        
//        float G   = GeometrySmith(N, V, L, roughness);      
//        vec3 F    = fresnelSchlick(max(dot(H, V), 0.0), F0);       
//        
//        vec3 kS = F;
//        vec3 kD = vec3(1.0) - kS;
//        kD *= 1.0 - metallic;	  
//        
//        vec3 numerator    = NDF * G * F;
//        float denominator = 4.0 * max(dot(N, V), 0.0) * max(dot(N, L), 0.0) + 0.0001;
//        vec3 specular     = numerator / max(denominator, 0.00001);  
//            
//         add to outgoing radiance Lo
//        float NdotL = max(dot(N, L), 0.0);                
//        Lo += (kD * color.rgb / PI + specular) * radiance * NdotL; 
//    }   
//
//            for(int i = 0; i < spotLightCount; ++i) 
//    {
//         calculate per-light radiance
//        vec3 L = normalize(spotLight[i].base.position);
//        vec3 H = normalize(V + L);
//        float distance    = length(spotLight[i].base.position - vPosition);
//        float attenuation = 1.0 / (distance * distance);
//        vec3 radiance     = spotLight[i].base.base.color * attenuation;        
//        
//         cook-torrance brdf
//        float NDF = DistributionGGX(N, H, roughness);        
//        float G   = GeometrySmith(N, V, L, roughness);      
//        vec3 F    = fresnelSchlick(max(dot(H, V), 0.0), F0);       
//        
//        vec3 kS = F;
//        vec3 kD = vec3(1.0) - kS;
//        kD *= 1.0 - metallic;	  
//        
//        vec3 numerator    = NDF * G * F;
//        float denominator = 4.0 * max(dot(N, V), 0.0) * max(dot(N, L), 0.0) + 0.0001;
//        vec3 specular     = numerator / max(denominator, 0.00001);  
//            
//         add to outgoing radiance Lo
//        float NdotL = max(dot(N, L), 0.0);                
//        Lo += (kD * color.rgb / PI + specular) * radiance * NdotL; 
//    }   
//
//  
//    vec3 ambient = directionalLight[0].base.color.rgb * ao;
//    vec3 totalColor = ambient + Lo;
//	
//    totalColor = totalColor / (totalColor + vec3(1.0));
//    totalColor = pow(totalColor, vec3(1.0/2.2));  
//   
//    FragColor = mix(texture(texture0, texCoords), color, colorMixing) * vec4(totalColor, 1.f);
//    FragColor = mix(vec4(fogColor, 1), FragColor, vVisibility);
//}