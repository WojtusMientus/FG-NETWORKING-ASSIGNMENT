using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkEmote : NetworkBehaviour
{
    private GameObject parentObject;
    private float emoteDuration;
    
    private void Start()
    {
        if (IsOwner)
            StartCoroutine(WaitLifetime());
    }
    
    private IEnumerator WaitLifetime()
    {
        yield return new WaitForSeconds(emoteDuration);
        
        NetworkObject networkObject = GetComponent<NetworkObject>();
        networkObject.Despawn();
    }

    public void SetLifetime(float duration)
    {
        emoteDuration = duration;
    }
    
}



