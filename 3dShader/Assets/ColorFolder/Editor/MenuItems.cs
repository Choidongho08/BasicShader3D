using UnityEditor;
using UnityEngine;

namespace ColorFolder
{
    public static class MenuItems
    {
        private const int _priority = 10000;

        [MenuItem("Assets/GGM/Red", false, _priority)]
        private static void Red()
        {
            ColorFolderEditor.SetIconName("Red");
        }
        [MenuItem("Assets/GGM/Green", false, _priority)]
        private static void Green()
        {
            ColorFolderEditor.SetIconName("Green");

        }
        [MenuItem("Assets/GGM/Blue", false, _priority)]
        private static void Blue()
        {
            ColorFolderEditor.SetIconName("Blue");

        }
        [MenuItem("Assets/GGM/Custom", false, _priority + 11)]
        private static void Custom()
        {
            IconFolderEditor.ChooseCustomIcon();
        }
        [MenuItem("Assets/GGM/ResetIcon", false, _priority + 21)]
        private static void ResetIcon()
        {
            ColorFolderEditor.ResetFolderIcon();
        }
        [MenuItem("Assets/GGM/Red", true)]
        [MenuItem("Assets/GGM/Green", true)]
        [MenuItem("Assets/GGM/Blue", true)]
        [MenuItem("Assets/GGM/Custom", true)]
        [MenuItem("Assets/GGM/ResetIcon", true)]
        private static bool ValidateFolder()
        {
            if(Selection.activeObject == null)
            {
                return false;
            }
            
            Object selectObject = Selection.activeObject;

            string path = AssetDatabase.GetAssetPath(selectObject);
            return AssetDatabase.IsValidFolder(path);
        }
    }
}

