using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsBundle
{
    public int index;
    public Vector2 position;
    public Slot[] Slots { get; private set; }

    public SlotsBundle(int Capacity)
    {
        Slots = new Slot[Capacity];
    }

    public void Clear()
    {
        index = -1;
        foreach(Slot slot in Slots)
        {
            if(slot != null)
            {
                slot.gameObject.SetActive(false);
                slot.Clear(-1);
            }
        }
    }

}
