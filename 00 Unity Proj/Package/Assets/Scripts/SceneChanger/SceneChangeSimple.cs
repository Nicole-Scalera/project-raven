using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneSwitching_cf
{
    public class SceneChangeSimple :  MonoBehaviour
    {
        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}