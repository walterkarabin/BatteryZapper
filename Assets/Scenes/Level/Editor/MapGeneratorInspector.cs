using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WD.ChessMaze;

namespace WD.ChessMaze
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorInspector : Editor
    {
        MapGenerator map;

        private void OnEnable()
        {
            map = (MapGenerator)target;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(Application.isPlaying)
            {
                if(GUILayout.Button("Generate New Map"))
                {
                    map.GenerateNewMap();
                }
            }
        }
    }
}

