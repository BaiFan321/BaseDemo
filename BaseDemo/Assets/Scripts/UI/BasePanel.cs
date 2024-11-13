using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class BasePanel : MonoBehaviour
{
    public UIPanelAsset panelAsset = new UIPanelAsset();

    public UIPanelAsset PanelAsset
    {
        get { return panelAsset; }
        set { panelAsset = value; }
    }

    private CanvasGroup canvasGroup;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    protected virtual void Start()
    {
        Init();
    }

    protected abstract void Init();

    public virtual void OnEnter() {
        canvasGroup.DOFade(1, 1);
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void OnPause() {
        canvasGroup.blocksRaycasts = false;
    }

    public virtual void OnResume() {
        canvasGroup.DOFade(1, 1);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public virtual void OnCover() {
        canvasGroup.interactable = false;
    }

    public virtual void OnReveal() { }

    public virtual void OnExit() {
        canvasGroup.DOFade(0f, 0.5f);
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
    }

}
