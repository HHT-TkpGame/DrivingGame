using UnityEngine;

public class WheelEffectController : MonoBehaviour
{
	[Header("タイヤから発生するエフェクト"), SerializeField] ParticleSystem[] particles;

	ParticleSystem particle;

	public void Init(WhetherController w)
	{
		int value = (int)w.CurrentState;
		particle = particles[value];
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
