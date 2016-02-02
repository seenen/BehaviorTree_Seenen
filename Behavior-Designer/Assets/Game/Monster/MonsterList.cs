using UnityEngine;
using System.Collections;

public class MonsterList : MonoBehaviour
{
    public GameObject[] monsters = null;

    public GameObject GetMonsterByIndex(int index)
    {
        return monsters[index];
    }
}
