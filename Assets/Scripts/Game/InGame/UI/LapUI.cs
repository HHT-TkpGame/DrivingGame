using TMPro;
using UnityEngine;

public class LapUI : MonoBehaviour
{
    [SerializeField] TMP_Text txtCheckPoint;
    int maxCheckPoint;
    void Start()
    {
        txtCheckPoint.text = "";
    }
    public void Initialize
    (
        int maxCheckPoint
    )
    {
        this.maxCheckPoint = maxCheckPoint;
        UpdateDisplay(0);
    }

    public void UpdateDisplay(int currentCheckPoint)
    {
        txtCheckPoint.text = $"CheckPoint\n {currentCheckPoint} / {maxCheckPoint}";
    }
}
