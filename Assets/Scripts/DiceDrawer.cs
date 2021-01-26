#if UNITY_EDITOR

using System;
using System.IO;
using UnityEngine;
using UnityEditor;

//[CanEditMultipleObjects]
[CustomEditor(typeof(Dice))]
public class DiceDrawer : Editor
{
    string folder;
    Dice mTarget;

    public void OnEnable()
    {
        mTarget = serializedObject.targetObject as Dice;
        folder = Application.dataPath;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            var path = EditorUtility.SaveFilePanel("Save Dice as Json", folder, mTarget.name, "json");
            if (!string.IsNullOrEmpty(path))
            {
                folder = Path.GetDirectoryName(path);
                SaveJson(path);
            }
        }
        if (GUILayout.Button("Load"))
        {
            var path = EditorUtility.OpenFilePanelWithFilters("Load Dice from Json", folder, new string[] { "JSON File", "json" });
            if (!string.IsNullOrEmpty(path))
            {
                folder = Path.GetDirectoryName(path);
                LoadJson(path);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    public void SaveJson(string path)
    {
        var str = JsonUtility.ToJson(new DiceJson(mTarget.directionVector, mTarget.results, mTarget.points));
        File.WriteAllText(path, str);
        AssetDatabase.Refresh();
    }

    public void LoadJson(string path)
    {
        var str = File.ReadAllText(path);
        var json = JsonUtility.FromJson<DiceJson>(str);
        json.Load(mTarget);
    }

    class DiceJson
    {
        public Vector3 directionVector;
        public int[] results;
        public Vector3[] points;
        public DiceJson(Vector3 dir, int[] results, Vector3[] points)
        {
            directionVector = dir;
            this.results = results;
            this.points = points;
        }
        public void Load(Dice dice)
        {
            dice.directionVector = directionVector;
            dice.results = results;
            dice.points = points;
            dice.isPreConfigured = true;
        }    
    }
}

#endif
