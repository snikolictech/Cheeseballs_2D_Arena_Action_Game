using UnityEngine;
using System.Collections;

#region - Script Synopsis
/*
This script is responsible for displaying the Score, PowerUp Meter, and CoolDown counter as text that scales
depending on the size of the screen.
*/
#endregion

public class HeadsUpDisplay : MonoBehaviour
{
    //Stores the font size (which scales to the size of the screen)
    private int FontSize;

    //The fontstyle (set in the inspector) for the resources in the upper-left of the screen
    public GUIStyle FontStyleResources = new GUIStyle();

    //The fontstyle (set in the inspector) for the scoreboard in the bottom-left of the screen
    public GUIStyle FontStyleScore = new GUIStyle();

    //A small offet (set in the inspector) used to create a dropshadow effect
    public int Offset;

    void Start()
    {
        FontStyleResources.font = FontStyleScore.font = Resources.Load<Font>("Fonts/trebucbd");
    }


    //Displays the "HUD" as a GUI overlay
    void OnGUI()
    {
        ScaleFontSize();
        FontBloom(FontStyleScore);

        if (WorldManager.GrandTotalScore > 0)
        {
            GUI.Label(new Rect(Screen.width / 85f + Offset, Screen.height / 1.12f + Offset, Screen.width, Screen.height),
                WorldManager.GrandTotalScore.ToString(), FontStyleScore
                );
        }

        GUI.Label(new Rect(Screen.width / 85f + Offset, Screen.height / 48 + Offset, Screen.width, Screen.height),
            string.Format("PowerUP: {0}", Mathf.RoundToInt(PowerUpManager.PowerUpMeter.Counter).ToString()), FontStyleResources
            );

        GUI.Label(new Rect(Screen.width / 85f + Offset, Screen.height / 16 + Offset, Screen.width, Screen.height),
            string.Format("CoolDown: {0}", Mathf.RoundToInt(CubeController.TeleportCool.Counter).ToString()), FontStyleResources
            );
    }

    //Scales the font relative to the size of the screen
    private void ScaleFontSize()
    {
        FontSize = Screen.width / 64;
        FontStyleResources.fontSize = FontSize;
    }

    //Scales the score in a "blooming" effect when adding to the score
    private void FontBloom(GUIStyle fontToScale)
    {
        int sizeFactor = (WorldManager.IsScoring) ? FontSize * 4 : FontSize * 2;
        fontToScale.fontSize = (int)Mathf.Lerp(fontToScale.fontSize, sizeFactor, 0.09f * Timer.DeltaTimeMod);
    }
}