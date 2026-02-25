using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemCraftable : StatsGameItem
{
    public Item itemCrafted;
    public int quantityCrafted;

    public List<CraftingItemInfo> craftingMaterials;



}

[System.Serializable]
    public class CraftingItemInfo
    {
        public ItemCraftable item;
        public int quantity;
    } 