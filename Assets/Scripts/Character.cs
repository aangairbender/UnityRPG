using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Character : NetworkBehaviour, ICharacterController
{
    public float Speed = 5f;

    public void Move(Vector3 direction)
    {
        var movement = direction * Speed;
        transform.Translate(movement);
    }
}
