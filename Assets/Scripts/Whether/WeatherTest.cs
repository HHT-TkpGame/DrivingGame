using UnityEngine;

public class WeatherTest : MonoBehaviour
{
    [SerializeField] ParticleSystem ParticleSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //ParticleSystem.Clear();
            ParticleSystem.Play();
        }
    }
}
