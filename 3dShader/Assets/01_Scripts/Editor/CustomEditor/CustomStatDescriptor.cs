using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(StatDescriptor))]
public class CustomStatDescriptor : PropertyDrawer
{
    [SerializeField] private VisualTreeAsset _statPropertyView = default;


    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement root = new VisualElement();
        _statPropertyView.CloneTree(root);

        SerializedProperty isShowProp = property.FindPropertyRelative("isShow");

        PropertyField isShowField = root.Q<PropertyField>("IsShowField");
        PropertyField statNameField = root.Q<PropertyField>("StatNameField");
        statNameField.style.display = isShowProp.boolValue ? DisplayStyle.Flex : DisplayStyle.None;

        isShowField.TrackPropertyValue(isShowProp, prop =>
        {
            statNameField.style.display = isShowProp.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
        });

        return root;
    }

}
