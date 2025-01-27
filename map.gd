extends Node2D

var camera : Camera2D
var map_size = Vector2(1920, 1080) # Укажите размер вашей карты
var margin = 64 # Граница для предотвращения выхода за пределы карты
var touch_ids = []
var initial_distance = 0.0
var initial_camera_zoom = Vector2()

func zoom_in():
	camera.zoom /= 1.1
	update_camera_limits()
	
func zoom_out():
	camera.zoom *= 1.1
	update_camera_limits()

func _ready():
	camera = $Camera2D
	update_camera_limits()

func update_camera_limits():
	var screen_size = get_viewport_rect().size
	var min_limit = (screen_size / 2) + Vector2(margin, margin)
	var max_limit = map_size - (screen_size / 2) - Vector2(margin, margin)
	
	camera.limit_left = -min_limit.x
	camera.limit_right = max_limit.x
	camera.limit_top = -min_limit.y
	camera.limit_bottom = max_limit.y

func _input(event):
	if event is InputEventScreenTouch:
		handle_touch_event(event)
	elif event is InputEventScreenDrag:
		handle_drag_event(event)
	elif event is InputEventMouseButton and event.button_index == MOUSE_BUTTON_WHEEL_UP:
		zoom_in()
	elif event is InputEventMouseButton and event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
		zoom_out()

func handle_touch_event(event: InputEventScreenTouch):
	if event.pressed:
		if touch_ids.size() < 2:
			touch_ids.append(event.index)
			if touch_ids.size() == 2:
				initial_distance = get_touch_distance()
				initial_camera_zoom = camera.zoom
	else:
		if touch_ids.has(event.index):
			touch_ids.erase(event.index)
			if touch_ids.is_empty():
				initial_distance = 0.0

func handle_drag_event(event: InputEventScreenDrag):
	if touch_ids.size() == 1:
		camera.position -= event.relative / camera.zoom

func get_touch_distance() -> float:
	if touch_ids.size() != 2:
		return 0.0
	
	var pos1 = get_touch_position(touch_ids[0])
	var pos2 = get_touch_position(touch_ids[1])
	return pos1.distance_to(pos2)

func get_touch_position(index: int) -> Vector2:
	for i in range(Input.get_last_events().size()):
		var event = Input.get_last_events()[i]
		if event is InputEventScreenTouch and event.index == index:
			return event.position
	return Vector2()

func _process(delta):
	if touch_ids.size() == 2:
		var current_distance = get_touch_distance()
		if current_distance > 0.0 and initial_distance > 0.0:
			var scale = current_distance / initial_distance
			camera.zoom = initial_camera_zoom / Vector2(scale, scale)
			update_camera_limits()
