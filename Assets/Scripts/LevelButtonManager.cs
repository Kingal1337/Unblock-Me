using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButtonManager : MonoBehaviour
{

    public GameObject buttonParent;//the parent of all button objects

    public List<Button> allButtons = new List<Button>();

    public Sprite emptyStar;
    public Sprite star;

    // Start is called before the first frame update
    void Start() {
        foreach (Transform child in buttonParent.transform) {
            Button button = child.gameObject.GetComponent<Button>();
            button.onClick.AddListener(() => {
                TextMeshProUGUI text = null;
                foreach (Transform buttonChild in button.gameObject.transform) {
                    text = buttonChild.GetComponent<TextMeshProUGUI>();
                    if (text != null) {
                        break;
                    }
                }
                if (text != null) {
                    LevelManager.levelMan.LoadLevel($"Level{text.text}");
                }
            });

            TextMeshProUGUI text2 = null;
            foreach (Transform buttonChild in button.gameObject.transform) {
                text2 = buttonChild.GetComponent<TextMeshProUGUI>();
                if (text2 != null) {
                    break;
                }
            }
            if (text2 != null) {
                UpdateStars(button, LevelStatsManager.manager.GetStats(int.Parse(text2.text)).stars);
            }

            allButtons.Add(button);
        }
    }

    void UpdateStars(Button button, int numOfStars) {
        Image star1 = null;
        Image star2 = null;
        Image star3 = null;

        foreach (Transform buttonChild in button.gameObject.transform) {
            string name = buttonChild.name;
            if (name.StartsWith("Star")) {
                int num = int.Parse(name.Substring(name.Length-1));
                switch (num) {
                    case 1:
                        star1 = buttonChild.gameObject.GetComponent<Image>();
                        break;
                    case 2:
                        star2 = buttonChild.gameObject.GetComponent<Image>();
                        break;
                    case 3:
                        star3 = buttonChild.gameObject.GetComponent<Image>();
                        break;
                }
            }
        }

        star1.color = Color.black;
        star1.sprite = emptyStar;
        star2.color = Color.black;
        star2.sprite = emptyStar;
        star3.color = Color.black;
        star3.sprite = emptyStar;


        if (numOfStars >= 1) {
            star1.color = Color.white;
            star1.sprite = star;
        }
        if (numOfStars >= 2) {
            star2.color = Color.white;
            star2.sprite = star;
        }
        if (numOfStars >= 3) {
            star3.color = Color.white;
            star3.sprite = star;
        }
    }
}
