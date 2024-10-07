using UnityEngine;
using TMPro;

// Main menu UI takes care of setting up generators and information about the game like difficulty or maps size

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuUI;
    public GameObject NewGameUI;
    public GameObject OptionsUI;
    public TextMeshProUGUI SeedText;
    public Loading LoadingManager;

    public GameObject NewGameMenu;
    public GameObject SavedGameMenu;

    private int generationType = 0;
    private int mapSize = 0;
    private int difficulty = 0;
    private int seed = 0;
    private string seedString = "";

    private void Start() {
        seed = Random.Range(0, 999999999);
        SeedText.text = seed.ToString();
        seedString = seed.ToString();

        if(PlayerPrefs.GetInt("Saved") == 1) {
            NewGameMenu.SetActive(false);
            SavedGameMenu.SetActive(true);
        } 
        else {
            NewGameMenu.SetActive(true);
            SavedGameMenu.SetActive(false);
        }

        // PlayerPrefs.DeleteAll(); // This line can be uncomented to delete all save data so we can try if UI work as intended
    }

    public void ToggleNewGame() {
        MainMenuUI.SetActive(!MainMenuUI.activeInHierarchy);
        NewGameUI.SetActive(!NewGameUI.activeInHierarchy);
    }

    public void StartNewGame(string levelName) {
        PlayerPrefs.SetInt("Gen", generationType);
        PlayerPrefs.SetInt("Size", 35 + mapSize * 15);
        PlayerPrefs.SetInt("Diff", difficulty);
        PlayerPrefs.SetInt("Seed", seed);
        PlayerPrefs.SetString("SeedString", seedString);
        PlayerPrefs.SetInt("Stage", 0);
        PlayerPrefs.SetInt("Saved", 1);
        LoadingManager.AsyncLoad(levelName);
    }

    public void Continue(string levelName) {
        LoadingManager.AsyncLoad(levelName);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void ChangeGeneration(int value) {
        generationType = value;
    }

    public void ChangeMapSize(int value) {
        mapSize = value;
    }

    public void ChangeDifficulty(int value) {
        difficulty = value;
        Debug.Log(difficulty);
    }

    public void ChangeSeed(string value) {
        seedString = value;
        seed = value.GetHashCode();
    }
}
