using UnityEngine;
using System.Collections;

#region - Script Synopsis
/*
This is the main script for controlling the player's movement, including teleporting, teleport cooldown,
and confining the player to the boundaries of the "arena."
*/
#endregion

public class CubeController : MonoBehaviour
{
    //Stores the player's position and speed, respectively
    Vector3 Move;
    public static float Speed = 0.07f;

    //Height and width of the "arena" in Unity units
    const float CamWidthX = 6.2f;
    const float CamHeightY = 3.4f;

    //Set according to the values of CamWidthX/CamHeight to establish
    //the edges of the screen
    float LeftEdge;
    float RightEdge;
    float BottomEdge;
    float TopEdge;

    //Stores the amount the player teleports once engaged, in Unity units.
    float Teleport;

    //Stores whether or not player diappears once teleport is engaged. Set in CubeAnimate script.
    public bool IsDisappear;

    //Cooldown counter for teleport (can't re-enage until back down to zero)
    public static Timer TeleportCool { get; private set; }

    void Start()
    {
        Move = transform.position;
        IsDisappear = false;

        LeftEdge = CamWidthX * -1;
        RightEdge = CamWidthX;
        BottomEdge = CamHeightY * -1;
        TopEdge = CamHeightY;

        TeleportCool = new Timer(0);
    }

    void Update()
    {
        TeleportCheck();
        Movement();
        BoundaryCheck();
    }


    //Checks to see if teleport has been engaged
    private void TeleportCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Teleport"))
        {
            if (TeleportCool.Counter == 0)
            {
                Teleport = 2f;
                IsDisappear = true;
                TeleportCool.Counter = 500;
                GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            Teleport = 0;
            IsDisappear = false;
        }

        TeleportCool.RunReverse();
    }


    //Handles movement for the player
    private void Movement()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0 || Input.GetAxis("DPad_Horizontal") < 0)
            Move.x -= Speed * Timer.DeltaTimeMod + Teleport;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("DPad_Horizontal") > 0)
            Move.x += Speed * Timer.DeltaTimeMod + Teleport;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Vertical") < 0 || Input.GetAxis("DPad_Vertical") < 0)
            Move.y += Speed * Timer.DeltaTimeMod + Teleport;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("Vertical") > 0 || Input.GetAxis("DPad_Vertical") > 0)
            Move.y -= Speed * Timer.DeltaTimeMod + Teleport;

        transform.position = Move;
    }

    //Check if at the edge of screen and constrain movement if reached
    private void BoundaryCheck()
    {
        if (transform.position.x < LeftEdge)
            Move.x = LeftEdge;
        if (transform.position.x > RightEdge)
            Move.x = RightEdge;
        if (transform.position.y < BottomEdge)
            Move.y = BottomEdge;
        if (transform.position.y > TopEdge)
            Move.y = TopEdge;

        transform.position = Move;
    }
}