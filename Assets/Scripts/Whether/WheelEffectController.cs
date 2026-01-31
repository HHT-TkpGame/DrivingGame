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
		value = (int)w.CurrentState;
		particle = particles[value];
	}

	private void OnTriggerEnter(Collider other)
	{
		StartEffect();
	}
	private void OnTriggerExit(Collider other)
	{
		StopEffect();
	}
	public void StartEffect()
	{
		particle.Play();
	}
	public void StopEffect()
	{
		particle.Stop();
	}
}
