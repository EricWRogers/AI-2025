using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public GameObject prefab;
    public int rowCount, columnCount;
    public List<List<SpriteRenderer>> spriteRenderers = new List<List<SpriteRenderer>>();
    public bool running = false;
    [Range(0.0f, 1.0f)]
    public float delay = 0.5f;
    float currentDelay = 0.0f;

    void Start()
    {
        spriteRenderers.Clear();

        for (int c = 0; c < columnCount; c++)
        {
            spriteRenderers.Add(new List<SpriteRenderer>());

            for (int r = 0; r < rowCount; r++)
            {
                GameObject go = Instantiate(prefab, new Vector3(r, c, 0.0f), Quaternion.identity, transform);
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                GameOfLifeCell cell = go.GetComponent<GameOfLifeCell>();
                spriteRenderers[c].Add(sr);
                // (condition) ? true : false;
                cell.alive = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            running = !running;
        }

        if (running)
        {
            currentDelay -= Time.deltaTime;

            if (currentDelay > 0.0f)
                return;

            currentDelay = delay;

            for (int c = 0; c < columnCount; c++)
            {
                for (int r = 0; r < rowCount; r++)
                {
                    int count = 0;

                    // rap the index around the screen
                    // (condition) ? if true : if false;
                    int right = ((r + 1) >= rowCount) ? 0 : r + 1;
                    int left = ((r - 1) < 0) ? rowCount - 1 : r - 1;
                    int up = ((c + 1) >= columnCount) ? 0 : c + 1;
                    int down = ((c - 1) < 0) ? columnCount - 1 : c - 1;

                    // check neighbors

                    GameOfLifeCell cell = spriteRenderers[c][r].GetComponent<GameOfLifeCell>();
                    bool currentState = spriteRenderers[c][r].color == Color.black;

                    //cell.alive = true;

                    const bool ALIVE = true;
                    const bool DEAD = false;

                    // rule one
                    if (count < 2 && currentState == ALIVE)
                    {
                        cell.alive = DEAD;
                    }

                    // rule two

                    // rule three
                    if (count > 3 && currentState == ALIVE)
                    {
                        cell.alive = DEAD;
                    }

                    // rule four
                    if (count == 3 && currentState == DEAD)
                    {
                        cell.alive = ALIVE;
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float halfSize = 0.5f;

                // handle paint and erase
            }
        }
    }
}
