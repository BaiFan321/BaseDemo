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

    //����ʱ��
    private float delayTime;
    //��ʼʱ��
    private float attackTime;
    //��ǰʱ��
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
            Debug.Log("��ǰʱ���ǣ�" + Time.time);
        }
        
    }
}
