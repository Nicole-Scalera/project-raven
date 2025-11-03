using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneSwitching_cf
{
    public class SceneChangeIndividualButton :  MonoBehaviour
    {
        public string scene = "<Insert scene name>";

        public void LoadScene()
        {
            SceneManager.LoadScene(scene);
        }
    }
}