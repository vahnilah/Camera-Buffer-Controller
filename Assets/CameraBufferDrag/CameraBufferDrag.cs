using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Attach a camera & player object to this class and then assign the correct joystick code used for your project.
 * 
 * This allow the camera to create a buffer of movement to pan the camera on the joystick direction
 * The decay rates can be adjusted the speed of the camera when moving back to center on the player
 */
[ExecuteInEditMode]
public class CameraBufferDrag : MonoBehaviour
{
    public GameObject thisCamera;
    public GameObject thisPlayer;
    public string LeftJoystickXAxisCode = "LeftJoystickX";
    public string LeftJoystickYAxisCode = "LeftJoystickY";

    private float BufferX = 0f;
    private float BufferY = 0f;

    [Space(20)]
    public bool isVerticalMovementOnZAxis = false;
    private float X;
    public float AdjustX = 0f;
    private float Y;
    public float AdjustY = 0f;
    public float AdjustZoom = 0f;

    [Space(20)]
    public float BufferOfX = 10f;
    public float BufferOfY = 10f;

    [Space(20)]
    public float decayRate = 2f;
    public float ResistanceBufferOfX = 3f;
    public float ResistanceBufferOfY = 3f;
    public float StartingDecayBufferOfX = 50f;
    public float EndingDecayBufferOfX = 11f;
    public float StartingDecayBufferOfY = 50f;
    public float EndingDecayBufferOfY = 11f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Change which axis vertical is on
        if (isVerticalMovementOnZAxis == true)
        {
            var position = thisCamera.transform.position;
            position.x = X + AdjustX;
            position.z = Y + AdjustY;
            position.y = AdjustZoom;
            thisCamera.transform.position = position;
        } else
        {
            var position = thisCamera.transform.position;
            position.x = X + AdjustX;
            position.y = Y + AdjustY;
            position.z = AdjustZoom;
            thisCamera.transform.position = position;
        }

        float playerPosX;
        float playerPosY;

        // Change which axis vertical is on
        if (isVerticalMovementOnZAxis == true)
        {
            playerPosX = thisPlayer.transform.position.x;
            playerPosY = thisPlayer.transform.position.z;
        }
        else
        {
            playerPosX = thisPlayer.transform.position.x;
            playerPosY = thisPlayer.transform.position.y;
        }

        var cameraFollowsX = playerPosX;
        var cameraFollowsY = playerPosY;

        var cameraDragY = 0f;
        var cameraDragX = 0f;

        // Camera buffer
        CameraBuffer();
        cameraDragX += BufferX;
        cameraDragY += BufferY;

        X = cameraFollowsX + cameraDragX;
        Y = cameraFollowsY + cameraDragY;
    }

    // Create a camera movement buffer for the direction of the player of the char
    // This will auto adjust back to the player when the joystick is released
    private void CameraBuffer()
    {
        // Add camera drag on X
        var movementX = Input.GetAxis(LeftJoystickXAxisCode);
        if (movementX > 0 && BufferX < BufferOfX)
            BufferX += movementX / ResistanceBufferOfX;
        if (movementX < 0 && BufferX > -BufferOfX)
            BufferX += movementX / ResistanceBufferOfX;

        // Add camera drag on Y
        var movementY = Input.GetAxis(LeftJoystickYAxisCode);
        if (movementY > 0 && BufferY < BufferOfY)
            BufferY += movementY / ResistanceBufferOfY;
        if (movementY < 0 && BufferY > -BufferOfY)
            BufferY += movementY / ResistanceBufferOfY;

        // Normalize 
        var BufferXNorm = BufferX / StartingDecayBufferOfX;
        var BufferYNorm = BufferY / StartingDecayBufferOfY;
        
        // Get c from a^2 + b^2 = c^2
        var c = Mathf.Sqrt(BufferXNorm * BufferXNorm + BufferYNorm * BufferYNorm);

        // Obtain the ratio to decay with
        var xRatio = (BufferXNorm / c);
        var yRatio = (BufferYNorm / c);

        // Adjust the speed of decay at different points of the decay
        var ratioBufferOfX = BufferX / BufferOfX;
        var ratioBufferOfY = BufferY / BufferOfY;
        float DecayBufferOfX = ratioBufferOfX * StartingDecayBufferOfX / EndingDecayBufferOfX;
        float DecayBufferOfY = ratioBufferOfY * StartingDecayBufferOfY / EndingDecayBufferOfY;

        // Renormalize X with the decay rate to self adjust camera at an even rate with Y
        if (Mathf.Abs(movementX) < .05f)
        {
            var adj = Mathf.Abs(xRatio / decayRate * DecayBufferOfX);
            if (Mathf.Abs(BufferX) < .20f)
                BufferX = 0;
            else if (BufferX > 0)
                BufferX -= Time.deltaTime * adj;
            else if (BufferX < 0)
                BufferX += Time.deltaTime * adj;
        }

        // Renormalize Y with the decay rate to self adjust camera at an even rate with X
        if (Mathf.Abs(movementY) < .05f)
        {
            var adj = Mathf.Abs(yRatio / decayRate * DecayBufferOfY);
            if (Mathf.Abs(BufferY) < .20f)
                BufferY = 0;
            else if (BufferY > 0)
                BufferY -= Time.deltaTime * adj;
            else if (BufferY < 0)
                BufferY += Time.deltaTime * adj;
        }
    }
}
