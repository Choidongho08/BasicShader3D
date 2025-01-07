using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    [SerializeField] private VisualTreeAsset rootTreeAsset = default;

    private TextField _nameField;
    private VisualElement _imgView;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        rootTreeAsset.CloneTree(root);
        Test test = target as Test;

        _nameField = root.Q<TextField>(name: "MyNameInput");
        _imgView = root.Q<VisualElement>(name: "Image");

        Toggle hasNameToggle = root.Q<Toggle>(name: "HasNameToggle");
        hasNameToggle.RegisterValueChangedCallback(HandleValueChanged);

        ObjectField spriteField = root.Q<ObjectField>(name: "SpriteField");
        spriteField.RegisterValueChangedCallback(HandleSpriteChange);
        Debug.Log(spriteField);

        return root;
    }

    private void HandleSpriteChange(ChangeEvent<UnityEngine.Object> evt)
    {
        Debug.Log("Event");
        if(evt.newValue is Sprite sprite)
            _imgView.style.backgroundImage = new StyleBackground(sprite);
    }

    private void HandleValueChanged(ChangeEvent<bool> evt)
    {
        DisplayStyle style = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
        _nameField.style.display = style;
    }
}
