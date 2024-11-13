using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using OfficeOpenXml;
using System.IO;
using System.Data;
using System.Text;
using System.Reflection;
using System.Linq;
using System;

public partial class UIUtility
{
    private static UIUtility _instance;

    public static UIUtility Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new UIUtility();
            }
            return _instance;
        }
    }

    //加载所有Item到该字典中,以id为key
    public static Dictionary<int, Item> ItemDic;



    /// <summary>
    /// 加载所有物品信息
    /// </summary>
    public static void LoadJsonFile(string path)
    {
        string jsonFilePath = path;
        TextAsset jsonTextAsset = Resources.Load<TextAsset>(jsonFilePath);
        string jsonInfo = jsonTextAsset.text;

        List<Item> bagItems = JsonMapper.ToObject<List<Item>>(jsonInfo);

        foreach(Item item in bagItems)
        {
            ItemDic.Add(item.Id, item);
        }
    }

    public static void LoadExcelFile(string path)
    {
        FileInfo fileInfo = new FileInfo(path);
        int rowCount = 0;
        int colCount = 0;
        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];
            rowCount = worksheet.Dimension.End.Row;
            colCount = worksheet.Dimension.End.Column;
            for(int i = 2; i <= rowCount; i++)
            {
                for(int j = 2; j <= colCount; j++)
                {
                    if(worksheet.Cells[i, j].Value != null)
                    {
                        Debug.Log(worksheet.Cells[i, j].Value.ToString());
                    }
                }
            }
        } 
        
    }

    /// <summary>
    /// Excel表单转化为DataTable对象
    /// </summary>
    /// <returns></returns>

    public static DataTable ExcelToDataTable(string path)
    {
        DataTable table = new();
        FileInfo fileInfo = new FileInfo(path);
        using(ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];
            int rowCount = worksheet.Dimension.End.Row;
            int colCount = worksheet.Dimension.End.Column;
            DataRow dr = null;
            for (int i = 1; i <= rowCount; i++)
            {  
                if (i > 1)
                    dr = table.Rows.Add();

                for(int j = 1; j <= colCount; j++)
                {
                    if(i == 1)
                        table.Columns.Add(worksheet.Cells[i, j].Text);
                    else
                    {
                        dr[j - 1] = worksheet.Cells[i, j].Value;
                    }
                }
            }
        }
        return table;
    }


    /// <summary>
    /// DataTable转化为实体类集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> DataTableToEntity<T>(DataTable table) where T : IReference, new()
    {
        List<T> entityList = new List<T>();
        foreach(DataRow row in table.Rows)
        {
            T obj = new T();
            foreach(DataColumn col in table.Columns)
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(col.ColumnName);
                if(propertyInfo == null)
                {
                    Debug.Log("propertyInfo为空");
                }
                if(row[col] == DBNull.Value)
                {
                    Debug.Log("加载失败");
                }
                if(propertyInfo != null && row[col] != DBNull.Value)
                {
                    propertyInfo.SetValue(obj, Convert.ChangeType(row[col], propertyInfo.PropertyType));
                    
                }
            }
            entityList.Add(obj);
        }
        return entityList;
    }
}
