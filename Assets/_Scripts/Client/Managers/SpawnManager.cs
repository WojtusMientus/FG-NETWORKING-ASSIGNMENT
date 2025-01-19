using Unity.Netcode;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    
    [SerializeField] private Vector3 hostSpawnPosition;
    [SerializeField] private Vector3 clientSpawnPosition;
    
    [SerializeField] private NetworkObject[] emotes;
    
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    public Vector3 GetSpawnPosition(bool isHost) => isHost ? hostSpawnPosition : clientSpawnPosition;

    public GameObject RequestCorrectEmote(string emoteNumber)
    {
        int emoteExpressionIndex = SelectEmote(emoteNumber);
        
        GameObject correctEmote = Instantiate(emotes[emoteExpressionIndex].gameObject);
        
        return correctEmote;
    }
    
    private int SelectEmote(string controlName)
    {
        switch (controlName)
        {
            case "1":
                return 0;
            case "2":
                return 1;
            case "3":
                return 2;
            case "4":
                return 3;
            default:
                return 0;
        }
    }
}
