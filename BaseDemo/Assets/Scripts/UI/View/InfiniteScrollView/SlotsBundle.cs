using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsBundle<TSlot> where TSlot:MonoBehaviour
{
    public int index;
    public Vector2 position;
    public TSlot[] Slots { get; private set; }

    public SlotsBundle(int Capacity)
    {
        Slots = new TSlot[Capacity];
    }

    public void Clear()
    {
        index = -1;
        foreach(var slot in Slots)
        {
            if(slot != null)
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

}
