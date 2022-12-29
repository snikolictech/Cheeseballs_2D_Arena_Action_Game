using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

#region - Script Synopsis
/*
This script handles camera zooming during PowerUp mode, as well as the slow time "suctioning" audio effect.
*/
#endregion

public class ZoomCam : MonoBehaviour
{
    //Stores reference to the player's position
    public Transform CubeTransform;

    //Stores reference to the audio mixers used in Audio/Mixer folder
    public AudioMixer AudMixerMain;
    public AudioMixer AudMixerSub;

    //Stores the float used to represent the frequency of the LowPass filters for each mixer
    float LPFrequencyMain;
    float LPFrequencySub;

    void Start()
    {
        LPFrequencyMain = LPFrequencySub = 22000;
    }

    //Upon enaging powerUp mode, zooms the camera in towards the player and
    //modifies the LowPass filter accordingly to create a "suction" effect
    void LateUpdate()
    {
        PowerUpManager powerUpManager = GameObject.Find("PowerUpSpawn").GetComponent<PowerUpManager>();

        if (powerUpManager.IsPowered)
        {
            Vector3 cubePosition = CubeTransform.position;
            cubePosition.z = cubePosition.z - 10;
            CameraLerp(Camera.main.orthographicSize, 2.5f,
                Camera.main.transform.position, cubePosition);

            LowPassFilter(600, 0.095f, AudMixerMain, "LowPass", ref LPFrequencyMain);
            LowPassFilter(5000, 0.080f, AudMixerSub, "LowPassSub", ref LPFrequencySub);
        }
        else
        {
            Vector3 defaultPosition = new Vector3(0, 0, -10);
            CameraLerp(Camera.main.orthographicSize, 3.6f, 
                Camera.main.transform.position, defaultPosition);

            LowPassFilter(22000, 0.040f, AudMixerMain, "LowPass", ref LPFrequencyMain);
            LowPassFilter(22000, 0.035f, AudMixerSub, "LowPassSub", ref LPFrequencySub);
        }
    }

    //Controls camera zoom in and out, called in LateUpdate()
    private void CameraLerp(float fromCamSize, float toCamSize, Vector3 fromPosition, Vector3 toPosition)
    {
        Camera.main.orthographicSize = Mathf.Lerp(fromCamSize, toCamSize, 0.08f * Timer.DeltaTimeMod);
        Camera.main.transform.position = Vector3.Lerp(fromPosition, toPosition, 0.08f * Timer.DeltaTimeMod);
    }

    //Controls the LowPass filters for each mixer, called in LateUpdate()
    private void LowPassFilter(float endLimit, float rate, AudioMixer mixer, string effect, ref float frequency)
    {
        frequency = Mathf.Lerp(frequency, endLimit, rate * Timer.DeltaTimeMod);
        mixer.SetFloat(effect, frequency);
    }
}