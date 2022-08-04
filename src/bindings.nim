
type Color* {.importc, header: "raylib.h".} = object
  r*: cint
  g*: cint
  b*: cint
  a*: cint

type Vector3* {.importc, header: "raylib.h".} = object
  x*: cfloat
  y*: cfloat
  z*: cfloat

type Shader* {.importc, header: "raylib.h".} = object
type Camera* {.importc, header: "raylib.h".} = object
type Texture2D* {.importc, header: "raylib.h".} = object
type Material* {.importc, header: "raylib.h".} = object
type Model* {.importc, header: "raylib.h".} = object

proc WindowShouldClose*(): bool {.importc, header: "raylib.h".}
proc CloseWindow*() {.importc, header: "raylib.h".}
proc BeginDrawing*() {.importc, header: "raylib.h".}
proc EndDrawing*() {.importc, header: "raylib.h".}
proc UpdateCamera*(camera: pointer) {.importc, header: "raylib.h".}

proc LoadTexture*(path: cstring): Texture2D {.importc, header: "raylib.h".}
proc LoadShader*(vertex, fragment: cstring): Shader {.importc, header: "raylib.h".}
proc UnloadShader*(shader: Shader) {.importc, header: "raylib.h".}
proc LoadModel*(path: cstring): Model {.importc, header: "raylib.h".}
proc UnloadModel*(model: Model) {.importc, header: "raylib.h".}

proc BeginMode3D*(camera: Camera) {.importc, header: "raylib.h".}
proc EndMode3D*() {.importc, header: "raylib.h".}

proc ClearBackground*(color: Color) {.importc, header: "raylib.h".}
proc DrawText*(text: cstring, x, y, z: cint, color: Color) {.importc, header: "raylib.h".}
proc DrawModel*(model: Model, pos: Vector3, scale: cfloat, tint: Color) {.importc, header: "raylib.h".}

proc init_camera*(): Camera {.importc.}
proc init_shader*(): Shader {.importc.}
proc apply_shader*(model: Model, shader: Shader, texMask: Texture2D) {.importc.}
