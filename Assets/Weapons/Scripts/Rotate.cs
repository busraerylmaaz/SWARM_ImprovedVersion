using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Camera cam;

    public SpriteRenderer weaponSR;
    public SpriteRenderer handsSR;
    public SpriteRenderer secondHandsSR;
    public Transform playerPos;
    private Transform pos;

    void Start()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerPos = playerController.transform;
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene!");
        }
        pos = GetComponent<Transform>();
        cam = Camera.main;

        Transform weaponTransform = transform.GetChild(0);
        weaponSR = weaponTransform.GetComponent<SpriteRenderer>();

        Transform handsTransform = weaponTransform.Find("Hands");
        handsSR = handsTransform.GetComponent<SpriteRenderer>();

        
        Transform secondHandsTransform = weaponTransform.Find("secondHands");
        if (secondHandsTransform != null)
        {
            secondHandsSR = secondHandsTransform.GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        float scaleOffset = 0f;
        if (pos.parent.localScale.x < 0) scaleOffset = 0f;

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = (mousePos - (Vector2)pos.position).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        pos.rotation = Quaternion.Euler(0, 0, angle + scaleOffset);

        
        pos.localScale = new Vector3(pos.parent.localScale.x, pos.localScale.y, pos.localScale.z);

        
        float zRotation = pos.rotation.eulerAngles.z;

        
        Vector2 lookDirPlayer = (mousePos - (Vector2)playerPos.position).normalized;
        float anglePlayer = Mathf.Atan2(lookDirPlayer.y, lookDirPlayer.x) * Mathf.Rad2Deg;
        float eastToNorthEast = 22.5f; 
        float northWestToWest = 157.5f; 
        if (anglePlayer > eastToNorthEast && anglePlayer < northWestToWest)
        {
            
            weaponSR.sortingOrder = 1;
            handsSR.sortingOrder = 1;
            if (secondHandsSR != null)
            {
                secondHandsSR.sortingOrder = 1;
            }
        }
        else
        {
            
            weaponSR.sortingOrder = 3;
            handsSR.sortingOrder = 4;
            if (secondHandsSR != null)
            {
                secondHandsSR.sortingOrder = 4;
            }
        }

        
        if (zRotation > 180) zRotation -= 360;

        if ((zRotation < -90 && zRotation >= -180) || (zRotation > 90 && zRotation <= 180))
        {
            pos.localScale = new Vector3(pos.localScale.x, -1, pos.localScale.z);
        }
        else
        {
            pos.localScale = new Vector3(pos.localScale.x, 1, pos.localScale.z);
        }

        if (Input.GetMouseButton(0))
        {
            weaponSR.enabled = true; 
            handsSR.enabled = true; 
            if (secondHandsSR != null)
            {
                secondHandsSR.enabled = true; 
            }
        }
        else
        {
            weaponSR.enabled = false; 
            handsSR.enabled = false; 
            if (secondHandsSR != null)
            {
                secondHandsSR.enabled = false; 
            }
        }
    }
}