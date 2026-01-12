using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] Image panel;
    public event Action OnFadeInEnd;
    public event Action OnFadeOutEnd;
    bool isFading;
    int fadeDir;
    float targetAlpha;
    float fadeSpeed = 1f;
    Color newColor;
    const float WAIT_FOR_SEC = 1f;
    float waitForSec;

    void Update()
    {
        //Debug.Log($"alpha:{newColor.a}");
        if (!isFading) { return; }
        waitForSec -= Time.deltaTime;
        if(waitForSec > 0) { return; }
        newColor.a = Mathf.MoveTowards(
            newColor.a,
            targetAlpha,
            fadeSpeed * Time.deltaTime
        );
        panel.color = newColor;
        if (newColor.a == targetAlpha)
        {
            isFading = false;
            if (fadeDir > 0)
            {
                OnFadeOutEnd?.Invoke();
            }
            else
            {
                OnFadeInEnd?.Invoke();
            }
        }
    }
    public void StartAnimation()
    {
        waitForSec = WAIT_FOR_SEC;
        isFading = true;
        fadeDir = panel.color.a < 0.5f ? 1 : -1;
        newColor = panel.color;
        targetAlpha = fadeDir > 0 ? 1 : 0;
        Debug.Log($"StartAnimation{fadeDir}");
    }
}
