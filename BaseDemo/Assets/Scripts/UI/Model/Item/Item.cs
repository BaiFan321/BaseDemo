using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class Item : IReference
{
    //��Ʒȫ��Ψһid
    public int Id { get; set; }

    //��Ʒ����
    public string Name { get; set; }

    //��Ʒ����
    public int Type { get; set; }

    //��Ʒͼ��
    public string IconPath { get; set; }

    //��Ʒ����
    public string Description { get; set; }
}
