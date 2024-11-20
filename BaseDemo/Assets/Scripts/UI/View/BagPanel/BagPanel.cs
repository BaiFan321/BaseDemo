using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : BasePanel
{

    public Button closeBtn;

    public Toggle allTog;
    public Toggle weaponTog;
    public Toggle drugTog;
    public Toggle materialTog;

    public override void OnEnter()
    {
        BagManager.Instance.FirstLoadItem();
        allTog.isOn = true;
    }

    protected override void Init()
    {
        closeBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePanel<BagPanel>();
        });

        allTog.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                BagManager.Instance.LoadItem();
            }
        });

        weaponTog.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                BagManager.Instance.LoadItem(PlayerBagItemType.Weapon);
            }
        });

        drugTog.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                BagManager.Instance.LoadItem(PlayerBagItemType.Drug);
            }
        });

        materialTog.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                BagManager.Instance.LoadItem(PlayerBagItemType.Material);
            }
        });
    }
}
