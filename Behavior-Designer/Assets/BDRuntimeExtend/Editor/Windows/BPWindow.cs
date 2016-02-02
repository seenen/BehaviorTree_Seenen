using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using BehaviorDesigner.Editor;

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
                BuildCommon();
            }

        }

        void BuildCommon()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                Debug.Log(go.name);
            }
        }
    }
}