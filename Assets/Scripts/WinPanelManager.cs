using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class WinPanelManager : MonoBehaviour
{
    public Image star1;
    public Image star2;
    public Image star3;

    public TextMeshProUGUI movesText;
    public TextMeshProUGUI bestMovesText;

    public Sprite starSprite;
    public Sprite emptyStarSprite;

    public Material star1DropShadow;
    public Material star2DropShadow;
    public Material star3DropShadow;

    public Button levelsButtons;
    public Button restart;
    public Button next;

    void Start() {
        levelsButtons.onClick.AddListener(()=> {
            //load levels scene
            LevelManager.levelMan.LoadLevel("Level Selector");
        });

        restart.onClick.AddListener(() => {
            //reload current scene
            LevelManager.levelMan.Reload();
        });

        next.onClick.AddListener(() => {
            //load next scene
            print(LevelManager.levelMan);
            LevelManager.levelMan.NextLevel();
        });
    }

    public void UpdateFields(int moves, int best, int[] movesForStars) {
        movesText.text = $"Moves : {moves}";
        bestMovesText.text = $"Best : {best}";
        UpdateStars(moves, movesForStars);
    }

    public void UpdateStars(int moves, int[] movesForStars) {
        int numOfStars = 3;
        for (int i = 0; i < 3; i++) {
            if (moves > movesForStars[i]) {
                numOfStars--;
            }
            else {
                break;
            }
        }

        star1.color = Color.black;
        star1.sprite = emptyStarSprite;
        star2.color = Color.black;
        star2.sprite = emptyStarSprite;
        star3.color = Color.black;
        star3.sprite = emptyStarSprite;
        
        float time = .5f;
        Sequence s = DOTween.Sequence();

        if (numOfStars >= 1) {
            star1.transform.localScale = new Vector3(0, 0, 0);
            star1.color = Color.white;
            star1.sprite = starSprite;
            s.Join(star1.transform.DOScale(new Vector3(1, 1, 0), time));
            s.Join(star1.transform.DOLocalRotate(new Vector3(0, 0, 360f), time, RotateMode.FastBeyond360));
        }
        if (numOfStars >= 2) {
            star2.transform.localScale = new Vector3(0, 0, 0);
            star2.color = Color.white;
            star2.sprite = starSprite;
            s.Append(star2.transform.DOScale(new Vector3(1, 1, 0), time));
            s.Join(star2.transform.DOLocalRotate(new Vector3(0, 0, 360f), time, RotateMode.FastBeyond360));
        }
        if (numOfStars >= 3) {
            star3.transform.localScale = new Vector3(0, 0, 0);
            star3.color = Color.white;
            star3.sprite = starSprite;
            s.Append(star3.transform.DOScale(new Vector3(1, 1, 0), 1f));
            s.Join(star3.transform.DOLocalRotate(new Vector3(0, 0, 360f), time, RotateMode.FastBeyond360));
        }
    }

    public void Hide() {
        gameObject.SetActive(false);
        transform.DOScale(new Vector3(1, .5f, 1), .8f).SetEase(Ease.OutBack);
    }

    public void Show() {
        gameObject.SetActive(true);
        transform.DOScale(new Vector3(1, .5f, 1), .8f).SetEase(Ease.OutBack);
    }
}
