using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSelector : MonoBehaviour
{
    private Transform[] _nodes;

    private void Awake()
    {
        _nodes = GetComponentsInChildren<Transform>();
        GameManager.Instance.AgentPatrolNodes.AddRange(_nodes);
    }
}
