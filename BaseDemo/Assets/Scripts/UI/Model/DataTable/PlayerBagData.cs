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
    /// ������ұ�����Ʒ��Ϣ,������BagItems
    /// </summary>
    /// <returns></returns>
    public List<PlayerBagItem> GetPlayerBagData()
    {
        DataTable table = UIUtility.ExcelToDataTable(ItemPath);
        bagItemList = UIUtility.DataTableToEntity<PlayerBagItem>(table);
        return bagItemList;
    }


    /// <summary>
    /// �˳�ʱ�����޸Ĳ��ֵ������ĵ���
    /// </summary>
    /// <param name="bagItems"></param>
    public void SetPlayerBagData(Dictionary<int, int> bagItems)
    {

    }
}

