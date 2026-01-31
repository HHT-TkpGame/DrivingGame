using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIController : MonoBehaviour
{
    [SerializeField] UIMover resultMover;
    [SerializeField] GameObject exitButtonObj;
    [SerializeField] TMP_Text txtTime;
    Button exitButton;
    public event Action exitButtonPressed;
    event Action onBlinkEnd;
    bool isBlinking;
    const float BLINK_INTERVAL = 0.1f;
    float blinkInterval = BLINK_INTERVAL;
    float blinkCount;
    const float MAX_BLINK = 10;
    VehicleInputHandler handler;
    public void Initialize(VehicleInputHandler handler)
    {
        handler.InMenuClick += OnExitButtonPressed;
    }

    void Start()
    {
        resultMover.OnMoveEnd += StartBlink;
        onBlinkEnd += DisplayButton;
        exitButton = exitButtonObj.GetComponent<Button>();
        exitButtonObj.SetActive(false);
    }

    void Update()
    {
        if (!isBlinking) { return; }
        
        blinkInterval -= Time.deltaTime;
        if (blinkInterval < 0)
        {
            txtTime.enabled = !txtTime.enabled;
            blinkInterval = BLINK_INTERVAL;
            blinkCount++;
            if (blinkCount >= MAX_BLINK)
            {
                onBlinkEnd?.Invoke();
                isBlinking = false;
            }
        }
    }
    void DisplayButton()
    {
        exitButtonObj.SetActive(true);
    }
    void OnExitButtonPressed()
    {
        exitButtonPressed?.Invoke();
    }
    public void StartAnimation()
    {
        resultMover.StartMove();
    }
    public void SetResult(string strTime)
    {
        txtTime.text = $"Time: {strTime}";
    }
    void StartBlink() 
    { 
        isBlinking = true;
    }
}
