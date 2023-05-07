using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MazeGame : MonoBehaviour       //x y좌표를 혼동하여 코딩함.... 이해바람
{
    public UnityEvent forwardAble, forwardDis;
    public TMP_Text text, pop_text;

    public GameObject pop;


    struct Tile
    {
        public bool forward, back, left, right;
        public bool isKey, exit;
        /*public Tile(bool forward, bool back, bool left, bool right, bool isKey, bool exit)
		{
            this.forward = forward; this.back = back; this.left = left; this.right = right; this.isKey = isKey; this.exit = exit;
		}*/
	}

    struct Player
	{
        public int x, y, dir; // dir 0은 왼쪽, 1은 위, 2는 오른쪽, 3은 아래를 향한것
        public bool hasKey;
        public Player(int x, int y, int dir, bool key)
		{
            this.x = x; this.y = y; this.dir = dir; hasKey = key;
		}
	}

    private Tile[,] Map = new Tile[10, 10];
    private int[,] code = new int[10, 10] { {12,5,5,5,5,5,5,5,5,7 },{6,5,5,5,5,5,5,5,5,9 },{8,1,5,5,6,5,5,1,5,7},{11,10,6,7,10,12,7,10,11,10 },
        {10,10,13,10,8,5,9,10,8,9 },{3,0,5,2,5,5,5,2,14,11 },{10,10,6,1,1,1,1,1,1,10 },{13,10,8,2,2,2,2,2,9,10 },{11,3,5,5,5,5,5,5,5,4 },{8,9,12,5,5,5,5,5,5,13 } };
    private int keyx = 3, keyy = 5, exitx = 9, exity = 9;
    private Player player = new Player(0, 0, 0, false); //플레이어 시작위치
    int[,] move = new int[4, 2] { {-1, 0 }, {0, -1 }, {1, 0 }, {0 , 1} }; //플레이어가 향한 방향에 따른 이동값

    void setwall(int i, int j) //code표에 의해 맵을 생성하는 코드 매우 비효율적인거 같으니 나중에 쓸거면 고쳐쓰자!
	{
        Map[i, j].forward = false; Map[i, j].back = false; Map[i, j].left = false; Map[i, j].right = false; Map[i, j].isKey = false; Map[i, j].exit = false;
        switch (code[i, j])     //true는 벽이 있다는 의미
		{
            case 0: 
                //no wall
                break;
            case 1:
                Map[i, j].forward = true;
                break;
            case 2:
                Map[i, j].back = true;
                break;
            case 3:
                Map[i, j].left = true;
                break;
            case 4:
                Map[i, j].right = true;
                break;
            case 5:
                Map[i, j].forward = true;
                Map[i, j].back = true;
                break;
            case 6:
                Map[i, j].left = true;
                Map[i, j].forward = true;
                break;
            case 7:
                Map[i, j].forward = true;
                Map[i, j].right = true;
                break;
            case 8:
                Map[i, j].left = true;
                Map[i, j].back = true;
                break;
            case 9:
                Map[i, j].right = true;
                Map[i, j].back = true;
                break;
            case 10:
                Map[i, j].right = true;
                Map[i, j].left = true;
                break;
            case 11:
                Map[i, j].right = true;
                Map[i, j].left = true;
                Map[i, j].forward = true;
                break;
            case 12:
                Map[i, j].back = true;
                Map[i, j].left = true;
                Map[i, j].forward = true;
                break;
            case 13:
                Map[i, j].right = true;
                Map[i, j].left = true;
                Map[i, j].back = true;
                break;
            case 14:
                Map[i, j].right = true;
                Map[i, j].back = true;
                Map[i, j].forward = true;
                break;
            case 15:
                Map[i, j].right = true;
                Map[i, j].left = true;
                Map[i, j].forward = true;
                Map[i, j].back = true;
                break;
        }
	}

    void debug()
	{
        Debug.Log("Player x, y : " + player.x + " " + player.y + "    dir : " + player.dir);
        Debug.Log(Map[player.x, player.y].forward + " " + Map[player.x, player.y].back + " " + Map[player.x, player.y].left + " "+ Map[player.x, player.y].right + " ");
	}
    void makeMap()
    {
        for(int i = 0; i < 10; i++)
		{
            for(int j = 0; j<10; j++)
			{
                setwall(i, j);  
			}
		}
        Map[keyx, keyy].isKey = true;
        Map[exitx, exity].exit = true;
    }
    void Start()
    {
        makeMap();
        buttondis();
        debug();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
		{
            pop_text.text = "Do you want to Exit?";
            Vector3 mid = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2); //카메라 정중앙 위치 계산
            pop.transform.position = mid;       //카메라 중앙으로 Setting창 이동
        }
    }

    public void buttondis()
	{
		switch (player.dir)
		{
            case 0:
                if (Map[player.x, player.y].left) forwardDis.Invoke();
                else forwardAble.Invoke();
                break;
            case 1:
                if (Map[player.x, player.y].forward) forwardDis.Invoke();
                else forwardAble.Invoke();
                break;
            case 2:
                if (Map[player.x, player.y].right) forwardDis.Invoke();
                else forwardAble.Invoke();
                break;
            case 3:
                if (Map[player.x, player.y].back) forwardDis.Invoke();
                else forwardAble.Invoke();
                break;
        }
	}

    public void moveforward()
	{
        player.y += move[player.dir, 0]; 
        player.x += move[player.dir, 1];
        buttondis();
        if (player.x == keyx && player.y == keyy) { text.text="Object : Find a way Out"; player.hasKey = true; Map[player.x, player.y].isKey = false; Debug.Log("Key found"); }
        if (player.x == exitx && player.y == exity)
		{
            if(player.hasKey)
			{
                Debug.Log("Success!!!!!");
			}
            else
			{
                Debug.Log("You need to find Key first.");
			}
		}
        debug();
    }

    public void back()  //player의 방향 반전
	{
		switch (player.dir)
		{
            case 0: 
                player.dir = 2;
                break;
            case 1:
                player.dir = 3;
                break;
            case 2:
                player.dir = 0;
                break;
            case 3:
                player.dir = 1;
                break;
        }
        buttondis();
        debug();
    }

    public void left()
	{
        if (player.dir == 0) player.dir = 3;
        else player.dir--;
        buttondis();
        debug();
    }
    public void right()
	{
        if (player.dir == 3) player.dir = 0;
        else player.dir++;
        buttondis();
        debug();
    }
}
