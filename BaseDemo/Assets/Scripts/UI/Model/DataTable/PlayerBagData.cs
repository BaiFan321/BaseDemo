using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerBagData
{
    private static PlayerBagData _instance;

    public static PlayerBagData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new PlayerBagData();
            }
            return _instance;
        }
    }
    
    public List<PlayerBagItem> bagItemList;

    private readonly string ItemPath = UIConfigDef.PLAYER_BAG_PATH;

    
    /// <summary>
    /// 加载玩家背包物品信息,缓存入BagItems
    /// </summary>
    /// <returns></returns>
    public List<PlayerBagItem> GetPlayerBagData()
    {
        DataTable table = UIUtility.ExcelToDataTable(ItemPath);
        bagItemList = UIUtility.DataTableToEntity<PlayerBagItem>(table);
        return bagItemList;
    }


    /// <summary>
    /// 退出时，将修改部分导出至文档中
    /// </summary>
    /// <param name="bagItems"></param>
    public void SetPlayerBagData(Dictionary<int, int> bagItems)
    {

    }
}

