using UnityEditor;

namespace marble810.NeckMaskMaker
{
    /// <summary>只在独立偏好缺失时继承开发版 Toolbox 的值，不删除旧键。</summary>
    internal static class NeckMaskPreferences
    {
        internal static void MigrateLegacy()
        {
            CopyInt("Language");
            CopyString("LastSaveDirectory");
            CopyFloat("PreviewPanelX");
            CopyFloat("PreviewPanelY");
            CopyFloat("PreviewPanelSize");
        }

        private static string Legacy(string name) => "MarbleAvatarToolbox.NeckMaskMaker." + name;
        private static string Current(string name) => "NeckMaskMaker." + name;

        private static void CopyInt(string name)
        {
            if (!EditorPrefs.HasKey(Current(name)) && EditorPrefs.HasKey(Legacy(name)))
                EditorPrefs.SetInt(Current(name), EditorPrefs.GetInt(Legacy(name)));
        }

        private static void CopyFloat(string name)
        {
            if (!EditorPrefs.HasKey(Current(name)) && EditorPrefs.HasKey(Legacy(name)))
                EditorPrefs.SetFloat(Current(name), EditorPrefs.GetFloat(Legacy(name)));
        }

        private static void CopyString(string name)
        {
            if (!EditorPrefs.HasKey(Current(name)) && EditorPrefs.HasKey(Legacy(name)))
                EditorPrefs.SetString(Current(name), EditorPrefs.GetString(Legacy(name)));
        }
    }
}
