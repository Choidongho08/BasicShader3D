using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(StatSO))]
public class CustomStatSO : Editor
{
    [SerializeField] private VisualTreeAsset _statSOViewAsset;
    private TextField _nameField;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        _statSOViewAsset.CloneTree(root);

        _nameField = root.Q<TextField>("StatNameField");

        _nameField.RegisterValueChangedCallback(HandleNameChange);

        return root;
    }

    private void HandleNameChange(ChangeEvent<string> evt)
    {
        if (string.IsNullOrEmpty(evt.newValue))
        {
            EditorUtility.DisplayDialog("Error", "Please enter a vaild name", "OK");
            _nameField.SetValueWithoutNotify(evt.previousValue);
            return;
        }

        string assetPath = AssetDatabase.GetAssetPath(target);
        string message = AssetDatabase.RenameAsset(assetPath, evt.newValue);
        if (string.IsNullOrEmpty(message)) // 성공적으로 완료
        {
            target.name = evt.newValue;
        }
        else
            EditorUtility.DisplayDialog("Error", message, "OK");
    }
}
