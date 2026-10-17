using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int targetScore = 10;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject winPanel;

    [Header("Bonus")]
    [SerializeField] GameObject redCoinPrefab;
    [SerializeField] GameObject blueCoinPrefab;
    [SerializeField] Transform player;
    [SerializeField] TMP_Text bonusText;
    [SerializeField] int blueCount = 50;
    [SerializeField] float circleRadius = 4f;
    [SerializeField] float arenaLimit = 7f;

    int score;
    int redSpawnScore;
    bool redSpawned;
    int blueCollected;

    void Awake() => Instance = this;

    void Start()
    {
        Time.timeScale = 1f;
        redSpawnScore = Random.Range(1, targetScore); // picks 1 to 9
        bonusText.gameObject.SetActive(false);
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
        if (!redSpawned && score >= redSpawnScore) SpawnRedCoin();
        if (score >= targetScore) Win();
    }

    void SpawnRedCoin()
    {
        redSpawned = true; // guarantees only one ever spawns
        Vector3 pos = new Vector3(
            Random.Range(-arenaLimit, arenaLimit), 0.5f,
            Random.Range(-arenaLimit, arenaLimit));
        Instantiate(redCoinPrefab, pos, Quaternion.identity);
    }

    public void CollectRedCoin()
    {
        Vector3 c = player.position;
        for (int i = 0; i < blueCount; i++)
        {
            float angle = i * Mathf.PI * 2f / blueCount;
            float x = Mathf.Clamp(c.x + Mathf.Cos(angle) * circleRadius, -arenaLimit, arenaLimit);
            float z = Mathf.Clamp(c.z + Mathf.Sin(angle) * circleRadius, -arenaLimit, arenaLimit);
            Instantiate(blueCoinPrefab, new Vector3(x, 0.5f, z), Quaternion.identity);
        }
        blueCollected = 0;
        bonusText.gameObject.SetActive(true);
        UpdateBonusUI();
    }
    public void CollectBlueCoin()
    {
        blueCollected++;
        UpdateBonusUI();
    }

    void UpdateUI() => scoreText.text = $"Score: {score} / {targetScore}";
    void UpdateBonusUI() => bonusText.text = $"BonusCoins: {blueCollected}/{blueCount}";

    void Win() { winPanel.SetActive(true); Time.timeScale = 0f; }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}