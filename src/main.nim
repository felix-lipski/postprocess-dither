import bindings
import pararules

let white = Color(r: 255, g: 255, b: 255, a: 255)
let blue  = Color(r: 24, g: 63, b: 57, a: 255)
let camera = init_camera()
let shader = init_shader()

let texMask = LoadTexture("res/textures/regions.png");

proc load_model(path: cstring): Model =
  let model = LoadModel(path);
  apply_shader(model, shader, texMask)
  return model

type Entity = object
  model: Model
  position: Vector3

let tree        = Entity(model: load_model("res/tree.obj"), position: Vector3(x: 4, z: -8))
let beams       = Entity(model: load_model("res/beams-smooth.obj"), position: Vector3(x: -4))
let boulder     = Entity(model: load_model("res/boulder.obj"), position: Vector3(x: -10))
let grass_plane = Entity(model: load_model("res/grass-plane.obj"), position: Vector3())
let corrugated1 = Entity(model: load_model("res/corrugated.obj"), position: Vector3(x: 0, z: -11))
let corrugated2 = Entity(model: load_model("res/corrugated.obj"), position: Vector3(x: 0, z: -13))
let mech        = Entity(model: load_model("res/mech.obj"), position: Vector3(x: 4))
let mech2       = Entity(model: load_model("res/mech2.obj"), position: Vector3(z: -4))
let house       = Entity(model: load_model("res/house.obj"), position: Vector3(x: -3, z: -8))
let man         = Entity(model: load_model("res/man.obj"), position: Vector3())
let skyModel    = add_sky()

let ents = @[
  man,
  beams,
  tree,
  grass_plane,
  boulder,
  corrugated1,
  corrugated2,
  mech,
  mech2,
  house
  ]

while not WindowShouldClose():
  UpdateCamera(unsafeAddr(camera))
  BeginDrawing()
  ClearBackground(blue)
  BeginMode3D(camera)
  for i in low(ents)..high(ents):
    let ent = ents[i]
    DrawModel(ent.model, ent.position, 1.0f, white)
    DrawModel(skyModel, Vector3(), 1.0f, white)
  EndMode3D()
  # DrawText("masked dithering", 10, 40, 20, Color(a:255))
  EndDrawing()

for i in low(ents)..high(ents):
    UnloadModel(ents[i].model)

UnloadShader(shader)
CloseWindow()
