#version 330

vec3 green[4] = vec3[](
    vec3(0.0, 0.2, 0.1),
    vec3(0.0, 0.5, 0.0),
    vec3(0.0, 0.7, 0.0),
    vec3(0.2, 1.0, 0.0)
);

vec3 alumi[3] = vec3[](
    vec3(0.2, 0.2, 0.4),
    vec3(0.5, 0.5, 1.0),
    vec3(1.0, 1.0, 1.0)
);

vec3 steel[3] = vec3[](
    vec3(0.0, 0.2, 0.0),
    vec3(0.1, 0.3, 0.3),
    vec3(0.3, 0.5, 0.5)
);

vec3 rust[3] = vec3[](
    vec3(0.3, 0.0, 0.0),
    vec3(0.5, 0.1, 0.0),
    vec3(0.7, 0.3, 0.0)
);

vec3 board[3] = vec3[](
    vec3(0.0, 0.2, 0.1),
    vec3(0.0, 0.5, 0.0),
    vec3(0.0, 0.7, 0.0)
);

float luma(vec3 color) {
  return dot(color, vec3(0.299, 0.587, 0.114));
}
float luma(vec4 color) {
  return dot(color.rgb, vec3(0.299, 0.587, 0.114));
}

vec3 dithermono(vec2 pos, vec4 col, vec4 maskCol) {
    vec3 palette[] = board;
    if (luma(maskCol) > 0.5) {
        palette = alumi;
    };
    
    float bands = palette.length();
    float bri = luma(col);
	float x = floor(mod(pos.x, 4.0));
	float y = floor(mod(pos.y, 4.0));
	float index = floor(x + y * 4.0);
	float limit = 0.0;
	float stepp = 1.0 / bands;

    float matrix[16] = float[](
		0.0625, 0.5625, 0.1875, 0.6875, 
        0.8125, 0.3125, 0.9375, 0.4375, 
        0.25,   0.75,   0.125,  0.625,  
        1.0,    0.5,    0.875,  0.375   
    );

    limit = matrix[int(index)];
	float a = bri - mod(bri,stepp);
    float b = a + stepp;
	limit = limit/bands + a;
	float _out = a;
	if (bri > limit) { _out = b; };
    /* return col.xyz; */
    /* return vec3(bri, bri, bri); */
    return palette[int(floor(_out*bands*0.99))];
}


in vec2 fragTexCoord;
in vec4 fragColor;

uniform sampler2D texture0;
uniform sampler2D mask;
uniform vec4 colDiffuse;

out vec4 finalColor;


void main()
{
    vec4 texelColor = texture(texture0, fragTexCoord);
    vec4 maskColor = texture(mask, fragTexCoord);

    /* finalColor = texelColor; */
    
    finalColor = vec4(dithermono(gl_FragCoord.xy, texelColor, maskColor), 1.0);
}

