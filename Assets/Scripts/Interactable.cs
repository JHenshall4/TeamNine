﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interactable : MonoBehaviour {

    public enum Selection
    {
        PickupableObject,
        AttachToThisObject,
        Trigger_Brain,
        ProjectorToggle
    };

    public Selection ScriptType;

    private float distanceFromPlayer;
    public GameObject attachableObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        distanceFromPlayer = GameVariables.DistanceFromPlayer(gameObject);
    }


    //If the mouse enters this object, assume an attempt to interact.
    public void OnMouseEnter()
    {
        if (distanceFromPlayer < 30.0)
        {
            GameVariables.interactAttempt = true;
        }

        if (distanceFromPlayer > 30.0)
        {
            GameVariables.interactAttempt = false;
        }
    }

    public void OnMouseExit()
    {
        GameVariables.interactAttempt = false;
    }


    public void OnMouseUp()
    {
        switch (ScriptType)
        {
            case Selection.PickupableObject:
                PickupableObject();
                    break;

            case Selection.AttachToThisObject:
                AttachToThisObject();
                break;

            case Selection.Trigger_Brain:
                TriggerBrain();
                break;

            case Selection.ProjectorToggle:
                ProjectorToggle();
                break;

             
        }
    }


    // = = = = = = = = = = = = //
    // Unique Events Go Here! //
    // = = = = = = = = = = = = //

    public void PickupableObject()
    {
        if (!GameVariables.playerHasObject && distanceFromPlayer < GameVariables.InteractRange)

        {
            //Remove physics from this object.
            this.GetComponent<Collider>().enabled = false;
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<Rigidbody>().useGravity = false;

            //attach object to the player.
            transform.parent = GameVariables.Player.transform;
            this.transform.localPosition = new Vector3(0.63f, 0.25f, 1.20f);
            this.transform.localEulerAngles = new Vector3(0, 0, 0);

            //Player is now holding an object
            GameVariables.playerHasObject = true;
            GameVariables.playerHoldingObject = gameObject;
        }
    }

    public void AttachToThisObject()
    {
        //Find out if the player is holding an object.
        if (GameVariables.playerHoldingObject == attachableObject && distanceFromPlayer < GameVariables.InteractRange)
        {
            //Player is no longer holding the object.
            GameVariables.playerHasObject = false;

            //Store the game object locally otherwise later we will be referencing a null object.
            GameObject holdingObject = GameVariables.playerHoldingObject;

            //Remove object from the player (Do we need this?)
            //   GameVariables.playerHoldingObject.transform.parent = null;

            //Attach the object to this object
            holdingObject.transform.parent = transform;
            holdingObject.transform.localPosition = new Vector3(0.002f, 0.391f, 0.017f);
            holdingObject.transform.localEulerAngles = new Vector3(0, 0, 0);

        }
    }

    public void TriggerBrain()
    {
        try
        {
            if (GameObject.Find("Head").transform.Find("fNRIS Hat").name == "fNRIS Hat")
            {
                GameObject BrainBit = (GameObject)Instantiate(Resources.Load("brain2_prefab"));
                BrainBit.transform.position = new Vector3(0.11f, 3.71f, -0.57f);
                BrainBit.transform.Rotate(0f, 180f, 0f);
                Destroy(GetComponent<Interactable>());
            }

        }
        catch (NullReferenceException e)
        {
            //do nothing
        }

        return;
    }


    public void ProjectorToggle()
    {
        if (attachableObject.active == true)
        {
            attachableObject.active = false;
        }
        else
        {
            attachableObject.active = true;
        }
        
    }
}
