using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Objective))]
public class ObjectiveSurviveRounds : MonoBehaviour
{
    [Tooltip("Chose whether you need to kill every enemies or only a minimum amount before starting the next round")]
    public bool mustKillAllEnemiesToAdvance = true;
    [Tooltip("If mustKillAllEnemiesToAdvance is false, this is the amount of enemy kills required before starting the next round")]
    public int killsToCompleteRound = 3;
    [Tooltip("Start sending notification about remaining enemies when this amount of enemies is left")]
    public int notificationEnemiesRemainingThreshold = 3;

    public SpawnController m_spawnController;
    public Text hudRoundCounter;

    EnemyManager m_EnemyManager;
    Objective m_Objective;
    int m_KillTotal;

    void Start()
    {
        m_Objective = GetComponent<Objective>();
        DebugUtility.HandleErrorIfNullGetComponent<Objective, ObjectiveSurviveRounds>(m_Objective, this, gameObject);

        m_EnemyManager = FindObjectOfType<EnemyManager>();
        DebugUtility.HandleErrorIfNullFindObject<EnemyManager, ObjectiveSurviveRounds>(m_EnemyManager, this);
        m_EnemyManager.onRemoveEnemy += OnKillEnemy;

        if (mustKillAllEnemiesToAdvance)
            killsToCompleteRound = m_EnemyManager.numberOfEnemiesTotal;


        // set a title and description specific for this type of objective, if it hasn't one
        if (string.IsNullOrEmpty(m_Objective.title))
            m_Objective.title = "Eliminate " + (mustKillAllEnemiesToAdvance ? "all the" : killsToCompleteRound.ToString()) + " enemies";

        if (string.IsNullOrEmpty(m_Objective.description))
            m_Objective.description = GetUpdatedCounterAmount();
    }

    void OnKillEnemy(EnemyController enemy, int remaining)
    {
        //Debug.Log("IM DED");
        if (m_Objective.isCompleted)
            return;

        if (mustKillAllEnemiesToAdvance)
            killsToCompleteRound = m_EnemyManager.numberOfEnemiesTotal;

        m_KillTotal = m_EnemyManager.numberOfEnemiesTotal - remaining;
        int targetRemaning = mustKillAllEnemiesToAdvance ? remaining : killsToCompleteRound - m_KillTotal;

        // update the objective text according to how many enemies remain to kill
        if (targetRemaning == 0)
        {
            m_Objective.UpdateObjective(string.Empty, GetUpdatedCounterAmount(), "Round " + m_spawnController.round + " completed");
            OnPassRound();
        }
        else
        {

            if (targetRemaning == 1)
            {
                string notificationText = notificationEnemiesRemainingThreshold >= targetRemaning ? "One enemy left" : string.Empty;
                m_Objective.UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
            }
            else if (targetRemaning > 1)
            {
                // create a notification text if needed, if it stays empty, the notification will not be created
                string notificationText = notificationEnemiesRemainingThreshold >= targetRemaning ? targetRemaning + " enemies to kill left" : string.Empty;

                m_Objective.UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
            }
        }
    }

    string GetUpdatedCounterAmount()
    {
        return "";
        //return "Round: " + m_spawnController.round.ToString();
    }

    void OnPassRound()
    {
        m_spawnController.round++;        
        m_spawnController.StartSpawners();

        hudRoundCounter.GetComponent<Text>().text = ("Round: " + m_spawnController.round.ToString());

        killsToCompleteRound = m_EnemyManager.numberOfEnemiesTotal;
        m_KillTotal = 0;
    }
}
