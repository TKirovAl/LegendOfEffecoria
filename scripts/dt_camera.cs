private bool isDragging = false;
private Vector2 dragStart;

public override void _Input(InputEvent @event)
{
    if (@event is InputEventMouseButton mouseButton)
    {
        // Начало перетаскивания при нажатии левой кнопки мыши
        if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
        {
            isDragging = true;
            dragStart = GetViewport().GetMousePosition();
        }

        // Окончание перетаскивания при отпускании кнопки
        if (mouseButton.ButtonIndex == MouseButton.Left && !mouseButton.Pressed)
        {
            isDragging = false;
        }
    }

    if (@event is InputEventMouseMotion mouseMotion && isDragging)
    {
        // Перемещаем камеру в зависимости от движения мыши
        Vector2 dragEnd = GetViewport().GetMousePosition();
        Vector2 delta = dragEnd - dragStart;

        camera.Position -= delta * camera.Zoom;
        dragStart = dragEnd;
    }
}
