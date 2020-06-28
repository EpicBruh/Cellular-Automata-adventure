using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cell : MonoBehaviour
{
    public Sprite sprite;
    int[] cells;
    public int[] ruleset;

    int generation = 0;

    int width;

    public int number;

    [SerializeField]
    bool randomRule = false;

    public int[] numToRule()
    {
        int num = 0;
        int[] test = new int[8];
        for (int i = 7; i >= 0; i--)
        {
            if (Mathf.Pow(2, i)+num <= number)
            {
                test[i] = 1;
                num += (int)Mathf.Pow(2, i);
            }
            else
                test[i] = 0;
        }
        return test;
    }

    public void ruleToNum()
    {
        int num = 0;
        for (int i = 0; i < 8; i++)
            num += (int)Mathf.Pow(2, i) * ruleset[i] ;
        number = num;
    }

    private void Start()
    {
        float halfHeight = Camera.main.orthographicSize;
        width =(int) (Camera.main.aspect * halfHeight * 2);

        cells = new int[width];
        ruleset = numToRule();

        for (int i = 0; i < cells.Length; i++)
            cells[i] = 0;
        cells[cells.Length / 2] = 1;
    }

    private void Update()
    {
        if (!finished())
        {
            render();
            generate();
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (randomRule)
            {
                randomize();
                ruleToNum();
            }
            restart();
        }   
    }

    public void generate() {
        int[] nextGen = new int[cells.Length];
        for(int i = 1; i < cells.Length - 1; i++)
        {
            int left = cells[i - 1];
            int mid = cells[i];
            int right = cells[i + 1];
            nextGen[i] = rules(left, mid, right);
        }
        cells = nextGen;
        generation++;
    }

    public int rules(int a, int b, int c)
    {
        string s = "" + a + b + c;
        int index = Convert.ToInt32(s, 2);
        return ruleset[index];
    }

    public void render()
    {
        Color fill = new Color();
        for(int i = 0; i < cells.Length; i++)
        {
            if (cells[i] == 0)
                fill = Color.white;
            else
                fill = Color.black;
            spawnRect(i, generation, 1, 1, fill);
        }
    }

    public bool finished()
    {
        if (generation > Camera.main.orthographicSize * 2)
            return true;
        return false;
    }

    public void randomize()
    {
        for (int i = 0; i < 8; i++)
            ruleset[i] = (int)UnityEngine.Random.Range(0, 2);
    }

    public void restart() {
        while (generation != 0)
        {
            for (int i = 0; i < cells.Length - 1; i++)
            {
                Destroy(GameObject.Find("x: " + i + " y: " + generation));
            }
            generation--;
        }
        for (int i = 0; i < cells.Length; i++)
            cells[i] = 0;
        cells[cells.Length / 2] = 1;
        if(!randomRule)
            ruleset = numToRule();
        //cells[(int)UnityEngine.Random.Range(0, cells.Length)] = 1;
    }

    public void spawnRect(float x, float y, float w, float h, Color fill)
    {
        GameObject test = new GameObject();
        test.name = "x: " + x + " y: " + y;

        test.AddComponent<SpriteRenderer>();
        test.GetComponent<SpriteRenderer>().sprite = sprite;
        test.GetComponent<SpriteRenderer>().color = fill;
        test.transform.position = new Vector3(x, y, 0);
        test.transform.localScale = new Vector3(w, h, 0);
    }
}
