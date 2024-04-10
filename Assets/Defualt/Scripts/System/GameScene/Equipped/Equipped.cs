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
        if (weapon.Count < 30 && equipment.equipment == EquipmentType.Weapon)
        {
            weapon.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddHead(Equipment equipment)
    {
        if (head.Count < 30 && equipment.equipment == EquipmentType.Head)
        {
            head.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddBody(Equipment equipment)
    {
        if (body.Count < 30 && equipment.equipment == EquipmentType.Body)
        {
            body.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddHands(Equipment equipment)
    {
        if (hands.Count < 30 && equipment.equipment == EquipmentType.Hands)
        {
            hands.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddLegs(Equipment equipment)
    {
        if (legs.Count < 30 && equipment.equipment == EquipmentType.Legs)
        {
            legs.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddFeet(Equipment equipment)
    {
        if (feet.Count < 30 && equipment.equipment == EquipmentType.Feet)
        {
            feet.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddAuxiliary(Equipment equipment)
    {
        if (auxiliary.Count < 30 && equipment.equipment == EquipmentType.Auxiliary)
        {
            auxiliary.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddEarring(Equipment equipment)
    {
        if (earring.Count < 30 && equipment.equipment == EquipmentType.Earring)
        {
            earring.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }
        
        return false;
    }

    public bool AddNecklace(Equipment equipment)
    {
        if (necklace.Count < 30 && equipment.equipment == EquipmentType.Necklace)
        {
            necklace.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddBracelet(Equipment equipment)
    {
        if (bracelet.Count < 30 && equipment.equipment == EquipmentType.Bracelet)
        {
            bracelet.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public bool AddRing(Equipment equipment)
    {
        if (ring.Count < 30 && equipment.equipment == EquipmentType.Ring)
        {
            ring.Add(equipment);
            onChangeGear?.Invoke();
            return true;
        }

        return false;
    }

    public void RemoveWeapon(Equipment equipment)
    {
        if (weapon.Contains(equipment))
        {
            weapon.Remove(equipment);
            onChangeGear?.Invoke();
        }
    }

    public void RemoveHead(Equipment equipment)
    {
        if (head.Contains(equipment))
        {
            head.Remove(equipment);
            onChangeGear?.Invoke();
        }
    }

    public void RemoveBody(Equipment equipment)
    {
        if (body.Contains(equipment))
        {
            body.Remove(equipment);
            onChangeGear?.Invoke();
        }
    }

    public void RemoveHands(Equipment equipment)
    {
        if (hands.Contains(equipment))
        {
            hands.Remove(equipment);
            onChangeGear?.Invoke();
        }
    }

    public void RemoveLegs(Equipment equipment)
    {
        if (legs.Contains(equipment))
        {
            legs.Remove(equipment);
            onChangeGear?.Invoke();
        }
    }

    public void RemoveFeet(Equipment equipment)
    {
        if (feet.Contains(equipment))
        {
            feet.Remove(equipment);
            onChangeGear?.Invoke();
        }
    }

    public void RemoveAuxiliary(Equipment equipment)
    {
        if (auxiliary.Contains(equipment))
        {
            auxiliary.Remove(equipment);
            onChangeGear?.Invoke();
        }
    }

    public void RemoveEarring(Equipment equipment)
    {
        if (earring.Contains(equipment))
        {
            earring.Remove(equipment);
            onChangeGear?.Invoke();
        }
    }

    public void RemoveNecklace(Equipment equipment)
    {
        if (necklace.Contains(equipment))
        {
            necklace.Remove(equipment);
            onChangeGear?.Invoke();
        }
    }

    public void RemoveBracelet(Equipment equipment)
    {
        if (bracelet.Contains(equipment))
        {
            bracelet.Remove(equipment);
            onChangeGear?.Invoke();
        }
    }

    public void RemoveRing(Equipment equipment)
    {
        if (ring.Contains(equipment))
        {
            ring.Remove(equipment);
            onChangeGear?.Invoke();
        }
    }
}
