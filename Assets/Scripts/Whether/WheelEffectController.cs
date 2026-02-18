using UnityEngine;

public class WheelEffectController : MonoBehaviour
{
	//[Header("晴れのエフェクト"), SerializeField] ParticleSystem sunnyParticle;
	//[Header("雨のエフェクト"), SerializeField] ParticleSystem rainyParticle;

	[Header("エフェクト"), SerializeField] ParticleSystem[] particles;

	ParticleSystem particle;
	int value;
	public enum Weather
	{
		sunny, rainy
	}
	public void Init(WhetherController w)
	{
        Debug.Log("other.name");
        value = (int)w.CurrentState;
		particle = particles[value];
		StopEffect();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Collider")) {

            StartEffect();
        }
		
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Collider")) { StopEffect(); }

	}
	public void StartEffect()
	{
		particle.Play();
	}
	public void StopEffect()
	{
		Debug.Log("ddd")
;		particle.Stop();
	}
}
