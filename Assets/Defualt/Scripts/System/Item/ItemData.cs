using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public static ItemData Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public List<Item> items = new List<Item>();
    public List<Equipment> equip = new List<Equipment>();

}
