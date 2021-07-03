#version 330

/* vec3 wood[5]; */
/* wood[0] = vec3(0.3, 0.0, 0.0); */
/* wood[1] = vec3(0.5, 0.1, 0.0); */
/* wood[2] = vec3(0.7, 0.3, 0.0); */
/* wood[3] = vec3(0.8, 0.5, 0.0); */

vec3 wood[4] = vec3[](
vec3(0.3, 0.0, 0.0),
vec3(0.5, 0.1, 0.0),
vec3(0.7, 0.3, 0.0),
vec3(0.8, 0.5, 0.0)
);


float luma(vec3 color) {
  return dot(color, vec3(0.299, 0.587, 0.114));
}
float luma(vec4 color) {
  return dot(color.rgb, vec3(0.299, 0.587, 0.114));
}

vec3 dithermono(vec2 pos, vec4 col) {
    /* vec3 palette[4]; */
    /* palette[0] = vec3(0.3, 0.0, 0.0); */
    /* palette[1] = vec3(0.5, 0.1, 0.0); */
    /* palette[2] = vec3(0.7, 0.3, 0.0); */
    /* palette[3] = vec3(0.8, 0.5, 0.0); */

    vec3 palette[] = wood;

    
    float bands = palette.length();
    float bri = luma(col);
	float x = floor(mod(pos.x, 4.0));
	float y = floor(mod(pos.y, 4.0));
	float index = floor(x + y * 4.0);
	float limit = 0.0;
	float stepp = 1.0 / bands;
	if(x < 8.0)
	{
		if (index == float(0))  { limit = 0.0625; }
		if (index == float(1))  { limit = 0.5625; }
		if (index == float(2))  { limit = 0.1875; }
		if (index == float(3))  { limit = 0.6875; }
		if (index == float(4))  { limit = 0.8125; }
		if (index == float(5))  { limit = 0.3125; }
		if (index == float(6))  { limit = 0.9375; }
		if (index == float(7))  { limit = 0.4375; }
		if (index == float(8))  { limit = 0.25;   }
		if (index == float(9))  { limit = 0.75;   }
		if (index == float(10)) { limit = 0.125;  }
		if (index == float(11)) { limit = 0.625;  }
		if (index == float(12)) { limit = 1.0;    }
		if (index == float(13)) { limit = 0.5;    }
		if (index == float(14)) { limit = 0.875;  }
		if (index == float(15)) { limit = 0.375;  } 
	}
	float a = bri - mod(bri,stepp);
	float b = bri - mod(bri,stepp)+stepp;
	limit = limit/bands + a;
	float _out = a;
	if (bri > limit) { _out = b; }
    /* return palette[4]; */
    return palette[int(floor(_out*bands*0.99))];
	/* return vec3(_out, _out, _out); */
}


in vec2 fragTexCoord;
in vec4 fragColor;

uniform sampler2D texture0;
uniform vec4 colDiffuse;

out vec4 finalColor;


void main()
{
    vec4 texelColor = texture(texture0, fragTexCoord);
    
    finalColor = vec4(dithermono(gl_FragCoord.xy, texelColor), 1.0);
}

