using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneSwitching_cf
{
    public class PlayerCollision : MonoBehaviour
    {
        // ===== Components =====
        private Rigidbody rb; // Rigidbody
        public Button door1Button, door2Button, groundButton; // UI Buttons (assigned in Inspector)

        enum PlayerLocation
        {
            Default,
            Door1,
            Door2,
            Ground
        }; // State of player locations
        // ======================

        // ===== Player Locations =====
        // Dictionary to hold the different player locations
        // and the corresponding Vector3 positions.
        private Dictionary<PlayerLocation, Vector3> locationPositions = new Dictionary<PlayerLocation, Vector3>
        {
            { PlayerLocation.Default, new Vector3(0f, 4.8f, 1.39324f) },
            { PlayerLocation.Door1, new Vector3(-3, 4.8f, 1.39324f) },
            { PlayerLocation.Door2, new Vector3(3, 4.8f, 1.39324f) },
            { PlayerLocation.Ground, new Vector3(0f, 4.8f, 1.39324f) }
        };
        // ============================

        // ===== Script References =====
        private SceneChanger sceneChanger; // SceneChanger.cs
        // =============================


        private void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody>(); //  Get the rigidbody component

            // ===== References =====
            sceneChanger = SceneChanger.Instance; // Access SceneChanger.cs
        }

        private void Start()
        {
            // Set the player's initial location
            // SetPlayerLocation(default);

            // Set this to false by default
            rb.useGravity = false;
            CheckForClicks();
        }

        // This method checks for button clicks. Called at Runtime.
        void CheckForClicks()
        {
            // Add listeners to the buttons to check when they are clicked. Here I am using
            // lambda expressions to pass the button name to the TaskOnClick method. This is
            // because standard Unity button listeners do not support parameters.
            door1Button.onClick.AddListener(() => TaskOnClick(door1Button.name));
            door2Button.onClick.AddListener(() => TaskOnClick(door2Button.name));
            groundButton.onClick.AddListener(() => TaskOnClick(groundButton.name));
        }

        void TaskOnClick(string buttonName)
        {
            // Print out what button has been clicked
            Debug.Log($"You have clicked the button: {buttonName}");

            switch (buttonName)
            {
                case "Door1Button":
                    // Move the player above Door1's location
                    SetPlayerLocation(PlayerLocation.Door1);
                    break;
                case "Door2Button":
                    // Move the player above Door2's location
                    SetPlayerLocation(PlayerLocation.Door2);
                    break;
                case "GroundButton":
                    // Move the player above the ground location
                    SetPlayerLocation(PlayerLocation.Ground);
                    // ^^^ Nothing changes, this just enables gravity
                    break;
            }
        }

        // Called when specific buttons are clicked
        void SetPlayerLocation(PlayerLocation location)
        {
            // Move the player to the specified locatio
            transform.position = locationPositions[location];
            Debug.Log($"Player moved to {location} at position {transform.position}");

            // Enable gravity so the player can fall
            rb.useGravity = true;
        }

        // In a 3D scene, we have attached this script to the Player.
        private void OnCollisionEnter(Collision other)
        {
            // When the player's collider comes in contact with another
            // object's collider, check the tag of that object.
            switch (other.gameObject.tag)
            {
                // In this scenario, I have set the tags of the two brown cubes
                // as "Door1" and "Door2" to transport them to different scenes
                // You can move the red cube above either object to test it.
                // At runtime, the red cube will hit one of the cubes and load
                // up a different scene.

                // If they hit an object tagged Door, check the name of that object.
                case "Door":
                    switch (other.gameObject.name)
                    {
                        case "Door1":
                            sceneChanger.LoadScene("SC_Level1"); // Load SC_Level1.unity
                            break;
                        case "Door2":
                            sceneChanger.LoadScene("MainMenu"); // Load MainMenu.unity
                            break;
                    }

                    break;

                // If they hit an object tagged Ground, print a message.
                case "Ground":
                    Debug.Log("Player has hit the ground.");
                    break;
            }
        }

    }
}
