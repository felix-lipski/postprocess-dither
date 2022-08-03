#include "raylib.h"
#include "raymath.h"


int main(void) {
    const int screenWidth = 800;
    const int screenHeight = 450;

    InitWindow(screenWidth, screenHeight, "raylib - simple shader mask");

    // Define the camera to look into our 3d world
    Camera camera = { 0 };
    camera.position = (Vector3){ 0.0f, 1.0f, 2.0f };
    camera.target = (Vector3){ 0.0f, 0.0f, 0.0f };
    camera.up = (Vector3){ 0.0f, 1.0f, 0.0f };
    camera.fovy = 45.0f;
    camera.projection = CAMERA_PERSPECTIVE;

    // Define our three models to show the shader on
    Mesh torus = GenMeshTorus(0.3f, 1, 16, 32);
    /* Model model1 = LoadModelFromMesh(torus); */

    Mesh cube = GenMeshCube(0.8f,0.8f,0.8f);
    Model model2 = LoadModelFromMesh(cube);

    // Generate model to be shaded just to see the gaps in the other two
    Mesh sphere = GenMeshSphere(1, 16, 16);
    Model model3 = LoadModelFromMesh(sphere);

    Model model1 = LoadModel("res/boiler.obj");                 // Load OBJ model
    // Load the shader
    /* Shader shader = LoadShader(0, TextFormat("resources/shaders/glsl%i/mask.fs", GLSL_VERSION)); */
    Shader shader = LoadShader(0, "mask-dither.fs");

    // Load and apply the diffuse texture (colour map)
    Texture texDiffuse = LoadTexture("res/plasma.png");
    /* Texture texDiffuse = LoadTexture("res/checker_1.png"); */
    model1.materials[0].maps[MATERIAL_MAP_DIFFUSE].texture = texDiffuse;
    model2.materials[0].maps[MATERIAL_MAP_DIFFUSE].texture = texDiffuse;

    // Using MATERIAL_MAP_EMISSION as a spare slot to use for 2nd texture
    // NOTE: Don't use MATERIAL_MAP_IRRADIANCE, MATERIAL_MAP_PREFILTER or  MATERIAL_MAP_CUBEMAP as they are bound as cube maps
    /* Texture texMask = LoadTexture("resources/mask.png"); */
    Texture texMask = LoadTexture("res/boiler-color.png");
    model1.materials[0].maps[MATERIAL_MAP_EMISSION].texture = texMask;
    model2.materials[0].maps[MATERIAL_MAP_EMISSION].texture = texMask;
    shader.locs[SHADER_LOC_MAP_EMISSION] = GetShaderLocation(shader, "mask");

    /* // Frame is incremented each frame to animate the shader */
    /* int shaderFrame = GetShaderLocation(shader, "frame"); */

    // Apply the shader to the two models
    model1.materials[0].shader = shader;
    model2.materials[0].shader = shader;

    int framesCounter = 0;
    Vector3 rotation = { 0 };

    SetTargetFPS(60);

    while (!WindowShouldClose()) {

        framesCounter++;
        rotation.x += 0.01f;
        rotation.y += 0.005f;
        rotation.z -= 0.0025f;

        /* // Send frames counter to shader for animation */
        /* SetShaderValue(shader, shaderFrame, &framesCounter, SHADER_UNIFORM_INT); */

        // Rotate one of the models
        model1.transform = MatrixRotateXYZ(rotation);

        UpdateCamera(&camera);
        BeginDrawing();

            ClearBackground(DARKBLUE);

            BeginMode3D(camera);

                DrawModel(model1, (Vector3){0.5,0,0}, 1, WHITE);
                DrawModelEx(model2, (Vector3){-.5,0,0}, (Vector3){1,1,0}, 50, (Vector3){1,1,1}, WHITE);
                DrawModel(model3,(Vector3){0,0,-1.5}, 1, WHITE);
                DrawGrid(10, 1.0f);        // Draw a grid

            EndMode3D();

            DrawRectangle(16, 698, MeasureText(TextFormat("Frame: %i", framesCounter), 20) + 8, 42, BLUE);
            DrawText(TextFormat("Frame: %i", framesCounter), 20, 700, 20, WHITE);

            DrawFPS(10, 10);

        EndDrawing();
    }

    UnloadModel(model1);
    UnloadModel(model2);
    UnloadModel(model3);
    UnloadTexture(texDiffuse);
    UnloadTexture(texMask);
    UnloadShader(shader);
    CloseWindow();
    return 0;
}
