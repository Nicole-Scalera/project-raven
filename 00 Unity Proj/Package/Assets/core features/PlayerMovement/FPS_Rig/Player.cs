using System;
using UnityEngine;
using UnityCommunity.UnitySingleton;
using UnityEngine.InputSystem;

// This is the PlayerInfo class. It encapsulates general data and info about
// the Player game object, which is utilized throughout other scripts.

namespace FPS_Rig_cf
{
    public class Player : PersistentMonoSingleton<Player>
    {

        // Singleton instance for global reference
        private static PlayerControls _controls;

        // Constructor that forces only a single
        // instance of PlayerControls to be created
        public static PlayerControls Controls
        {
            get
            {
                // If Controls are null, create a new instance
                if (_controls == null)
                {
                    _controls = new PlayerControls(); // Access movement controls
                    _controls.Enable();
                }

                // Return the PlayerControls instance
                return _controls;
            }
        }
        
        // ========== Variables ==========
#if ENABLE_INPUT_SYSTEM
        // The PlayerInput component, which contains a
        // direct reference to the input actions file.
        private PlayerInput _input;
#endif
        private CapsuleCollider _capsule; // Capsule Collider
        private CharacterController _characterController; // Character Controller
        private PlayerInputProcessor _inputProcessor; // PlayerInputProcessor.cs
        private Vector3 _position; // The current position of the Player
        // ===============================
        
        void Awake()
        {
            _input = GetComponent<PlayerInput>();
        }
    
        // Get the Player's location in the scene
        public Vector3 GetPlayerPosition()
        {
            _position = transform.position;
            return _position;
        }

        // Get the Player's capsule collider
        public CapsuleCollider GetPlayerCollider()
        {
            return _capsule;
        }
        
        // Get the Player's character controller
        public CharacterController GetPlayerController()
        {
            return _characterController;
        }
        
        // Get the PlayerInput component
        public PlayerInput GetPlayerInput()
        {
            return _input;
        }
        
    }

}