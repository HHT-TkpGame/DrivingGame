using System;
using UnityEngine;

public class GameStartPanel : MonoBehaviour
{
    [SerializeField] GameObject panel;
    UIMover mover;
    public event Action OnStartRequested;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mover = GetComponent<UIMover>();
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckStart()
    {
        if (panel.activeSelf)
        {
            //表示されていたら　＝＝　始めていいかの確認が出てるときに押されたらスタートのリクエスト
            OnStartRequested();
            mover.Return();
        }
        SetVisible(true);
    }
    public void SetVisible(bool visible)
    {
        panel.SetActive(visible);
    }
}
