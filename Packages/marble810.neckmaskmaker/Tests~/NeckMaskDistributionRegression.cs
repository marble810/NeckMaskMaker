// UnityMCP execute_code / Roslyn 方法体；合成验证，偏好在 finally 恢复，不修改场景。
var type = AppDomain.CurrentDomain.GetAssemblies()
    .Select(a => a.GetType("marble810.NeckMaskMaker.NeckMaskMaker")).First(t => t != null);
void Check(bool ok, string label) { if (!ok) throw new Exception("FAIL: " + label); }
Check(type.Assembly.GetName().Name == "marble810.neckmaskmaker.editor", "独立 Editor 程序集");
Check(type.Assembly.GetReferencedAssemblies().All(a =>
    !a.Name.Contains("marbleavatartoolbox") && !a.Name.StartsWith("nadena") && !a.Name.Contains("VRCFury")), "无合集/NDMF/MA/VRCFury 程序集依赖");
Check(AppDomain.CurrentDomain.GetAssemblies().All(a =>
    a.GetType("marble810.MarbleAvatarToolbox.NeckMaskMaker.NeckMaskMaker") == null), "旧实现不再编译");
var menu = (MenuItem)type.GetMethod("ShowWindow").GetCustomAttributes(typeof(MenuItem), false).Single();
Check(menu.menuItem == "Tools/Marble/Neck Mask Maker", "独立菜单");
var computePath = AssetDatabase.GUIDToAssetPath("8afdbd39c9274d0db2af4684d791d51b");
Check(computePath == "Packages/marble810.neckmaskmaker/Shaders/NeckMaskBake.compute", "Compute GUID 保留且来自新包");
var baker = type.Assembly.GetType("marble810.NeckMaskMaker.NeckMaskGpuBaker");
var staticFlags = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public;
Check(baker.GetMethod("LoadComputeShader", staticFlags).Invoke(null, null) != null, "GUID 资源定位");
Check(Shader.Find("Hidden/NeckMaskMaker/NeckMaskPreview") != null
    && Shader.Find("Hidden/NeckMaskMaker/NeckMaskEdge") != null, "独立 Shader 名称");
var prefs = type.Assembly.GetType("marble810.NeckMaskMaker.NeckMaskPreferences");
var migrate = prefs.GetMethod("MigrateLegacy", staticFlags);
var names = new[] { "Language", "LastSaveDirectory", "PreviewPanelX", "PreviewPanelY", "PreviewPanelSize" };
var keys = names.SelectMany(n => new[] { "MarbleAvatarToolbox.NeckMaskMaker." + n, "NeckMaskMaker." + n }).ToArray();
var had = keys.ToDictionary(k => k, EditorPrefs.HasKey);
var values = keys.ToDictionary(k => k, k => k.EndsWith("Language") ? (object)EditorPrefs.GetInt(k)
    : k.EndsWith("LastSaveDirectory") ? (object)EditorPrefs.GetString(k) : EditorPrefs.GetFloat(k));
try
{
    foreach (string key in keys) EditorPrefs.DeleteKey(key);
    EditorPrefs.SetInt("MarbleAvatarToolbox.NeckMaskMaker.Language", 2);
    EditorPrefs.SetString("MarbleAvatarToolbox.NeckMaskMaker.LastSaveDirectory", "legacy-test-directory");
    EditorPrefs.SetFloat("MarbleAvatarToolbox.NeckMaskMaker.PreviewPanelX", 23f);
    EditorPrefs.SetFloat("MarbleAvatarToolbox.NeckMaskMaker.PreviewPanelY", 45f);
    EditorPrefs.SetFloat("MarbleAvatarToolbox.NeckMaskMaker.PreviewPanelSize", 180f);
    migrate.Invoke(null, null);
    Check(EditorPrefs.GetInt("NeckMaskMaker.Language") == 2, "语言继承");
    Check(EditorPrefs.GetString("NeckMaskMaker.LastSaveDirectory") == "legacy-test-directory", "最近目录继承");
    Check(EditorPrefs.GetFloat("NeckMaskMaker.PreviewPanelX") == 23f
        && EditorPrefs.GetFloat("NeckMaskMaker.PreviewPanelY") == 45f
        && EditorPrefs.GetFloat("NeckMaskMaker.PreviewPanelSize") == 180f, "预览偏好继承");
    EditorPrefs.SetInt("NeckMaskMaker.Language", 1);
    EditorPrefs.SetString("NeckMaskMaker.LastSaveDirectory", "new-test-directory");
    EditorPrefs.SetFloat("NeckMaskMaker.PreviewPanelSize", 200f);
    migrate.Invoke(null, null);
    Check(EditorPrefs.GetInt("NeckMaskMaker.Language") == 1
        && EditorPrefs.GetString("NeckMaskMaker.LastSaveDirectory") == "new-test-directory"
        && EditorPrefs.GetFloat("NeckMaskMaker.PreviewPanelSize") == 200f, "已存在新偏好优先");
    Check(EditorPrefs.GetInt("MarbleAvatarToolbox.NeckMaskMaker.Language") == 2, "旧偏好保留");
    return "PASS: 独立程序集/菜单/无合集依赖/Compute GUID/Shader/全部偏好迁移/新值优先/旧键保留";
}
finally
{
    foreach (string key in keys)
    {
        EditorPrefs.DeleteKey(key);
        if (!had[key]) continue;
        if (key.EndsWith("Language")) EditorPrefs.SetInt(key, (int)values[key]);
        else if (key.EndsWith("LastSaveDirectory")) EditorPrefs.SetString(key, (string)values[key]);
        else EditorPrefs.SetFloat(key, (float)values[key]);
    }
}
