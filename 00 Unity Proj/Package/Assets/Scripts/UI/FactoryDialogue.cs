using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SearchService;
using UnityEngine.UI;

public class FactoryDialogue : MonoBehaviour
{

    public GameObject textBackground;
    public TextMeshProUGUI dialogueBox;
    public bool fadeIn = false;

    [Header("Duration")]
    public float timer = 0;
    public float maxTime = 3;

    [Header("Messages")]
    public string lastMessage = string.Empty;
    public string controls = "Use W A S D to move around in the Factory";
    public string cameraControls = "Use your mouse to move the camera";
    public string interactControls = "Use E to interact with items in the world";
    public string standardPackageMessage = "These standard packages can go right onto the trucks";
    public string goToHomeMessage = "Finally... the day is over.\nI can't wait to go home";

    [Header("Stages")]
    public bool introductionCompleted = false;
    public bool goToHomeMessageCompleted = false;

    public GameObject global;
    public float maxBackgroundA = 0.227451f;


    void Start()
    {

        global = GameObject.FindGameObjectWithTag("Global");
        textBackground = transform.GetChild(0).gameObject;
        
        if (global.GetComponent<InterSceneData>().factoryDay != 1)
        {

            introductionCompleted = true;

        }

        dialogueBox.color = new Color(1,1,1,0);
        textBackground.GetComponent<Image>().color = new Color(0,0,0,0);

    }

    private void FixedUpdate()
    {

        if (lastMessage != dialogueBox.text)
        {
            fadeIn = true;
            lastMessage = dialogueBox.text;
        }

        if (fadeIn)
        {
            ShowText();

            if (dialogueBox.color.a <= maxBackgroundA)
            {
                ShowBackground();
            }

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
            if (dialogueBox.color.a > 0)
            {
                HideText();
                if (dialogueBox.color.a <= maxBackgroundA)
                {
                    HideBackground();
                }
            }
            else if (dialogueBox.color.a <= 0f && !introductionCompleted)
            {
                if (dialogueBox.text.Contains(interactControls))
                {
                    dialogueBox.text = standardPackageMessage;
                    introductionCompleted = true;
                    timer = 0f;
                }
                else if (dialogueBox.text.Contains(controls))
                {
                    dialogueBox.text = interactControls;
                    timer = 0;
                }
                else if (dialogueBox.text.Contains(string.Empty))
                {
                    dialogueBox.text = controls + "\n" + cameraControls;
                    timer = 0;
                }
            }
            else if (dialogueBox.color.a <= 0 && global.GetComponent<InterSceneData>().factoryCompleted)
            {
                dialogueBox.text = goToHomeMessage;
                goToHomeMessageCompleted = true;
                timer = 0;
            }

        }

    }

    private void ShowText()
    {

        Color textColor = dialogueBox.color;
        textColor.a += .33f * Time.deltaTime;
        dialogueBox.color = textColor;

    }

    private void HideText()
    {
        Color textColor = dialogueBox.color;
        textColor.a -= .33f * Time.deltaTime;
        dialogueBox.color = textColor;
    }

    private void ShowBackground()
    {
        Color textColor = textBackground.GetComponent<Image>().color;
        textColor.a += .33f * Time.deltaTime;
        textBackground.GetComponent<Image>().color = textColor;
    }

    private void HideBackground()
    {
        Color textColor = textBackground.GetComponent<Image>().color;
        textColor.a -= .33f * Time.deltaTime;
        textBackground.GetComponent<Image>().color = textColor;
    }
}
