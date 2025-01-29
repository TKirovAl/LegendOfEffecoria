using Godot;

public class CameraController : Node2D
{
    [Export] private NodePath spritePath;
    private Sprite2D mapSprite;
    private Camera2D camera;

    private const float MinZoom = 0.5f;
    private const float MaxZoom = 3.0f;

    private bool isDragging = false;
    private Vector2 dragStart;

    public override void _Ready()
    {
        mapSprite = GetNode<Sprite2D>(spritePath);
        camera = GetViewport().GetCamera2D();

        if (camera == null)
        {
            GD.PrintErr("Camera2D not found! Make sure you have a Camera2D node in the scene.");
        }
    }

    public override void _Process(double delta)
    {
        if (camera == null || mapSprite == null) return;

        // Получаем размеры спрайта и камеры
        Vector2 spriteSize = mapSprite.Texture.GetSize();
        Vector2 viewportSize = GetViewportRect().Size;

        // Вычисляем границы для камеры
        float minX = viewportSize.x / 2;
        float maxX = spriteSize.x - viewportSize.x / 2;
        float minY = viewportSize.y / 2;
        float maxY = spriteSize.y - viewportSize.y / 2;

        // Ограничиваем позицию камеры
        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(camera.Position.x, minX, maxX),
            Mathf.Clamp(camera.Position.y, minY, maxY)
        );

        camera.Position = clampedPosition;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMagnifyGesture magnifyGesture)
        {
            // Изменяем масштаб камеры
            float zoomFactor = magnifyGesture.Factor;
            camera.Zoom *= zoomFactor;

            // Ограничиваем масштаб
            camera.Zoom = new Vector2(
                Mathf.Clamp(camera.Zoom.x, MinZoom, MaxZoom),
                Mathf.Clamp(camera.Zoom.y, MinZoom, MaxZoom)
            );
        }

        if (@event is InputEventPanGesture panGesture)
        {
            // Двигаем камеру в противоположную сторону от жеста
            camera.Position -= panGesture.Delta * camera.Zoom;
        }

        if (@event is InputEventMouseButton mouseButton)
        {
            GD.Print($"Mouse button event: ButtonIndex={mouseButton.ButtonIndex}, Pressed={mouseButton.Pressed}");
            if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
            {
                isDragging = true;
                dragStart = GetViewport().GetMousePosition();
            }

            if (mouseButton.ButtonIndex == MouseButton.Left && !mouseButton.Pressed)
            {
                isDragging = false;
            }
        }

        if (@event is InputEventMouseMotion mouseMotion && isDragging)
        {
            GD.Print($"Mouse motion event: Delta={mouseMotion.Relative}");
            Vector2 dragEnd = GetViewport().GetMousePosition();
            Vector2 delta = dragEnd - dragStart;

            camera.Position -= delta * camera.Zoom;
            dragStart = dragEnd;
        }
    }
}