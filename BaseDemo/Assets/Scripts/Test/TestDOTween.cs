using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class TestDOTween : MonoBehaviour
{
    public Button btn;

    public CanvasGroup canvasGroup;

    private bool isFade = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        btn.onClick.AddListener(() =>
        {
            if (!isFade)
            {
                canvasGroup.DOFade(0, 1);
                isFade = true;
            }else if (isFade)
            {
                canvasGroup.DOFade(1, 1);
                isFade = false;
            }
        });
    }
}
