using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private int score = 0;
    [SerializeField] private int money = 0;

    [SerializeField] private FlapyBird fb = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private SpikesManager spikesManager = null;
    [SerializeField] private Coint coint = null;
    [SerializeField] private SpawnCurrency spawnCurrency = null;

    [SerializeField] private System.Action OnTouchWall;
    [SerializeField] private System.Action OnTouchCoint;
    [SerializeField] private System.Action OnpjDie;
    [SerializeField] private System.Action<int> OnScoreChange;
    [SerializeField] private System.Action<int> OnMoneyChange;

    private Vector3 pjPos;
    private void Start()
    {
        spikesManager.Init(ref OnTouchWall);
        OnTouchCoint += Added1Money;
        coint.Init(ref OnTouchCoint);
        OnTouchWall += Added1Score;
        uiManager.Init(ref OnScoreChange,ref OnpjDie,ref OnMoneyChange);
        fb.Init(ref OnTouchWall,ref OnpjDie, UpdatePjPos);
    }
    void UpdatePjPos(Vector3 pos)
    {
        pjPos = pos;
    }
    void Added1Score()
    {
        score++;
        OnScoreChange?.Invoke(score);
        if (score%5 == 0)
            spawnCurrency.SpawnCoint(pjPos);
    }

    public void ResetScore()
    {
        score = 0;
    }
    void Added1Money()
    {
        print("pj touch coint");
        money++;
        OnMoneyChange?.Invoke(money);
    }
}
