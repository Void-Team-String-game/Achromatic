using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static float GetRandomNormalDistribution(float mean, float standard)  // ���� ������ ���� �������� �������� �Լ� 
    {
        var x1 = Random.Range(0f, 1f);
        var x2 = Random.Range(0f, 1f);
        return mean + standard * (Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2));
    }
}
