using UnityEngine;
using System.Collections;

#region - Script Synopsis
/*
This script manages spawning of PowerUps as well as the PowerUp meter and PowerUp mode (slow time).
*/
#endregion

public class PowerUpManager : MonoBehaviour
{
    //Reference to PowerUp prefab, set in the inspector
    public GameObject PowerUpPrefab;

    //Stores a timer that is used for spawning powerUps at regular intervals
    private Timer SpawnTimer;

    //Stores a counter representing a "meter" for the PoweredUp state
    public static Timer PowerUpMeter;

    //Stores whether or not player is currently poweredUp
    public bool IsPowered;

    void Start()
	{
        IsPowered = false;
        SpawnTimer = new Timer(0);
        PowerUpMeter = new Timer(50); //Default PowerUp bonus on start
    }

	void Update()
	{
        //Spawns PowerUp ever 300 ticks
        if (SpawnTimer.Counter > 300)
        {
            Vector3 randomSpawn = new Vector3(
                Random.Range(-6.2f, 6.2f),
                Random.Range(-3.4f, 3.4f),
                transform.position.z
            );

            GameObject gO = (GameObject)GameObject.Instantiate(PowerUpPrefab, randomSpawn, transform.rotation);
            gO.transform.parent = gameObject.transform;
        }

        //Handles the player engaging/disengaging powerUp mode, having the effect of changing time speed
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("PowerUp")) && !IsPowered && PowerUpMeter.Counter > 0)
            TimeChange(0.5f);
        else if ((Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("PowerUp")) && IsPowered)
            TimeChange(2f);


        //Depletes the PowerUpMeter if powerUp mode is engaged
        if (IsPowered)
        {
            PowerUpMeter.RunReverse();
            if(PowerUpMeter.Counter == 0)
            {
                TimeChange(2f);
            }
        }

        SpawnTimer.RunForwardTo(300);
	}

    //Handles a pseudo-timechange effect. Time.timeScale is not used
    //as we only want to slow down the player and enemies
    private void TimeChange(float speedFactor)
    {
        CubeController.Speed = CubeController.Speed * speedFactor;
        SphereController.Speed = SphereController.Speed * speedFactor;
        IsPowered = !IsPowered;
    }
}