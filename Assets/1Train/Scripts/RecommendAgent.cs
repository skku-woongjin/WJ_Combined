using UnityEngine;
using Unity.MLAgents;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class RecommendAgent : Agent
{
    public bool trivariate = false;
    public int logLen = 20;
    public bool warp;
    public int worldSize;

    Vector2 userMean;
    Vector3 userMean3;
    Vector2[] userLog;
    Vector3[] userLog3;

    public override void CollectObservations(VectorSensor sensor)
    {
        if (trivariate)
        {
            foreach (Vector3 vec in userLog3)
                sensor.AddObservation(vec);
        }
        else
        {
            foreach (Vector2 vec in userLog)
                sensor.AddObservation(vec);
        }

    }

    float maxGaus = 0;

    public override void Initialize()
    {
        userLog = new Vector2[logLen];
        userLog3 = new Vector3[logLen];

        maxcount = 24;
    }


    public void generateLog()
    {
        int count = 0;
        float dist;
        int visited;
        float time;
        while (count < logLen)
        {
            dist = UnityEngine.Random.Range(0, maxdist);
            visited = UnityEngine.Random.Range(0, (int)maxcount + 1);
            if (trivariate)
            {
                time = UnityEngine.Random.Range(0, maxdist);
                if (UnityEngine.Random.Range(0, 1.0f) < (float)GetTrivariateGuassian(userMean3.x, 5, userMean3.y, 10 * maxdist / maxcount, userMean3.z, 10 * maxdist / maxcount, visited, dist, time) / (maxGaus * 2))
                {
                    userLog3[count] = new Vector3(dist / maxdist, (float)visited / maxcount, time / maxdist);
                    count++;
                }
            }
            else
            {

                if (UnityEngine.Random.Range(0, 1.0f) < (float)GetBivariateGuassian(userMean.x, 5, userMean.y, 10 * maxdist / maxcount, visited, dist, 0) / (maxGaus * 2))
                {
                    userLog[count] = new Vector2((float)visited / maxcount, dist / maxdist);
                    count++;
                }
            }
        }
    }


    int logcount = 0;
    public void addLog3(int visit, float dist, float dur)
    {
        userLog3[logcount] = new Vector3(dist / maxdist, visit / maxcount, dur / maxdist);
        logcount++;
        if (logcount == logLen)
        {
            logcount = 0;
        }
    }

    float maxdist = 100 * Mathf.Sqrt(2);

    float maxcount;
    public static double GetBivariateGuassian(double muX, double sigmaX, double muY, double sigmaY, double x, double y, double rho = 0)
    {
        var sigmaXSquared = Math.Pow(sigmaX, 2);
        var sigmaYSquared = Math.Pow(sigmaY, 2);

        var dX = x - muX;
        var dY = y - muY;

        var exponent = -0.5;
        var normaliser = 2 * Math.PI * sigmaX * sigmaY;
        if (rho != 0)
        {
            normaliser *= Math.Sqrt(1 - Math.Pow(rho, 2));
            exponent /= 1 - Math.Pow(rho, 2);
        }

        var sum = Math.Pow(dX, 2) / sigmaXSquared;
        sum += Math.Pow(dY, 2) / sigmaYSquared;
        sum -= 2 * rho * dX * dY / (sigmaX * sigmaY);

        exponent *= sum;

        return Math.Exp(exponent) / normaliser;
    }

    public static double GetTrivariateGuassian(double muX, double sigmaX, double muY, double sigmaY, double muZ, double sigmaZ, double x, double y, double z)
    {
        var dX = x - muX;
        var dY = y - muY;
        var dZ = z - muZ;

        var exponent = Math.Pow(dX, 2) * sigmaY * sigmaZ + Math.Pow(dY, 2) * sigmaX * sigmaZ + Math.Pow(dZ, 2) * sigmaX * sigmaY;
        exponent *= (-1 / (2 * sigmaX * sigmaY * sigmaZ));
        var normaliser = Math.Pow(2 * Math.PI, 1.5) * Math.Sqrt(sigmaX * sigmaY * sigmaZ);

        return Math.Exp(exponent) / normaliser;
    }

    public double rew;

    public int recommendResult; // 추천한 결과가 여기에 저장됨 


    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var count = (actionBuffers.ContinuousActions[0] + 1) / 2 * maxcount;
        var dist = (actionBuffers.ContinuousActions[1] + 1) / 2 * maxdist;
        var time = 0.0f;


        if (trivariate)
        {
            time = (actionBuffers.ContinuousActions[2] + 1) / 2 * maxdist;
            rew = Math.Sqrt(Math.Pow(userMean3.x - count, 2) + Math.Pow(userMean3.y - dist, 2) + Math.Pow(userMean3.z - time, 2)) / Math.Sqrt(Math.Pow(maxcount, 2) + Math.Pow(maxdist, 2) + Math.Pow(maxdist, 2));
            rew = 1 - rew;
        }
        else
        {
            rew = GetBivariateGuassian(userMean.x, 5, userMean.y, 10 * maxdist / maxcount, count, dist, 0) / maxGaus;
        }

        AddReward((float)rew);

        return;

    }


}
