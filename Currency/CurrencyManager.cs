using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [Header("Pearl Settings")]
    public int startingPearls = 100;
    public int currentPearls;

    [Header("UI References")]
    public TMP_Text pearlText;

    public int Money => currentPearls;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (!SaveSystem.HasSaveFile())
            currentPearls = startingPearls;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        UpdatePearlUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // The old UI got destroyed when we changed scenes, so clear the reference
        pearlText = null;

        // Wait a frame so the new scene UI exists, then try to bind
        StartCoroutine(BindUIAfterOneFrame());
    }

    private IEnumerator BindUIAfterOneFrame()
    {
        yield return null;

        RefreshUIReference();
        UpdatePearlUI();
    }

    private void RefreshUIReference()
    {
        // Only bind to the exact HUD text name, so Main Menu can't steal it
        GameObject go = GameObject.Find("PearlText");
        if (go != null)
        {
            pearlText = go.GetComponent<TMP_Text>();
        }
    }

    public void SetMoney(int amount)
    {
        currentPearls = Mathf.Max(0, amount);
        UpdatePearlUI();
    }

    public void AddPearls(int amount)
    {
        currentPearls = Mathf.Max(0, currentPearls + amount);
        UpdatePearlUI();
        SaveSystem.Instance?.SaveGame();
    }

    public bool SpendPearls(int amount)
    {
        if (currentPearls < amount) return false;

        currentPearls = Mathf.Max(0, currentPearls - amount);
        UpdatePearlUI();
        SaveSystem.Instance?.SaveGame();
        return true;
    }

    public void UpdatePearlUI()
    {
        // If we are in Main Menu, PearlText won't exist, that's fine
        if (pearlText == null)
            RefreshUIReference();

        if (pearlText != null)
            pearlText.text = currentPearls.ToString();
    }
}
