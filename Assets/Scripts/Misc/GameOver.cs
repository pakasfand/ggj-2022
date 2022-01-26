using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class GameOver : MonoBehaviour
    {
        public void OnPlayerAgain()
        {
            SceneManager.LoadScene(0);
        }
    }
}
