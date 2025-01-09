using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class StatEditorWindow : EditorWindow
{
    [SerializeField] private VisualTreeAsset _viewUI = default;
    [SerializeField] private VisualTreeAsset _statViewUI = default;
    [SerializeField] private StatDatabase _statDatabase = default;

    private ScrollView _listScrollView;
    private VisualElement _inspector;
    private Editor _cachedEditor;
    private bool _isSelect;
    private StatVIewUI _selectedItem = null;

    [MenuItem("Tools/StatEditor")]
    public static void ShowWindow()
    {
        StatEditorWindow wnd = GetWindow<StatEditorWindow>();
        wnd.titleContent = new GUIContent("StatEditorWindow");
        wnd.minSize = new Vector2(800, 600);
    }

    public void CreateGUI()
    { 
        VisualElement root = rootVisualElement;

        _viewUI.CloneTree(root);

        InitializeTable(root);
        AddListner(root);
    }

    private void AddListner(VisualElement root)
    {
        Button createBtn = root.Q<Button>("CreateBtn");
        createBtn.clicked += HandleCreateStat;
    }

    private void HandleCreateStat()
    {
        StatSO newStat = CreateInstance<StatSO>();
        Guid guid = Guid.NewGuid();
        newStat.statName = guid.ToString();

        string savePath = $"{_statDatabase.assetPath}/Stat";
        CreateIfNotExists(savePath);
        AssetDatabase.CreateAsset(newStat, $"{savePath}/{guid}.asset");

        if(_statDatabase.table == null)
            _statDatabase.table = new List<StatSO>();
        _statDatabase.table.Add(newStat);

        EditorUtility.SetDirty(_statDatabase);
        AssetDatabase.SaveAssets();
    }

    private void CreateIfNotExists(string savePath)
    {
        if(!System.IO.Directory.Exists(savePath))
        {
            System.IO.Directory.CreateDirectory(savePath);
        }
    }

    private void InitializeTable(VisualElement root)
    {
        _listScrollView = root.Q<ScrollView>("ListScrollView");
        _inspector = root.Q<VisualElement>("Inspector");
        
        RefreshUI();
    }

    private void HandleItemDelete(StatVIewUI target)
    {
        if (EditorUtility.DisplayDialog("Delete", "Are u sure?", "Yes", "No"))
        {
            string assetPath = AssetDatabase.GetAssetPath(target.targetStat);
            AssetDatabase.DeleteAsset(assetPath);
            //DB테이블에서 제거
            _statDatabase.table.Remove(target.targetStat);
            // 저장하기 전 뭘 저장할지 알려주는 SetDirty작업이 있어야함
            EditorUtility.SetDirty(_statDatabase);
            AssetDatabase.SaveAssets();

            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        _listScrollView.Clear();
        _inspector.Clear();
        foreach (StatSO stat in _statDatabase.table)
        {
            TemplateContainer statViewUI = _statViewUI.Instantiate();
            _listScrollView.Add(statViewUI);

            StatVIewUI statView = new StatVIewUI(statViewUI, stat);

            statView.OnSelect += HandleItemSelect;
            statView.OnDelete += HandleItemDelete;
        }
    }

    private void HandleItemSelect(StatVIewUI selectUI)
    {   
        Editor.CreateCachedEditor(selectUI.targetStat, null, ref _cachedEditor);
        VisualElement statInspector = _cachedEditor.CreateInspectorGUI();

        SerializedObject so = new SerializedObject(selectUI.targetStat);
        statInspector.Bind(so);

        statInspector.TrackSerializedObjectValue(so, (target) =>
        {
            selectUI.RefreshUI();
        });

        _inspector.Clear();
        _inspector.Add(statInspector);

        _selectedItem?.SetSelection(false);
        _selectedItem = selectUI;
        _selectedItem.SetSelection(true);
    }
}
