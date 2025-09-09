using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(DirtManager))]
public class DirtManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DirtManager dm = (DirtManager)target;

        if (GUILayout.Button("Spawn"))
        {
            dm.Spawn();
        }
    }
}

public class DirtManager : MonoBehaviour
{
    public GameObject dirtPrefab;
    public GameObject floor;
    public TMP_Text collectedText;
    public TMP_Text timeLeftText;
    public int spawnCount = 100;
    public float timeLeft = 90f;

    void Update()
    {
        if ((spawnCount - transform.childCount) == 0)
            collectedText.text = "Dirt Collected: 100%";
        else
            collectedText.text = "Dirt Collected: " + (int)(((spawnCount - transform.childCount + 0.0f) / spawnCount) * 100) + "%";

        timeLeftText.text = "Time Left: " + timeLeft.ToString("0.00") + "s";
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0.0f)
            timeLeft = 0.0f;
    }

    public void Spawn()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        if (floor == null)
        {
            Debug.LogError("DirtManager missing floor");
        }

        for (int i = 0; i < spawnCount; i++)
        {
            /*Vector3 randomScreenSpacePoint = new Vector3(
                Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f),
                0.0f
            );

            Vector3 worldPoint = Camera.main.ViewportToWorldPoint(randomScreenSpacePoint);
            worldPoint.z = 0.0f;
            */

            Vector3 floorPos = floor.transform.position;
            Vector3 floorScale = floor.transform.localScale;
            float halfWidth = floorScale.x * 0.5f;
            float halfHeight = floorScale.y * 0.5f;

            Vector3 point = new Vector3(
                Random.Range(floorPos.x - halfWidth, floorPos.x + halfWidth),
                Random.Range(floorPos.y - halfHeight, floorPos.y + halfHeight),
                0.0f
            );

            GameObject dirt = Instantiate(dirtPrefab);
            dirt.transform.parent = transform;
            dirt.transform.position = point;
        }
    }

    public List<GameObject> FindDirtInCircle(Vector3 _pos, float _radius)
    {
        List<GameObject> results = new List<GameObject>();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (Vector3.Distance(_pos, transform.GetChild(i).position) < _radius)
            {
                results.Add(transform.GetChild(i).gameObject);
                // example of accessing component
                transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        return results;
    }
}