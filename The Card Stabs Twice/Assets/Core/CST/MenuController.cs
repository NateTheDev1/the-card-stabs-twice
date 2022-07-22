using UnityEngine;
using UnityEngine.SceneManagement;

namespace CST
{
    public class MenuController : MonoSingleton<MenuController>
    {
        public void OnPlayGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}