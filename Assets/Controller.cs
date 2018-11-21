using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using ARobj;
using System.IO;
using ARIO;
using System.Linq;

public class Controller : MonoBehaviour
{
    //CAUTION: XML may not be loaded in Android platform

    //TODO: SQL server data storage and access (may do)

    public GameObject[] Targets;

    public BoxCollider[] boxes;
    private List<BoxCollider> boxesPresent = new List<BoxCollider>();
    private string recognizedString;

    private ObjectData[] objList;

    private GameObject obj;

    private readonly string filepath = Path.Combine(Environment.CurrentDirectory,"data.xml");  //name of the xml file MUST be data.xml

    //Events
    private string lastString = "";

    private bool isObjectModeOn = false;
    private bool isFirstInstant;

    // Use this for initialization
    void Start()
    {   
        ObjectLoader loader = new ObjectLoader();
        objList = loader.LoadXML(this.filepath);
        if (objList == null)
            throw new Exception("FAILIED TO LOAD XML FILE");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isObjectModeOn)
        {
            foreach (BoxCollider bc in boxes)
            {
                if (bc.enabled)
                {
                    if (onStateChanged(bc.name))
                    {
                        boxesPresent.Add(bc);
                        GameObject.Find("ARcontrol").GetComponent<UIControl>().DisplayLetterOnCenter(bc.name);
                        lastString = bc.name;
                    }

                }
            }
        }
        else
        {
            
            try
            {
                if (isFirstInstant)
                {
                    foreach (BoxCollider bc in boxesPresent)
                    {
                        obj = Instantiate(Resources.Load(recognizedString)) as GameObject;
                        obj.tag = "Respawn";
                        obj.transform.parent = bc.transform.parent;
                        obj.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                        obj.transform.localPosition = new Vector3(0, 0.5f, 0);
                        obj.transform.localRotation = new Quaternion(0, 0, 0, 0);
                        obj.SetActive(true);
                    }
                    isFirstInstant = false;
                }
            }
            catch (Exception e) { Debug.Log(e.Message); }
        }
    }

    private bool onStateChanged(string currentString)
    {
        if (currentString != lastString)
            return true;
        else
            return false;
    }

    //Called by users
    public void DoneScanning()
    {
        if (boxesPresent != null)
        {
            recognizedString = "I";

            foreach (BoxCollider bc in boxesPresent)
            {
                recognizedString += bc.name;
            }
            isObjectModeOn = true;
            isFirstInstant = true;
        }
        else
            recognizedString = "";
        Debug.Log(recognizedString);
    }

    public void StartScanning()
    {
        GameObject[] objToDestroy = GameObject.FindGameObjectsWithTag("Respawn");
        foreach(GameObject o in objToDestroy)
        {
            Destroy(o);
        }
        GameObject.Find("ARcontrol").GetComponent<UIControl>().ResetText();
        isObjectModeOn = false;
        boxesPresent.Clear();
    }
}
