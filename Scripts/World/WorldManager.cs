using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#region - Script Synopsis
/*
This script handles general "World" behaviour, including keeping score, managing general audio and game pause.
*/
#endregion

public class WorldManager : MonoBehaviour
{
    //Stores all enemy references in a List
    List<GameObject> AllSpheres = new List<GameObject>();

    //Stores the current score total
    public static int GrandTotalScore;

    //Used to determine a bonus to the PowerUp meter
    //set in BonusAnnounce() method at set score intervals
    bool BonusSwitch;

    //Stores reference to enemy SphereController script
    SphereController SphereScript;

    //Stores an array of audio components attached to this GameObject (MainCamer)
    AudioSource[] Audio;

    //A timer for sounding a horn before every enemy spawn wave
    Timer HornTimer;

    //Stores whether or not the game is paused
    bool PauseSwitch;

    //Stores whether or not player is currently scoring by grazing the enemy, accessible by other scripts
    public static bool IsScoring;

	void Start()
	{
        Application.targetFrameRate = 60; //Set the framerate for testing purposes
        QualitySettings.vSyncCount = 1; //0 for testing in Unity, 1 for final build

        BonusSwitch = true;
        SphereScript = GameObject.Find("Sphere").GetComponent<SphereController>();
        Audio = GetComponents<AudioSource>();
        MusicStart();

        HornTimer = new Timer(0);
	}

	void Update()
	{
        GetCloseCallScore();
        PreSpawnAirHorn();
        MusicModifier();
        BonusAnnounce();

        GamePause();
	}

    //Updates the main score by adding up the CloseCall score for each sphere
    //the player grazes and adding it to the previous score total
    private void GetCloseCallScore()
    {
        int previousGrandTotalScore = GrandTotalScore;
        foreach (GameObject sphere in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!AllSpheres.Contains(sphere))
            {
                AllSpheres.Add(sphere);
            }
        }

        float tempTotal = 0;
        foreach (GameObject sphere in AllSpheres)
        {
            SphereController eachSphereScript = sphere.GetComponent<SphereController>();
            tempTotal += eachSphereScript.CloseCall;
        }
        GrandTotalScore = (int)tempTotal;

        IsScoring = ScoringCheck(previousGrandTotalScore, GrandTotalScore);
    }

    //Sounds the airhorn before the enemy spawn wave hits
    private void PreSpawnAirHorn()
    {
        float hornInterval = SphereScript.SpawnInterval - 150;

        if (HornTimer.Counter > hornInterval)
        {
            if (!GameOverManager.IsGameOver)
                Audio[1].Play();
        }

        if (SphereScript.SpawnTimer.Counter == 0)
        {
            HornTimer.Counter = 0;
        }
        HornTimer.RunForwardTo(hornInterval);
    }

    //Plays the main music loops on start
    private void MusicStart()
    {
        Audio[2].Play();
        Audio[3].Play();
    }

    //Changes which of the two main music loops play depending on current wave count
    //also pitches up the music loop when wave count is greater or equal to 5
    private void MusicModifier()
    {
        if (SphereScript.WaveCount < 3)
        {
            Audio[2].mute = false;
            Audio[3].mute = true;
        }
        else if (SphereScript.WaveCount == 3)
        {
            Audio[2].mute = true;
            Audio[3].mute = false;
        }
        else if (SphereScript.WaveCount == 5)
        {
            Audio[3].pitch = Mathf.Lerp(Audio[3].pitch, 1.2f, 0.01f * Timer.DeltaTimeMod);
        }
        else if (SphereScript.WaveCount > 5)
        {
            Audio[3].pitch = Mathf.Lerp(Audio[3].pitch, 1.3f, 0.01f * Timer.DeltaTimeMod);
        }
    }

    //Hands out a bonus to the powerUp meter when preset score thresholds are reached
    private void BonusAnnounce()
    {
        if (GrandTotalScore > 25000 && GrandTotalScore < 50000 && BonusSwitch)
        {
            Audio[4].Play();
            PowerUpManager.PowerUpMeter.Counter *= 1.5f;
            BonusSwitch = !BonusSwitch;
        }
        else if (GrandTotalScore > 50000 && !BonusSwitch)
        {
            Audio[5].Play();
            PowerUpManager.PowerUpMeter.Counter *= 1.25f;
            BonusSwitch = !BonusSwitch;
        }
    }


    //Handles pausing of the game (via Time.timeScale)
    private void GamePause()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Start"))
            PauseSwitch = !PauseSwitch;

        if (PauseSwitch && !GameOverManager.IsGameOver)
        {
            Time.timeScale = 0;
            GameObject.Find("Cube").GetComponent<CubeController>().enabled = false;
        }
        else if (!PauseSwitch && !GameOverManager.IsGameOver)
        {
            Time.timeScale = 1;
            GameObject.Find("Cube").GetComponent<CubeController>().enabled = true;
        }
    }

    //Used in GetCloseCallScore() in order to determine if score is different from the previous frame
    //and, therefore, if player is currently scoring
    private bool ScoringCheck(int previousFramesScore, int currentFramesScore)
    {
        if (previousFramesScore < currentFramesScore)
            return true;
        else
            return false;
    }
}