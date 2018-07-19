#if UNITY_EDITOR
using System.IO;
using UnityEditor;

namespace TemplateKeywords
{
    /// <summary>
	/// Unity Template Keywords Replacement
	/// </summary>
	public class ScriptTemplateKeywordsReplacement : UnityEditor.AssetModificationProcessor
    {
        #region Static Methods
        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            if (path.ToLower().EndsWith(".cs") || path.ToLower().EndsWith(".lua"))
            {
                string content = File.ReadAllText(path);
                content = content.Replace("#CREATIONDATE#", System.DateTime.Now.ToString());
                content = content.Replace("#PROJECTNAME#", ProjectConfig.Project_Name);
                content = content.Replace("#SMARTDEVELOPERS#", ProjectConfig.Coder_Name);
                System.IO.File.WriteAllText(path, content);
                AssetDatabase.Refresh();
            }
        }
        #endregion
    }
}
#endif
