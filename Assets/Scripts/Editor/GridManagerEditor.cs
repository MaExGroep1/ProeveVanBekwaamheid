using Grid;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : UnityEditor.Editor
    {
        private Vector2 _scrollPos;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GridManager myTarget = (GridManager)target;
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            if (myTarget.Grid != null) GenerateGrid(myTarget);

            EditorGUILayout.EndScrollView();
        }

        private void GenerateGrid(GridManager myTarget)
        {
            for (int i = myTarget.Grid.GetLength(0) - 1; i >= 0; i--)
            {
                GUILayout.BeginHorizontal();
                for (int j = 0; j < myTarget.Grid.GetLength(1); j++)
                {
                    var array = myTarget.Grid[i, j].GetBlockType().ToString();
        
                    GUILayout.Box(array.Substring(0, 3), GUILayout.Width(30), GUILayout.Height(30));
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}
