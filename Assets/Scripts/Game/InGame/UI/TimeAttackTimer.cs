using TMPro;
using UnityEngine;

public class TimeAttackTimer : MonoBehaviour
{
    [SerializeField] TMP_Text txtTimer;
    float timer;
    bool isActive;
    void Start()
    {
        txtTimer.text = "";
    }
    public void StartTimer()
    {
        isActive = true;
    }
    public void StopTimer()
    {
        isActive = false;
    }

    void Update()
    {
        if (!isActive) { return; }
        timer += Time.deltaTime;
        txtTimer.text = ToMinuteSecondFrame(timer);
    }
    /// <summary>
    /// MM.SS.FFŒ`Ž®‚É•ÏŠ·
    /// </summary>
    /// <param name="timeSeconds"></param>
    /// <returns></returns>
    string ToMinuteSecondFrame(float timeSeconds)
    {
        int minutes = (int)(timeSeconds / 60f);
        int seconds = (int)(timeSeconds % 60f);
        int frames = (int)((timeSeconds - Mathf.Floor(timeSeconds)) * 100f);
        
        return $"{minutes:00}.{seconds:00}.{frames:00}";
    }
}
