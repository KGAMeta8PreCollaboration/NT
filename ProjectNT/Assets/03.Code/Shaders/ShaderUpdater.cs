using UnityEngine;
# if UNITY_EDITOR
using UnityEditor;
#endif
public class ShaderUpdater : MonoBehaviour
{
	#if UNITY_EDITOR
	[MenuItem("Tools/Update Shaders")]
	public static void UpdateShaders()
	{
		string[] materialGuids = AssetDatabase.FindAssets("t:Material");
		foreach (string guid in materialGuids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
			if (material.shader.name == "Hidden/InternalErrorShader")
			{
				material.shader = Shader.Find("Universal Render Pipeline/Lit");
				EditorUtility.SetDirty(material);
			}
		}
		AssetDatabase.SaveAssets();
	}
	#endif
}
