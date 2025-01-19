using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject playingUI;

    [SerializeField] private TMP_Text leftScore;
    [SerializeField] private TMP_Text rightScore;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        mainMenuUI.SetActive(true);
    }
    
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        ChangeUI();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        ChangeUI();
    }

    private void ChangeUI()
    {
        mainMenuUI.SetActive(false);
        playingUI.SetActive(true);
    }

    public void UpdateScore(bool isLeft, int score)
    {
        if (!isLeft)
            leftScore.text = score.ToString();
        else
            rightScore.text = score.ToString();
    }
}
