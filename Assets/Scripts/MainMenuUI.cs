using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{   
    [SerializeField] Animator _animation;
    public void PlayGame()
    {   
        SceneManager.LoadScene("Gameplay Scene");
    }

    public void PlayLoadingScreen()
    {   
        _animation.SetBool("isGamePlay", true);
    }
}
