using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI
{
    public Sprite icon;
    public string count;

    public Item item;

    public ItemUI(PlayerBagItem item)
    {
        icon= Resources.Load<Sprite>(item.IconPath);
        count = item.Count.ToString();
        this.item = item;
    }

    public ItemUI() { }
}
