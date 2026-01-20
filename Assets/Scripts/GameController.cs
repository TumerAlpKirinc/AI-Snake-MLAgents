using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro; 

public class GameController : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] int gridWidth = 34;
    [SerializeField] int gridHeight = 18;
    public float step = 0.5f;

    [Header("References")]
    [SerializeField] SnakeController snake; 
    [SerializeField] GameObject foodPrefab;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject wallPrefab;

    [Header("State")]
    private int score = 0;
    private GameObject currentFood;
    private Vector2 foodPos;

    private int maxScore = 0;

    public float xLimit { get; private set; }
    public float yLimit { get; private set; }

    void Awake() 
    {
        xLimit = (gridWidth / 2f) * step + step;
        yLimit = (gridHeight / 2f) * step + step;
    }

    void Start()
    {
        if (snake == null) snake = GetComponentInChildren<SnakeController>();

        GenerateWalls();
        UpdateScoreUI();
        SpawnFood();
    }

    public void SpawnFood()
    {
        if (currentFood != null) Destroy(currentFood);

        Vector2 localSpawnPos = Vector2.zero;
        bool validPos = false;
        int attempts = 0;

        while (!validPos && attempts < 100)
        {
            attempts++;
            int xRandom = Random.Range(-gridWidth / 2, (gridWidth / 2) + 1);
            int yRandom = Random.Range(-gridHeight / 2, (gridHeight / 2) + 1);
            localSpawnPos = new Vector2(xRandom * step, yRandom * step);

            
            if (!snake.snakePos.Contains(localSpawnPos))
            {
                validPos = true;
            }
        }

        currentFood = Instantiate(foodPrefab, transform);
        currentFood.transform.localPosition = localSpawnPos;
        foodPos = localSpawnPos;
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (score > maxScore)
        {
            maxScore = score;
            
        }
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score + "\nMax Score: "+ maxScore;
    }

    void GenerateWalls()
    {
         

        // Üst Duvar
        SpawnWall(new Vector2(0, yLimit), new Vector2(gridWidth * step + (step * 3), step));
        // Alt Duvar
        SpawnWall(new Vector2(0, -yLimit), new Vector2(gridWidth * step + (step * 3), step));
        // Sað Duvar
        SpawnWall(new Vector2(xLimit, 0), new Vector2(step, gridHeight * step + (step * 1)));
        // Sol Duvar
        SpawnWall(new Vector2(-xLimit, 0), new Vector2(step, gridHeight * step + (step * 1)));
    }

    void SpawnWall(Vector2 localPosition, Vector2 scale)
    {
        
        GameObject wall = Instantiate(wallPrefab, transform);
        wall.transform.localPosition = localPosition; 
        wall.transform.localScale = scale;
    }

    public float getManhattanDistance()
    {
        if (snake == null || snake.snakePos.Count == 0) return 0f;
        Vector2 snakeHeadPos = snake.snakePos[0];
        return Mathf.Abs(snakeHeadPos.x - foodPos.x) + Mathf.Abs(snakeHeadPos.y - foodPos.y);
    }
    public Vector2 getFoodPos()
    {
        return foodPos;
    }

    public void ResetFood()
    {
        if (currentFood != null)
        {
            Destroy(currentFood);
            currentFood = null;
        }

        score = 0;
        UpdateScoreUI();

        SpawnFood();
    }
}