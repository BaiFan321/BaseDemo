using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : BasePanel
{
    public Button closeBtn;
    protected override void Init()
    {
        closeBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePanel<CharacterPanel>();
        });
    }
}
