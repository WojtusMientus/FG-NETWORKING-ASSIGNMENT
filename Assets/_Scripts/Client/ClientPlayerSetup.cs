using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

// [DefaultExecutionOrder(0)]
public class ClientPlayerSetup : NetworkBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private NetworkEmoteHandler emoteHandler;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
        {
            playerInput.enabled = false;
            emoteHandler.enabled = false;
            playerMovement.enabled = false;
            enabled = false;
        }
        
        SetPlayersPosition();
    }
    
    private void SetPlayersPosition()
    {
        Vector3 spawnPosition = SpawnManager.Instance.GetSpawnPosition(IsOwner);
        
        transform.position = spawnPosition;
    }
}
