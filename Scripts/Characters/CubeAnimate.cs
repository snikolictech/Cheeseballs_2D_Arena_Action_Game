using UnityEngine;
using System.Collections;

#region - Script Synopsis
/*
This script demonstrates how a simple foreach() loop can be used to iterate through each SpriteRenderer
in the parent GameObject and its children. In this case, it's used to fade the alpha property (opacity) of each
SpriteRenderer when "Teleport" is engaged, which makes the player's individual GameObjects (body, mouth, eyes) disappear.
*/
#endregion

public class CubeAnimate : MonoBehaviour
{
	void Update()
	{
        TeleportFadeForEach();
	}


    //Iterates over each parent/child SpriteRenderer attached to this GameObject and fades it's opacity
    //when Teleport is activated in CubeController script.
    private void TeleportFadeForEach()
    {
        SpriteRenderer[] cubeRenderers = GetComponentsInChildren<SpriteRenderer>();

        CubeController controller = GetComponent<CubeController>();

        foreach (SpriteRenderer renderer in cubeRenderers)
        {
            Color color = renderer.color;

            if (controller.IsDisappear)
                color.a = 0;
            else
                color.a = Mathf.Lerp(color.a, 1f, 0.029f * Timer.DeltaTimeMod);

            renderer.color = color;
        }
    }
}