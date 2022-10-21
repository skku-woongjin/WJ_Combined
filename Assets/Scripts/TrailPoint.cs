using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBaske.Sensors.Grid;

public class TrailPoint : MonoBehaviour
{
    public float energy;
    public int gridW;
    public int gridH;

    private GridBuffer m_SensorBuffer;

    float getEnergy()
    {
        return energy;
    }

    void Awake()
    {

    }

}
