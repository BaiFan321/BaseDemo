using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBagItem : Item
{
    public int BagType { get; set; }

    public int Count { get; set; }
}

public enum PlayerBagItemType
{
    Weapon = 1001,
    Drug = 1002,
    Material = 1003,
    Other = 1004
}
