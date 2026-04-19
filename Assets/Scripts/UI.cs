using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private TMP_Text score;

    [SerializeField] private Image hotbar1;
    [SerializeField] private Image hotbar2;
    [SerializeField] private GameObject menu;
    public bool menuOpen = false;

    private GameObject deathBackground;
    private GameObject restart;
    private GameObject mainMenu;
    private GameObject deathText;

    private int maxButtons;

    [SerializeField] private GameObject GameOver;
    void Start()
    {
        score = GameObject.Find("Score").GetComponent<TMP_Text>();
        maxButtons = GameObject.FindGameObjectsWithTag("Button").Length;
        UpdateScore(0);
        HideMenu();
    }

    void UpdateScore(int buttons)
    {
        if (maxButtons == buttons)
        {
            score.text = "ALL BUTTONS ACTIVATED\nGET TO THE DOOR";
        }
        else
        {
            score.text = "Buttons Pressed: "+buttons+"/"+maxButtons;
        }
    }

    private void SelectHotbarButton(short button)
    {
        // Vector4 selectedColor = new Vector4(95 / 255, 180 / 255, 255 / 255, 1);
        if (button == 1)
        {
            hotbar1.color = Color.cyan;
            hotbar2.color = Color.white;
        }
        else
        {
            hotbar1.color = Color.white;
            hotbar2.color = Color.cyan;
        }
    }

    private void ShowDeath()
    {
        GameOver.SetActive(true);
        Animation a = GameOver.GetComponent<Animation>();
        a.Play();
    }
    private void ShowMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        menuOpen = true;
        menu.SetActive(true);
    }

    private void HideMenu()
    {
        menuOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        menu.SetActive(false);
    }

    private void Menu()
    {
        SceneManager.LoadScene("Main");
    }

    private void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
