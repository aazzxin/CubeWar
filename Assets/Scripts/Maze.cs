using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class Maze : MonoBehaviour
{
    public struct RoomData
    {
        public int x, y;
        public bool isInMap;
    }
    private int _distance;

    public string MazeType = "Plane";

    private const int MapLength = 7;
    public RoomData[,] tileMap = new RoomData[MapLength, MapLength];

    public Maze()
    {
        for (int i = 0; i < MapLength; i++)
        {
            for (int j = 0; j < MapLength; j++)
            {
                tileMap[i, j].x = j;
                tileMap[i, j].y = i;
                tileMap[i, j].isInMap = false;
            }
        }
    }

    //List<RoomData> moves = new List<RoomData>();
    Stack<RoomData> moves = new Stack<RoomData>();

    private int S, M, L;//记录小、中、大型房间的编号
    private int BOSS;
    private int Boss_x, Boss_y;
    private bool isSettingBoss = false;
    private int SHOP;
    private int Shop_x, Shop_y;
    private bool isSettingShop = false;//当设置房间为Shop时为true，使得下一次的迷宫设置步骤一定为后退一格，确保Shop的入口只有一个

    int startX, startY;
    public void Awake()
    {
        _distance = 0;

        for (int i = 0; i < MapLength; i++)
        {
            for (int j = 0; j < MapLength; j++)
            {
                PlayerPrefs.SetString("room_name_" + j + "_" + i, "");//初始化x，y坐标Rooom的名称为空
                PlayerPrefs.SetInt("isInMap_" + j + "_" + i, 0);//设置x，y坐标Room不在地图上
                PlayerPrefs.SetInt("isRead_" + j + "_" + i, 0);
            }
        }
        S = 0;M = 0;L = 0;
        BOSS = 0;
        SHOP = 0;
        for(int i=1;i<=5;i++)
        {
            PlayerPrefs.SetInt(MazeType + "_S_" + i + "_x", -1);
            PlayerPrefs.SetInt(MazeType + "_S_" + i + "_y", -1);
            AssetDatabase.DeleteAsset("Assets/Prefabs/resources/Room/" + MazeType + "_S_" + i + ".prefab");
            PlayerPrefs.SetInt(MazeType + "_M_" + i + "_x", -1);
            PlayerPrefs.SetInt(MazeType + "_M_" + i + "_y", -1);
            AssetDatabase.DeleteAsset("Assets/Prefabs/resources/Room/" + MazeType + "_M_" + i + ".prefab");
            PlayerPrefs.SetInt(MazeType + "_L_" + i + "_x", -1);
            PlayerPrefs.SetInt(MazeType + "_L_" + i + "_y", -1);
            AssetDatabase.DeleteAsset("Assets/Prefabs/resources/Room/" + MazeType + "_L_" + i + ".prefab");
        }
        PlayerPrefs.SetInt("Boss_x", -1);
        PlayerPrefs.SetInt("Boss_y", -1);
        PlayerPrefs.SetInt("Shop_x", -1);
        PlayerPrefs.SetInt("Shop_y", -1);

        

        //设定初始坐标及其房间
        startX = Random.Range(0, 4);
        startX *= 2;
        startY = Random.Range(0, 4);
        startY *= 2;
        PlayerPrefs.SetInt("isRead_" + startX + "_" + startY, 1);
        //SetRoom(startX, startY);
        M++;//初始房间设定为中型房
        PlayerPrefs.SetString("room_name_" + startX + "_" + startY, MazeType + "_M_" + M);
        PlayerPrefs.SetInt("isInMap_" + startX + "_" + startY, 1);
        tileMap[startY, startX].isInMap = true;
        PlayerPrefs.SetInt(MazeType + "_M_" + M + "_x", startX);
        PlayerPrefs.SetInt(MazeType + "_M_" + M + "_y", startY);
        SceneChange normalModeSceneChange = this.gameObject.GetComponent<SceneChange>();
        normalModeSceneChange.direction = "C";
        normalModeSceneChange.nextscene_name = MazeType + "_M_" + M;

        //开始随机迷宫
        Query(tileMap[startY, startX]);
        CutWall();

    }

    public void Query(RoomData start)//迷宫随机生成
    {
        int x = start.x, y = start.y;

        string dirs = "";
        if(!isSettingShop)
        {
            if ((y - 2 >= 0) && !(tileMap[y - 2, x].isInMap)) dirs += 'N';
            if ((x - 2 >= 0) && !(tileMap[y, x - 2].isInMap)) dirs += 'W';
            if ((y + 2 < MapLength) && !(tileMap[y + 2, x].isInMap)) dirs += 'S';
            if ((x + 2 < MapLength) && !(tileMap[y, x + 2].isInMap)) dirs += 'E';
        }

        if (dirs == "")
        {
            _distance--;

            //moves.RemoveAt(moves.Count - 1);
            moves.Pop();
            if (isSettingShop)
            {
                isSettingShop = false;
                if (moves.Count == 0)
                    Query(tileMap[startY, startX]);
                else
                    Query(moves.Peek());//Query(moves[moves.Count - 1]);
            }
            else
            {
                if (moves.Count == 0)
                    return;//Draw();
                else
                    Query(moves.Peek());//Query(moves[moves.Count - 1]);
            }
            
        }
        else
        {
            _distance++;

            string dir = dirs.Substring((Random.Range(0, dirs.Length)), 1);

            switch (dir)
            {
                case "N":
                    tileMap[y - 1,x].isInMap = true;
                    PlayerPrefs.SetInt("isInMap_" + x + "_" + (y - 1), 1);
                    //SetRoom(x, y - 1);
                    y = y - 2;
                    break;
                case "W":
                    tileMap[y,x - 1].isInMap = true;
                    PlayerPrefs.SetInt("isInMap_" + (x - 1) + "_" + y, 1);
                    //SetRoom(x - 1, y);
                    x = x - 2;
                    break;
                case "S":
                    tileMap[y + 1,x].isInMap = true;
                    PlayerPrefs.SetInt("isInMap_" + x + "_" + (y + 1), 1);
                    //SetRoom(x, y + 1);
                    y = y + 2;
                    break;
                case "E":
                    tileMap[y,x + 1].isInMap = true;
                    PlayerPrefs.SetInt("isInMap_" + (x + 1) + "_" + y, 1);
                    //SetRoom(x + 1, y);
                    x = x + 2;
                    break;
            }
            tileMap[y, x].isInMap = true;
            if (BOSS < 1 && _distance == 4) 
            {
                isSettingBoss = true;
                SetRoom(x, y);
                isSettingBoss = false;
            }
            else
            {
                SetRoom(x, y);
            }
            //moves.Add(tileMap[y, x]);
            moves.Push(tileMap[y, x]);
            Query(tileMap[y, x]);
        }
    }

    private void SetRoom(int x, int y, int setoff = -1)
    {
        int index;
        do
            index = Random.Range(0, 4);
        while (index == setoff);

        switch (index)
        {
            case 0:
                if (SHOP < 1)
                {
                    if(isSettingBoss)
                        SetRoom(x, y, 0);
                    else
                    {
                        SHOP++;
                        PlayerPrefs.SetString("room_name_" + x + "_" + y, "Shop");
                        PlayerPrefs.SetInt("isInMap_" + x + "_" + y, 1);
                        PlayerPrefs.SetInt("Shop_x", x);
                        PlayerPrefs.SetInt("Shop_y", y);
                        Shop_x = x;
                        Shop_y = y;

                        isSettingShop = true;
                    }
                }
                else
                    SetRoom(x, y, 0);
                break;
            case 1:
                if (S < 5)
                {
                    S++;
                    PlayerPrefs.SetString("room_name_" + x + "_" + y, MazeType + "_S_" + S);
                    PlayerPrefs.SetInt("isInMap_" + x + "_" + y, 1);
                    PlayerPrefs.SetInt(MazeType + "_S_" + S + "_x", x);
                    PlayerPrefs.SetInt(MazeType + "_S_" + S + "_y", y);
                    if (isSettingBoss)
                    {
                        BOSS++;
                        PlayerPrefs.SetInt("Boss_x", x);
                        PlayerPrefs.SetInt("Boss_y", y);
                        Boss_x = x;
                        Boss_y = y;
                    }
                }
                else
                    SetRoom(x, y, 1);
                break;
            case 2:
                if (M < 5)
                {
                    M++;
                    PlayerPrefs.SetString("room_name_" + x + "_" + y, MazeType + "_M_" + M);
                    PlayerPrefs.SetInt("isInMap_" + x + "_" + y, 1);
                    PlayerPrefs.SetInt(MazeType + "_M_" + M + "_x", x);
                    PlayerPrefs.SetInt(MazeType + "_M_" + M + "_y", y);
                    if (isSettingBoss)
                    {
                        BOSS++;
                        PlayerPrefs.SetInt("Boss_x", x);
                        PlayerPrefs.SetInt("Boss_y", y);
                        Boss_x = x;
                        Boss_y = y;
                    }
                }
                else
                    SetRoom(x, y, 2);
                break;
            case 3:
                if (L < 5)
                {
                    L++;
                    PlayerPrefs.SetString("room_name_" + x + "_" + y, MazeType + "_L_" + L);
                    PlayerPrefs.SetInt("isInMap_" + x + "_" + y, 1);
                    PlayerPrefs.SetInt(MazeType + "_L_" + L + "_x", x);
                    PlayerPrefs.SetInt(MazeType + "_L_" + L + "_y", y);
                    if (isSettingBoss)
                    {
                        BOSS++;
                        PlayerPrefs.SetInt("Boss_x", x);
                        PlayerPrefs.SetInt("Boss_y", y);
                        Boss_x = x;
                        Boss_y = y;
                    }
                }
                else
                    SetRoom(x, y, 3);
                break;
        }


    }

    private void CutWall()
    {
        //随机挖墙，使迷宫更复杂
        for (int i = 1; i < MapLength; i+=2)
        {
            for (int j = 0; j < MapLength; j+=2)
            {
                if ((j != Shop_x || (i != Shop_y + 1 && i != Shop_y - 1))
                    && (j != Boss_x || (i != Boss_y + 1 && i != Boss_y - 1))) //不在商店和Boss房周围才能挖墙
                {
                    float index = Random.Range(0, 4);
                    if (index < 1)
                    {
                        tileMap[i, j].isInMap = true;
                        PlayerPrefs.SetInt("isInMap_" + j + "_" + i, 1);
                    }
                }
            }
        }
        for (int i = 0; i < MapLength; i += 2)
        {
            for (int j = 1; j < MapLength; j += 2)
            {
                if ((i != Shop_y || (j != Shop_x - 1 && j != Shop_x + 1))
                    && (i != Boss_y || (j != Boss_x - 1 && j != Boss_x + 1))) //不在商店和Boss房周围才能挖墙
                {
                    float index = Random.Range(0, 4);
                    if (index < 1)
                    {
                        tileMap[i, j].isInMap = true;
                        PlayerPrefs.SetInt("isInMap_" + j + "_" + i, 1);
                    }
                }
            }
        }
    }

    //private void Draw()//迷宫生成时，Room关系完成时的善后工作。即完善房间东南西北Room对象
    //{
    //    for (int i = 0; i < MapLength; i++)
    //    {
    //        for (int j = 0; j < MapLength; j++)
    //        {
    //            if(tileMap[i,j].isInMap)
    //            {
    //                if (i - 1 >= 0)
    //                {
    //                    if (tileMap[i - 1, j].isInMap)
    //                    {
    //                        tileMap[i, j].northRoom = tileMap[i - 1, j];
    //                        tileMap[i - 1, j].southRoom = tileMap[i, j];
    //                    }
    //                }
    //                if (j - 1 >= 0)
    //                {
    //                    if (tileMap[i, j - 1].isInMap)
    //                    {
    //                        tileMap[i, j].westRoom = tileMap[i, j - 1];
    //                        tileMap[i, j - 1].eastRoom = tileMap[i, j];
    //                    }
    //                }
    //                if (i + 1 < MapLength)
    //                {
    //                    if (tileMap[i + 1, j].isInMap)
    //                    {
    //                        tileMap[i, j].southRoom = tileMap[i + 1, j];
    //                        tileMap[i + 1, j].northRoom = tileMap[i, j];
    //                    }
    //                }
    //                if (j + 1 < MapLength)
    //                {
    //                    if (tileMap[i, j + 1].isInMap)
    //                    {
    //                        tileMap[i, j].eastRoom = tileMap[i, j + 1];
    //                        tileMap[i, j + 1].westRoom = tileMap[i, j];
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}
