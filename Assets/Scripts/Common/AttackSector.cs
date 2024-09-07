using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSector : MonoBehaviour
{
    [SerializeField] private int _sectorPoints;
    [SerializeField] private float _radius;

    private Transform[] _points;

    public Stack<Transform> freePoints = new Stack<Transform>();

    private void Start()
    {
        float angle = 360 / (float)_sectorPoints;
        float currentAngle = 0;

        _points = new Transform[_sectorPoints];

        for (int i = 0; i < _sectorPoints; i++)
        {
            _points[i] = new GameObject().transform;
            _points[i].position = GetPointOnCircule(currentAngle);
            _points[i].SetParent(transform);
            freePoints.Push(_points[i]);

            currentAngle += angle;
        }
    }

    private void OnDrawGizmos()
    {
        float angle = 360 / (float)_sectorPoints;
        float currentAngle = 0;
        Gizmos.color = Color.red;

        for (int i = 0; i < _sectorPoints; i++)
        {
            Gizmos.DrawSphere(GetPointOnCircule(currentAngle), 0.1f);
            currentAngle += angle;
        }
    }

    private Vector3 GetPointOnCircule(float angle)
    {
        Vector3 point = transform.position;
        point.x += _radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        point.z += _radius * Mathf.Sin(angle * Mathf.Deg2Rad);

        return point;
    }
}
