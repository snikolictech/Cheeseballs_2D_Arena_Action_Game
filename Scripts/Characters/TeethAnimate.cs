using UnityEngine;
using System.Collections;

#region - Script Synopsis
/*
This script has a reference to the Animator that controls the "TeethChatter" animation. When in PowerUp mode (slow time) 
it engages the animation.
*/
#endregion

public class TeethAnimate : MonoBehaviour
{
    //Reference to PowerUpManger script
    public GameObject PowerUpManagerReference;

    //Reference to Animator component for controlling the "teeth chatter" animation
    //upon engaging PowerUp mode
    Animator AnimatorReference;

	void Start()
	{
        AnimatorReference = GetComponent<Animator>();
	}

    //Controls the "TeethAnimator" animation to create "teetch chatter animation
	void Update()
	{
        bool IsPowered = PowerUpManagerReference.GetComponent<PowerUpManager>().IsPowered;
        AnimatorReference.SetBool("IsPoweredUp", IsPowered);
	}
}