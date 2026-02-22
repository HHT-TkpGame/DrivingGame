using System;
using UnityEngine;

public class UIMover : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    public event Action OnMoveEnd;//ƒ{ƒ^ƒ“®‚ğ‚â‚ß‚½‚ç‘€ì—LŒø‰»‚Ìˆ—‘‚­
    public event Action OnReturnEnd;
    bool isMoving;
    bool isReturning;
    [SerializeField] float speed;
    [SerializeField] float returnPos;
    [SerializeField] bool onlyReturn;
    float targetPos;
    float startPosX;
    public void StartMove()
    {
        isMoving = true;
        targetPos = startPosX;
        isReturning = false;
    }
    public void Return()
    {
        isMoving = true;
        targetPos = returnPos;
        isReturning = true;
    }
    void Start()
    {
        startPosX = rect.anchoredPosition.x;
        if (!onlyReturn)
        {
            rect.anchoredPosition = new Vector3(returnPos, rect.anchoredPosition.y, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving) { return; }
        
        Vector3 pos = rect.anchoredPosition;
        pos.x = Mathf.MoveTowards(pos.x, targetPos, speed * Time.deltaTime);
        rect.anchoredPosition = pos;
        if (Mathf.Approximately(pos.x, targetPos))
        {
            if (isReturning)
            {
                OnReturnEnd?.Invoke();
            }
            else
            {
                OnMoveEnd?.Invoke();
            }
            isMoving = false;
        }
    }
}
