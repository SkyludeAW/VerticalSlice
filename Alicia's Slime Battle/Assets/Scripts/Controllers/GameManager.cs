using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private ScoreManager scoreManager;
    public int Score { get; private set; }
    public static GameManager Instance { get; private set; }

    [SerializeField] GameObject slimePrefab;
    [SerializeField] GameObject fireSlimePrefab;

    [SerializeField] private bool drawGizmos;

    [SerializeField] private Vector2 spawnBoxLocation;
    [SerializeField] private Vector2 spawnBoxSize;
    [SerializeField] private float spawnInterval;
    private float nextSpawnTime = 5f;
    [SerializeField] private float speedUpInterval;
    private float nextSpeedUpTime = 10f;

    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start() {
        player = PlayerLocator.Instance.GetComponent<PlayerController>();
        player.PlayerDied += Titlepage.Lost;
    }

    public void IncreaseScore(int scoreamount) {
        Score += scoreamount;
        scoreManager.UpdateScoreUI(Score.ToString());
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(spawnBoxLocation, spawnBoxSize);
    }

    private void Update() {
        if (Time.time > nextSpawnTime) {
            nextSpawnTime = Time.time + spawnInterval;
            SpawnSlime();
        }
        if (Time.time > nextSpeedUpTime) {
            spawnInterval *= 0.99f;
            nextSpeedUpTime = Time.time + speedUpInterval;
        }
    }

    public void SpawnSlime() {
        GameObject slimeToSpawn = (Random.value > 0.25f) ? slimePrefab : fireSlimePrefab;

        Instantiate(slimeToSpawn, new Vector2(Random.Range(-spawnBoxSize.x / 2, spawnBoxSize.x / 2), Random.Range(-spawnBoxSize.y / 2, spawnBoxSize.y / 2)) + spawnBoxLocation, Quaternion.identity);
    }

    public void SpawnPowerup() {

    }

    public void Unpause() {
        CustomEvent.Trigger(this.gameObject, "Unpause");
    }

    public void Pause() {
        CustomEvent.Trigger(this.gameObject, "Pause");
    }
}
