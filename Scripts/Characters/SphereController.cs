using UnityEngine;
using System.Collections;

#region - Script Synopsis
/*
This script controls enemy behaviour, including movement, spawning new instances, emulated "collision" detection and so forth.
*/
#endregion

public class SphereController : MonoBehaviour
{
    //Stores reference to the player GameObject and Position, respectively
    public GameObject CubeReference;
    Vector3 CubeRefPosition;

    //Stores the enemy's speed
    public static float Speed = 0.06f;

    //Stores the distance between the enemy and the player
    Vector2 Distance;

    //Stores the audio component attached to this script's GameObject
    AudioSource Audio;

    //Stores the current "wave" of spawned enemies
    public int WaveCount;

    //When the player "grazes" the enemy it issues a score multiplier stored in this variable
    public float CloseCall;

    //Stores the interval amount for each wave of spawned enemies
    public int SpawnInterval = 500;

    //A timer which is used to control audio and spawning, respectively
    private Timer CoolCrowd;
    public Timer SpawnTimer;

	void Start()
	{
        Audio = GetComponent<AudioSource>();
        CoolCrowd = new Timer(220);
        SpawnTimer = new Timer(0);

        CloseCall = 0;
        WaveCount = 0;
	}

	void Update()
	{
        Movement();
        EnemyProximity();
        SpawnEnemy();
        CrowdCheer();
	}

    //Enemy tracks position of player and follows
    private void Movement()
    {
        CubeRefPosition = CubeReference.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, CubeRefPosition, Speed * Timer.DeltaTimeMod);
    }

    //Check Distance between player & enemy. If occupying same space, player is "eaten"!
    private void EnemyProximity()
    {
        
        float x = transform.position.x - CubeRefPosition.x;
        float y = transform.position.y - CubeRefPosition.y;
        Distance = new Vector2(x,y);


        //If player is "eaten" it sets off a chain of end-game events:
        //Disabling player controller
        //Disabling powerUp state (if enabled) so the camera zooms back out automatically
        //Disables enemy script, thereby disabling any further spawning
        //Scaling and rotating the player in a "vortex" animation
        //Sets IsGameOver to true for handling other end-game states
        if (((Distance.x < 0.1f) && (Distance.x > -0.1f)) && ((Distance.y < 0.1f) && (Distance.y > -0.1f)))
        {
            CubeReference.GetComponent<CubeController>().enabled = false;
            CubeReference.transform.localScale = Vector3.Lerp(CubeReference.transform.localScale, new Vector3(0.01f, 0.01f, 0), 0.05f * Timer.DeltaTimeMod);
            CubeReference.transform.rotation = new Quaternion(0, 0, transform.rotation.z * 0.5f, transform.rotation.w);

            GameOverManager.IsGameOver = true;

            PowerUpManager powerUpManager = GameObject.Find("PowerUpSpawn").GetComponent<PowerUpManager>();
            powerUpManager.IsPowered = false;
            powerUpManager.enabled = false;
        }

        if (CubeReference.transform.localScale.y < 0.011f)
        {
            GetComponent<SphereController>().enabled = false;
            CubeReference.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    //Spawns a new enemy copied from this script every 500 ticks, increases the WaveCount multiplier
    private void SpawnEnemy()
    {
        if (SpawnTimer.Counter > SpawnInterval)
        {
            float x = Random.Range(1f, 6.4f) * (Random.Range(0, 2) * 2 - 1);
            float y = Random.Range(1f, 3.5f) * (Random.Range(0, 2) * 2 - 1);
            float z = transform.position.z;
            GameObject.Instantiate(this.gameObject, new Vector3(x, y, z), transform.rotation);

            WaveCount++;
        }
        SpawnTimer.RunForwardTo(SpawnInterval);
    }

    //If enemy "grazes" the player, sets off the attached audio component
    //for each enemy, creating a "crowd cheering" effect that becomes louder
    //with every enemy being "grazed"
    private void CrowdCheer()
    {
        int maxCoolTime = 220;

        if (((Distance.x < 0.9f) && (Distance.x > -0.9f)) && ((Distance.y < 0.9f) && (Distance.y > -0.9f)))
        {
            if (CoolCrowd.Counter == maxCoolTime)
            {
                Audio.panStereo = Random.Range(0f, 0.6f) * (Random.Range(0, 2) * 2 - 1);
                Audio.PlayDelayed(Random.Range(0.01f, 0.3f));
                CoolCrowd.Counter = maxCoolTime - 1;
            }
            CloseCall += 2 * WaveCount * Timer.DeltaTimeMod;
        }
        if (CoolCrowd.Counter < maxCoolTime)
        {
            CoolCrowd.RunReverseFrom(maxCoolTime);
        }
    }
}