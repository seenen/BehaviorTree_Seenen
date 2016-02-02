using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;

public class Demo : MonoBehaviour
{
    public GameObject yuren = null;

    public Dictionary<int, BehaviorTree> behaviorTreeGroup = new Dictionary<int, BehaviorTree>();

    public MonsterList mMonsterList = null;

    public WayPoints mWayPoints = null;

    public BehaviorTree btBirth = null;

    // Use this for initialization
    void Start ()
    {

        StartInnal();

        //StartExternal();
    }

    void StartInnal()
    {
        BehaviorTree[] bts = yuren.GetComponents<BehaviorTree>();

        for (int i = 0; i < bts.Length; ++i)
        {
            if (bts[i].BehaviorName == "MonsterNormal")
                btBirth = bts[i];
        }

        Task EntryTask = null;
        Task rootTask = null;
        List<Task> detachedTasks = null;

        BehaviorSource bsSource = btBirth.GetBehaviorSource();

        bsSource.Load(out EntryTask, out rootTask, out detachedTasks);

        for (int i = 0; i < 4; ++i)
        {
            GameObject obj = (GameObject)GameObject.Instantiate(mMonsterList.GetMonsterByIndex(i));
            obj.AddComponent<NavMeshAgent>();

            BehaviorTree behaviorTree = obj.AddComponent<BehaviorTree>();

            BehaviorSource bs = new BehaviorSource();
            bs.behaviorName             = i.ToString();
            bs.behaviorDescription      = bsSource.behaviorDescription;
            bs.Owner                    = behaviorTree;
            bs.TaskData                 = bsSource.TaskData;

            bs.CheckForSerialization(true);

            behaviorTree.SetBehaviorSource(bs);


            MonsterMono mm  = obj.AddComponent<MonsterMono>();
            mm.mPathList    = mWayPoints;
            mm.Begin();
        }


    }

    void StartExternal()
    {
        for (int i = 0; i < 4; ++i)
        {
            GameObject obj = (GameObject)GameObject.Instantiate(mMonsterList.GetMonsterByIndex(i));
            obj.AddComponent<NavMeshAgent>();

            BehaviorTree behaviorTree = obj.AddComponent<BehaviorTree>();

            ExternalBehaviorTree beExtHaviorTree = (ExternalBehaviorTree)GameObject.Instantiate(Resources.Load("Monster/MonsterNormal", typeof(ExternalBehaviorTree)));
            //ExternalBehaviorTree beExtHaviorTree = (ExternalBehaviorTree)(Resources.Load(assetResourcePath, typeof(ExternalBehaviorTree)));
            behaviorTree.ExternalBehavior = beExtHaviorTree;

            MonsterMono mm = obj.AddComponent<MonsterMono>();
            mm.mPathList = mWayPoints;
            mm.Begin();
        }
    }

    private enum BehaviorSelectionType
    {
        Birth = 0,
    }

    private BehaviorSelectionType selectionType = BehaviorSelectionType.Birth;
    private BehaviorSelectionType prevSelectionType = BehaviorSelectionType.Birth;

    void OnGUI()
    {
        GUILayout.BeginVertical(GUILayout.Width(300));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<-"))
        {
            prevSelectionType = selectionType;
            selectionType = (BehaviorSelectionType)(((int)selectionType - 1) % 1);
            if ((int)selectionType < 0) selectionType = BehaviorSelectionType.Birth;
            SelectionChanged();
        }
        GUILayout.Box(SplitCamelCase(selectionType.ToString()), GUILayout.Width(220));
        if (GUILayout.Button("->"))
        {
            prevSelectionType = selectionType;
            selectionType = (BehaviorSelectionType)(((int)selectionType + 1) % 1);
            SelectionChanged();
        }
        GUILayout.EndHorizontal();
    }

    private static string SplitCamelCase(string s)
    {
        var r = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
        s = r.Replace(s, " ");
        return (char.ToUpper(s[0]) + s.Substring(1)).Trim();
    }

    private void SelectionChanged()
    {
        DisableAll();

        StartCoroutine("EnableBehavior");

    }

    private void DisableAll()
    {
        StopCoroutine("EnableBehavior");
    }

    private IEnumerator EnableBehavior()
    {
        Debug.Log("selectionType = " + selectionType);

        yield return new WaitForSeconds(0.5f);

    }
}
