using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}

public class SnakeController : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] GameObject bodyPrefab; 

    public AIAgent agent;

    public List<GameObject> bodies = new List<GameObject>();
    public List<Vector2> snakePos = new List<Vector2>();

    [SerializeField, Range(0, 1f)]
    float moveInterval = 0.25f;
    float timerTick = 0f;

    Direction currentDirection = Direction.Right;
    Direction nextDirection = Direction.Right;

    bool isGameOver = false;

    [SerializeField] float stepSize = 0.5f;

   
    /*
    void Update()
    {
        if (isGameOver) return;

        HandleInput();

        // Hareket zamanlamasý
        timerTick += Time.deltaTime;
        if (timerTick >= moveInterval)
        {
            timerTick = 0f;
            Move();
        }
    }
    */
    public void SetDirection(Direction dir)
    {
        if (dir == Direction.Up && currentDirection == Direction.Down) return;
        if (dir == Direction.Down && currentDirection == Direction.Up) return;
        if (dir == Direction.Left && currentDirection == Direction.Right) return;
        if (dir == Direction.Right && currentDirection == Direction.Left) return;

        nextDirection = dir;
    }


    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && currentDirection != Direction.Down) nextDirection = Direction.Up;
        if (Input.GetKeyDown(KeyCode.S) && currentDirection != Direction.Up) nextDirection = Direction.Down;
        if (Input.GetKeyDown(KeyCode.A) && currentDirection != Direction.Right) nextDirection = Direction.Left;
        if (Input.GetKeyDown(KeyCode.D) && currentDirection != Direction.Left) nextDirection = Direction.Right;
    }

    void Move()
    {
        currentDirection = nextDirection;
        Vector2 newPos = snakePos[0];

        switch (currentDirection)
        {
            case Direction.Up: newPos.y += stepSize; break;
            case Direction.Down: newPos.y -= stepSize; break;
            case Direction.Left: newPos.x -= stepSize; break;
            case Direction.Right: newPos.x += stepSize; break;
        }
        if (Mathf.Abs(newPos.x) > gameController.xLimit || Mathf.Abs(newPos.y) > gameController.yLimit)
        {
            GameOver();
            if (agent != null) agent.OnDeath();
            return; 
        }
        if (snakePos.Contains(newPos))
        {
            GameOver();
            if (agent != null)
            {
                agent.OnDeath(); 
            }
            return;
        }

        snakePos.Insert(0, newPos);
        snakePos.RemoveAt(snakePos.Count - 1);

        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].transform.localPosition = snakePos[i];
        }
    }


    public void Grow()
    {
        if (bodies.Count != 612)
        {
            Vector2 lastPos = snakePos[snakePos.Count - 1];
            snakePos.Add(lastPos);

            
            GameObject newBody = Instantiate(bodyPrefab, transform.parent);

            newBody.transform.localPosition = (Vector3)lastPos;
            bodies.Add(newBody);
        }
        else
        {
            agent.Win();
        }

    }

    void GameOver()
    {
        isGameOver = true; 
        
    }

    public void ResetSnake()
    {
        for (int i = 1; i < bodies.Count; i++)
        {
            Destroy(bodies[i].gameObject);

        }
        bodies.Clear();
        snakePos.Clear();
        transform.localPosition = Vector3.zero;
        snakePos.Add(transform.localPosition);
        bodies.Add(gameObject);
        nextDirection = Direction.Right;
        currentDirection = Direction.Right;
        isGameOver = false;
        
    }
    public Direction GetDirection()
    {
        return currentDirection;
    }
    public void ManualMove()
    {
        if (isGameOver) return;
        Move();
    }

}