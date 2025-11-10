using UnityEngine;

public class QC_EnemyKilled : QuestComponent
{
    private int _enemyID;
    private int _killCount;
    private int _killsNeeded;
    
    public QC_EnemyKilled(string name, string description, int enemyID, int killsNeeded) : base(name, description)
    {
        _enemyID = enemyID;
        _killsNeeded = killsNeeded;
        _killCount = 0;
        ComponentType = QuestComponentType.EnemyKilled;
    }

    public override void EnableComponent()
    {
        base.EnableComponent();
        // Subscribe to enemy kill event
    }

    public override void MarkCompleted()
    {
        base.MarkCompleted();
        // Unsubscribe from enemy kill event
    }
}