using Godot;

public class CameraMap : Node2D
{
    [Export] private NodePath mapSpritePath; // Путь к спрайту карты
    [Export] private float minZoom = 0.5f;  // Минимальный зум
    [Export] private float maxZoom = 3.0f;  // Максимальный зум

    private Sprite2D mapSprite;             // Спрайт карты
    private Camera2D camera;                // Камера
    private bool isDragging = false;        // Флаг перетаскивания
    private Vector2 dragStart;              // Начальная позиция перетаскивания

    public override void _Ready()
    {
        // Инициализация спрайта карты
        mapSprite = GetNode<Sprite2D>(mapSpritePath);
        if (mapSprite == null)
        {
            GD.PrintErr("Map sprite not found! Check the mapSpritePath.");
            return;
        }

        // Инициализация камеры
        camera = GetViewport().GetCamera2D();
        if (camera == null)
        {
            GD.PrintErr("Camera2D not found! Make sure you have a Camera2D node in the scene.");
            return;
        }
    }

    public override void _Process(double delta)
    {
        if (camera == null || mapSprite == null) return;

        // Ограничение позиции камеры в рамках карты
        ClampCameraPosition();
    }

    public override void _Input(InputEvent @event)
    {
        // Обработка масштабирования (зума)
        if (@event is InputEventMagnifyGesture magnifyGesture)
        {
            HandleZoom(magnifyGesture.Factor);
        }

        // Обработка перемещения с помощью жестов
        if (@event is InputEventPanGesture panGesture)
        {
            HandlePanGesture(panGesture.Delta);
        }

        // Обработка нажатия кнопки мыши для начала перетаскивания
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
            {
                StartDragging(mouseButton.Position);
            }
            else if (mouseButton.ButtonIndex == MouseButton.Left && !mouseButton.Pressed)
            {
                StopDragging();
            }
        }

        // Обработка движения мыши при перетаскивании
        if (@event is InputEventMouseMotion mouseMotion && isDragging)
        {
            HandleDragging(mouseMotion.Position);
        }
    }

    private void HandleZoom(float zoomFactor)
    {
        // Изменяем масштаб камеры
        camera.Zoom *= zoomFactor;

        // Ограничиваем масштаб
        camera.Zoom = new Vector2(
            Mathf.Clamp(camera.Zoom.x, minZoom, maxZoom),
            Mathf.Clamp(camera.Zoom.y, minZoom, maxZoom)
        );
    }

    private void HandlePanGesture(Vector2 delta)
    {
        // Двигаем камеру в противоположную сторону от жеста
        camera.Position -= delta * camera.Zoom;
    }

    private void StartDragging(Vector2 position)
    {
        isDragging = true;
        dragStart = position;
    }

    private void StopDragging()
    {
        isDragging = false;
    }

    private void HandleDragging(Vector2 currentPosition)
    {
        // Вычисляем разницу между текущей и начальной позицией
        Vector2 delta = currentPosition - dragStart;

        // Перемещаем камеру с учетом масштаба
        camera.Position -= delta / camera.Zoom;

        // Обновляем начальную позицию
        dragStart = currentPosition;
    }

    private void ClampCameraPosition()
    {
        // Получаем размеры карты и видового окна
        Vector2 mapSize = mapSprite.Texture.GetSize();
        Vector2 viewportSize = GetViewportRect().Size / camera.Zoom;

        // Вычисляем границы для камеры
        float minX = viewportSize.x / 2;
        float maxX = mapSize.x - viewportSize.x / 2;
        float minY = viewportSize.y / 2;
        float maxY = mapSize.y - viewportSize.y / 2;

        // Ограничиваем позицию камеры
        camera.Position = new Vector2(
            Mathf.Clamp(camera.Position.x, minX, maxX),
            Mathf.Clamp(camera.Position.y, minY, maxY)
        );
    }
}