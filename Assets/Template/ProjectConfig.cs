#if UNITY_EDITOR
namespace TemplateKeywords
{
	/// <summary>
	/// keywords repacement, 
    /// the project name will be used as namespace, 
    /// the coder name will be written as author
	/// </summary>
	public static class ProjectConfig 
	{
		#region  Attributes and Properties
		//
		// Static Public Members
		//
		static public string Project_Name = "Debugger_For_Unity";
        static public string Coder_Name = "He, Mingfei";
        #endregion
    }
}
#endif