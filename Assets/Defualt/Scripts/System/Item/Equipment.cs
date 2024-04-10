using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    None,
    Weapon,
    Head,
    Body,
    Hands,
    Legs,
    Feet,
    Auxiliary,
    Earring,
    Necklace,
    Bracelet,
    Ring
}

public enum GradeType
{
    None,
    Normal,
    UnRare,
    Rare,
    Unique
}

[Serializable]
public class Equipment : Item
{
    public EquipmentType equipment;
    public GradeType grede; 
    public int str;
    public int _int;
    public int dex;
    public int spi;
    public int vit;
    public int luk;
    public int crt;
    public int dh;
    public int det;
    public int def;
    public int mef;
    public int sks;
    public int sps;
    public int ten;
    public int pie;
}
