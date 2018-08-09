using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public float OffsetY;

    public GameObject Ball;
    public GameObject Maze;
    private Vector3 _averagePos;

    void Start()
    {

    }

    void Update()
    {
        _averagePos = (Ball.transform.position + Maze.transform.position) / 2;
        transform.position = new Vector3(_averagePos.x, _averagePos.y + OffsetY, _averagePos.z);

    }
}
