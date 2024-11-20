using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TimeItem : MonoBehaviour
{
    
    public float delayTime;

    public float attackTime;

    public Action onFinished; 

    public TimeItem(float delayTime, float attackTime, Action onFinished)
    {
        this.delayTime = delayTime;
        this.attackTime = attackTime;
        this.onFinished = onFinished;
    }

    public void TimerRun(float curTime)
    {
        if(curTime - attackTime < delayTime)
        {
            TimerFinished();
        }
    }

    public void TimerFinished()
    {
        this.onFinished();
    }
}
