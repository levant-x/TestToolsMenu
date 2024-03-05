using System;

[Serializable]
public enum ToolType 
{
    Armor, Arrow, MeleeWeapon1H
}

[Serializable]
public enum ToolBonus 
{
    Healing, Strength, Stunning, Flight, Freeze, Blood, Poison, Flame, Magia, Cross, Slowing, Storm
}

[Serializable]
public struct Tool : IIdentifiable
{
    public string Key { get; set; }
    public ToolType Type { get; set; }
    public string TexturePath { get; set; }
    public ToolBonus Bonus { get; set; }
    public int BonusRate { get; set; }
}
