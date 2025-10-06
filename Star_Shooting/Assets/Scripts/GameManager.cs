using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private GameObject gameOverUI;

    [Header("UI")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    public int Score { get; private set; } = 0;
    public int Lives { get; private set; } = 5;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (Lives <= 0 && Input.GetKeyDown(KeyCode.Return))
            NewGame();
    }

    private void NewGame()
    {
        foreach (Asteroid asteroid in FindObjectsOfType<Asteroid>())
            Destroy(asteroid.gameObject);

        gameOverUI.SetActive(false);
        SetScore(0);
        SetLives(5);
        Respawn();
    }

    private void SetScore(int value)
    {
        Score = value;
        scoreText.text = Score.ToString();
    }

    private void SetLives(int value)
    {
        Lives = value;
        livesText.text = Lives.ToString();
    }

    private void Respawn()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
    }

    public void OnAsteroidDestroyed(Asteroid asteroid)
    {
        if (asteroid.size < 0.7f)
            SetScore(Score + 100);
        else if (asteroid.size < 1.4f)
            SetScore(Score + 50);
        else
            SetScore(Score + 25);
    }

    public void OnPlayerDeath(Player player)
    {
        player.gameObject.SetActive(false);
        SetLives(Lives - 1);

        if (Lives <= 0)
            gameOverUI.SetActive(true);
        else
            Invoke(nameof(Respawn), player.respawnDelay);
    }
}
