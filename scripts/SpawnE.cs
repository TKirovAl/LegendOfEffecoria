public static class EntitySpawner
{
    private static List<Entity> EntityList = new List<Entity>();

    public static void InitializeEntitys()
    {
        var burningEffect = new Effect("Горение", "Наносит 5 урона каждый ход.", 3);
        var poisonEffect = new Effect("Яд", "Наносит 3 урона каждый ход.", 4);

        var wolf = new Entity("Волк", 50, 10, 5, new List<Effect> { burningEffect });
        var outlaw = new Entity("Разбойник", 100, 20, 10, new List<Effect> { poisonEffect });
        var vivern = new Entity("Вивера", 300, 50, 25);

        EntityList.Add(goblin);
        EntityList.Add(orc);
        EntityList.Add(dragon);
    }

    public static Entity SpawnRandomEntity()
    {
        if (EntityList.Count == 0)
        {
            GD.Print("Список монстров пуст.");
            return null;
        }

        int randomIndex = new Random().Next(EntityList.Count);
        return EntityList[randomIndex];
    }
}