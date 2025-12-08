using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SearchService;
using UnityEngine.UI;

public class ApartmentDialogue : MonoBehaviour
{

    public GameObject textBackground;
    public TextMeshProUGUI dialogueBox;
    public bool fadeIn = false;

    [Header("Duration")]
    public float timer = 0;
    public float maxTime = 3;

    [Header("Messages")]
    public string lastMessage = string.Empty;
    public string controls = "Use W A S D to move around in the world";
    public string cameraControls = "Use your mouse to move the camera";
    public string interactControls = "Use E to interact with items in the world";
    public string standardReturnMessage = "What a day of work.\nSomething doesn't feel right about that place";
    public string goToWorkMessage = "Welp.\nGuess it's time for work.\nI wonder how today will go";

    [Header("Stages")]
    public bool introductionCompleted = false;
    public bool standardReturnMessageCompleted = false;
    public bool goToWorkMessageCompleted = false;

    public GameObject global;
    public float maxBackgroundA = 0.227451f;


    void Start()
    {

        global = GameObject.FindGameObjectWithTag("Global");
        textBackground = transform.GetChild(0).gameObject;
        
        if (global.GetComponent<InterSceneData>().factoryDay != 0)
        {

            introductionCompleted = true;
            standardReturnMessageCompleted = !global.GetComponent<InterSceneData>().factoryCompleted;

        }
        else
        {
            standardReturnMessageCompleted = true;
        }

        goToWorkMessageCompleted = global.GetComponent<InterSceneData>().factoryCompleted;

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
                if (dialogueBox.text.Contains(controls))
                {
                    dialogueBox.text = interactControls;
                    introductionCompleted = true;
                    timer = 0;
                }
                else if (dialogueBox.text.Contains(string.Empty))
                {
                    dialogueBox.text = controls + "\n" + cameraControls;
                    timer = 0;
                }
            }
            else if (dialogueBox.color.a <= 0f && !standardReturnMessageCompleted)
            {
                dialogueBox.text = standardReturnMessage;
                standardReturnMessageCompleted = true;
                timer = 0;
            }
            else if (dialogueBox.color.a <= 0f && !goToWorkMessageCompleted)
            {
                dialogueBox.text = goToWorkMessage;
                goToWorkMessageCompleted = true;
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
