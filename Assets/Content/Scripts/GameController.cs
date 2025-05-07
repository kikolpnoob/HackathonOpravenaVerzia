using System;
using MoreMountains.Feedbacks;
using MyBox;
using UnityEditor.Rendering.Universal;
using UnityEngine;


public enum GameState
{
    Cinematic,
    Gameplay,
    Upgrade,
    Paused
}
public class GameController : MonoBehaviour
{
    public static GameState state { get; private set; }
    public Transform DummyKnight;
    public Transform DummyKnightTargetPoint;
    public MMF_Player DummyKnightFeedback;
    public Boss bossScript;
    public Movement movement;
    public Camera mainCamera;
    [Header("Gameplay variables")]
    public int xpPerLevel;
    static int _Level;
    static int _XP;
    
    void Start()
    {
        movement.enabled = false;
        bossScript.enabled = false;
    }

    void Update()
    {
        switch (state)
        {
            case GameState.Cinematic:
                CinematicGameState();
                break;
            case GameState.Gameplay:
                Gameplay();
                break;
            case GameState.Upgrade:
                Upgrade();
                break;
            case GameState.Paused:
                break;
        }
    }

    private void Gameplay()
    {
        if (_XP == (_Level+1) * xpPerLevel)
            LevelUp();
    }

    private void LevelUp()
    {
        state = GameState.Upgrade;
        Time.timeScale = 0;
    }

    private void Upgrade()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;
        Upgrade();
        Time.timeScale = 1;
    }

    private void CinematicGameState()
    {
        DummyKnight.position = Vector2.MoveTowards(DummyKnight.position, DummyKnightTargetPoint.position, 5.0f * Time.deltaTime);
        if (!Input.GetKeyDown(KeyCode.Mouse0) || !Mathf.Approximately(DummyKnight.position.y, DummyKnightTargetPoint.position.y))
            return;
        StartGameplay();
    }

    public void AddXP(int value)
    {
        _XP += value;
    }

    private void StartGameplay()
    {
        state = GameState.Gameplay;
        bossScript.GetComponent<SpriteAnimator>().PlayAnimation("Attack1");
        DummyKnightFeedback.PlayFeedbacks();
        movement.enabled = true;
        bossScript.enabled = true;
        Destroy(DummyKnight.gameObject);
    }
}
