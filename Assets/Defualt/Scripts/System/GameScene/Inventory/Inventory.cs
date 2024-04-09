using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public delegate void OnItemChanged();
    public OnItemChanged onChangeItem;

    public List<Item> items = new List<Item>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public bool AddItem(Item item)
    {
        if (items.Count < 120)
        {
            items.Add(item);
            onChangeItem?.Invoke();
            return true;
        }

        return false;
    }
}
