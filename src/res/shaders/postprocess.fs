#define PALETTE_SIZE 24
vec3 palette[PALETTE_SIZE] = vec3[](
  vec3(0.1211, 0.1406, 0.0391),
  vec3(0.2227, 0.3398, 0.1094),
  vec3(0.6445, 0.5469, 0.1523),
  vec3(0.9336, 0.6719, 0.1562),
  vec3(0.9336, 0.8438, 0.6289),
  vec3(0.6680, 0.3594, 0.1094),
  vec3(0.0938, 0.2461, 0.2227),
  vec3(0.9336, 0.4102, 0.1836),
  vec3(0.9336, 0.7148, 0.4570),
  vec3(0.6445, 0.3828, 0.2617),
  vec3(0.4648, 0.2031, 0.1289),
  vec3(0.4453, 0.2539, 0.0742),
  vec3(0.1641, 0.1133, 0.0508),
  vec3(0.2227, 0.1641, 0.1094),
  vec3(0.4062, 0.2969, 0.2344),
  vec3(0.5703, 0.4922, 0.4141),
  vec3(0.1523, 0.3906, 0.4062),
  vec3(0.9336, 0.2266, 0.0469),
  vec3(0.2695, 0.1367, 0.0508),
  vec3(0.2344, 0.6211, 0.6094),
  vec3(0.6055, 0.1016, 0.0391),
  vec3(0.2109, 0.0898, 0.0469),
  vec3(0.3320, 0.0586, 0.0391),
  vec3(0.1875, 0.0586, 0.0391)
);

  const float PHI = 1.61803398874989484820459; // Φ = Golden Ratio 

  float gold_noise(in vec2 xy, in float seed)
  {
    return fract(tan(distance(xy*PHI, xy)*seed)*xy.x);
  }
  
  float r_offset(in vec2 xy, in float seed)
  {
    return (gold_noise(xy, seed) - 0.5) * 0.3;
  }


void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    float seed = fract(iTime); 

    // Normalized pixel coordinates (from 0 to 1)
    fragCoord -= mod(fragCoord, 4.0);
    vec2 uv = fragCoord/iResolution.xy;

    // Time varying pixel color
    vec3 col = 0.5 + 0.5*cos(iTime+uv.xyx+vec3(0,2,4));

    float dist = 1000000000.0;
  //col.r *= gold_noise(fragCoord, seed+0.1);
  //col.g *= gold_noise(fragCoord, seed+0.2);
  //col.b *= gold_noise(fragCoord, seed+0.3);
    col.r += r_offset(fragCoord, seed+0.1);
    col.g += r_offset(fragCoord, seed+0.2);
    col.b += r_offset(fragCoord, seed+0.3);
    vec3 c = col;
    for (int i = 0; i < PALETTE_SIZE; i++) {
        float d = distance(col, palette[i]);
        if (d < dist) {
            c = palette[i];
            dist = d;
        };
    };
    col = c;
    // Output to screen
    fragColor = vec4(col,1.0);
    
}
