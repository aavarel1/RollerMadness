using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
// what
public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public GameObject player;
    public enum gameStates { Playing, Death, GameOver, BeatLevel };
    public gameStates gameState = gameStates.Playing;
    public int score;
    public int playerHealth = 3;
    public bool canBeatLevel = false;
    public int beatLevelScore = 0;
    public TMP_Text scoreText;
    public TMP_Text healthText;
    public GameObject winText;
    public GameObject gameOverText;
    public GameObject playAgainButton;
    public GameObject nextLevelButton;
    public AudioSource backgroundMusic;
    public GameObject restartButton;
    public AudioClip gameOverSFX;
    public AudioClip beatLevelSFX;
    public GameObject PauseCanvas;
    public GameObject[] spawnPoint1;
    public float maxX = 5f;
    public float maxZ = 5f;
    public GameObject EnemyPrefab;
    public GameObject CoinPrefab;
    Vector3 playerSpawnLocation;
    GameObject cam;
    AudioSource audioSource;
    float originalSpeed = 8f;
    public Transform[] spawnPoint;
    public bool pauseGame = false;

    private InputSystem_Actions inputActions;

    void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        audioSource = cam.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing from camera!");
        }
        playAudioRepeat(backgroundMusic.clip);

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (player == null)
        {
            Debug.LogError("Player not found in Game Manager");
        }

        setupDefaults();
        InvokeRepeating("spawnEnemy", 2f, 2f);
        InvokeRepeating("spawnCoins", 2f, 2f);

        inputActions = new InputSystem_Actions(); // Initialize input actions
    }



    void playAudioOneTime(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, cam.transform.position);
    }

    void playAudioRepeat(AudioClip clip)
    {
        audioSource.loop = true;
        audioSource.clip = clip;
        audioSource.Play();
    }

    void setupDefaults()
    {
        gameOverText.SetActive(false);
        winText.SetActive(false);
        playAgainButton.SetActive(false);
        nextLevelButton.SetActive(false); 
        restartButton.SetActive(false);
        playerSpawnLocation = player.transform.position;
        displayPlayerHealth();
    }

    void displayPlayerHealth()
    {
        healthText.text = "Health: " + playerHealth.ToString();
    }

    public void add_score(int amount)
    {
        score += amount;

        if (canBeatLevel)
        {
            scoreText.text = "Score = " + score.ToString() + " of " + beatLevelScore.ToString();
            if (score >= beatLevelScore)
            {
                winText.SetActive(true);
                nextLevelButton.SetActive(true);
                playAgainButton.SetActive(true); 
                restartButton.SetActive(true);
                audioSource.Stop();
                playAudioOneTime(beatLevelSFX);
                destroyAllEnemy(); 
            }
        }
        else
        {
            scoreText.text = "Score = " + score.ToString();
        }
    }

    public void pause()
    {
        Time.timeScale = pauseGame ? 0f : 1f;
        float enemySpeed = pauseGame ? 0 : originalSpeed;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject g in enemies)
        {
            var chase = g.GetComponent<chase>();  // Use lowercase 'chase'
            if (chase != null)
            {
                chase.speed = enemySpeed;
            }
        }
    }

    public void decHealth()
    {
        playerHealth -= 1;
        displayPlayerHealth();
        if (playerHealth == 0)
        {
            gameOverText.SetActive(true);
            playAgainButton.SetActive(true);
            restartButton.SetActive(true);
            gameState = gameStates.GameOver;
            audioSource.Stop();
            playAudioOneTime(gameOverSFX); 
            CancelInvoke("spawnEnemy"); 
            CancelInvoke("spawnCoins"); 
            destroyAllEnemy();
            
        }
        else destroyAllEnemy();
    }

    void destroyAllEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject g in enemies)
        {
            if(g.activeInHierarchy) {
                Debug.Log("Destroying " + g.name); 
                Destroy(g);
            }
        } 
    }

    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void nextLevel()
    {
        SceneManager.LoadScene("NextLevel");
    }

    void Update()
    {
        if (gameState == gameStates.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 10f;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                Instantiate(CoinPrefab, worldPos, Quaternion.identity);
            }
        } 

    }

    void spawnEnemy()
    {
        int randomCorner = UnityEngine.Random.Range(0, spawnPoint.Length);
        Instantiate(EnemyPrefab, spawnPoint[randomCorner].position, Quaternion.identity); 
        Debug.Log("Spawned Enemny " + EnemyPrefab + "with tag " + EnemyPrefab.tag);
    }

    void spawnCoins()
    {
        float randomX = UnityEngine.Random.Range(-maxX, maxX);
        float randomZ = UnityEngine.Random.Range(-maxZ, maxZ);
        Vector3 randomPos = new Vector3(randomX, 10f, randomZ);
        Instantiate(CoinPrefab, randomPos, Quaternion.identity);
    }
}