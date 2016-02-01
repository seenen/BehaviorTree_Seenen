using UnityEngine;
using System.Collections;

public class PathList : MonoBehaviour
{

    public Transform[] listTransforms = null;

    public Vector3[] listVector3 = null;

    void Init()
    {
        //if (listTransforms != null)
        //    return;

        listTransforms = gameObject.GetComponentsInChildren<Transform>();

        listVector3 = new Vector3[listTransforms.Length];

        for (int i = 0; i < listTransforms.Length; ++i)
        {
            listVector3[i] = listTransforms[i].position;
        }
    }

    public Vector3 RandomPoint()
    {
        Init();

        int index = Random.Range(0, listTransforms.Length - 1);

        Transform trans = listTransforms[index];

        return trans.position;
    }

    public Vector3 RandomNextPoint(Vector3 trans)
    {
        Init();

        int length = Random.Range(0, listVector3.Length - 1);

        for (int i = 0; i < listVector3.Length; ++i)
        {
            if (trans == listVector3[i])
            {
                length += i;

                length %= listVector3.Length;

                break;
            }
        }

        return listVector3[length];
    }
}
