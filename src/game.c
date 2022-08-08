#include "raylib.h"
#include "raymath.h"

Camera init_camera() {
    InitWindow(1920, 1080, "masked dither");

    Camera camera = { 0 };
    /* camera.position = (Vector3){ 12.0f, 10.0f, 0.0f }; */
    camera.position = (Vector3){ 12.0f, 1.8f, 0.0f };
    camera.target = (Vector3){ 0.0f, 3.5f, 0.0f };  
    camera.up = (Vector3){ 0.0f, 1.0f, 0.0f };      
    camera.fovy = 50.0f;                           
    camera.projection = CAMERA_PERSPECTIVE;        

    /* SetCameraMode(camera, CAMERA_ORBITAL); */
    SetCameraMode(camera, CAMERA_FIRST_PERSON);
    SetTargetFPS(60);                       
    return camera;
}

Shader init_shader() {
    Shader shader = LoadShader("res/shaders/base_lighting.vs", "res/shaders/mask-light-dither.fs");
    shader.locs[SHADER_LOC_VECTOR_VIEW] = GetShaderLocation(shader, "viewPos");
    shader.locs[SHADER_LOC_MAP_EMISSION] = GetShaderLocation(shader, "mask");

    return shader;
}

Model add_sky() {
    Shader shader = LoadShader("res/shaders/base_lighting.vs", "res/shaders/sky.fs");
    shader.locs[SHADER_LOC_VECTOR_VIEW] = GetShaderLocation(shader, "viewPos");
    
    Texture texMask = LoadTexture("res/textures/regions.png");
    shader.locs[SHADER_LOC_MAP_EMISSION] = GetShaderLocation(shader, "mask");

    Model sky = LoadModel("res/sky.obj");
    sky.materials[0].maps[MATERIAL_MAP_EMISSION].texture = texMask;
    sky.materials[0].shader = shader;

    return sky;
}

void apply_shader(Model model, Shader shader, Texture2D texMask) {
    model.materials[0].maps[MATERIAL_MAP_EMISSION].texture = texMask;
    model.materials[0].shader = shader;
}

int main2(void) {
    InitWindow(1920, 1080, "masked dither");

    Camera camera = { 0 };
    camera.position = (Vector3){ 12.0f, 10.0f, 0.0f };
    camera.target = (Vector3){ 0.0f, 3.5f, 0.0f };  
    camera.up = (Vector3){ 0.0f, 1.0f, 0.0f };      
    camera.fovy = 50.0f;                           
    camera.projection = CAMERA_PERSPECTIVE;        
    
    Shader shader = LoadShader("res/base_lighting.vs", "lighting-mask.fs");
    shader.locs[SHADER_LOC_VECTOR_VIEW] = GetShaderLocation(shader, "viewPos");
    
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

    SetCameraMode(camera, CAMERA_ORBITAL);
    /* SetCameraMode(camera, CAMERA_FIRST_PERSON); */
    SetTargetFPS(60);                       

    while (!WindowShouldClose()) {
        UpdateCamera(&camera);              
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
