using UnityEditor;

public class DontImportObj : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        // ���� ��� Ȯ��
        if (assetPath.EndsWith(".obj") && assetPath.Contains("Assets/Scripts"))
            AssetDatabase.DeleteAsset(assetPath);
    }
}