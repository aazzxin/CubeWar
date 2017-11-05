using UnityEngine;
using System.Collections;
using UnityEditor;

public class Room : MonoBehaviour {
    //public Room westRoom, southRoom, northRoom, eastRoom;
    public GameObject westDoor, southDoor, northDoor, eastDoor;
    private bool westLink = false, southLink = false, northLink = false, eastLink = false;//初始为 东西南北门都没有连接
    private GameObject westDoorChild0, westDoorChild1, southDoorChild0, southDoorChild1, northDoorChild0, northDoorChild1, eastDoorChild0, eastDoorChild1;

    public int x, y;
    //public int distance;
    //public bool isInMap = false;

    private string _sceneName;//房间名称

    private bool _savePrefab = false;


    [SerializeField]
    private int _roomType;//1为小型房间，2为中型房，3为大型房
    private int _floorWidth;

    private GameObject _creation;
    public GameObject GetCreation()
    {
        return _creation;
    }
    private GameObject _creeationPrefab;

    private GameObject[,] _floor;
    [SerializeField]
    private GameObject[] _floorPrefab;
    private Transform _floorFather;

    private GameObject[,] _obstacle;
    [SerializeField]
    private GameObject[] _obstaclePrefab;
    private Transform _obstacleFather;

    private GameObject[,,] _enemy;
    [SerializeField]
    private GameObject[] _enemyPrefab, _BossPrefab;
    private Transform _enemyFather;
    private bool isBossRoom = false;
    private bool isAllOut = false;//判断所有怪物是否清空

    [SerializeField]
    private Transform _player;
    private Transform _orignalTransform;//[0,0]点的位置
    private float _orignalPosition_x, _orignalPosition_z;
    private int _tile_x, _tile_y;//角色在矩阵的坐标

    void Awake()
    {
        MainGameManager.playerManager.SetPlayerTransform();
        MainGameManager.uiManager.FindMinimap();

        _sceneName = Application.loadedLevelName;

        x = PlayerPrefs.GetInt(_sceneName + "_x");
        y = PlayerPrefs.GetInt(_sceneName + "_y");
        PlayerPrefs.SetInt("isRead_" + x + "_" + y, 2);
        if (x == PlayerPrefs.GetInt("Boss_x") && y == PlayerPrefs.GetInt("Boss_y")) 
        {
            isBossRoom = true;
        }

        string westRoomName = "", southRoomName="", northRoomName="", eastRoomName="";
        if (PlayerPrefs.GetInt("isInMap_" + (x - 1) + "_" + y) == 1)
            westRoomName = PlayerPrefs.GetString("room_name_" + (x - 2) + "_" + y);
        if (PlayerPrefs.GetInt("isInMap_" + x + "_" + (y + 1)) == 1)
            southRoomName = PlayerPrefs.GetString("room_name_" + x + "_" + (y + 2));
        if (PlayerPrefs.GetInt("isInMap_" + x + "_" + (y - 1)) == 1)
            northRoomName = PlayerPrefs.GetString("room_name_" + x + "_" + (y - 2));
        if (PlayerPrefs.GetInt("isInMap_" + (x + 1) + "_" + y) == 1)
            eastRoomName = PlayerPrefs.GetString("room_name_" + (x + 2) + "_" + y);

        _player = MainGameManager.playerManager.player.transform;

        if(westRoomName!="")
        {
            if (PlayerPrefs.GetInt("isRead_" + (x - 2) + "_" + y) == 0)
            {
                PlayerPrefs.SetInt("isRead_" + (x - 2) + "_" + y, 1);
                PlayerPrefs.SetInt("isRead_" + (x - 1) + "_" + y, 1);
            }
                

            westDoorChild0 = westDoor.transform.GetChild(0).gameObject;
            westDoorChild1 = westDoor.transform.GetChild(1).gameObject;
            SceneChange chuansonmen = westDoorChild1.GetComponent<SceneChange>();
            chuansonmen.nextscene_name = westRoomName;
            chuansonmen.direction = "L";

            //westRoom = new Room();
            //westRoom.name = westRoomName;
            westDoorChild1.SetActive(false);
            westDoorChild0.SetActive(true);

            westLink = true;
        }
        else
        {
            westDoor.SetActive(false);
        }
        if(southRoomName!="")
        {
            if (PlayerPrefs.GetInt("isRead_" + x + "_" + (y + 2)) == 0)
            {
                PlayerPrefs.SetInt("isRead_" + x + "_" + (y + 2), 1);
                PlayerPrefs.SetInt("isRead_" + x + "_" + (y + 1), 1);
            }

            southDoorChild0 = southDoor.transform.GetChild(0).gameObject;
            southDoorChild1 = southDoor.transform.GetChild(1).gameObject;
            SceneChange chuansonmen = southDoorChild1.GetComponent<SceneChange>();
            chuansonmen.nextscene_name = southRoomName;
            chuansonmen.direction = "D";

            //southRoom = new Room();
            //southRoom.name = southRoomName;
            southDoorChild1.SetActive(false);
            southDoorChild0.SetActive(true);

            southLink = true;
        }
        else
        {
            southDoor.SetActive(false);
        }
        if(northRoomName!="")
        {
            if (PlayerPrefs.GetInt("isRead_" + x + "_" + (y - 2)) == 0)
            {
                PlayerPrefs.SetInt("isRead_" + x + "_" + (y - 2), 1);
                PlayerPrefs.SetInt("isRead_" + x + "_" + (y - 1), 1);
            }

            northDoorChild0 = northDoor.transform.GetChild(0).gameObject;
            northDoorChild1 = northDoor.transform.GetChild(1).gameObject;
            SceneChange chuansonmen = northDoorChild1.GetComponent<SceneChange>();
            chuansonmen.nextscene_name = northRoomName;
            chuansonmen.direction = "U";

            //northRoom = new Room();
            //northRoom.name = northRoomName;
            northDoorChild1.SetActive(false);
            northDoorChild0.SetActive(true);

            northLink = true;
        }
        else
        {
            northDoor.SetActive(false);
        }
        if(eastRoomName!="")
        {
            if (PlayerPrefs.GetInt("isRead_" + (x + 2) + "_" + y) == 0)
            {
                PlayerPrefs.SetInt("isRead_" + (x + 2) + "_" + y, 1);
                PlayerPrefs.SetInt("isRead_" + (x + 1) + "_" + y, 1);
            }

            eastDoorChild0 = eastDoor.transform.GetChild(0).gameObject;
            eastDoorChild1 = eastDoor.transform.GetChild(1).gameObject;
            SceneChange chuansonmen = eastDoorChild1.GetComponent<SceneChange>();
            chuansonmen.nextscene_name = eastRoomName;
            chuansonmen.direction = "R";

            //eastRoom = new Room();
            //eastRoom.name = northRoomName;
            eastDoorChild1.SetActive(false);
            eastDoorChild0.SetActive(true);

            eastLink = true;
        }
        else
        {
            eastDoor.SetActive(false);
        }
        

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
        _enemy = new GameObject[2, 2, 4];

        _creation = transform.Find("Creation").gameObject;
        _creeationPrefab = Resources.Load("Room/" + _sceneName) as GameObject;
        _floorFather = _creation.transform.Find("Floor");
        _obstacleFather = _creation.transform.Find("Obsatcle");
        _enemyFather = _creation.transform.Find("Enemy");

        

        if (_creeationPrefab!=null)
        {
            GameObject creationTemp= Instantiate(_creeationPrefab);
            creationTemp.transform.SetParent(transform);
            creationTemp.transform.localPosition = _creation.transform.localPosition;
            Destroy(_creation);
            _creation = creationTemp;

            _floorFather = _creation.transform.Find("Floor");
            _obstacleFather = _creation.transform.Find("Obsatcle");
            _enemyFather = _creation.transform.Find("Enemy");

            for (int i = 0; i < _floorWidth; i++)
            {
                for (int j = 0; j < _floorWidth; j++)
                {
                    _floor[i, j] = _floorFather.GetChild(i * _floorWidth + j).gameObject;
                }
            }
            _orignalTransform = _floor[0, 0].transform;
            _orignalPosition_x = _orignalTransform.position.x - 7.5f;
            _orignalPosition_z = _orignalTransform.position.z - 7.5f;
        }
        else
        {
            //floor
            for (int i = 0; i < _floorWidth; i++)
            {
                for (int j = 0; j < _floorWidth; j++)
                {
                    int index = Random.Range(0, _floorPrefab.Length);
                    _floor[i, j] = Instantiate(_floorPrefab[index]);
                    _floor[i, j].transform.SetParent(_floorFather);
                    _floor[i, j].transform.localPosition = new Vector3(6.75f + i * 15f, 0, 6.75f + j * 15f);
                    int angle = Random.Range(0, 4);
                    _floor[i, j].transform.localRotation = Quaternion.Euler(new Vector3(0, angle * 90, 0));
                    //_floor[i, j].SetActive(false);

                }
            }
            _orignalTransform = _floor[0, 0].transform;
            _orignalPosition_x = _orignalTransform.position.x - 7.5f;
            _orignalPosition_z = _orignalTransform.position.z - 7.5f;
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
            //怪物设置
            if(!isBossRoom)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int index = Random.Range(0, _enemyPrefab.Length);
                        int num = Random.Range(-1, 5);
                        for (int z = 0; z < num; z++)
                        {
                            _enemy[i, j, z] = Instantiate(_enemyPrefab[index]);
                            _enemy[i, j, z].transform.SetParent(_enemyFather);
                            _enemy[i, j, z].transform.localPosition =
                                new Vector3(_floorWidth * 5f * (i + 1) + Random.Range(_floorWidth, 5 * _floorWidth),
                                1.5f,
                                _floorWidth * 5f * (j + 1) + Random.Range(_floorWidth, 5 * _floorWidth));
                            int angle = Random.Range(0, 4);
                            _enemy[i, j, z].transform.localRotation = Quaternion.Euler(new Vector3(0, angle * 90, 0));
                        }
                    }
                }
            }
            else
            {
                int index = Random.Range(0, _BossPrefab.Length);
                _enemy[0, 0, 0] = Instantiate(_BossPrefab[index]);
                _enemy[0, 0, 0].transform.SetParent(_enemyFather);
                _enemy[0, 0, 0].transform.localPosition = new Vector3(_orignalPosition_x + 7.5f * _floorWidth, 1.5f, _orignalPosition_z + 7.5f * _floorWidth);
                int angle = Random.Range(0, 4);
                _enemy[0, 0, 0].transform.localRotation = Quaternion.Euler(new Vector3(0, angle * 90, 0));
            }
        }

        //保存预设
        PrefabUtility.CreatePrefab("Assets/Prefabs/resources/Room/" + _sceneName + ".prefab", _creation, ReplacePrefabOptions.ReplaceNameBased);

        for (int i = 0; i < _floorWidth; i++)
        {
            for (int j = 0; j < _floorWidth; j++)
            {
                _floor[i, j].SetActive(false);
            }
        }

        

        for (int i = 0; i < _floorWidth; i++)
        {
            if (_player.position.x > _orignalPosition_x + 15 * i && _player.position.x < _orignalPosition_x + 15 * (i + 1))
            {
                _tile_x = i;
            }
        }
        for (int j = 0; j < _floorWidth; j++)
        {
            if (_player.position.z > _orignalPosition_z + 15 * j && _player.position.z < _orignalPosition_z + 15 * (j + 1))
            {
                _tile_y = j;
            }
        }
        _floor[_tile_x, _tile_y].SetActive(true);
        if (_tile_x > 0)
            _floor[_tile_x - 1, _tile_y].SetActive(true);
        if (_tile_x < _floorWidth - 1)
            _floor[_tile_x + 1, _tile_y].SetActive(true);
        if (_tile_y > 0)
        {
            _floor[_tile_x, _tile_y - 1].SetActive(true);
            if (_tile_x > 0)
                _floor[_tile_x - 1, _tile_y - 1].SetActive(true);
            if (_tile_x < _floorWidth - 1)
                _floor[_tile_x + 1, _tile_y - 1].SetActive(true);
        }
        if (_tile_y < _floorWidth - 1)
        {
            _floor[_tile_x, _tile_y + 1].SetActive(true);
            if (_tile_x > 0)
                _floor[_tile_x - 1, _tile_y + 1].SetActive(true);
            if (_tile_x < _floorWidth - 1)
                _floor[_tile_x + 1, _tile_y + 1].SetActive(true);
        }
    }

    void Update()
    {
        SquaredFloor();

        OpenDoor();

        Globe.time += Time.deltaTime;

        //if(_sceneName!=Globe.loadName&&!_savePrefab)
        //{
        //    //保存预设
        //    PrefabUtility.CreatePrefab("Assets/Prefabs/resources/Room/" + _sceneName + ".prefab", _creation, ReplacePrefabOptions.ReplaceNameBased);
        //    _savePrefab = true;
        //}
    }

    void SquaredFloor()
    {
        if (_player.position.x > _orignalPosition_x + (_tile_x + 1) * 15)
        {
            if (_tile_x > 0)
            {
                _floor[_tile_x - 1, _tile_y].SetActive(false);
                if (_tile_y > 0)
                    _floor[_tile_x - 1, _tile_y - 1].SetActive(false);
                if (_tile_y < _floorWidth - 1)
                    _floor[_tile_x - 1, _tile_y + 1].SetActive(false);
            }
            _tile_x += 1;
            if (_tile_x < _floorWidth - 1)
            {
                _floor[_tile_x + 1, _tile_y].SetActive(true);
                if (_tile_y > 0)
                    _floor[_tile_x + 1, _tile_y - 1].SetActive(true);
                if (_tile_y < _floorWidth - 1)
                    _floor[_tile_x + 1, _tile_y + 1].SetActive(true);
            }

        }
        else if (_player.position.x < _orignalPosition_x + (_tile_x) * 15)
        {
            if (_tile_x < _floorWidth - 1)
            {
                _floor[_tile_x + 1, _tile_y].SetActive(false);
                if (_tile_y > 0)
                    _floor[_tile_x + 1, _tile_y - 1].SetActive(false);
                if (_tile_y < _floorWidth - 1)
                    _floor[_tile_x + 1, _tile_y + 1].SetActive(false);
            }
            _tile_x -= 1;
            if (_tile_x > 0)
            {
                _floor[_tile_x - 1, _tile_y].SetActive(true);
                if (_tile_y > 0)
                    _floor[_tile_x - 1, _tile_y - 1].SetActive(true);
                if (_tile_y < _floorWidth - 1)
                    _floor[_tile_x - 1, _tile_y + 1].SetActive(true);
            }

        }
        if (_player.position.z > _orignalPosition_z + (_tile_y + 1) * 15)
        {
            if (_tile_y > 0)
            {
                _floor[_tile_x, _tile_y - 1].SetActive(false);
                if (_tile_x > 0)
                    _floor[_tile_x - 1, _tile_y - 1].SetActive(false);
                if (_tile_x < _floorWidth - 1)
                    _floor[_tile_x + 1, _tile_y - 1].SetActive(false);
            }
            _tile_y += 1;
            if (_tile_y < _floorWidth - 1)
            {
                _floor[_tile_x, _tile_y + 1].SetActive(true);
                if (_tile_x > 0)
                    _floor[_tile_x - 1, _tile_y + 1].SetActive(true);
                if (_tile_x < _floorWidth - 1)
                    _floor[_tile_x + 1, _tile_y + 1].SetActive(true);
            }
        }
        else if (_player.position.z < _orignalPosition_z + (_tile_y) * 15)
        {
            if (_tile_y < _floorWidth - 1)
            {
                _floor[_tile_x, _tile_y + 1].SetActive(false);
                if (_tile_x > 0)
                    _floor[_tile_x - 1, _tile_y + 1].SetActive(false);
                if (_tile_x < _floorWidth - 1)
                    _floor[_tile_x + 1, _tile_y + 1].SetActive(false);
            }
            _tile_y -= 1;
            if (_tile_y > 0)
            {
                _floor[_tile_x, _tile_y - 1].SetActive(true);
                if (_tile_x > 0)
                    _floor[_tile_x - 1, _tile_y - 1].SetActive(true);
                if (_tile_x < _floorWidth - 1)
                    _floor[_tile_x + 1, _tile_y - 1].SetActive(true);
            }
        }
    }
    void OpenDoor()
    {
        if(!isAllOut)
        {
            isAllOut = true;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        if (_enemy[i, j, z] != null)
                        {
                            isAllOut = false;
                        }
                    }
                }
            }
            if (isAllOut)
            {
                if(!isBossRoom)
                {
                    if (westLink)
                    {
                        westDoorChild1.SetActive(true);
                        westDoorChild0.SetActive(false);
                    }
                    if (southLink)
                    {
                        southDoorChild1.SetActive(true);
                        southDoorChild0.SetActive(false);
                    }
                    if (northLink)
                    {
                        northDoorChild1.SetActive(true);
                        northDoorChild0.SetActive(false);
                    }
                    if (eastLink)
                    {
                        eastDoorChild1.SetActive(true);
                        eastDoorChild0.SetActive(false);
                    }
                }
                else
                {
                    PlayerPrefs.SetString("direction", "C");

                    Globe.loadName = "Liquidation";
                    Application.LoadLevel("Loading");
                }
            }
        }
        
    }
}
