using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private AudioSource buttonSFX;
    private bool started = false;


    public void StartLoad()
    {
        if (!started)
        {
            started = true;
            StartCoroutine(LoadLevel());
        }
    }
    void OnQuit()
    {
        print("Game Quitting!");
        Application.Quit();
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    public void PlaySFX()
    {
        buttonSFX.Play();
    }
}
