using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private Button _startGameBtn;

    private void Start() => _startGameBtn.onClick.AddListener(StartGame);

    private void StartGame() => SceneManager.LoadScene("GameScene");
}