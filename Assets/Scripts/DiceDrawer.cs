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
        //base.OnInspectorGUI();
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mTarget), typeof(MonoScript), false);
        EditorGUI.EndDisabledGroup();

        serializedObject.Update();
        //ObjectNames.NicifyVariableName

        //GET PROPS
        var directionVector = serializedObject.FindProperty(nameof(mTarget.directionVector));

        var selectedResult = serializedObject.FindProperty(nameof(mTarget.selectedResult));
        var selectedVector = serializedObject.FindProperty(nameof(mTarget.selectedVector));

        var isPreConfigured = serializedObject.FindProperty(nameof(mTarget.isPreConfigured));

        var results = serializedObject.FindProperty(nameof(mTarget.results));
        var points = serializedObject.FindProperty(nameof(mTarget.points));

        // CODE
        EditorGUILayout.PropertyField(directionVector);

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(selectedResult);
        EditorGUILayout.PropertyField(selectedVector);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(isPreConfigured);

        EditorGUI.BeginDisabledGroup(!mTarget.isPreConfigured);
        EditorGUILayout.PropertyField(results);
        EditorGUILayout.PropertyField(points);
        EditorGUI.EndDisabledGroup();

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

        serializedObject.ApplyModifiedProperties();
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
