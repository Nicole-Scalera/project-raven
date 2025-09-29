using System;
using UnityEngine;
using UnityCommunity.UnitySingleton;

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

    }

}