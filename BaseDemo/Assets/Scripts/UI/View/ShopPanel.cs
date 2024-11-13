using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : BasePanel
{
    public Button btnClose;
    public ToggleGroup togSort;
    public InputField fieldSearch;
    public Dropdown dropOption;

    protected override void Init()
    {
        btnClose.onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePanel<ShopPanel>();
        });
    }
}
