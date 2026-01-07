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
    float fadeSpeed = 1f;
    Color newColor;
    const float WAIT_FOR_SEC = 1f;
    float waitForSec;

    void Update()
    {
        if (!isFading) { return; }
        waitForSec -= Time.deltaTime;
        if(waitForSec > 0) { return; }
        Debug.Log("Fading");
        newColor.a += Time.deltaTime * fadeSpeed * fadeDir;
        panel.color = newColor;
        if (newColor.a >= 1)
        {
            isFading = false;
            OnFadeOutEnd?.Invoke();
        }
        if(newColor.a <= 0)
        {
            isFading = false;
            OnFadeInEnd?.Invoke();
        }
    }
    public void StartAnimation()
    {
        waitForSec = WAIT_FOR_SEC;
        isFading = true;
        fadeDir = panel.color.a < 0.5f ? 1 : -1;
        newColor = panel.color;
        Debug.Log($"StartAnimation{fadeDir}");
    }
}
