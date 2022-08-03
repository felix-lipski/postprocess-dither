#include "raylib.h"
#include "raymath.h"
#define RLIGHTS_IMPLEMENTATION
#include "rlights.h"


int main(void)
{
    const int screenWidth = 1920;
    const int screenHeight = 1080;

    /* SetConfigFlags(FLAG_MSAA_4X_HINT); */
    InitWindow(1920, 1080, "masked dither");

    Camera camera = { 0 };
    camera.position = (Vector3){ 12.0f, 10.0f, 0.0f };
    /* camera.position = (Vector3){ 12.0f, 1.0f, 0.0f }; */
    camera.target = (Vector3){ 0.0f, 3.5f, 0.0f };  
    camera.up = (Vector3){ 0.0f, 1.0f, 0.0f };      
    camera.fovy = 50.0f;                           
    camera.projection = CAMERA_PERSPECTIVE;        
    
    Shader shader = LoadShader("res/base_lighting.vs", "lighting-mask.fs");
    shader.locs[SHADER_LOC_VECTOR_VIEW] = GetShaderLocation(shader, "viewPos");
    

    int ambientLoc = GetShaderLocation(shader, "ambient");
    /* SetShaderValue(shader, ambientLoc, (float[4]){ 0.1f, 0.1f, 0.1f, 1.0f }, SHADER_UNIFORM_VEC4); */
    SetShaderValue(shader, ambientLoc, (float[4]){ 0.4f, 0.4f, 0.4f, 1.0f }, SHADER_UNIFORM_VEC4);

    Texture texMask = LoadTexture("res/regions.png");
    shader.locs[SHADER_LOC_MAP_EMISSION] = GetShaderLocation(shader, "mask");

    Model tree = LoadModel("res/tree.obj");
    tree.materials[0].maps[MATERIAL_MAP_EMISSION].texture = texMask;
    tree.materials[0].shader = shader;

    Model beams = LoadModel("res/beams.obj");
    beams.materials[0].maps[MATERIAL_MAP_EMISSION].texture = texMask;
    beams.materials[0].shader = shader;

    Model boulder = LoadModel("res/boulder.obj");
    boulder.materials[0].maps[MATERIAL_MAP_EMISSION].texture = texMask;
    boulder.materials[0].shader = shader;

    Model grass_plane = LoadModel("res/grass-plane.obj");
    grass_plane.materials[0].maps[MATERIAL_MAP_EMISSION].texture = texMask;
    grass_plane.materials[0].shader = shader;


    Light lights[1] = { 0 };
    lights[0] = CreateLight(LIGHT_POINT, (Vector3){ 100, 100, 100 }, Vector3Zero(), WHITE, shader);

    SetCameraMode(camera, CAMERA_ORBITAL);
    /* SetCameraMode(camera, CAMERA_FIRST_PERSON); */
    SetTargetFPS(60);                       

    while (!WindowShouldClose()) {
        UpdateCamera(&camera);              
        UpdateLightValues(shader, lights[0]);
        float cameraPos[3] = { camera.position.x, camera.position.y, camera.position.z };
        SetShaderValue(shader, shader.locs[SHADER_LOC_VECTOR_VIEW], cameraPos, SHADER_UNIFORM_VEC3);
        BeginDrawing();
            ClearBackground(RAYWHITE);
            BeginMode3D(camera);
                DrawModel(tree, Vector3Zero(), 1.0f, WHITE);
                DrawModel(beams, (Vector3){3,0,3}, 1.0f, WHITE);
                DrawModel(boulder, (Vector3){-5,0,-4}, 1.0f, WHITE);
                DrawModel(grass_plane, (Vector3){-5,0,-4}, 1.0f, WHITE);
            EndMode3D();
            DrawFPS(10, 10);
            DrawText("Use keys [Y][R][G][B] to toggle lights", 10, 40, 20, DARKGRAY);
        EndDrawing();
    }
    UnloadModel(tree);   
    UnloadModel(beams);    
    UnloadModel(boulder);    
    UnloadModel(grass_plane);    
    UnloadShader(shader); 
    CloseWindow();        
    return 0;
}


