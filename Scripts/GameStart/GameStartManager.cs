using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

#region - Script Synopsis
/*
This script allows the game to be restarted, and shows a "Press Enter" message when it's Game Over.
It's also used for loading the game and observing the credits from the StartScreen.
*/
#endregion

public class GameStartManager : MonoBehaviour
{
    //Demonstrates using a property for getting/setting blinking behaviour of
    //the "Press Enter" image
    Timer _blinkTimer;
    Timer BlinkTimer
    {
        get
        {
            _blinkTimer.RunForwardTo(30);
            return _blinkTimer;
        }
        set
        {
            _blinkTimer = value;
        }
    }

	void Start()
	{
        BlinkTimer = new Timer(0);
	}

	void Update()
	{
        MenuSelection();
        LoadGame();
    }

    //Controls the blinking "Press Enter" image when starting/restarting the game
    private void LoadGame()
    {
        if (BlinkTimer.Counter == 0)
        {
            SpriteRenderer pressEnterRenderer = gameObject.GetComponent<SpriteRenderer>();
            pressEnterRenderer.enabled = !pressEnterRenderer.enabled;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Start"))
        {
            SceneManager.LoadScene("MainGame");
            CubeController.Speed = 0.07f;
            SphereController.Speed = 0.06f;
        }
    }

    //Controls the "menu" for the start screen
    private void MenuSelection()
    {
        GameObject splash = GameObject.Find("Splash");
        GameObject arrows = GameObject.Find("Arrows");

        if (splash != null)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("DPad_Vertical") > 0)
            {
                splash.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Credits");
                arrows.transform.localScale = new Vector3(1, -1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("DPad_Vertical") < 0)
            {
                splash.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Instructions");
                arrows.transform.localScale = new Vector3(1, 1, 1);
            }
        }    
    }
}