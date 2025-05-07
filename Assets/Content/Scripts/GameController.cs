using System;
using System.Linq;
using MoreMountains.Feedbacks;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum GameState
{
    Cinematic,
    Gameplay,
    Upgrade,
    Paused
}
public class GameController : MonoBehaviour
{
    [System.Serializable]
    public struct HeroPrefabs
    {
        public Knight knight;
        public Archer archer;
        public Priest priest;
    }
    public HeroPrefabs heroPrefabs;
    public static GameState state { get; private set; }
    public static bool abilityChoice;
    public TMP_Text clickToPlayText;
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

    [SerializeField, ReadOnly] private float spawnTimer;
    [SerializeField, ReadOnly] private float difficultyMod;
    public Tilemap tilemap;
    public LayerMask wallMask;
    float spawnMargin = 1f;

    // Vector2 GetOffscreenSpawn(int side)
    // {
    //     // Camera bounds
    //     float camHeight = 2f * Camera.main.orthographicSize;
    //     float camWidth = camHeight * Camera.main.aspect;
    //     Vector2 camCenter = Camera.main.transform.position;
    //     float halfWidth = camWidth / 2f;
    //     float halfHeight = camHeight / 2f;

    //     // Map bounds
    //     BoundsInt bounds = tilemap.cellBounds;
    //     Vector3 min = tilemap.CellToWorld(bounds.min);
    //     Vector3 max = tilemap.CellToWorld(bounds.max);
    //     float mapMinX = min.x;
    //     float mapMaxX = max.x;
    //     float mapMinY = min.y;
    //     float mapMaxY = max.y;

    //     // Pick side
    //     Vector2 spawnPos = Vector2.zero;
    //     switch (side)
    //     {
    //         case 0: // Top
    //             spawnPos = new Vector2(
    //                 Mathf.Clamp(UnityEngine.Random.Range(camCenter.x - halfWidth, camCenter.x + halfWidth), mapMinX, mapMaxX),
    //                 Mathf.Min(mapMaxY, camCenter.y + halfHeight + spawnMargin)
    //             );
    //             break;
    //         case 1: // Bottom
    //             spawnPos = new Vector2(
    //                 Mathf.Clamp(UnityEngine.Random.Range(camCenter.x - halfWidth, camCenter.x + halfWidth), mapMinX, mapMaxX),
    //                 Mathf.Max(mapMinY, camCenter.y - halfHeight - spawnMargin)
    //             );
    //             break;
    //         case 2: // Left
    //             spawnPos = new Vector2(
    //                 Mathf.Max(mapMinX, camCenter.x - halfWidth - spawnMargin),
    //                 Mathf.Clamp(UnityEngine.Random.Range(camCenter.y - halfHeight, camCenter.y + halfHeight), mapMinY, mapMaxY)
    //             );
    //             break;
    //         case 3: // Right
    //             spawnPos = new Vector2(
    //                 Mathf.Min(mapMaxX, camCenter.x + halfWidth + spawnMargin),
    //                 Mathf.Clamp(UnityEngine.Random.Range(camCenter.y - halfHeight, camCenter.y + halfHeight), mapMinY, mapMaxY)
    //             );
    //             break;
    //     }
    //     return spawnPos;
    // }

    bool IsValidSpawn(Vector2 pos)
    {
        float radius = 0.25f;
        return Physics2D.OverlapCircle(pos, radius, wallMask) == null && !Physics2D.Raycast(pos, ((Vector2)Boss.Transform.position - pos).normalized, Vector2.Distance(Boss.Transform.position, pos), wallMask);
    }

    // Vector2 GetValidSpawnPosition(int side)
    // {
    //     Vector2 spawnPos;
    //     int tries = 0;
    //     do
    //     {
    //         spawnPos = GetOffscreenSpawn(side);
    //         tries++;
    //     } while (!IsValidSpawn(spawnPos) && tries < 10);

    //     return spawnPos;
    // }
    Vector2 GetValidMapSpawnPosition()
    {
        BoundsInt bounds = tilemap.cellBounds;
        int tries = 0;
        while (tries < 10)
        {
            // Pick a random cell inside bounds
            int x = UnityEngine.Random.Range(bounds.xMin, bounds.xMax);
            int y = UnityEngine.Random.Range(bounds.yMin, bounds.yMax);
            Vector3Int randomCell = new Vector3Int(x, y, 0);

            // Convert to world position (center of the tile)
            Vector3 spawnWorldPos = tilemap.GetCellCenterWorld(randomCell);

            // Check if it overlaps with walls
            float radius = 0.25f;
            if (Physics2D.OverlapCircle(spawnWorldPos, radius, wallMask) == null)
            {
                return spawnWorldPos;
            }

            tries++;
        }

        // Fallback: return the center of the map if all else fails
        Debug.LogWarning("Failed to find a valid spawn position after many tries.");
        return tilemap.GetCellCenterWorld(bounds.center.ToVector3Int());
    }

    
    void Start()
    {
        movement.enabled = false;
        bossScript.enabled = false;
        clickToPlayText.color = new Color(1, 1, 1, 0);
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
        difficultyMod += Time.deltaTime * 0.05f;
        SpawningLogic();
    }

    private void SpawningLogic()
    {
        spawnTimer += Time.deltaTime * Mathf.Clamp(difficultyMod * 0.5f + 1, 1, 5);

        if (spawnTimer > 0)
        {
            SpawnNewHeroParty();
            spawnTimer -= 7;
        }
    }

    private void SpawnNewHeroParty()
    {
        int count = Mathf.RoundToInt(difficultyMod + 1);

        var heroes = GetRandomHeroes(count);
        for (int i = 0; i < count; i++)
        {
            Instantiate(heroes[i], GetValidMapSpawnPosition(), Quaternion.identity);
        }
    }

    private Hero[] GetRandomHeroes(int count)
    {
        Hero[] heroes = new Hero[count];
        for (int i = 0; i < count; i++)
        {
            if (count > 2 && !PartyHasPriest(heroes))
            {
                heroes[i] = heroPrefabs.priest;
                continue;
            }
            int x = UnityEngine.Random.Range(0, 2);
            if (x == 0)
                heroes[i] = heroPrefabs.knight;
            else
                heroes[i] = heroPrefabs.archer;
        }
        return heroes;
    }

    bool PartyHasPriest(Hero[] heroes)
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            if (heroes[i] != null && heroes[i] is Priest)
                return true;
        }
        return false;
    }

    private void LevelUp()
    {
        abilityChoice = true;
        _XP = 0;
        Time.timeScale = 0;
        state = GameState.Upgrade;
    }

    private void Upgrade()
    {
        if (abilityChoice)
            return;
        // DOTO: Show ability choice
        Time.timeScale = 1;
    }

    private void CinematicGameState()
    {
        DummyKnight.position = Vector2.MoveTowards(DummyKnight.position, DummyKnightTargetPoint.position, 5.0f * Time.deltaTime);
        if (!Mathf.Approximately(DummyKnight.position.y, DummyKnightTargetPoint.position.y))
            return;
        clickToPlayText.color = Color.Lerp(clickToPlayText.color, new Color(1, 1, 1, 1), Time.deltaTime * 5);
        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;
        StartGameplay();
    }

    public void AddXP(int value)
    {
        _XP += value;
    }

    private void StartGameplay()
    {
        clickToPlayText.color = new Color(1, 1, 1, 0);
        clickToPlayText.gameObject.SetActive(false);
        state = GameState.Gameplay;
        bossScript.GetComponent<SpriteAnimator>().PlayAnimation("Attack1");
        DummyKnightFeedback.PlayFeedbacks();
        movement.enabled = true;
        bossScript.enabled = true;
        Destroy(DummyKnight.gameObject);
    }

    public static void AddEXP(int amount)
    {
        _XP += amount;
    }
}
