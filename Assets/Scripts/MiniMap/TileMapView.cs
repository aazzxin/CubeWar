using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TileMapView : MonoBehaviour {
    private string _sceneName;
    private int x, y;

    private const int _mapLength = 7;
    private GameObject[,] _map = new GameObject[_mapLength, _mapLength];
    private Image[,] _tile = new Image[_mapLength, _mapLength];

    private float min = 0.5f;
    private float max = 1.5f;
    private float currentValue = 1.0f;
    private float changValue = 0.25f;
    void Awake()
    {
        _sceneName = Application.loadedLevelName;
        x = PlayerPrefs.GetInt(_sceneName + "_x");
        y = PlayerPrefs.GetInt(_sceneName + "_y");
        
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 pivot = rect.pivot;
        rect.pivot = new Vector2(pivot.x + x * 0.123f, pivot.y - y * 0.123f);
        transform.localPosition = new Vector3(0, 0, 0);

        for (int i = 0; i < _mapLength; i ++)
        {
            for (int j = 0; j < _mapLength; j ++)
            {
                _map[i, j] = transform.GetChild(7 * i + j).gameObject;
            }
        }

        for (int i=0;i< _mapLength; i+=2)
        {
            for(int j=0;j<_mapLength;j+=2)
            {
                _tile[i, j] = transform.GetChild(7 * i + j).GetChild(0).GetComponent<Image>();
                _map[i, j].SetActive(false);
            }
        }
        for (int i = 1; i < _mapLength; i += 2)
        {
            for (int j = 0; j < _mapLength; j += 2)
            {
                _tile[i, j] = transform.GetChild(7 * i + j).GetComponent<Image>();
                _map[i, j].SetActive(false);
            }
        }
        for (int i = 0; i < _mapLength; i += 2)
        {
            for (int j = 1; j < _mapLength; j += 2)
            {
                _tile[i, j] = transform.GetChild(7 * i + j).GetComponent<Image>();
                _map[i, j].SetActive(false);
            }
        }

        
    }

    void Start()
    {
        for (int i = 0; i < _mapLength; i+=2)
        {
            for (int j = 0; j < _mapLength; j+=2)
            {
                if (PlayerPrefs.GetInt("isRead_" + j + "_" + i) != 0)
                {
                    _map[i, j].SetActive(true);
                    if(PlayerPrefs.GetInt("isRead_" + j + "_" + i) == 1)
                    {
                        GameObject bossIcon = Instantiate(Resources.Load("Minimap/QuestionMark")) as GameObject;
                        bossIcon.transform.SetParent(_map[i, j].transform);
                        bossIcon.transform.localPosition = new Vector3(0, 0, 0);
                    }
                    if (j == PlayerPrefs.GetInt("Boss_x") && i == PlayerPrefs.GetInt("Boss_y")) 
                    {
                        GameObject bossIcon = Instantiate(Resources.Load("Minimap/Boss")) as GameObject;
                        bossIcon.transform.SetParent(_map[i, j].transform);
                        bossIcon.transform.localPosition = new Vector3(0, 0, 0);
                    }
                }
            }
        }
        for (int i = 1; i < _mapLength; i += 2)
        {
            for (int j = 0; j < _mapLength; j += 2)
            {
                if (PlayerPrefs.GetInt("isRead_" + j + "_" + i) == 1)
                {
                    _map[i, j].SetActive(true);
                }
            }
        }
        for (int i = 0; i < _mapLength; i += 2)
        {
            for (int j = 1; j < _mapLength; j += 2)
            {
                if (PlayerPrefs.GetInt("isRead_" + j + "_" + i) == 1)
                {
                    _map[i, j].SetActive(true);
                }
            }
        }
        _tile[y, x].color = new Color(255, 0, 0);
    }

    void Update()
    {
        if (Input.GetButtonDown("Q") && currentValue < max)
        {
            currentValue += changValue;
            Debug.Log("+");
        }

        if (Input.GetButtonDown("E") && currentValue > min)
        {
            currentValue -= changValue;
            Debug.Log("-");
        }

        transform.localScale = new Vector3(currentValue, currentValue, 1);
    }
}
