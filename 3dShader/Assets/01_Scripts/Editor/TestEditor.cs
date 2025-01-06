using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    [SerializeField] private VisualTreeAsset rootTreeAsset = default;

    private TextField _nameField;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        rootTreeAsset.CloneTree(root);
        Test test = target as Test;

        _nameField = root.Q<TextField>(name: "MyNameInput");

        Toggle hasNameToggle = root.Q<Toggle>(name: "HasNameToggle");
        hasNameToggle.RegisterValueChangedCallback(HandleValueChanged);

        return root;
    }

    private void HandleValueChanged(ChangeEvent<bool> evt)
    {
        DisplayStyle style = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
        _nameField.style.display = style;
    }
}
