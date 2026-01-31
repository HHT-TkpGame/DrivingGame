using System;
using UnityEngine;

public class BGMSetter : MonoBehaviour
{
	bool downStart = false;

	const float MAXVOL = 0.1f;
	const float SPEED = 0.1f;
	float vol;
	AudioSource se;

    public void StartBGMDown()
    {
		downStart = true;
		se=GetComponent<AudioSource>();
		vol = MAXVOL;
	}
	private void Update()
	{
		if (downStart)
		{
			vol-=Time.deltaTime*SPEED;
			se.volume = vol;
		}
	}
}
