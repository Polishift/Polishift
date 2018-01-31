using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit to damien_oconnell from http://forum.unity3d.com/threads/39513-Click-drag-camera-movement
// for using the mouse displacement for calculating the amount of camera movement and panning code.
using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour
{
    //
    // VARIABLES
    //

    public float turnSpeed = 4.0f; // Speed of camera turning when mouse moves in along an axis
    public float panSpeed = 4.0f; // Speed of the camera when being panned
    public float zoomSpeed = 4.0f; // Speed of the camera going back and forth

    private Vector3 mouseOrigin; // Position of cursor when mouse dragging starts
    private bool isPanning; // Is the camera being panned?
    private bool isRotating; // Is the camera being rotated?
    private bool isZooming; // Is the camera zooming?

    //
    // UPDATE
    //

    void Update()
    {
        // Get the right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            mouseOrigin = Input.mousePosition;
            isPanning = true;
        }

        // Get the middle mouse button
        if (Input.GetMouseButtonDown(2))
        {
            mouseOrigin = Input.mousePosition;
            isZooming = true;
        }

        // Disable movements on button release
        if (!Input.GetMouseButton(0)) isRotating = false;
        if (!Input.GetMouseButton(1)) isPanning = false;
        if (!Input.GetMouseButton(2)) isZooming = false;

        // Rotate camera along X and Y axis
        if (isRotating)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
            transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
        }

        // Move the camera on it's XY plane
        if (isPanning)
        {
            if (Camera.main.transform.position.y <= 1094.609 && Camera.main.transform.position.y >= 700 && 
                Camera.main.transform.position.x >= -104.2855 && Camera.main.transform.position.x <= 610.1899)
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

                Vector3 move = new Vector3(pos.x * panSpeed, pos.y * panSpeed, 0);
                transform.Translate(move, Space.Self);
            }
            //xpan wrong
            if (Camera.main.transform.position.x <= -104.2855)
            {
                //go right lil bit
                Vector3 move = Vector3.right;
                transform.Translate(move, Space.World);
            }
            if (Camera.main.transform.position.x >= 610.1899)
            {
                //go left lil bit
                Vector3 move = Vector3.left;
                transform.Translate(move, Space.World);
            }
            //ypan wrong
            if (Camera.main.transform.position.y >= 1094.609)
            {
                //Go down lil bit
                Vector3 move = Vector3.down;
                transform.Translate(move, Space.World);
            }
            if (Camera.main.transform.position.y <= 700)
            {
                //Go up lil bit
                Vector3 move = Vector3.up;
                transform.Translate(move, Space.World);
            }
        }

        // Move the camera linearly along Z axis
        if (isZooming)
        {
            if (Camera.main.transform.position.z < -214.8676 && Camera.main.transform.position.z > -891)
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

                Vector3 move = pos.y * zoomSpeed * transform.forward;
                transform.Translate(move, Space.World);
            }
            if (Camera.main.transform.position.z >= -214.8676)
            {
                //zoom back lil bit
                Vector3 move = Vector3.back;
                transform.Translate(move, Space.World);
            }
            if (Camera.main.transform.position.z <= -891)
            {
                //zoom in lil bit
                Vector3 move = Vector3.forward;
                transform.Translate(move, Space.World);
            }
        }
    }
}