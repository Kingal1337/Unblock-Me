using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager levelMan;
    private string currentLevel;

    private void Awake() {
        if (levelMan == null) {
            levelMan = this;
            DontDestroyOnLoad(this);
        }
        else {
            print("Level Manager already exists");
            Destroy(this);
        }
    }

    public void LoadLevel(string level) {
        if (Application.CanStreamedLevelBeLoaded(level)) {
            currentLevel = level;
            SceneManager.LoadScene(level);
        }
        else {
            Debug.Log($"Scene \"{level}\" cannot be loaded");
        }
    }

    public void NextLevel() {
        int current = int.Parse(currentLevel.Substring(currentLevel.IndexOf("Level") + 5));
        LoadLevel($"Level{current+1}");
    }

    public void Reload() {
        LoadLevel(currentLevel);
    }

    public void LoadLevelSelector() {
        string levelSelector = "Level Selector";
        if (Application.CanStreamedLevelBeLoaded(levelSelector)) {
            SceneManager.LoadScene(levelSelector);
        }
        else {
            Debug.Log($"Scene \"{levelSelector}\" cannot be loaded");
        }
    }

    public void LoadMainMenu() {
        string mainMenu = "Main Menu";
        if (Application.CanStreamedLevelBeLoaded(mainMenu)) {
            SceneManager.LoadScene(mainMenu);
        }
        else {
            Debug.Log($"Scene \"{mainMenu}\" cannot be loaded");
        }
    }
}
