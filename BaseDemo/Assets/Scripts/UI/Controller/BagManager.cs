using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagManager
{
    private static BagManager _instance;

    public static BagManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new BagManager();
            }
            return _instance;
        }
    }
    
    private InfiniteScrollList scrollList;

    //分类
    private List<PlayerBagItem> m_AllItems = new();
    private List<PlayerBagItem> m_WeaponItems = new();
    private List<PlayerBagItem> m_DrugItems = new();
    private List<PlayerBagItem> m_MaterialItems = new();

    private List<ItemUI> itemUIList = new();

    private Canvas canvas;

    private BagManager()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    /// <summary>
    /// 加载玩家数据
    /// </summary>
    public void LoadItem()
    {
        scrollList = canvas.GetComponent<Transform>().Find("BagPanel").GetComponent<Transform>().Find("Items").GetComponent<InfiniteScrollList>();

        //读取玩家数据,并实例化为ItemUI
        InitPlayerBagData();

    }

    public void LoadItem(PlayerBagItemType itemType)
    {
        switch (itemType)
        {
            case PlayerBagItemType.Weapon:
                    break;
            default:
                break;
        }
    }


    private void InitPlayerBagData() 
    {
        List<PlayerBagItem> playerBagItems = PlayerBagData.Instance.GetPlayerBagData();
        if(playerBagItems != null)
        {
            foreach (var item in playerBagItems)
            {
                if(item != null)
                {
                    DataToUI(item);
                }
            }
        }
        else
        {
            Debug.Log("BagManager 输出失败");
        }

        if(itemUIList != null)
        {
            LoadInSlot(itemUIList);
        }
    }

    private void DataToUI(PlayerBagItem item)
    {
        ItemUI itemUI = new ItemUI(item);
        itemUIList.Add(itemUI);
    }

    //装载进入Slot里
    private void LoadInSlot(List<ItemUI> itemUIList)
    {
        scrollList.Initialize(itemUIList);
    }

    //玩家数据分类
    private void BagDataSort(List<PlayerBagItem>  m_items)
    {
        foreach (var bagItem in m_items)
        {
            switch (bagItem.BagType)
            {
                case (int)PlayerBagItemType.Weapon:
                    m_WeaponItems.Add(bagItem);
                    break;
                case (int)PlayerBagItemType.Drug:
                    m_DrugItems.Add(bagItem);
                    break;
                case (int)PlayerBagItemType.Material:
                    m_MaterialItems.Add(bagItem);
                    break;
                default:
                    break;
            }
        }
    }



    #region 对背包物品增删改查
    public void AddItem()
    {
        //往PlayerBagData里添加物品
    }

    public void DeleteItem() { }

    public void FindItem() { }

    public void FindItem(int id)
    {

    }

    public void ChangeItem() { }

    public void ChangeItemPosition() { }
    #endregion


    //监听到事件后，更新背包状态
    public void Refresh()
    {

    }
}
