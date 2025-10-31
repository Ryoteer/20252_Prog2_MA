using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    public float legsLayerWeight = 0.0f;

    private PlayerBehaviour _parent;

    private void Start()
    {
        _parent = GetComponentInParent<PlayerBehaviour>();
    }

    public void Attack()
    {
        _parent.Attack();
    }

    public void Fire()
    {
        _parent.Fire();
    }

    public void Grenade()
    {
        _parent.Invoke("Grenade", 4.0f);
    }

    public void Jump()
    {
        _parent.Jump();
    }

    public void PlayAttackClip()
    {
        _parent.PlayAttackClip();
    }

    public void PlayJumpClip(int state)
    {
        _parent.PlayJumpClip(state);
    }

    public void PlayMoveClip()
    {
        _parent.PlayMoveClip();
    }

    //public void SetLegsLayerWeight(float value)
    //{
    //    _parent.SetLegsLayerWeight(value);
    //}
}
