using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance
    public static GameManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private PlayerBehaviour _player;
    public PlayerBehaviour Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private List<EnemyBehaviour> _enemies;
    public List<EnemyBehaviour> Enemies
    {
        get { return _enemies; }
        set { _enemies = value; }
    }

    private List<Transform> _agentPatrolNodes = new();
    public List<Transform> AgentPatrolNodes
    {
        get { return _agentPatrolNodes; }
        set { _agentPatrolNodes = value; }
    }
}
