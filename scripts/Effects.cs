using Godot;
using System;

public class Effect
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Duration { get; private set; }

    public Effect(string name, string description, int duration)
    {
        Name = name;
        Description = description;
        Duration = duration;
    }

    public void Apply(Monster target)
    {
        GD.Print($"Применяется эффект {Name}: {Description} на {target.Name}");
    }
}