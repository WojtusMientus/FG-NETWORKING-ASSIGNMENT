using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    
    public NetworkVariable<int> leftScore = new (0);
    public NetworkVariable<int> rightScore = new (0);

    [SerializeField] private NetworkObject ball;

    [SerializeField] private bool startGame;
    
    public override void OnNetworkSpawn()
    {
        if (Instance == null)
            Instance = this;
        
        base.OnNetworkSpawn();

        NetworkManager.Singleton.OnClientConnectedCallback += StartGame;
        
        leftScore.OnValueChanged += UpdateUIScoreLeft;
        rightScore.OnValueChanged += UpdateUIScoreRight;
    }

    public override void OnNetworkDespawn() 
    {
        base.OnNetworkDespawn();

        NetworkManager.Singleton.OnClientConnectedCallback -= StartGame;
        
        leftScore.OnValueChanged -= UpdateUIScoreLeft;
        rightScore.OnValueChanged -= UpdateUIScoreRight;
    }

    private void StartGame(ulong obj)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(obj, out _) && IsOwner && startGame)
            StartCoroutine(StartingGame());
    }
    
    private IEnumerator StartingGame()
    {
        yield return new WaitForSeconds(1f);
        NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(ball);
    }
    
    private void UpdateUIScoreLeft(int previousValue, int newValue)
    {
        UIManager.Instance.UpdateScore(true, newValue);
    }
    
    private void UpdateUIScoreRight(int previousValue, int newValue)
    {
        UIManager.Instance.UpdateScore(false, newValue);
    }
    
    
    [ServerRpc]
    public void OnPointScoreServerRpc(bool isLeft)
    {
        if (isLeft)
            leftScore.Value++;
        else
            rightScore.Value++;
        
        StartCoroutine(StartingGame());
    }
}
