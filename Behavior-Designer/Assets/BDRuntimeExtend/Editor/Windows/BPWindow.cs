using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using BehaviorDesigner.Editor;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.IO;
using UnityEditor;

namespace BDRuntimeExtend
{
    public class BPWindow : EditorWindow
    {
        [MenuItem("Tools/BDRuntimeExtend %E", false, 0)]
        public static void Main()
        {
            thisWindow = (BPWindow)EditorWindow.GetWindow(typeof(BPWindow));
            thisWindow.title = "BPWindow";
            thisWindow.ShowPopup();
            thisWindow.Focus();
        }

        static BPWindow thisWindow;

        public GUISkin skin;

        Vector2 scrollPos = Vector2.zero;

        void OnGUI()
        {
            if (skin == null)
                skin = new GUISkin();

            GUILayout.Box("Build Project Tool By Zhang Shining Publish at 2014-1-11", skin.FindStyle("Box"), GUILayout.ExpandWidth(true));
			
			if (GUILayout.Button("Build", GUILayout.ExpandWidth(true)))
            {
                Build();
            }

			
			if (GUILayout.Button("Restore", GUILayout.ExpandWidth(true)))
            {
                Restore();
            }
        }

        //  怪物的AI存放地址.
        public static string ASSETBUNDLE_PATH = "Assets/Resources/Monster/";

        //  把 BehaviorTree 存放成 ExternalBehaviorTree 的.asset文件
        void Build()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                Debug.Log(go.name);

                BehaviorTree bt = go.GetComponent<BehaviorTree>();

                ExternalBehaviorTree holder = ScriptableObject.CreateInstance<ExternalBehaviorTree>();
                holder.BehaviorSource = bt.GetBehaviorSource();

                AssetDatabase.CreateAsset(holder, ASSETBUNDLE_PATH + bt.BehaviorName + ".asset");

                AssetDatabase.Refresh();
            }

        }

        //  把 ExternalBehaviorTree 还原成 BehaviorTree
        void Restore()
        {
            string assetPath = "";
            string RESOURCESPATH = "Assets/Resources/";

            foreach (Object go in Selection.objects)
            {
                Debug.Log(go.name);

                assetPath = Path.GetFullPath(AssetDatabase.GetAssetPath(go));

                assetPath = assetPath.Replace("\\", "/");

                Debug.Log("assetPath = " + assetPath);

                //ExternalBehaviorTree o = (ExternalBehaviorTree)AssetDatabase.LoadAssetAtPath(assetPath, typeof(ExternalBehaviorTree));
                //Debug.Log("o = " + o);

                ExternalBehaviorTree ebt = null;

                if (assetPath.Contains(RESOURCESPATH))
                {
                    int index = assetPath.LastIndexOf(RESOURCESPATH);
                    Debug.Log("index = " + index);
                    assetPath = assetPath.Substring(index + RESOURCESPATH.Length);
                    Debug.Log("assetPath = " + assetPath);
                    string endfix = Path.GetExtension(assetPath);
                    assetPath = assetPath.Substring(0, assetPath.Length - endfix.Length);
                    Debug.Log("assetPath = " + assetPath);

                    ebt = Resources.Load(assetPath, typeof(ExternalBehaviorTree)) as ExternalBehaviorTree;
                    Debug.Log("ebt = " + ebt);
                }

                GameObject obj = new GameObject(ebt.GetBehaviorSource().behaviorName + "_Template");

                BehaviorTree bt = obj.AddComponent<BehaviorTree>();
                bt.SetBehaviorSource(ebt.GetBehaviorSource());
                bt.GetBehaviorSource().TaskData = ebt.GetBehaviorSource().TaskData;

            }
        }
    }
}