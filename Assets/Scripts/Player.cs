using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public GameObject CharacterPrefab;
    public Transform Spawn;

    [SyncVar]
    private GameObject character;

    public override void OnStartClient()
    {
        base.OnStartClient();

        SpawnCharacter();
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            UpdateInputs();
        }
    }

    [Client]
    private void UpdateInputs()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        var direction = Vector3.right * horizontal + Vector3.forward * vertical;
        if (direction.sqrMagnitude < 0.01) { return; }
        
        character.GetComponent<Character>().Move(direction.normalized * Time.deltaTime);
    }

    [Command]
    private void SpawnCharacter()
    {
        var character = Instantiate(CharacterPrefab, Spawn);
        NetworkServer.Spawn(character, connectionToClient);
        LinkCharacter(character);
    }

    [ClientRpc]
    private void LinkCharacter(GameObject character)
    {
        this.character = character;
    }
}
