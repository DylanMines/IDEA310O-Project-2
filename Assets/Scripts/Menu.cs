using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject selection;
    public string nextScene;

    void Start()
    {
        
    }

    void Start1Pressed()
    {
        selection.SetActive(false);
        tutorial.SetActive(true);
    }

    void Start2Pressed()
    {
        SceneManager.LoadScene(nextScene);
    }

    void QuitPressed()
    {
        Application.Quit();
    }


}