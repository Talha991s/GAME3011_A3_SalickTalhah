using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Manager : MonoBehaviour
{
    public ArrayLayout BoardLayout;
    [Header("UI Element")]
    public Sprite[] pieces;
    public RectTransform gameboard;

    [Header("Prefabs")]
    public GameObject nodePieace;

    int width = 9;
    int height = 14;
    Node[,] board;

    System.Random random;
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }
    void StartGame()
    {
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());
        InitializedBoard();
        VerifyBoard();
    }
    void InitializedBoard()
    {
        board = new Node[width, height];
        for( int y = 0; y < height; y++)
        {
            for( int x =0; x <width; x++)
            {
                board[x, y] = new Node((BoardLayout.rows[y].row[x])?-1:FillPiece(), new Mark(x, y));
            }
        }
    }
    void VerifyBoard()
    {
        List<int> remove;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Mark p = new Mark(x, y);
                int val = getValuePoint(p);
                if (val <= 0) continue;

                remove = new List<int>();
                while(isConnected(p,true).Count >0)
                {
                    val = getValuePoint(p);
                    if(!remove.Contains(val))
                    {
                        remove.Add(val);
                    }
                    setValueatPoint(p, newValue(ref remove));
                }
                // board[x, y] = new Node((BoardLayout.rows[y].row[x]) ? -1 : FillPiece(), new Mark(x, y));
            }
        }
    }

    void InstantiateBoard()
    {
        for (int x = 0; x < width; x++)
        {

            for (int y = 0; y < height; y++)
            {
                int val = board[x, y].value;
                if (val <= 0) continue;
                GameObject p = Instantiate(nodePieace, gameboard);
                RectTransform rect = p.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(32 + (64 * x), -32-(64 * y));
            }
        }
    } 

    List<Mark> isConnected(Mark p, bool main)
    {
        List<Mark> connected = new List<Mark>();
        int val = getValuePoint(p);
        Mark[] directions =
        {
            Mark.up,
            Mark.right,
            Mark.down,
            Mark.left
        };
        foreach(Mark dir in directions)  //checking if there is 2 or more of the same shape in the direction
        {
            List<Mark> line = new List<Mark>();

            int same = 0;
            
            for(int i = 1; i <3; i ++)
            {
                Mark check = Mark.add(p, Mark.mult(dir, i));
                if(getValuePoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }
            }

            if (same > 1) // more than one of the same shape -->match
                AddPoint(ref connected, line); // add thse points to the connected list
        }
        for( int i = 0; i < 2; i++) // checking if we are in the middle of the 2 same shape
        {
            List<Mark> line = new List<Mark>();

            int same = 0;
            Mark[] check = { Mark.add(p, directions[i]), Mark.add(p, directions[i + 2]) };
            foreach (Mark next in check) //check both side of the piece if they have the same value
            {
                if (getValuePoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            }
            if(same > 1)
            {
                AddPoint(ref connected, line);
            }
        }
        for (int i = 0; i < 4; i++) // check for 2x2
        {
            List<Mark> square = new List<Mark>();
            int same = 0;
            int next = i + 1;

            if (next >= 4)
                next -= 4;

            Mark[] check = {Mark.add(p, directions[i]), Mark.add(p, directions[next]), Mark.add(p, Mark.add(directions[i], directions[next]))};
            foreach (Mark pnt in check) //check both side of the piece if they have the same value
            {
                if (getValuePoint(pnt) == val)
                {
                    square.Add(pnt);
                    same++;
                }
            }

            if (same > 2)
                AddPoint(ref connected, square);
        }
        if (main) // check for other matches along  the current match
        {
            for(int i = 0; i < connected.Count; i++)
            {
                AddPoint(ref connected, isConnected(connected[i], false));
            }
        }

        if (connected.Count > 0)
            connected.Add(p);

        return connected;
    }
    void AddPoint(ref List<Mark> points, List<Mark> add)
    {
        foreach(Mark p in add)
        {
            bool Doadd = true;



            for(int i = 0; i < points.Count; i++)
            {
                if(points[i].Equals(p))
                {
                    Doadd = false;
                    break;
                }
            }
            if (Doadd) points.Add(p);

        }
    }


    int FillPiece()
    {
        int val = 1;
        val = (random.Next(0,100)/(100/pieces.Length))+1; // look for the piece and place it 
        return val;
    }

    int getValuePoint(Mark p)
    {
        if (p.x < 0 || p.x >= width || p.y < 0 || p.y >= height) return -1;
        return board[p.x, p.y].value;
    }

    void setValueatPoint(Mark p, int v)
    {
        board[p.x, p.y].value = v;
    }

    int newValue(ref List<int>remove)
    {
        List<int> available = new List<int>();
        for( int i = 0; i < pieces.Length; i ++)
        {
            available.Add(i + 1);
        }
        foreach (int i in remove)
            available.Remove(i);

        if (available.Count <= 0) return 0;
        return available[random.Next(0, available.Count)];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    string getRandomSeed()
    {
        string seed = "";
        string acceptablechar = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
        for (int i = 0; i < 20; i++)
            seed += acceptablechar[Random.Range(0, acceptablechar.Length)];// amount of see on board
        
        return seed;
        
    }
}

[System.Serializable]
public class Node
{
    public int value;
    public Mark index;

    public Node(int v, Mark i)
    {
        value = v;
        index = i;
    }
}
