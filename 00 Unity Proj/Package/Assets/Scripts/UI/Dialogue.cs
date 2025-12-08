using System;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{

    public TextMeshProUGUI dialogueBox;
    public bool fadeIn = false;

    [Header ("Duration")]
    public float timer = 0;
    public float maxTime = 3;

    [Header ("Messages")]
    public string lastMessage = "";
    public string controls = "Use W A S D to move around in the world";
    public string cameraControls = "Use your mouse to move the camera";

    public GameObject global;


    void Start()
    {

        global = GameObject.FindGameObjectWithTag("Global");

        if (global.GetComponent<InterSceneData>().factoryDay != 1)
        {
            dialogueBox.text = "Welcome Back to the Package Co. ...";
        }
        else
        {
            dialogueBox.text = "Welcome to the Package Co. ...";
        }
        
        lastMessage = dialogueBox.text;

    }

    private void FixedUpdate()
    {

        if (lastMessage != dialogueBox.text)
        {
            fadeIn = true;
        }

        if (fadeIn)
        {
            ShowText();
            lastMessage = dialogueBox.text;
            if (dialogueBox.color.a >= 1)
            {
                fadeIn = false;
            }
        }
        else if (timer < maxTime)
        {

            timer += Time.deltaTime;

        }
        else
        {

            HideText();

        }

    }

    private void ShowText()
    {

        Color textColor = dialogueBox.color;
        textColor.a += 0.1f;
        dialogueBox.color = textColor;

    }

    private void HideText()
    {
        Color textColor = dialogueBox.color;
        textColor.a -= 0.1f;
        dialogueBox.color = textColor;
    }
}
