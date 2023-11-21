using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int puntos;

    [Header("Fases")]
    private StageData currentStage;
    public StageData[] stages;

    [SerializeField] private int currentStageIndex;

    [Header("Spawn")]
    public EnemiesSpawner enemiesSpawner;

    [SerializeField] private float currentStageTime;

    private PlayerHP playerHealth;

    [SerializeField] private Text puntosText;
    private void Start()
    {
        puntos = 0;
        currentStage = stages[0];
        currentStageIndex = 0;
        currentStageTime = 0;
        playerHealth = GameObject.FindGameObjectWithTag("canon").GetComponent<PlayerHP>();

        enemiesSpawner.Enemies = currentStage.list;
    }

    private void Update()
    {
        ManageStagesFlow();
        RefreshScoreUI();
    }

    private void ManageStagesFlow()
    {
        currentStage = stages[currentStageIndex];
        bool currentStageTimeHasPassed = currentStageTime >= currentStage.tiempoSiguienteFase;
        if (currentStageTimeHasPassed)
        {
            GoToNextStage();
        }
        currentStageTime += Time.deltaTime;
    }

    private void GoToNextStage()
    {
        currentStageTime = 0;
        if (currentStageIndex < stages.Length - 1)
        {
            currentStageIndex++;
            enemiesSpawner.Enemies = currentStage.list;
        }
        else
        {
            SceneManager.LoadScene("PosGame");
        }
    }

    private void RefreshScoreUI()
    {
        puntosText.text = puntos.ToString();
    }
}
