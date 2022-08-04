import bindings

let white = Color(r: 255, g: 255, b: 255, a: 255)
let camera = init_camera()
let shader = init_shader()

let texMask = LoadTexture("res/regions.png");

proc load_model(path: cstring): Model =
  let model = LoadModel(path);
  apply_shader(model, shader, texMask)
  return model

type Entity = object
  model: Model
  position: Vector3

let tree        = Entity(model: load_model("res/tree.obj"), position: Vector3())
let beams       = Entity(model: load_model("res/beams.obj"), position: Vector3(z: 8))
let boulder     = Entity(model: load_model("res/boulder.obj"), position: Vector3(x: -4, z: -8))
let grass_plane = Entity(model: load_model("res/grass-plane.obj"), position: Vector3())

let ents = @[beams, tree, grass_plane, boulder]

while not WindowShouldClose():
  UpdateCamera(unsafeAddr(camera))
  BeginDrawing()
  ClearBackground(white)
  BeginMode3D(camera)
  for i in low(ents)..high(ents):
    let ent = ents[i]
    DrawModel(ent.model, ent.position, 1.0f, white)
  EndMode3D()
  # DrawText("masked dithering", 10, 40, 20, Color(a:255))
  EndDrawing()

for i in low(ents)..high(ents):
    UnloadModel(ents[i].model)

UnloadShader(shader)
CloseWindow()
