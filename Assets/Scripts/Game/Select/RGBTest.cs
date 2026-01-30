using UnityEngine;
using UnityEngine.UI;

public class RGBTest : MonoBehaviour
{
    [SerializeField] Scrollbar[] scrollbar;
    [SerializeField] Image image;

    SaveCarRGB carRGB;

    [SerializeField] Material bodyMaterial;

    Material currentMaterial;

	private void Start()
	{
        currentMaterial = bodyMaterial;
        //carRGB = new SaveCarRGB();
	}
	// Update is called once per frame
	void Update()
    {
        Color c = new Color(scrollbar[0].value, scrollbar[1].value, scrollbar[2].value, 1);
        image.color = c;
        bodyMaterial.color = c;
        //こっちのほうはシーンが変わる行動が決定されたときに実行
        //carRGB.SetCarColor(c); 
	}
}
