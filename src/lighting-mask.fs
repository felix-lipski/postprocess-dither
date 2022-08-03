#version 330

// Input vertex attributes (from vertex shader)
in vec3 fragPosition;
in vec2 fragTexCoord;
in vec4 fragColor;
in vec3 fragNormal;

// Input uniform values
uniform sampler2D texture0;
uniform vec4 colDiffuse;

// Output fragment color
out vec4 finalColor;

// NOTE: Add here your custom variables

#define     MAX_LIGHTS              4
#define     LIGHT_DIRECTIONAL       0
#define     LIGHT_POINT             1

struct MaterialProperty {
    vec3 color;
    int useSampler;
    sampler2D sampler;
};

struct Light {
    int enabled;
    int type;
    vec3 position;
    vec3 target;
    vec4 color;
};

uniform Light lights[MAX_LIGHTS];
uniform vec4 ambient;
uniform vec3 viewPos;


vec3 alumi[] = vec3[](
    vec3(0.223, 0.164, 0.109),
    vec3(0.407, 0.298, 0.235),
    vec3(0.572, 0.494, 0.415),
    vec3(0.937, 0.847, 0.631)
);

vec3 steel[] = vec3[](
    vec3(0.164, 0.113, 0.050),
    vec3(0.223, 0.164, 0.109),
    vec3(0.407, 0.298, 0.235),
    vec3(0.572, 0.494, 0.415)
);

vec3 tire[] = vec3[](
    vec3(0.164, 0.113, 0.050),
    vec3(0.121, 0.141, 0.039),
    vec3(0.223, 0.164, 0.109),
    vec3(0.407, 0.298, 0.235)
);

vec3 wood[] = vec3[](
    vec3(0.211, 0.090, 0.047),
    vec3(0.270, 0.137, 0.050),
    vec3(0.447, 0.254, 0.074),
    vec3(0.670, 0.360, 0.109)
);

vec3 grass[] = vec3[](
    vec3(0.121, 0.141, 0.039),
    vec3(0.223, 0.341, 0.109),
    vec3(0.647, 0.549, 0.152),
    vec3(0.937, 0.674, 0.156)
);

vec3 leaf[] = vec3[](
    vec3(0.164, 0.113, 0.050),
    vec3(0.121, 0.141, 0.039),
    vec3(0.223, 0.341, 0.109),
    vec3(0.647, 0.549, 0.152)
);

    
vec3 sand[] = vec3[](
    vec3(0.164, 0.113, 0.050),
    vec3(0.937, 0.674, 0.156),
    vec3(0.937, 0.717, 0.458),
    vec3(0.937, 0.847, 0.631)
);

vec3 water[] = vec3[](
    vec3(0.094, 0.247, 0.223),
    vec3(0.152, 0.392, 0.407),
    vec3(0.235, 0.623, 0.611),
    vec3(0.937, 0.847, 0.631)
);

vec3 sky[] = vec3[](
    vec3(0.164, 0.113, 0.050),
    vec3(0.094, 0.247, 0.223),
    vec3(0.152, 0.392, 0.407),
    vec3(0.235, 0.623, 0.611)
);

vec3 warn[] = vec3[](
    vec3(0.164, 0.113, 0.050),
    vec3(0.937, 0.674, 0.156),
    vec3(0.937, 0.674, 0.156),
    vec3(0.937, 0.847, 0.631)
);

vec3 blood[] = vec3[](
    vec3(0.211, 0.090, 0.047),
    vec3(0.333, 0.058, 0.039),
    vec3(0.607, 0.101, 0.039),
    vec3(0.937, 0.227, 0.047)
);

vec3 clear[] = vec3[](
    vec3(0.164, 0.113, 0.050),
    vec3(0.164, 0.113, 0.050),
    vec3(0.937, 0.847, 0.631),
    vec3(0.937, 0.847, 0.631)
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
    vec3 palette[] = steel;


    if (red < 64) { // 20
        if (green < 64) { // 20
            if (blue < 64) { // 20
                palette = tire;
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
            } else { // E0
            };
        } else if (green < 128) { // 60
            if (blue < 64) { // 20
                palette = leaf;
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
            } else { // E0
                palette = water;
            };
        } else if (green < 192) { // A0
            if (blue < 64) { // 20
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
            } else { // E0
                palette = sky;
            };
        } else { // E0
            if (blue < 64) { // 20
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
            } else { // E0
            };
        };
    } else if (red < 128) { // 60
        if (green < 64) { // 20
            if (blue < 64) { // 20
                palette = wood;
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
            } else { // E0
            };
        } else if (green < 128) { // 60
            if (blue < 64) { // 20
            } else if (blue < 128) { // 60
                palette = steel;
            } else if (blue < 192) { // A0
            } else { // E0
            };
        } else if (green < 192) { // A0
            if (blue < 64) { // 20
                palette = grass;
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
            } else { // E0
            };
        } else { // E0
            if (blue < 64) { // 20
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
            } else { // E0
            };
        };
    } else if (red < 192) { // A0
        if (green < 64) { // 20
            if (blue < 64) { // 20
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
            } else { // E0
            };
        } else if (green < 128) { // 60
            if (blue < 64) { // 20
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
            } else { // E0
            };
        } else if (green < 192) { // A0
            if (blue < 64) { // 20
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
                palette = alumi;
            } else { // E0
            };
        } else { // E0
            if (blue < 64) { // 20
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
            } else { // E0
            };
        };
    } else { // E0
        if (green < 64) { // 20
            if (blue < 64) { // 20
                palette = blood;
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
            } else { // E0
            };
        } else if (green < 128) { // 60
            if (blue < 64) { // 20
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
            } else { // E0
            };
        } else if (green < 192) { // A0
            if (blue < 64) { // 20
                palette = warn;
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
            } else { // E0
            };
        } else { // E0
            if (blue < 64) { // 20
            } else if (blue < 128) { // 60
            } else if (blue < 192) { // A0
                palette = sand;
            } else { // E0
                palette = clear;
            };
        };
    };

    /* if (red == 0) { */
    /*     palette = leaf; */
    /* }; */     
    /* if (red == 0) { */
    /*     palette = leaf; */
    /* } else if (red == 128) { */
    /*     if (blue == 128) { */
    /*         palette = yellow; */
    /*     } else { */
    /*         palette = wood; */
    /*     }; */
    /* } else if (red == 256) { */
    /*     palette = blood; */
    /* }; */
    
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


/* in vec2 fragTexCoord; */
/* in vec4 fragColor; */

/* uniform sampler2D texture0; */
uniform sampler2D mask;
/* uniform vec4 colDiffuse; */

/* out vec4 finalColor; */


/* void main() */
/* { */
/*     vec4 texelColor = texture(texture0, fragTexCoord); */
/*     vec4 maskColor = texture(mask, fragTexCoord); */

/*     /1* finalColor = texelColor; *1/ */
    
/*     finalColor = vec4(dithermono(gl_FragCoord.xy, texelColor, maskColor), 1.0); */
/* } */

void main()
{
    // Texel color fetching from texture sampler
    vec4 texelColor = texture(texture0, fragTexCoord);
    vec3 lightDot = vec3(0.0);
    vec3 normal = normalize(fragNormal);
    vec3 viewD = normalize(viewPos - fragPosition);
    vec3 specular = vec3(0.0);

    // NOTE: Implement here your fragment shader code

    for (int i = 0; i < MAX_LIGHTS; i++)
    {
        if (lights[i].enabled == 1)
        {
            vec3 light = vec3(0.0);

            if (lights[i].type == LIGHT_DIRECTIONAL) {
                light = -normalize(lights[i].target - lights[i].position);
            }
            if (lights[i].type == LIGHT_POINT) {
                light = normalize(lights[i].position - fragPosition);
            }
            float NdotL = max(dot(normal, light), 0.0);
            lightDot += lights[i].color.rgb*NdotL;

            float specCo = 0.0;
            // gloss, shine
            /* if (NdotL > 0.0) specCo = pow(max(0.0, dot(viewD, reflect(-(light), normal))), 16.0); // 16 refers to shine */
            specular += specCo;
        }
    }

    /* finalColor = (texelColor*((colDiffuse + vec4(specular, 1.0))*vec4(lightDot, 1.0))); */
    finalColor += texelColor*(ambient/10.0)*colDiffuse;
    finalColor = (texelColor*((colDiffuse + vec4(specular, 1.0))*0.5*vec4(lightDot, 1.0)));
    /* finalColor += texelColor*(0.8/10.0)*colDiffuse; */

    // Gamma correction
    finalColor = pow(finalColor, vec4(1.0/2.2));


    /* vec4 texelColor = texture(texture0, fragTexCoord); */
    vec4 maskColor = texture(mask, fragTexCoord);

    /* finalColor = texelColor; */
    
    /* finalColor = vec4(dithermono(gl_FragCoord.xy, texelColor, maskColor), 1.0); */
    finalColor = vec4(dithermono(gl_FragCoord.xy, finalColor, maskColor), 1.0);
}
