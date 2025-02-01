public static class EntitySpawner
{
    private static List<Entity> EntityList = new List<Entity>();

    public static void InitializeEntitys()
    {
        var burningEffect = new Effect("Горение", "Наносит 5 урона каждый ход.", 3);
        var poisonEffect = new Effect("Яд", "Наносит 3 урона каждый ход.", 4);

        var wolf = new Entity("Волк", 50, 10, 0, new List<Effect> { burningEffect });
        var vivern = new Entity("Виверна", 300, 50, 25);
        var bargest = new Entity("Баргест", 150, 70, 0);
        var gidra = new Entity("Гидра", 100000, 50, 0);

        var outlaw = new Entity("Разбойник", 100, 20, 10, new List<Effect> { poisonEffect });
        var warrior_of_nfrcturia = new Entity("Воин Северной Фрактурии", 100, 20, 30);
       
		var shapeshifter = new Entity("Оборотень", 300, 30, 0, new List<Effect> { poisonEffect });
		var aniuka = new Entity("Аниука", 30, 10, 0);
    

        EntityList.Add(wolf);
        EntityList.Add(outlaw);
        EntityList.Add(vivern);
		EntityList.Add(shapeshifter);
		EntityList.Add(warrior_of_nfrcturia);
        EntityList.Add(aniuka);
        EntityList.Add(bargest);
        EntityList.Add(gidra);
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