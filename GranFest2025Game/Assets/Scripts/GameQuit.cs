using UnityEngine;

public class GameQuit : MonoBehaviour
{
    MasterInput controls;

    private void Awake()
    {
        controls= new MasterInput();
        controls.Enable();
    }

    public void OnQuit()
    {
        print("Quit");
        Application.Quit();
    }
}
