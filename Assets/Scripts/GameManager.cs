using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    public Color[] colors;
    Color _currentColor;
    public Color _selectColor;

    public GameObject[] _levels;
    public GameObject _textUI;
    public GameObject _selectBlok;
    public GameObject[] _startScreen;
    public GameObject _mainMenu;
    public GameObject _gameOverMenu;

    SpriteRenderer _spriteRenderer;

    public bool _levelChanged;
    public  bool _blockSelected;
    bool _gameStarted;

    public int levelCount;
    public int _bloksCount;
    int _maxRandomNumber;
    public int _skoreCount;
    public int _bestScore;
    public Text _skoreText;
    public Text _bestScoreText;

    private static GameManager _myInstance;
    public static GameManager _MyInstance {
        get 
        {
           return _myInstance; 
        } 
    }
    #endregion

    #region Unity Funcs
    private void Awake()
    {
        if (_myInstance == null)
        {
            _myInstance = this;
        }
        PlayerPrefs.GetInt("BestScore");
    }
    void Start()
    {
        _bestScore =   PlayerPrefs.GetInt("BestScore");
        _gameStarted = false;
        _textUI.SetActive(false);
        _gameOverMenu.SetActive(false);
        ChangeColor();    
    }
    // Update is called once per frame
    void Update()
    {
        StartScreenAnimation();      
    }
    private void LateUpdate()
    {
        _skoreText.text = "skore: " + _skoreCount;
        _bestScoreText.text = "Best Score : " + _bestScore;
    }

    #endregion

    #region Level Managment  and  select true block
    //  yanıp söncek bloğun int  değerini verir
    int SelectRandomBlok() 
    {
        int selectRandomBlok = Random.Range(0, CreateMaxRandomNumber());
        return selectRandomBlok;
    }
    //   yanıp sönecek blogu GameObject olarak döndürür
    public GameObject GetSelectedBlok() 
    {
        _selectBlok = _levels[_bloksCount].transform.GetChild(SelectRandomBlok()).gameObject;
        _selectBlok.GetComponent<SpriteRenderer>().color = Color.black;
        _blockSelected = true;
        return _selectBlok;
    }
    public void LevelChanger() 
    {   
        _levels[_bloksCount].SetActive(false);
        levelCount++;
        CreateLevels();
        Invoke("ActiveBloks", 0.1f);
        ChangeColor();
        GetSelectedBlok();
        Invoke("FeedBackForBlok", 0.2f);
    }
    //  hangi level aktifse o  blokları aktif eder
    void ActiveBloks() 
    {
        _levels[_bloksCount].SetActive(true);
    }
    public int CreateLevels() 
    {
        if (levelCount < 4)
        {
            _bloksCount = 0;
        }
        else if (levelCount > 4 && levelCount<8)
        {
            _bloksCount = 1;
        }
        else if (levelCount > 8 && levelCount < 16)
        {
            _bloksCount = 2;
        }
        else if (levelCount > 16 && levelCount < 32)
        {
            _bloksCount = 3;
        }
        else if (levelCount > 32 && levelCount < 64)
        {
            _bloksCount =4;
        }
        else if (levelCount > 64 && levelCount < 128)
        {
            _bloksCount = 5;
        }
        else if (levelCount > 128 )
        {
            _bloksCount = 6;
        }
   
        return _bloksCount;
       
    }
    #endregion

    #region Color Progress
    // bölümdeki blok sayısının en  fazla ne kadar olacağını belirler

    int CreateMaxRandomNumber() 
    {
        switch (_bloksCount)
        {
            case 0:
                _maxRandomNumber = 3;
                break;
            case 1:
                _maxRandomNumber = 8;
                break;
            case 2:
                _maxRandomNumber = 15;
                break;
            case 3:
                _maxRandomNumber = 24;
                break;
            case 4:
                _maxRandomNumber = 35;
                break;
            case 5:
                _maxRandomNumber = 48;
                break;
            case 6:
                _maxRandomNumber = 63;
                break;
            
            default:
                _maxRandomNumber = 3;
                break;
        }
        return _maxRandomNumber;
    }
    public void ReturnCurrentColor() 
    {
        
        _selectBlok.GetComponent<SpriteRenderer>().color = _currentColor;
        _blockSelected = false;
    }
    void ApplyColor() 
    {
        for (int i = 0; i <= CreateMaxRandomNumber(); i++)
        {
            GameObject child = _levels[CreateLevels()].transform.GetChild(i).gameObject;
            child.GetComponent<SpriteRenderer>().color = _currentColor;
        }
    }
    void ReturnColor() 
    {
        _selectBlok.GetComponent<SpriteRenderer>().color = _currentColor;
    }
    void SelectColor() 
    {
        _selectBlok.GetComponent<SpriteRenderer>().color = _selectColor ;
    }
    void FeedBackForBlok() 
    {
        SelectColor();
        Invoke("ReturnColor", 0.05f);
            
    }

    public void ChangeColor()
    {
        int random = Random.RandomRange(0, colors.Length);
        _currentColor = colors[random];
        ApplyColor();
    }
    #endregion
    #region Gamestate and UI progress
    public void GameStar()
    {
        levelCount = 0;
        _gameStarted = true;
        _mainMenu.SetActive(false);
        _gameOverMenu.SetActive(false);
        _textUI.SetActive(true);
        LevelChanger();
    }
    public void GameOver() 
    {
        
        if (PlayerPrefs.HasKey("BestScore"))
        {
            if (_skoreCount > _bestScore)
            {
                _bestScore = _skoreCount;
                PlayerPrefs.SetInt("BestScore", _bestScore);
            }     
        }
        else
        {
            _bestScore = _skoreCount;
            PlayerPrefs.SetInt("BestScore", _bestScore);
        }
        _textUI.SetActive(false);
        _mainMenu.SetActive(false);
        _gameOverMenu.SetActive(true);
    }
    public void TryAgain() 
    {
     SceneManager.LoadScene("Game");
    }
    #endregion
    #region startScreenAnim
    void StartScreenAnimation() 
    {
        if (_gameStarted)
        {
            for (int i = 0; i < _startScreen.Length; i++)
            {
                _startScreen[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < _startScreen.Length; i++)
            {
                _startScreen[i].SetActive(true);
                int startRandom = Random.Range(0, colors.Length);
                _startScreen[i].GetComponent<SpriteRenderer>().color = colors[startRandom];
            }
        }
        
    }
    #endregion
}
