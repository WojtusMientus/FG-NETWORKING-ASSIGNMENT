using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkEmoteHandler : NetworkBehaviour
{
    [SerializeField] private NetworkObject emote;
    
    [SerializeField] private float emoteCooldown;
    [SerializeField] private float emoteDuration;

    private bool canEmote = true;
    
    public void HandlePlayerEmote(InputAction.CallbackContext context)
    {
        if (context.started && canEmote)
        {
            SpawnEmoteServerRpc(GetComponent<NetworkObject>().OwnerClientId, context.control.name);
            StartCoroutine(DecreaseEmoteCooldown());
        }   
    }

    private IEnumerator DecreaseEmoteCooldown()
    {
        canEmote = false;
        
        yield return new WaitForSeconds(emoteCooldown);
        
        canEmote = true;
    }

    [ServerRpc]
    private void SpawnEmoteServerRpc(ulong clientId, string emoteNumber)
    {
        Quaternion rotation = Quaternion.identity;
        
        if (NetworkManager.ServerClientId != clientId)
            rotation.eulerAngles = new Vector3(0, 180, 0);

        NetworkObject spawnedEmote = SpawnManager.Instance.RequestCorrectEmote(emoteNumber).GetComponent<NetworkObject>();
        spawnedEmote.transform.position = transform.position;
        spawnedEmote.transform.rotation = rotation;
        
        spawnedEmote.GetComponent<NetworkEmote>().SetLifetime(emoteDuration);
        spawnedEmote.Spawn();
        spawnedEmote.transform.SetParent(transform);
    }
}
