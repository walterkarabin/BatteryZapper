using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WD.ChessMaze;

namespace WD.ChessMaze
{
    [CustomEditor(typeof(MapVisualizer))]
    public class PathInspector : Editor
    {
         MapVisualizer map;

        private void OnEnable()
        {
            map = (MapVisualizer)target;
        }

        // Update is called once per frame
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(Application.isPlaying)
            {
                if(GUILayout.Button("Refresh Path Inlfuence"))
                {
                    map.RefreshPath();
                }
                if(GUILayout.Button("Clear Map"))
                {
                    map.ClearMap();
                }
            }
        }
    }
}