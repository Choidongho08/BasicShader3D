using System;
using UnityEditor;
using UnityEngine;

namespace ColorFolder
{
    [InitializeOnLoad]
    public class IconFolderEditor
    {
        private static string _selectedFolderGUID;
        private static int _controlID;

        static IconFolderEditor()
        {
            EditorApplication.projectWindowItemOnGUI -= OnGUICall;
            EditorApplication.projectWindowItemOnGUI += OnGUICall;
        }

        private static void OnGUICall(string guid, Rect selectionRect)
        {
            if (guid != _selectedFolderGUID) return;

            if (Event.current.commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == _controlID)
            {
                UnityEngine.Object selectedObject = EditorGUIUtility.GetObjectPickerObject();
                string path = AssetDatabase.GetAssetPath(selectedObject);
                string textureGUID = AssetDatabase.GUIDFromAssetPath(path).ToString();

                EditorPrefs.SetString(_selectedFolderGUID, textureGUID);
            }
        }

        public static void ChooseCustomIcon()
        {
            _selectedFolderGUID = Selection.assetGUIDs[0];
            _controlID = EditorGUIUtility.GetControlID(FocusType.Passive);

            // 디폴트로 선택될 오브젝트, 씬오브젝트도 가져오게 할건지?, 파일이름 필터, 컨트롤 아이디
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, "", _controlID);
        }
    }
}
