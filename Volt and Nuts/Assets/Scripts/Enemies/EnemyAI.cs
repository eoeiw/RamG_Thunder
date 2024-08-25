using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming
    }

    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Transform playerTransform; // Reference to the player

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        // Assuming the player has a tag "Player"
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(RoamingRoutine());
    }

    private IEnumerator RoamingRoutine()
    {
        while(state == State.Roaming)
        {
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathfinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(2f);
        }
    }

    private Vector2 GetRoamingPosition()
    {
        if (playerTransform == null)
        {
            // Fallback to random roaming if playerTransform is not set
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        // Get direction towards the player
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        return directionToPlayer;
    }
}
