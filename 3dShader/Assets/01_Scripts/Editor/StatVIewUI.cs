using System;
using UnityEngine;
using UnityEngine.UIElements;

public class StatVIewUI
{
    public event Action<StatVIewUI> OnSelect;
    public event Action<StatVIewUI> OnDelete;

    public VisualElement sprite;
    public Label titleLable;
    public Button deleteButton;

    public StatSO targetStat;

    private VisualElement _container;

    public StatVIewUI(TemplateContainer root, StatSO stat)
    {
        root.RegisterCallback<ClickEvent>(HandleItemSelect);
        targetStat = stat;

        _container = root.Q<VisualElement>("Container");
        sprite = root.Q<VisualElement>("Image");
        titleLable = root.Q<Label>("StatTitle");
        deleteButton = root.Q<Button>("DeleteBtn");
        deleteButton.RegisterCallback<ClickEvent>(HandleDelete);

        RefreshUI();
    }

    public void SetSelection(bool isSelected)
    {
        if (isSelected) 
            _container.AddToClassList("select");
        else
            _container.RemoveFromClassList("select");
    }

    public void RefreshUI()
    {
        sprite.style.backgroundImage = new StyleBackground(targetStat.icon);
        titleLable.text = targetStat.statName;
    }

    private void HandleDelete(ClickEvent evt)
    {
        evt.StopPropagation(); // 이벤트 전파를 멈추고, 끝냄 
        OnDelete?.Invoke(this);
    }

    private void HandleItemSelect(ClickEvent evt)
    {
        OnSelect?.Invoke(this);
    }
}
