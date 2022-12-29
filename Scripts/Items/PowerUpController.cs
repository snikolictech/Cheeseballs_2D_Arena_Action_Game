using UnityEngine;
using System.Collections;

#region - Script Synopsis
/*
This script controls collection of PowerUps by the player, adding to the PowerUp meter. It automatically destroys the PowerUp
after some time has passed if it has not been collected.
*/
#endregion

public class PowerUpController : MonoBehaviour
{
    //Stores reference to the player (to determine if player collects the powerUp or not)
    GameObject CubeReference;

    //A timer that is used to automatically destroy powerUp if not collected after a set amount of time passes
    public Timer KillTimer;

	void Start()
	{
        KillTimer = new Timer(0);
        CubeReference = GameObject.Find("Cube");
	}

	void Update()
	{
        AnimatePowerUp();
        DestroyPowerUp();
    }

    //Returns true when player collects the powerUp
    private bool AtePowerUp()
    {
        Vector2 distance = new Vector2(
            transform.position.x - CubeReference.transform.position.x,
            transform.position.y - CubeReference.transform.position.y
        );

        //Determines whether or not player is near the PowerUp (some leniency, within 0.5 units)
        if (((distance.x < 0.5f) && (distance.x > -0.5f)) && ((distance.y < 0.5f) && (distance.y > -0.5f)))
            return true;
        else
            return false;
    }

    //Destroys the powerUp if collected, or if a set time (140 ticks/Update calls) passes
    private void DestroyPowerUp()
    {
        if (AtePowerUp())
        {
            GameObject.Destroy(gameObject);
            PowerUpManager.PowerUpMeter.Counter += 100;

            AudioSource[] audio = GetComponentsInParent<AudioSource>();
            int rand = Random.Range(0, 3);
            audio[rand].Play();
        }

        if (KillTimer.Counter > 140)
        {
            GameObject.Destroy(gameObject);
        }
        KillTimer.RunForwardTo(140);
    }

    //Creates a spawning animation
    private void AnimatePowerUp()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1,1,1), 0.05f * Timer.DeltaTimeMod);
    }
}