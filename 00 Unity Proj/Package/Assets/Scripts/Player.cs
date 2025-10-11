using Unity.VisualScripting;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.InputSystem;

// This is the Player class that will allow us to get specific data about the Player.

public class Player : MonoSingleton<Player>
{
        [SerializeField] private Rigidbody rb; // Rigidbody
        [SerializeField] private PlayerInput _playerInput;

        void Awake()
        {
                if (_playerInput == null)
                {
                        _playerInput = GetComponent<PlayerInput>();
                }
        }

        // Returns the PlayerInput component attached to the player
        public PlayerInput GetPlayerInput()
        {
                return _playerInput;
        }

}