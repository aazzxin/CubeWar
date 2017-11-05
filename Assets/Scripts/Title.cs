using UnityEngine;
using System.Collections;
using UnityEditor;

public class Title : MonoBehaviour {

    private string _sceneName;//房间名称

    [SerializeField]
    private int _roomType;//1为小型房间，2为中型房，3为大型房
    private int _floorWidth;

    private GameObject _creation;

    private GameObject[,] _floor;
    [SerializeField]
    private GameObject[] _floorPrefab;
    private Transform _floorFather;

    private GameObject[,] _obstacle;
    [SerializeField]
    private GameObject[] _obstaclePrefab;
    private Transform _obstacleFather;
    void Awake()
    {
        _sceneName = Application.loadedLevelName;

        switch (_roomType)
        {
            case 1:
                _floorWidth = 3;
                _floor = new GameObject[3, 3];
                _obstacle = new GameObject[30, 30];
                break;
            case 2:
                _floorWidth = 6;
                _floor = new GameObject[6, 6];
                _obstacle = new GameObject[60, 60];
                break;
            case 3:
                _floorWidth = 9;
                _floor = new GameObject[9, 9];
                _obstacle = new GameObject[90, 90];
                break;
        }

        _creation = transform.Find("Creation").gameObject;
        _floorFather = _creation.transform.Find("Floor");
        _obstacleFather = _creation.transform.Find("Obsatcle");

        //floor
        for (int i = 0; i < _floorWidth; i++)
        {
            for (int j = 0; j < _floorWidth; j++)
            {
                int index = Random.Range(0, _floorPrefab.Length);
                if (index < _floorPrefab.Length)
                {
                    _floor[i, j] = Instantiate(_floorPrefab[index]);
                    _floor[i, j].transform.SetParent(_floorFather);
                    _floor[i, j].transform.localPosition = new Vector3(6.75f + i * 15f, 0, 6.75f + j * 15f);
                    int angle = Random.Range(0, 4);
                    _floor[i, j].transform.localRotation = Quaternion.Euler(new Vector3(0, angle * 90, 0));
                    //_floor[i, j].SetActive(false);
                }
            }
        }
        //障碍物设置
        for (int i = 0; i < _floorWidth * 10; i++)
        {
            for (int j = 0; j < _floorWidth * 10; j++)
            {
                int index = Random.Range(0, 200);
                if (index < _obstaclePrefab.Length)
                {
                    _obstacle[i, j] = Instantiate(_obstaclePrefab[index]);
                    _obstacle[i, j].transform.SetParent(_obstacleFather);
                    _obstacle[i, j].transform.localPosition = new Vector3(i * 1.5f, 0.95f, j * 1.5f);
                    int angle = Random.Range(0, 4);
                    _obstacle[i, j].transform.localRotation = Quaternion.Euler(new Vector3(0, angle * 90, 0));
                }
            }
        }


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
