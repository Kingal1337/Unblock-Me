using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public int[] movesForStars;//[3 stars, 2 stars, 1 stars]

    public int initExerciseLevel;//the level of the game
    public int initStars;//when you load in, it should display this many stars
    public int initBest;//when you load in, display users best

    public GameObject player;
    public GameObject allMoveablePartsParent;

    private List<BlockMovement> allBlocks = new List<BlockMovement>();//all the blocks including the player

    public FinishCollider finishCollider;//the finish line collider

    public TextMeshPro movesCounter;
    public TextMeshPro best;
    public TextMeshPro exerciseLevel;

    public SpriteRenderer star1;
    public SpriteRenderer star2;
    public SpriteRenderer star3;

    public Sprite emptyStarSprite;
    public Sprite fullStarSprite;

    public WinPanelManager winScreen;
    public ParticleSystem confetti;

    private bool canFinish;//this means that the moment the user releases their mouse the game is allowed to finish
    private int allMoves;//a total of all the moves from all the blocks

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in allMoveablePartsParent.transform) {
            GameObject go = child.gameObject;
            BlockMovement blockMovement = go.GetComponent<BlockMovement>();
            if (blockMovement != null) {
                allBlocks.Add(blockMovement);
            }
        }

        finishCollider.triggerEnter.AddListener((collider) => {
            GameObject go = collider.gameObject;
            if (go.tag.Equals("Player")) {
                canFinish = true;
            }
        });

        exerciseLevel.text = initExerciseLevel.ToString();

        LevelStats stats = LevelStatsManager.emptyStats;
        if (LevelStatsManager.manager != null) {
            stats = LevelStatsManager.manager.GetStats(initExerciseLevel);
        }

        UpdateBestMoves(stats.best.ToString());
        ApplyStarCount(stats.stars);
    }

    // Update is called once per frame
    void Update()
    {

        bool didBlocksStopMoving = true;
        foreach (BlockMovement block in allBlocks) {
            if (block.IsDragging) {
                didBlocksStopMoving = false;
            }
        }

        allMoves = 0;
        foreach (BlockMovement block in allBlocks) {
            allMoves += block.moves;
        }

        if (canFinish && didBlocksStopMoving) {
            allMoves = 0;
            foreach (BlockMovement block in allBlocks) {
                allMoves += block.moves;
            }
            canFinish = false;
            Finished();
        }

        UpdateTexts();
    }

    void UpdateTexts() {
        movesCounter.text = allMoves.ToString();
    }

    void UpdateBestMoves(string bestMoves) {
        best.text = $"Best: {bestMoves}/{movesForStars[0]}";
    }

    public void Finished() {
        confetti.Play();
        ApplyStarCount(GetNumOfStars());
        UpdateBestMoves(allMoves.ToString());
        ShowWinningPanel();
        LevelStatsManager.manager.AddUpdateStats(initExerciseLevel, GetNumOfStars(), CalculateBest());
    }

    int GetNumOfStars() {
        int numOfStars = 3;
        for (int i=0;i<3;i++) {
            if (allMoves > movesForStars[i]) {
                numOfStars--;
            }
            else {
                break;
            }
        }

        return numOfStars;
    }

    int CalculateBest() {
        LevelStats stat = LevelStatsManager.manager.GetStats(initExerciseLevel);
        if (stat.best == 0) {
            return allMoves;
        }
        int best = allMoves < stat.best ? allMoves : stat.best;
        return best;
    }

    void ApplyStarCount(int numOfStars) {
        print($"Stars : {numOfStars}");

        star1.color = Color.black;
        star1.sprite = emptyStarSprite;
        star2.color = Color.black;
        star2.sprite = emptyStarSprite;
        star3.color = Color.black;
        star3.sprite = emptyStarSprite;

        if (numOfStars >= 1) {
            star1.color = Color.white;
            star1.sprite = fullStarSprite;
        }
        if (numOfStars >= 2) {
            star2.color = Color.white;
            star2.sprite = fullStarSprite;
        }
        if (numOfStars >= 3) {
            star3.color = Color.white;
            star3.sprite = fullStarSprite;
        }
    }

    void ShowWinningPanel() {
        winScreen.UpdateFields(allMoves, CalculateBest(), movesForStars);
        winScreen.Show();
    }
}
