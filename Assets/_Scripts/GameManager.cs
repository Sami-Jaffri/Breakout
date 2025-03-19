using UnityEngine;
using TMPro;
using NUnit.Framework.Constraints;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform bricksContainer;
    [SerializeField] private TextMeshProUGUI livesCount;
    [SerializeField] private GameObject gameOverMenu;

    private int currentBrickCount;
    private int totalBrickCount;

    private void Start()
    {
        gameOverMenu.SetActive(false);
    }
    private void OnEnable()
    {
        InputHandler.Instance.OnFire.AddListener(FireBall);
        ball.ResetBall();
        totalBrickCount = bricksContainer.childCount;
        currentBrickCount = bricksContainer.childCount;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFire.RemoveListener(FireBall);
    }

    private void FireBall()
    {
        ball.FireBall();
    }

    public void OnBrickDestroyed(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, 0.1f);
        foreach (Collider hitCollider in hitColliders)
        {
            Brick brick = hitCollider.GetComponent<Brick>();
            if (brick != null)
            {
                brick.StartCoroutine(brick.DestroyWithDelay());
            }
        }
        // fire audio here
        // implement particle effect here
        // add camera shake here
        currentBrickCount--;
        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");
        CameraShake shake = FindObjectOfType<CameraShake>();
        if (shake != null)
        {
            shake.ShakeCamera(0.5f, 0.2f);
        }
        if (currentBrickCount == 0) SceneHandler.Instance.LoadNextScene();
    }

    public void KillBall()
    {
        maxLives--;
        // update lives on HUD here
        // game over UI if maxLives < 0, then exit to main menu after delay
        livesCount.SetText(maxLives.ToString());
        if (maxLives <= 0)
        {
            GameOver();
        }
        else
        {
            ball.ResetBall();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);
    }
    public void returnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
