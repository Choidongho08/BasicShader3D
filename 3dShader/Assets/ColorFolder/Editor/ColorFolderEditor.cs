using System;
using UnityEditor;
using UnityEngine;

namespace ColorFolder
{
    [InitializeOnLoad] // 유니티 로드시 클래스 초기화
    public static class ColorFolderEditor
    {
        private static string _iconName;
        static ColorFolderEditor()
        {
            EditorApplication.projectWindowItemOnGUI -= OnGUICall;
            EditorApplication.projectWindowItemOnGUI += OnGUICall;
        }

        private static void OnGUICall(string guid, Rect selectionRect)
        {
            GetRectAndColor(selectionRect, out Rect folderRect, out Color backColor);

            string iconGUID = EditorPrefs.GetString(guid);
            if(string.IsNullOrEmpty(iconGUID) || iconGUID.Equals("00000000000000000000000000000000")) 
                return;

            string iconPath = AssetDatabase.GUIDToAssetPath(iconGUID);
            EditorGUI.DrawRect(folderRect, backColor);
            Texture2D folderTex = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
            GUI.DrawTexture(folderRect, folderTex);
        }

        private static void GetRectAndColor(Rect sr, out Rect folderRect, out Color backColor)
        {
            backColor = new Color(0.2f, 0.2f, 0.2f);
            if (sr.x < 15)
            {
                folderRect = new Rect(sr.x + 3, sr.y, sr.height, sr.height);
            }
            else if (sr.x >= 15 && sr.height < 25)
            {
                backColor = new Color(0.2196079f, 0.2196079f, 0.2196079f);
                folderRect = new Rect(sr.x, sr.y, sr.height, sr.height);
            }
            else
            {
                folderRect = new Rect(sr.x, sr.y, sr.width, sr.width);
            }
        }

        public static void SetIconName(string iconName)
        {
            UnityEngine.Object target = Selection.activeObject;
            string folderPath = AssetDatabase.GetAssetPath(target);
            string folderGUID = AssetDatabase.GUIDFromAssetPath(folderPath).ToString();

            string iconPath = $"Assets/ColorFolder/Icons/{iconName}.png";
            string iconGUID = AssetDatabase.GUIDFromAssetPath(iconPath).ToString();

            EditorPrefs.SetString(folderGUID, iconGUID);
        }
        public static void ResetFolderIcon()
        {
            EditorPrefs.DeleteKey(GetActiveObject());
        }

        private static string GetActiveObject()
        {
            UnityEngine.Object target = Selection.activeObject;
            string folderPath = AssetDatabase.GetAssetPath(target);
            return AssetDatabase.GUIDFromAssetPath(folderPath).ToString(); 
        }
    }
}
