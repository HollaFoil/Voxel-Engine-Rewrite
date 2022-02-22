#version 330 core
out vec4 result;                                                                                     
in vec2 tex;   
flat in int id;           
in float AO;
uniform sampler2DArray uTexture;                                             
void main()                                                
{               
	float a = (AO+2) / 5;
	vec4 texColor = texture(uTexture, vec3(tex, 0)) * vec4(a,a,a,1.0);
    texColor.a = 1.0;
    if(texColor.a < 0.1)
        discard;
    result = texColor;
} 