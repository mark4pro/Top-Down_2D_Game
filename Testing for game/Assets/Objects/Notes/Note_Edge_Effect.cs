using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Note_Edge_Effect : MonoBehaviour
{
    public GameObject Light;
    public Transform NoteWithEffect;
    public Transform NoteWithNoEffect;
    public Boolean LightOnAlways = false;
    public Boolean LightOnWhenInTrigger = false;

    void Update()
    {
        //Set up/Show light.
        if (LightOnAlways == true)
        {
            Light.SetActive(true);
        }
        if (LightOnAlways == false && LightOnWhenInTrigger == false)
        {
            Light.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        NoteWithEffect.transform.position = new Vector3(NoteWithEffect.transform.position.x, NoteWithEffect.transform.position.y, this.transform.position.z);
        NoteWithNoEffect.transform.position = new Vector3(NoteWithNoEffect.transform.position.x, NoteWithNoEffect.transform.position.y, 1);
        if (LightOnWhenInTrigger == true)
        {
            Light.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        NoteWithEffect.transform.position = new Vector3(NoteWithEffect.transform.position.x, NoteWithEffect.transform.position.y, 1);
        NoteWithNoEffect.transform.position = new Vector3(NoteWithNoEffect.transform.position.x, NoteWithNoEffect.transform.position.y, this.transform.position.z);
        if (LightOnWhenInTrigger == true)
        {
            Light.SetActive(false);
        }
    }
}
