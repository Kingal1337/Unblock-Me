using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats {
    public int level;
    public int stars;
    public int best;

    public LevelStats(int level, int stars, int best) {
        this.level = level;
        this.stars = stars;
        this.best = best;
    }
}

public class LevelStatsManager : MonoBehaviour
{
    public static LevelStatsManager manager;
    public static LevelStats emptyStats = new LevelStats(0, 0, 0);
    public Dictionary<int, LevelStats> stats = new Dictionary<int, LevelStats>();
    // Start is called before the first frame update
    void Awake()
    {
        if (manager == null) {
            manager = this;
            DontDestroyOnLoad(this);
        }
        else {
            print("Level States Manager already exists");
            Destroy(this);
        }
    }

    public LevelStats GetStats(int level) {
        if (stats.ContainsKey(level)) {
            return stats[level];
        }
        return emptyStats;
    }

    public void AddUpdateStats(int level, int stars, int best) {
        if (stats.ContainsKey(level)) {
            LevelStats stat = stats[level];
            stat.best = best;
            stat.stars = stars;
        }
        else {
            LevelStats stat = new LevelStats(level, stars, best);
            stats.Add(level, stat);
        }
    }
}
