using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cell2D : MonoBehaviour
{
    public Sprite sprite;

    int[,] board;
    int w = 1;
    int columns, rows;

    

    private void Start()
    {
        float halfHeight = Camera.main.orthographicSize;
        columns = (int)(Camera.main.aspect * halfHeight * 2)/w;
        rows = (int)(halfHeight * 2 / w);
        board = new int[columns, rows];

        init();
    }

    private void Update()
    {
        generate();
        display();
    }

    public void init()
    {
        
        for(int i = 1; i < columns - 1; i++)
        {
            
            for(int j = 1; j < rows - 1; j++)
            {
                
                board[i, j] = (int)UnityEngine.Random.Range(0, 2);

            }
        }
    }

    public void generate() {
        int[,] next = new int[columns, rows];
        for(int x = 1; x <columns - 1; x++)
        {
            for(int y = 1; y < rows - 1; y++)
            {
                int neighbours = 0;
                for(int i = -1; i <= 1; i++)
                {
                    for(int j = -1; j <= 1; j++)
                    {
                        neighbours += board[x + i, y + j];
                    }
                }
                neighbours -= board[x, y];

                //Rules
                if (board[x, y] == 1 && neighbours < 2) next[x, y] = 0; //Loneliness
                else if (board[x, y] == 1 && neighbours > 3) next[x, y] = 0; //Overpopulation
                else if (board[x, y] == 0 && neighbours == 3) next[x, y] = 1; //Reproduction
                else next[x, y] = board[x, y]; //Stasis
            }
        }
        board = next;
    }

    public void display()
    {
        Color color = new Color();
        for(int i = 0; i < columns; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                if (board[i, j] == 1) color = Color.black;
                else color = Color.white;
                spawnRect(i * w, j * w, w, w, color);
            }
        }
    }


    public void spawnRect(float x, float y, float w, float h, Color fill)
    {

        GameObject present = GameObject.Find("x: " + x + " y: " + y);
        if (present)
            Destroy(present);
        GameObject test = new GameObject();
        test.name = "x: " + x + " y: " + y;

        

        test.AddComponent<SpriteRenderer>();
        test.GetComponent<SpriteRenderer>().sprite = sprite;
        test.GetComponent<SpriteRenderer>().color = fill;
        test.transform.position = new Vector3(x, y, 0);
        test.transform.localScale = new Vector3(w, h, 0);
    }
}
