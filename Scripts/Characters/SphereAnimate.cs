using UnityEngine;
using System.Collections;

#region - Script Synopsis
/*
This script simply rotates the enemy for visual effect.
*/
#endregion

public class SphereAnimate : MonoBehaviour
{
    int RandRotateDir;
    float RandRotateSpeed;

	void Start()
	{
        RandRotateDir = Random.Range(0, 2) * 2 - 1;
        RandRotateSpeed = Random.Range(3.0f, 9.0f);
	}

    //Simply rotates the enemy in a semi-random speed and direction
	void Update()
	{
        transform.Rotate(0f, 0f, (RandRotateSpeed * RandRotateDir) * Timer.DeltaTimeMod);
	}
}