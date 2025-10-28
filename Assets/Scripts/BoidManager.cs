using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public float maxAlignmentDistance = 3.0f;
    public float maxCohesionDistance = 2.0f;
    public float maxSeparationDistance = 0.5f;
    public float targetWeight = 0.35f;
    public float fleeWeight = 0.2f;
    public float separationWeight = 1.0f;
    public float alignmentWeight = 0.4f;
    public float cohesionWeight = 0.15f;
    public float obsticalWeight = 20.0f;
    public float drag = 0.95f;
    public float speed = 1.0f;
    public float maxSpeed = 3.0f;
    public QuadTree rocksQuadTree;
    public float biggestRockSize = 0.0f;
    private Boid[] m_boids = {};
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("ROCK");

        rocksQuadTree = new QuadTree(transform.position, 50.0f, 50.0f);

        foreach (GameObject go in gos)
        {
            rocksQuadTree.Add(go);

            if (go.transform.localScale.x > biggestRockSize)
                biggestRockSize = go.transform.localScale.x;
        }

        m_boids = GetComponentsInChildren<Boid>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        

        foreach(Boid boid in m_boids)
        {
            Vector2 pos = new Vector2(boid.transform.position.x, boid.transform.position.y);

            Vector2 seekDirection = Seek(pos, targetPos);
            Vector2 fleeDirection = Flee(pos, Vector2.zero);
            Vector2 sepalicoh = SepAliCoh(boid, pos);
            Vector2 obsticalDirection = Obstical(pos);

            boid.acceleration = (seekDirection * targetWeight) +
                                sepalicoh +
                                (fleeDirection * fleeWeight) +
                                (obsticalDirection * obsticalWeight);

            boid.velocity += boid.acceleration * speed * Time.deltaTime;

            // make the boid look the direction it is going
            boid.transform.right = new Vector3(boid.velocity.x, boid.velocity.y, 0.0f).normalized;

            // handle max speed
            boid.speed = boid.velocity.magnitude;

            if (boid.speed > maxSpeed)
            {
                boid.velocity = boid.velocity.normalized * maxSpeed;
            }

            boid.velocity *= drag;

            pos += boid.velocity;

            boid.transform.position = new Vector3(pos.x, pos.y, boid.transform.position.z);
        }
    }

    Vector2 Obstical(Vector2 _agentPos)
    {
        Vector2 separation = Vector2.zero;

        List<Transform> result = rocksQuadTree.FindComponent<Transform>(_agentPos, 5.0f);

        foreach(Transform ob in result)
        {
            Vector2 obPos = ob.position;
            float distance = Vector2.Distance(_agentPos, obPos);
            if (distance < ob.localScale.x * 0.5f)
            {
                // separation += (_agentPos - neighborPos).normalized * (maxSeparationDistance - distance);
                separation += _agentPos - obPos;// * (maxSeparationDistance - distance);
            }
        }

        if (separation != Vector2.zero)
        {
            separation.Normalize();
        }

        return separation;
    }

    Vector2 Seek(Vector2 _agentPos, Vector2 _targetPos)
    {
        Vector2 seek = _targetPos - _agentPos;
        return seek.normalized;
    }

    Vector2 Flee(Vector2 _agentPos, Vector2 _targetPos)
    {
        Vector2 flee = _agentPos - _targetPos;
        return flee.normalized;
    }

    Vector2 Separation(Boid _boid, Vector2 _agentPos)
    {
        Vector2 separation = Vector2.zero;
        int numberOfNeighbors = transform.childCount;

        foreach(Boid neighborBoid in m_boids)
        {
            if (_boid.gameObject != neighborBoid.gameObject)
            {
                Vector2 neighborPos = new Vector2(neighborBoid.transform.position.x, neighborBoid.transform.position.y);
                float distance = Vector2.Distance(_agentPos, neighborPos);
                if (distance < maxSeparationDistance)
                {
                    // separation += (_agentPos - neighborPos).normalized * (maxSeparationDistance - distance);
                    separation += (_agentPos - neighborPos).normalized * (maxSeparationDistance - distance);
                }
            }
        }

        if (separation != Vector2.zero)
        {
            separation.Normalize();
        }

        return separation;
    }

    Vector2 Alignment(Boid _boid, Vector2 _agentPos)
    {
        Vector2 alignment = Vector2.zero;
        int numberOfNeighbors = 0;

        foreach (Boid neighborBoid in m_boids)
        {
            if (neighborBoid != _boid)
            {
                Vector2 neighborPos = neighborBoid.transform.position;
                float distance = Vector2.Distance(_agentPos, neighborPos);

                if (distance < maxAlignmentDistance)
                {
                    numberOfNeighbors ++;
                    alignment += neighborBoid.velocity;
                }
            }
        }

        if (alignment != Vector2.zero)
            return alignment.normalized;


        return alignment;
    }

    Vector2 Cohesion(Boid _boid, Vector2 _agentPos)
    {
        Vector2 cohesion = Vector2.zero;
        int numberOfNeighbors = 0;

        foreach (Boid neighborBoid in m_boids)
        {
            Vector2 neighborPos = neighborBoid.transform.position;
            float distance = Vector2.Distance(_agentPos, neighborPos);
            if (distance < maxCohesionDistance)
            {
                numberOfNeighbors++;
                cohesion += neighborPos;
            }
        }

        if (numberOfNeighbors > 0)
        {
            Vector2 averagePos = cohesion / (float)numberOfNeighbors;

            cohesion = averagePos - _agentPos;

            if (cohesion != Vector2.zero) return cohesion.normalized;
        }

        return Vector2.zero;
    }
    
    Vector2 SepAliCoh(Boid _boid, Vector2 _agentPos)
    {
        Vector2 mag = Vector2.zero;

        Vector2 cohesion = Vector2.zero;
        Vector2 alignment = Vector2.zero;
        Vector2 separation = Vector2.zero;
        int numberOfNeighbors = 0;

        foreach (Boid neighborBoid in m_boids)
        {
            Vector2 neighborPos = neighborBoid.transform.position;
            float distance = Vector2.Distance(_agentPos, neighborPos);
            if (distance < maxCohesionDistance)
            {
                numberOfNeighbors++;
                cohesion += neighborPos;

                if (distance < maxAlignmentDistance)
                {
                    alignment += neighborBoid.velocity;

                    if (distance < maxSeparationDistance)
                    {
                        separation += (_agentPos - neighborPos).normalized * (maxSeparationDistance - distance);
                    }
                }
            }
        }

        if (numberOfNeighbors > 0)
        {
            Vector2 averagePos = cohesion / (float)numberOfNeighbors;

            cohesion = averagePos - _agentPos;

            if (cohesion != Vector2.zero)
                cohesion = cohesion.normalized;
        }

        if (alignment != Vector2.zero)
            alignment = alignment.normalized;
        
        if (separation != Vector2.zero)
            separation = separation.normalized;

        mag = (cohesion * cohesionWeight) + (alignment * alignmentWeight) + (separation * separationWeight);

        return mag;
    }
}
