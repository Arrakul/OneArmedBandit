using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsBandit : MonoBehaviour
{
    public static SettingsBandit Instance;

    public DataForFormulas dataForFormulas;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

[Serializable]
public class DataForFormulas
{
    public int GlobalMultiplier = 10;
    public int Xn = 2;
    
    public Toggle[] toggles;
}
