using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuPanel : BasePanel
{
    public Button shopBtn;
    public Button bagBtn;
    public Button characterBtn;
    public Button taskBtn;
    public Button startBtn;
    public Button closeBtn;

    public Transform money;
    public Transform diamond;

    protected override void Init()
    {
        shopBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.OpenPanel<ShopPanel>();
        });

        bagBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.OpenPanel<BagPanel>();
        });

        characterBtn.onClick.AddListener(()=> 
        {
            UIManager.Instance.OpenPanel<CharacterPanel>();
        });

        taskBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.OpenPanel<TaskPanel>();
        });

        startBtn.onClick.AddListener(() =>
        {
            //UIManager.Instance.OpenPanel<BattlePanel>();
            LoadBattleScene();
        });

        closeBtn.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    private void LoadBattleScene()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            //Debug.Log("progress = " + asyncOperation.progress);
        }

        asyncOperation.allowSceneActivation = true;
        
        yield return null;

        if (asyncOperation.isDone)
        {
            //UIManager.Instance.ClosePanel<MainMenuPanel>();
            Debug.Log("ÕÍ≥…º”‘ÿ");
        }

    }
}
