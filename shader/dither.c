// This is a modified example from the raylib library. The original example can be found in examples/shaders/shaders_postprocessing.c
#include "raylib.h"

#if defined(PLATFORM_DESKTOP)
    #define GLSL_VERSION            330
#else   // PLATFORM_RPI, PLATFORM_ANDROID, PLATFORM_WEB
    #define GLSL_VERSION            100
#endif

#define MAX_POSTPRO_SHADERS         12

typedef enum {
    /* FX_GRAYSCALE = 0, */
    FX_DITHER,
} PostproShader;

static const char *postproShaderText[] = {
    "DITHER",
};

int main(void)
{
    // Initialization
    //--------------------------------------------------------------------------------------
    const int screenWidth = 800;
    const int screenHeight = 450;

    SetConfigFlags(FLAG_MSAA_4X_HINT);      // Enable Multi Sampling Anti Aliasing 4x (if available)

    InitWindow(screenWidth, screenHeight, "raylib [shaders] example - postprocessing shader");

    // Define the camera to look into our 3d world
    Camera camera = { { 2.0f, 3.0f, 2.0f }, { 0.0f, 1.0f, 0.0f }, { 0.0f, 1.0f, 0.0f }, 45.0f, 0 };

    Model model = LoadModel("resources/models/church.obj");                 // Load OBJ model
    Texture2D texture = LoadTexture("resources/models/church_diffuse.png"); // Load model texture (diffuse map)
    model.materials[0].maps[MATERIAL_MAP_DIFFUSE].texture = texture;                     // Set model diffuse texture

    /* Model robot01_model = LoadModel("resources/models/church.obj"); */
    Model robot01_model = LoadModel("res/robot01.obj");
    Texture2D robot01_texture = LoadTexture("resources/models/church_diffuse.png");
    robot01_model.materials[0].maps[MATERIAL_MAP_DIFFUSE].texture = robot01_texture;

    Vector3 position = { -0.5f, 0.0f, -0.5f };                             // Set model position
    Vector3 position2 = { 0.5f, 0.0f, 0.5f };                             // Set model position

    // Load all postpro shaders
    // NOTE 1: All postpro shader use the base vertex shader (DEFAULT_VERTEX_SHADER)
    // NOTE 2: We load the correct shader depending on GLSL version
    Shader shaders[MAX_POSTPRO_SHADERS] = { 0 };

    // NOTE: Defining 0 (NULL) for vertex shader forces usage of internal default vertex shader
    /* shaders[FX_GRAYSCALE] = LoadShader(0, TextFormat("resources/shaders/glsl%i/grayscale.fs", GLSL_VERSION)); */
    shaders[FX_DITHER] = LoadShader(0, TextFormat("dither.fs"));

    int currentShader = FX_DITHER;

    // Create a RenderTexture2D to be used for render to texture
    RenderTexture2D target = LoadRenderTexture(screenWidth, screenHeight);
    /* RenderTexture2D target2 = LoadRenderTexture(screenWidth, screenHeight); */

    // Setup orbital camera
    SetCameraMode(camera, CAMERA_ORBITAL);  // Set an orbital camera mode

    SetTargetFPS(60);                       // Set our game to run at 60 frames-per-second
    //--------------------------------------------------------------------------------------

    // Main game loop
    while (!WindowShouldClose())            // Detect window close button or ESC key
    {
        // Update
        //----------------------------------------------------------------------------------
        UpdateCamera(&camera);              // Update camera

        if (IsKeyPressed(KEY_RIGHT)) currentShader++;
        else if (IsKeyPressed(KEY_LEFT)) currentShader--;

        if (currentShader >= MAX_POSTPRO_SHADERS) currentShader = 0;
        else if (currentShader < 0) currentShader = MAX_POSTPRO_SHADERS - 1;
        //----------------------------------------------------------------------------------

        // Draw
        //----------------------------------------------------------------------------------
        BeginDrawing();

            ClearBackground(RAYWHITE);

            BeginTextureMode(target);       // Enable drawing to texture

                ClearBackground(RAYWHITE);  // Clear texture background

                BeginMode3D(camera);        // Begin 3d mode drawing

                    DrawModel(model, position, 0.1f, WHITE);   // Draw 3d model with texture

                    DrawModel(robot01_model, position2, 0.4f, WHITE);   // Draw 3d model with texture

                    DrawGrid(10, 1.0f);     // Draw a grid

                EndMode3D();                // End 3d mode drawing, returns to orthographic 2d mode

            EndTextureMode();               // End drawing to texture (now we have a texture available for next passes)

            // Render previously generated texture using selected postpro shader
            BeginShaderMode(shaders[currentShader]);

                // NOTE: Render texture must be y-flipped due to default OpenGL coordinates (left-bottom)
                DrawTextureRec(target.texture, (Rectangle){ 0, 0, (float)target.texture.width/2.0, (float)-target.texture.height }, (Vector2){ 0, 0 }, WHITE);

            EndShaderMode();

            // Draw 2d shapes and text over drawn texture
            DrawRectangle(0, 9, 580, 30, Fade(LIGHTGRAY, 0.7f));

            DrawText("(c) Church 3D model by Alberto Cano", screenWidth - 200, screenHeight - 20, 10, GRAY);

            DrawText("CURRENT POSTPRO SHADER:", 10, 15, 20, BLACK);
            DrawText(postproShaderText[currentShader], 330, 15, 20, RED);
            DrawText("< >", 540, 10, 30, DARKBLUE);

            DrawFPS(700, 15);

        EndDrawing();
        //----------------------------------------------------------------------------------
    }

    // De-Initialization
    //--------------------------------------------------------------------------------------

    // Unload all postpro shaders
    for (int i = 0; i < MAX_POSTPRO_SHADERS; i++) UnloadShader(shaders[i]);

    UnloadTexture(texture);         // Unload texture
    UnloadTexture(robot01_texture);         // Unload texture
    UnloadModel(model);             // Unload model
    UnloadModel(robot01_model);             // Unload model
    UnloadRenderTexture(target);    // Unload render texture

    CloseWindow();                  // Close window and OpenGL context
    //--------------------------------------------------------------------------------------

    return 0;
}
