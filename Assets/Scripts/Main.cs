using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Main : MonoBehaviour
{
    private const int PLAYER_X = 1;
    private const int PLAYER_O = 2;
    private const float CORD_Z = 1.6f;
    private const float DECREASE_CORD_Z = 0.8f;
    private const float DEFAULT_POS_X = 3.0f;
    private readonly Quaternion ROTATION_OBJ = Quaternion.Euler(-90, 0, 0);

    private readonly PlayerObject[] _circlePlayer = new PlayerObject[5];
    private readonly PlayerObject[] _crossPlayer = new PlayerObject[5];
    private readonly Transform[] _pointsOnField = new Transform[9];
    private int _circleNumber;
    private int _crossNumber;
    private int[] _cell = new int[9];
    private bool _endGame;
    private int _crossCounter = 5;
    private bool _isMenuWindowActive = false;
    private int[][] _winPositions =
    {
        new[] { 0, 1, 2 }, // Horizontal012
        new[] { 3, 4, 5 }, // Horizontal345
        new[] { 6, 7, 8 }, // Horizontal678
        new[] { 0, 3, 6 }, // Vertical036
        new[] { 1, 4, 7 }, // Vertical147
        new[] { 2, 5, 8 }, // Vertical258
        new[] { 0, 4, 8 }, // Diagonal048
        new[] { 2, 4, 6 }  // Diagonal246
    };

    [SerializeField] private Collider[] _col;
    [SerializeField] private PlayerObject _crossObj;
    [SerializeField] private PlayerObject _circleObj;
    [SerializeField] private Transform _resultObj;
    [SerializeField] private Animator _resultAnimator;
    [SerializeField] private GameObject _winText;
    [SerializeField] private GameObject _loseText;
    [SerializeField] private GameObject _textDraw;
    [SerializeField] private GameObject _restartBtnObj;
    [SerializeField] private GameObject _menuWindow;
    [SerializeField] private Button _restartGameBtn;
    [SerializeField] private Button _menuGameBtn;
    [SerializeField] private Button _exitBtn;

    private enum Animations
    {
        Horizontal012, 
        Horizontal345, 
        Horizontal678, 
        Vertical036,
        Vertical147,
        Vertical258,
        Diagonal048,
        Diagonal246 
    }
    
    private void Start()
    {
        _restartGameBtn.onClick.AddListener(Restart);
        _menuGameBtn.onClick.AddListener(MenuWindow);
        _exitBtn.onClick.AddListener(Quit);

        float cordZ = CORD_Z;
        for (int i = 0; i < 5; i++)
        {
            _circlePlayer[i] = Instantiate(_circleObj, new Vector3(DEFAULT_POS_X, 0, cordZ), ROTATION_OBJ);
            _crossPlayer[i] = Instantiate(_crossObj, new Vector3(-DEFAULT_POS_X, 0, cordZ), ROTATION_OBJ);
            cordZ -= DECREASE_CORD_Z;
        }

        for (int i = 0; i < _col.Length; i++)
        {
            _pointsOnField[i] = _col[i].transform;
        }
    }

    private void MenuWindow()
    {
        _isMenuWindowActive = !_isMenuWindowActive;
        _menuWindow.SetActive(_isMenuWindowActive);
    }

    private void Restart() => SceneManager.LoadScene("SampleScene");

    private void Quit() => Application.Quit();

    private void CheckWinPosition()
    {
        int[] players = { PLAYER_X, PLAYER_O };
        bool gameEnded = false;

        foreach (int player in players)
        {
            foreach (int[] winPosition in _winPositions)
            {
                if (_cell[winPosition[0]] == player &&
                    _cell[winPosition[1]] == player &&
                    _cell[winPosition[2]] == player)
                {
                    ShowEndGameMessage(player == PLAYER_X, (Animations)Array.IndexOf(_winPositions, winPosition));
                    gameEnded = true;
                    break;
                }
            }

            if (gameEnded)
            {
                break;
            }
        }

        if (!gameEnded && _crossCounter == 0)
        {
            Draw();
        }
    }

    private void ShowEndGameMessage(bool win, Animations anim)
    {
        EndGame(anim);
        if (win)
        {
            _winText.SetActive(true);
        }
        else
        {
            _loseText.SetActive(true);
        }
    }
    
    private void Draw()
    {
        _endGame = true;
        _textDraw.SetActive(true);
        _restartBtnObj.SetActive(true);
    }
    
    private void EndGame(Animations anim)
    {
        _endGame = true;
        _restartBtnObj.SetActive(true);
        _resultObj.gameObject.SetActive(true);
        _resultAnimator.SetInteger("Anim",(int) anim );
        foreach (var c in _col)
        {
            c.enabled = false;
        }
    }

    public void SetCross(int id)
    {
        _col[id].enabled = false;
        _crossPlayer[_crossNumber].StartMove(_pointsOnField[id].position, false);
        _crossNumber++;
        _cell[id] = 1;
        _crossCounter--;
        CheckWinPosition();
        if (!_endGame)
        {
            SetCircle();
        }
    }

    private void SetCircle()
    {
        int id = FindPosition();
        _col[id].enabled = false;
        _circlePlayer[_circleNumber].StartMove(_pointsOnField[id].position, true);
        _circleNumber++;
        _cell[id] = 2;
        CheckWinPosition();
    }

    private int FindPosition()
    {
        int freePlaceIndex = 0;
        bool stop = false;
        while (!stop)
        {
            freePlaceIndex = Random.Range(0, _cell.Length);

            if (_cell[freePlaceIndex] == 0)
            {
                stop = true;
            }
        }

        return freePlaceIndex;
    }
}