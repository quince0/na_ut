﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class respawn : MonoBehaviour {

    public static int levelN = 0;

    private Vector3 startPos;
    private Quaternion startRot;
	// Use this for initialization
	void Start () {
        startPos = transform.position;
        startRot = transform.rotation;
	}



    //detect collision with collider
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "death")
        {
            transform.position = startPos;
            transform.rotation = startRot;
            GetComponent<Animator>().Play("LOSE00", -1, 0f);
            GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
        }
        else if(col.tag == "checkpoint")
        {
            startPos = col.transform.position;
            startRot = col.transform.rotation;
            Destroy(col.gameObject);
        }
        else if (col.tag == "falseFloor")
        {
            Destroy(col.gameObject);
        }

        else if (col.tag == "goal")
        {
            Destroy(col.gameObject);
        }
    }
    
}
