using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkBallBehaviour : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D ballRigidbody;
    [SerializeField] private float originalSpeed;
    
    private readonly float maxSpawnAngle = 35f;
    private readonly int[] sign = new int[] { -1, 1 };
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (NetworkManager.Singleton.IsServer)
            ballRigidbody.linearVelocity = CalculateInitialVelocity();
    }
    
    private Vector2 CalculateInitialVelocity()
    {
        float randomAngle = Random.Range(-maxSpawnAngle, maxSpawnAngle);
        Vector2 directionVector = sign[Random.Range(0, 2)] * Vector2.right;

        Vector2 finalVelocity = Quaternion.AngleAxis(randomAngle, Vector3.forward).normalized * directionVector * originalSpeed;

        return finalVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsOwner)
        {
            if (collision.gameObject.CompareTag("Player"))
                AddRandomForceServerRpc();

            if (collision.gameObject.CompareTag("LeftBound"))
                AddPointAndDestroyBall(true);

            else if (collision.gameObject.CompareTag("RightBound"))
                AddPointAndDestroyBall(false);
        }
    }
    
    private void AddPointAndDestroyBall(bool isLeft)
    {
        GameManager.Instance.OnPointScoreServerRpc(isLeft);
        
        NetworkObject networkObject = GetComponent<NetworkObject>();
        networkObject.Despawn();
    }

    [ServerRpc]
    private void AddRandomForceServerRpc()
    {
        float minAngle = 30;
        float maxAngle = 50;
        float randomForceSpeedMultiplier = ballRigidbody.linearVelocity.magnitude / 2.5f;
        float currentSpeedMultiplier = 0.75f;

        Vector2 normalizedVelocity = ballRigidbody.linearVelocity.normalized;
        float randomAngle = 0f;

        if (Random.Range(0f, 1f) < 0.5f)
            randomAngle = Random.Range(-maxAngle, -minAngle);
        else
            randomAngle = Random.Range(minAngle, maxAngle);

        Vector2 randomForce = Quaternion.AngleAxis(randomAngle, Vector3.forward) * normalizedVelocity;

        randomForce *= randomForceSpeedMultiplier;

        ballRigidbody.linearVelocity *= currentSpeedMultiplier;

        ballRigidbody.AddForce(randomForce, ForceMode2D.Impulse);
    }
    
    
    
}
