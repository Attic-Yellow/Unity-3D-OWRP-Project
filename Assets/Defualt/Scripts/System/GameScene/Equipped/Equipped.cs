using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipped : MonoBehaviour
{
    public static Equipped Instance;

    public delegate void OnGearChanged();
    public OnGearChanged onChangeGear;

    public List<Equipment> weapon = new List<Equipment>();
    public List<Equipment> head = new List<Equipment>();
    public List<Equipment> body = new List<Equipment>();
    public List<Equipment> hands = new List<Equipment>();
    public List<Equipment> legs = new List<Equipment>();
    public List<Equipment> feet = new List<Equipment>();
    public List<Equipment> auxiliary = new List<Equipment>();
    public List<Equipment> earring = new List<Equipment>();
    public List<Equipment> necklace = new List<Equipment>();
    public List<Equipment> bracelet = new List<Equipment>();
    public List<Equipment> ring = new List<Equipment>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public bool AddWeapon(Equipment equipment)
    {
        if (weapon.Count < 30)
        {
            weapon.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddHead(Equipment equipment)
    {
        if (head.Count < 30)
        {
            head.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddBody(Equipment equipment)
    {
        if (body.Count < 30)
        {
            body.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddHands(Equipment equipment)
    {
        if (hands.Count < 30)
        {
            hands.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddLegs(Equipment equipment)
    {
        if (legs.Count < 30)
        {
            legs.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddFeet(Equipment equipment)
    {
        if (feet.Count < 30)
        {
            feet.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddAuxiliary(Equipment equipment)
    {
        if (auxiliary.Count < 30)
        {
            auxiliary.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddEarring(Equipment equipment)
    {
        if (earring.Count < 30)
        {
            earring.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }
        
        return false;
    }

    public bool AddNecklace(Equipment equipment)
    {
        if (necklace.Count < 30)
        {
            necklace.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddBracelet(Equipment equipment)
    {
        if (bracelet.Count < 30)
        {
            bracelet.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddRing(Equipment equipment)
    {
        if (ring.Count < 30)
        {
            ring.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }
}
