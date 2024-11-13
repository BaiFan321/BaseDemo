using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private static PlayerData _instance;
    public  PlayerData Instance
    {
        get{
            if(_instance == null)
            {
                _instance = gameObject.AddComponent<PlayerData>();
            }
            return _instance;
        }
    }
}
