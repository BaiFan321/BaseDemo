using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private static MonsterController instance;
    public MonsterController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = gameObject.AddComponent<MonsterController>();
            }
            return instance;
        }
    }

    public PlayerInput playerInput;
}
