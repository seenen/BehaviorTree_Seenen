using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;

public class MonsterMono : MonoBehaviour
{
    public float speed = 0;

    public GameObject target = null;

    public PathList mPathList = null;

    public BehaviorTree mBehaviorTree = null;

    public MonsterBirth mMonsterBirth = null;

    public void Begin()
    {
        Debug.Log("Begin");

        mBehaviorTree = gameObject.GetComponent<BehaviorTree>();

        //  设置一个出生点
        Vector3 birth = mPathList.RandomPoint();

        SharedVector3 sharedBirth = new SharedVector3();
        sharedBirth.Value = birth;

        GlobalVariables.Instance.SetVariable("MonsterBirth_Pos", sharedBirth);

        //  设置移动目标点
        Vector3 target = mPathList.RandomNextPoint(birth);

        SharedVector3 sharedTarget = new SharedVector3();
        sharedTarget.Value = target;

        GlobalVariables.Instance.SetVariable("MonsterMovement_Target", sharedTarget);

        //var myIntVariable = (SharedVector3)mBehaviorTree.GetVariable("MonsterBirth_Pos");

        //mBehaviorTree.SetVariableValue("MonsterBirth_Pos", birth);


        mBehaviorTree.EnableBehavior();
    }
}
