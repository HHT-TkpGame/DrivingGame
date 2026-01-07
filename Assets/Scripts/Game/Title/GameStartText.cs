using System;
using TMPro;
using UnityEngine;

public class GameStartText : MonoBehaviour
{
    public event Action OnBlinkEnd;//始めるを押されてから点滅演出が終わったことを伝える
    [SerializeField] TMP_Text text;
    Color baseColor = Color.white;
    Color idleColor = new Color(0.8f, 0.8f, 0.8f);
    float timer;
    [SerializeField] float speed;

    bool isBlinking;
    const float BLINK_INTERVAL = 0.2f;
    float blinkInterval;
    int blinkCount;
    const int MAX_BLINK = 6;//偶数であること
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.color = baseColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlinking)
        {
            blinkInterval -= Time.deltaTime;
            if(blinkInterval < 0)
            {
                text.enabled = !text.enabled;
                blinkInterval = BLINK_INTERVAL;
                blinkCount++;
                if(blinkCount >= MAX_BLINK)
                {
                    OnBlinkEnd?.Invoke();
                    isBlinking = false;
                }
            }
        }
        else
        {
            timer += Time.deltaTime * speed;
            float t = (Mathf.Sin(timer) + 1f) * 0.5f;
            text.color = Color.Lerp(baseColor, idleColor, t);
        }
    }

    public void StartBlink()
    {
        isBlinking = true;
        text.color = baseColor;
    }
}
