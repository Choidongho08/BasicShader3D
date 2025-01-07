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

    public StatVIewUI(TemplateContainer root, StatSO stat)
    {
        root.RegisterCallback<ClickEvent>(HandleItemSelect);
        targetStat = stat;

        sprite = root.Q<VisualElement>("Image");
        titleLable = root.Q<Label>("StatTitle");
        deleteButton = root.Q<Button>("DeleteBtn");
        deleteButton.clicked += HandleDelete;

        RefreshUI();
    }

    private void RefreshUI()
    {
        sprite.style.backgroundImage = new StyleBackground(targetStat.icon);
        titleLable.text = targetStat.statName;
    }

    private void HandleDelete()
    {

    }

    private void HandleItemSelect(ClickEvent evt)
    {
        OnSelect?.Invoke(this);
    }
}
