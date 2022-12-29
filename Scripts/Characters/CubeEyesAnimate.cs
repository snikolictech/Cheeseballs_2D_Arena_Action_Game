using UnityEngine;
using System.Collections;

#region - Script Synopsis
/*
This script rotates the player's eyes to look at the nearest main enemy.
*/
#endregion

public class CubeEyesAnimate : MonoBehaviour
{
    //Reference to the initial enemy on the screen
    GameObject SphereReference;

	void Start()
	{
        SphereReference = GameObject.Find("Sphere");
	}


    //Used to rotate the player's eyes and follow the initial enemy
	void Update()
	{
        Vector3 sphereRefPos = SphereReference.transform.position;

        Vector3 lookPos = sphereRefPos - transform.position;
        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg - 145f;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}