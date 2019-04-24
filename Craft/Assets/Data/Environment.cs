using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Environment", menuName = "Environment")]
public class Environment : ScriptableObject
{
    public enum WeatherType { Sunny, Cloudy, Rain, Storm, Snow}

    public WeatherType weather;

    public int moonPhase = 0; //max 7
}
