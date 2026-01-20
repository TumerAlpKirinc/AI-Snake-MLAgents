using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIAgent : Agent
{
    public SnakeController snake;
    public GameController gameController;

    float distancePrev;

    public override void OnEpisodeBegin()
    {
        snake.ResetSnake();
        gameController.ResetFood();
        distancePrev = gameController.getManhattanDistance();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector2 headPos = snake.snakePos[0];
        float step = 0.5f; 

        
        Vector2[] directions = { Vector2.up * step, Vector2.down * step, Vector2.left * step, Vector2.right * step };

        foreach (Vector2 dir in directions)
        {
            Vector2 checkPos = headPos + dir;
            sensor.AddObservation(IsObstacleAt(checkPos) ? 1.0f : 0.0f);
            sensor.AddObservation(checkPos == gameController.getFoodPos() ? 1.0f : 0.0f);
        }

        sensor.AddObservation(snake.GetDirection() == Direction.Up);
        sensor.AddObservation(snake.GetDirection() == Direction.Down);
        sensor.AddObservation(snake.GetDirection() == Direction.Left);
        sensor.AddObservation(snake.GetDirection() == Direction.Right);

        Vector2 relativeFoodPos = (gameController.getFoodPos() - headPos);
        sensor.AddObservation(relativeFoodPos.x);
        sensor.AddObservation(relativeFoodPos.y);

        sensor.AddObservation(gameController.getManhattanDistance());
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];

        
        switch (action)
        {
            case 1: snake.SetDirection(Direction.Up); break;
            case 2: snake.SetDirection(Direction.Down); break;
            case 3: snake.SetDirection(Direction.Left); break;
            case 4: snake.SetDirection(Direction.Right); break;
        }

        
        snake.ManualMove();

        
        AddReward(-0.01f);

        if (gameController.getManhattanDistance() != distancePrev)
        {
            if (gameController.getManhattanDistance() < distancePrev)
            {
                AddReward(0.01f); 
            }
            else if (gameController.getManhattanDistance() > distancePrev)
            {
                AddReward(-0.01f); 
            }
            distancePrev = gameController.getManhattanDistance();
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discrete = actionsOut.DiscreteActions;
        
        if (Input.GetKey(KeyCode.W)) discrete[0] = 1;
        else if (Input.GetKey(KeyCode.S)) discrete[0] = 2;
        else if (Input.GetKey(KeyCode.A)) discrete[0] = 3;
        else if (Input.GetKey(KeyCode.D)) discrete[0] = 4;
    }

    

    public void OnDeath()
    {
        AddReward(-1f);
        EndEpisode();
    }
    private bool IsObstacleAt(Vector2 localPos)
    {
        if (snake.snakePos.Contains(localPos)) return true;

        if (Mathf.Abs(localPos.x) > gameController.xLimit || Mathf.Abs(localPos.y) > gameController.yLimit)
            return true;

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            snake.Grow();
            Destroy(collision.gameObject);
            gameController.AddScore(1);
            gameController.SpawnFood();
            AddReward(5f);
        }
        else if (collision.CompareTag("Wall"))
        {
            OnDeath();
        }
    }
    
    public void Win()
    {
        AddReward(1f);
        EndEpisode();
    }

}