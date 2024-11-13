using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : BasePanel
{
    public Button bagBtn;
    protected override void Init()
    {
        bagBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.OpenPanel<BattleBagPanel>();
        });

    }
}
