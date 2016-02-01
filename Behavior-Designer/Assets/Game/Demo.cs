using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class Demo : MonoBehaviour
{
    public GameObject yuren = null;

    public Dictionary<int, BehaviorTree> behaviorTreeGroup = new Dictionary<int, BehaviorTree>();

    // Use this for initialization
    void Start ()
    {
        //ExternalBehaviorTree btBirth = (ExternalBehaviorTree)Resources.Load("BTAsset/Birth", typeof(ExternalBehaviorTree));
        ExternalBehaviorTree btBirth = (ExternalBehaviorTree)GameObject.Instantiate(Resources.Load("BTAsset/btBirthBehavior", typeof(ExternalBehaviorTree)));

        if (yuren == null) return;

        CreateMonster(yuren, new ExternalBehaviorTree[] { btBirth });
    }

    /// <summary>
    /// 创建一个怪物
    /// </summary>
    /// <param name="monster"></param>
    void CreateMonster(GameObject monster, ExternalBehaviorTree[] ebts)
    {
        for (int i = 0; i < ebts.Length; ++i)
        {
            ExternalBehaviorTree ebt = (ExternalBehaviorTree)ebts[i];
            BehaviorTree bt = AddBT(yuren, ebt);

            behaviorTreeGroup.Add(bt.Group, bt);
        }
    }

    /// <summary>
    /// 给对象绑定行为树
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="ebt"></param>
    BehaviorTree AddBT(GameObject obj, ExternalBehaviorTree ebt)
    {
        BehaviorTree behaviorTree = obj.AddComponent<BehaviorTree>();
        behaviorTree.ExternalBehavior = ebt;
        //behaviorTree.SetBehaviorSource(ebt.GetBehaviorSource());
        //behaviorTree.GetBehaviorSource().behaviorName = ebt.GetBehaviorSource().behaviorName;
        //behaviorTree.GetBehaviorSource().behaviorDescription = ebt.GetBehaviorSource().behaviorDescription;
        //behaviorTree.GetBehaviorSource().RootTask = ebt.GetBehaviorSource().RootTask;

        behaviorTree.StartWhenEnabled = false;

        return behaviorTree;
    }

    // Update is called once per frame
    void Update ()
    {
	
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

        behaviorTreeGroup[(int)prevSelectionType].DisableBehavior();

    }

    private IEnumerator EnableBehavior()
    {
        Debug.Log("selectionType = " + selectionType);

        yield return new WaitForSeconds(0.5f);

        behaviorTreeGroup[(int)selectionType].EnableBehavior();

    }
}
