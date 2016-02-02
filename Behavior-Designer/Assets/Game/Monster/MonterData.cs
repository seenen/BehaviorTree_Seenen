using UnityEngine;
using System.Collections;

[System.Serializable]
public class MonterData
{
    public int myInt;
}

[System.Serializable]
public class SharedCustomClass : SharedVariable<MonterData>
{
    MonterData Value = null;

    public static implicit operator SharedCustomClass(MonterData value) { return new SharedCustomClass { Value = value }; }
}