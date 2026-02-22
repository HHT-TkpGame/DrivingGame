using Unity.VisualScripting;
using UnityEngine;

public class WheelEffectController : MonoBehaviour
{
	//[Header("晴れのエフェクト"), SerializeField] ParticleSystem sunnyParticle;
	//[Header("雨のエフェクト"), SerializeField] ParticleSystem rainyParticle;

	[Header("エフェクト"), SerializeField] ParticleSystem[] particles;
	
	//wheelController.WheelRPMを使う用
	[SerializeField] WheelController wheelController;

	ParticleSystem.EmissionModule emission;
	ParticleSystem.MinMaxCurve defaultCurve;

	const float MAXRPM = 750;

	//float RPMRatio; //= wheelController.WheelRPM / MAXRPM;

	public float NormalizedRPM
	{
		get {
			float w = Mathf.Clamp01(Mathf.Abs(wheelController.WheelRPM) / MAXRPM);

			w = Mathf.Round(w*1000f)/1000f;
			//100％を越えないように
			if (w > 1)
			{
				w = 1;
			}
			else if (w < 0.1f)
			{
				w = 0;
			}
			return w;
		}
		
	}

	ParticleSystem particle;
	int value;
	bool isSunny;
	public void Init(WhetherController w)
	{
        Debug.Log("other.name");
        value = (int)w.CurrentState;
		if(value <= 0 ) { isSunny = true; }
		particle = particles[value];
		emission = particle.emission;
		defaultCurve = emission.rateOverTime;

		StopEffect();
	}

	private void FixedUpdate()
	{
		if (particle == null||!particle.isPlaying||!isSunny) return;

		emission.rateOverTime = (int)(defaultCurve.constant * NormalizedRPM);
		//Debug.Log("emission.rateOverTime" + defaultCurve.constant*NormalizedRPM);
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
;		particle.Stop();
	}
}
