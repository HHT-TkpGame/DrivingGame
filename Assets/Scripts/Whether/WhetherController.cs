using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class WhetherController : MonoBehaviour
{
	[Header("晴天時のVolume"), SerializeField] VolumeProfile sunnyVolume;
	[Header("雨天時のVolume"), SerializeField] VolumeProfile rainyVolume;

	[Header("晴天時のグラウンドのマテリアル"), SerializeField] Material sunnyGroundMat;
	[Header("雨天時のグラウンドのマテリアル"), SerializeField] Material rainyGroundMat;

	[Header("晴天時のフロントガラスのマテリアル"), SerializeField] Material sunnyGlassMat;
	[Header("雨天時のフロントガラスのマテリアル"), SerializeField] Material rainyGlassMat;

	[Header("晴天時の道路のマテリアル"), SerializeField] Material sunnyCourseMat;
	[Header("雨天時の道路のマテリアル"), SerializeField] Material rainyCourseMat;

	[Header("雨エフェクト"), SerializeField] VisualEffect rainEffect;

	[Header("水たまりがある場所のコライダー"), SerializeField] GameObject puddleColliders;
	[Header("砂地がある場所のコライダー"), SerializeField] GameObject sandColliders;

	[Header("晴天時のPhysicMat"), SerializeField] PhysicsMaterial sunnyCoursePhysic;
	[Header("雨天時のPhysicMat"), SerializeField] PhysicsMaterial rainyCoursePhysic;

	[Header("晴天時のPhysicMat"), SerializeField] PhysicsMaterial sunnyGroundPhysic;
	[Header("雨天時のPhysicMat"), SerializeField] PhysicsMaterial rainyGroundPhysic;

	[Header("コースのコライダー"), SerializeField] MeshCollider courseCollider;
	[Header("地面のコライダー"), SerializeField] MeshCollider groundCollider;


	[SerializeField] Volume weatherVol;

	[SerializeField] MeshRenderer groundMat;
	[SerializeField] MeshRenderer glassMat;
	[SerializeField] MeshRenderer courseMats;
	[SerializeField] MeshRenderer slopeMat;
	[SerializeField] WheelEffectController[] wheelEffectControllers;

	

	/// <summary>
	/// ゲーム内における天候の差
	/// 
	/// 路面,グラウンド
	/// ・晴天時
	/// 　　晴天時のマテリアルを設定
	/// ・雨天時
	///     雨天時のマテリアルを設定
	/// エフェクト
	/// ・晴天時
	///     ダートは行った時に土煙を再生
	///     雨粒エフェクトを停止
	/// ・雨天時
	///     どこを走ってても小さい水しぶき
	///     水たまりに入ったときに水しぶき
	///     雨粒エフェクトを再生
	///     
	/// 車体について
	/// ・晴天時
	///		フロントガラスのマテリアルのSizeとTimeの値を0にする
	///	・雨天時
	///		フロントガラスのマテリアルのSizeとTimeの値を3.7と7.1にする
	///		
	/// エフェクトについてゲームが始まったときに天候に合わせて土埃と水しぶきのどちらを再生するか
	/// </summary>

	///車から出るエフェクト系どうやって設計するか

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        Init();
    }

	public enum WhetherState
	{
		Sunny,
		Rainy
	}

	public WhetherState CurrentState {  get; private set; }



    public void Init()
    {
		int length = Enum.GetValues(typeof(WhetherState)).Length;
        int whether = UnityEngine.Random.Range(0, length);
		CurrentState = (WhetherState)whether;
		
		for(int i = 0; i < wheelEffectControllers.Length; i++)
		{
			wheelEffectControllers[i].Init(this);

		}

		Material[] mats = courseMats.materials;


		switch (CurrentState)
        {
            case WhetherState.Sunny:


				weatherVol.profile = sunnyVolume;
				groundMat.material = sunnyGroundMat;
				glassMat.material = sunnyGlassMat;
				mats[1] = sunnyCourseMat;
				courseMats.materials = mats;
				slopeMat.material = sunnyCourseMat;
				courseCollider.material = sunnyCoursePhysic;
				groundCollider.material = sunnyGroundPhysic;


				rainEffect.Stop();
				Debug.Log("晴れた");
				CollidersState(false);
				break;
            case WhetherState.Rainy:

				weatherVol.profile = rainyVolume;
				groundMat.material = rainyGroundMat;
				glassMat.material = rainyGlassMat;
				courseMats.materials[1] = rainyCourseMat;
				slopeMat.material = rainyCourseMat;
				courseCollider.material = rainyCoursePhysic;
				groundCollider.material = rainyGroundPhysic;

				rainEffect.Play();
				Debug.Log("雨");
				CollidersState(true);
				break;
        }
		//Debug.Log(courseMats.material);
    }


	void CollidersState(bool state)
	{
		Debug.Log(state);
		puddleColliders.SetActive(state);
		sandColliders.SetActive(!state);
	}
}
