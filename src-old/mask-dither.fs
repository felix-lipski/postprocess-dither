#version 330

vec3 alumi[] = vec3[](
    vec3(0.223, 0.164, 0.109),
    vec3(0.407, 0.298, 0.235),
    vec3(0.572, 0.494, 0.415)
);

vec3 steel[] = vec3[](
    vec3(0.164, 0.113, 0.050),
    vec3(0.223, 0.164, 0.109),
    vec3(0.407, 0.298, 0.235)
);

vec3 black[] = vec3[](
    vec3(0.121, 0.141, 0.039),
    vec3(0.164, 0.113, 0.050),
    vec3(0.223, 0.164, 0.109)
);

vec3 wood[] = vec3[](
    /* vec3(0.6, 0.2, 0.8), */
    /* /1* vec3(0.3, 0.3, 0.8), *1/ */
    /* vec3(0.3, 0.8, 0.3), */
    /* vec3(0.0, 0.5, 0.2) */
    vec3(0.333, 0.058, 0.039),
    /* vec3(0.0, 0.5, 0.2), */
    vec3(0.447, 0.254, 0.074),
    vec3(0.670, 0.360, 0.109)
);

/* vec3 wood[3] = vec3[]( */
/*     vec3(0.333, 0.058, 0.039), */
/*     /1* vec3(0.270, 0.137, 0.050), *1/ */
/*     vec3(0.447, 0.254, 0.074), */
/*     vec3(0.670, 0.360, 0.109) */
/* ); */

vec3 grass[] = vec3[](
    vec3(0.223, 0.341, 0.109),
    vec3(0.647, 0.549, 0.152),
    vec3(0.937, 0.674, 0.156)
);

vec3 leaf[] = vec3[](
    vec3(0.121, 0.141, 0.039),
    vec3(0.223, 0.341, 0.109),
    vec3(0.647, 0.549, 0.152)
);

vec3 sand[] = vec3[](
    vec3(0.407, 0.298, 0.235),
    vec3(0.937, 0.717, 0.458),
    vec3(0.937, 0.847, 0.631)
);

vec3 water[] = vec3[](
    vec3(0.094, 0.247, 0.223),
    vec3(0.152, 0.392, 0.407),
    vec3(0.235, 0.623, 0.611)
);

vec3 sky[] = vec3[](
    vec3(0.152, 0.392, 0.407),
    vec3(0.235, 0.623, 0.611),
    vec3(0.937, 0.847, 0.631)
);

vec3 yellow[] = vec3[](
    vec3(0.333, 0.058, 0.039),
    /* vec3(0.270, 0.137, 0.050), */
    /* vec3(0.121, 0.141, 0.039), */
    vec3(0.937, 0.674, 0.156),
    vec3(0.937, 0.847, 0.631)
);

/* vec3 yellow[3] = vec3[]( */
/*     vec3(0.647, 0.549, 0.152), */
/*     vec3(0.937, 0.674, 0.156), */
/*     vec3(0.937, 0.847, 0.631) */
/* ); */

vec3 blood[] = vec3[](
    vec3(0.333, 0.058, 0.039),
    vec3(0.607, 0.101, 0.039),
    vec3(0.937, 0.227, 0.047)
);


float luma(vec3 color) {
  return dot(color, vec3(0.299, 0.587, 0.114));
}
float luma(vec4 color) {
  return dot(color.rgb, vec3(0.299, 0.587, 0.114));
}

vec3 dithermono(vec2 pos, vec4 col, vec4 maskCol) {
    int red = int(maskCol.r * 256.0);
    int green = int(maskCol.g * 256.0);
    int blue = int(maskCol.b * 256.0);

    vec3 palette[] = yellow;
    if (red == 0) {
        palette = leaf;
    } else if (red == 128) {
        if (blue == 128) {
            palette = yellow;
        } else {
            palette = wood;
        };
    } else if (red == 256) {
        palette = blood;
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
    /* return maskCol.xyz; */
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

