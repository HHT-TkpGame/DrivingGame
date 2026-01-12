using System;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public event Action OnCountdownEnd;
    [SerializeField] TMP_Text txtTimer;
    bool enable;
    float timer = 3;
    float displayTimer = 1;
    bool goTxtDisplaying;
    
    void Start()
    {
        txtTimer = GetComponent<TMP_Text>();
        txtTimer.text = "";
    }
    public void StartCountdown()
    {
        enable = true;
    }

    void Update()
    {
        if (enable)
        {
            timer -= Time.deltaTime;
            if (timer > 0)
            {
                txtTimer.text = Mathf.CeilToInt(timer).ToString("f0");
            }
            else
            {
                OnCountdownEnd?.Invoke();
                txtTimer.text = "GO!!";
                enable = false;
                goTxtDisplaying = true;
            }
        }
        if (goTxtDisplaying)
        {
            displayTimer -= Time.deltaTime;
            if (displayTimer < 0)
            {
                txtTimer.text = "";
                goTxtDisplaying = false;
            }
        }
    }
}
