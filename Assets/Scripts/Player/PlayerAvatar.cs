using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    private PlayerBehaviour _parent;

    private void Start()
    {
        _parent = GetComponentInParent<PlayerBehaviour>();
    }

    public void Attack()
    {
        _parent.Attack();
    }

    public void Jump()
    {
        _parent.Jump();
    }
}
