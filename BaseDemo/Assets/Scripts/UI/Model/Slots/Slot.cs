using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    private int curItemUI_id = -1;

    private ItemUI _itemUI;

    public int m_id = -1;

    private bool hasItem = false;

    public GameObject ItemIntroduce;

    private GameObject curItemIntroduce;

    //将物品放入Slot中
    public void LoadItemUI(ItemUI itemUI, int index)
    {
        if (IsFilled())
        { 
            if(curItemUI_id != itemUI.item.Id)
            { 
                _itemUI = itemUI;
                Transform go = transform.Find("ItemUI");
                go.transform.Find("Icon").GetComponent<Image>().sprite = _itemUI.icon;
                go.transform.Find("Count").GetComponent<Text>().text = _itemUI.count;
                m_id = index;
                curItemUI_id = _itemUI.item.Id;
            }
        }
        else
        {
            AddItemUI(itemUI);
            m_id = index;
        }
    }

    private void AddItemUI(ItemUI itemUI)
    {
        _itemUI = itemUI;
        Transform go = transform.Find("ItemUI");
        go.transform.Find("Icon").GetComponent<Image>().sprite = _itemUI.icon;
        go.transform.Find("Count").GetComponent<Text>().text = _itemUI.count;
        curItemUI_id = _itemUI.item.Id;
    }

    private bool IsFilled()
    {
        hasItem = m_id == -1;
        return !hasItem;
    }

    public void Clear(int index)
    {
        m_id = index;
        if (IsFilled())
        {
            Transform go = transform.Find("ItemUI");
            go.transform.Find("Icon").GetComponent<Image>().sprite = null;
            go.transform.Find("Count").GetComponent<Text>().text = null;
            curItemUI_id = -1;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        ///左键点击slot，移动slot中的物品
        ///右键点击slot，进行装备或者抛弃
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsFilled())
        {
            ///展示物品信息
            if (curItemIntroduce == null)
            {
                curItemIntroduce = GameObject.Instantiate(ItemIntroduce, transform);
                curItemIntroduce.name = ItemIntroduce.name;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsFilled())
        {
            ///停止展示物品信息
            Transform introduce = gameObject.transform.Find("ItemIntroduce");
            if (introduce != null)
            {
                Debug.Log("销毁介绍");
                introduce.gameObject.SetActive(false);
                GameObject.Destroy(introduce.gameObject);
                curItemIntroduce = null;
            }
        }
    }
}
