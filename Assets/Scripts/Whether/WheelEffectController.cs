using UnityEngine;

public class WheelEffectController : MonoBehaviour
{
	[Header("タイヤから発生するエフェクト"), SerializeField] ParticleSystem[] particles;

	ParticleSystem particle;
	int value;

	public void Init(WhetherController w)
	{
		particle = GetComponent<ParticleSystem>();
		value = (int)w.CurrentState;
	}

	public void StartEffect()
	{
		particles[value].Play();
	}
	public void StopEffect()
	{
		particles[value].Stop();
	}
}
