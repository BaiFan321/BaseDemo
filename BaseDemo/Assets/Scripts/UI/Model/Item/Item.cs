using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class Item : IReference
{
    //物品全局唯一id
    public int Id { get; set; }

    //物品名称
    public string Name { get; set; }

    //物品类型
    public int Type { get; set; }

    //物品图标
    public string IconPath { get; set; }

    //物品描述
    public string Description { get; set; }
}
