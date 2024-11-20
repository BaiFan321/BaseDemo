using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerManager : MonoBehaviour
{
    private static TimerManager instance;

    public static TimerManager Instance
    {
        get{
            if(instance == null)
            {
                instance = new TimerManager();
            }
            return instance;
        }
    }

    //持续时长
    private float delayTime;
    //开始时间
    private float attackTime;
    //当前时间
    private float curTime;

    private List<TimeItem> TimeList = new List<TimeItem>();

    private List<TimeItem> TimePool = new List<TimeItem>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine("UpdateUICoroutine");
    }

    private void Update()
    {
        for(int i = 0; i < TimeList.Count; i++)
        {
            float curTime = Time.time;
            TimeList[i].TimerRun(curTime);
        }
    }

    private TimeItem CreateTimer(float delayTime, float attackTime, Action finished)
    {   
        TimeItem timeItem = new TimeItem(delayTime, attackTime, finished);
        return timeItem;
    }

    private void UpdateTimer(TimeItem item)
    {
        curTime = Time.time;
        if(curTime - attackTime > delayTime)
        {
            return;
        }else if(curTime - attackTime <= delayTime)
        {
            item.TimerFinished() ;
        }
    }

    IEnumerator UpdateUICoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("当前时间是：" + Time.time);
        }
        
    }
}
