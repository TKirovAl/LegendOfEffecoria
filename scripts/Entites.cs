using Godot;
using System;
using System.Collections.Generic;

public class Entity : Node
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public List<Effect> Effects { get; private set; }

    public Entity(string name, int health, int attack, int defense, List<Effect> effects = null)
    {
        Name = name;
        Health = health;
        Attack = attack;
        Defense = defense;
        Effects = effects ?? new List<Effect>();
    }

    public void ApplyEffects(Monster target)
    {
        foreach (var effect in Effects)
        {
            effect.Apply(target);
        }
    }

    public override string ToString()
    {
        return $"{Name} (HP: {Health}, Attack: {Attack}, Defense: {Defense})";
    }
}