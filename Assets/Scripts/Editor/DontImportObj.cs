using UnityEditor;

public class DontImportObj : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        // 파일 경로 확인
        if (assetPath.EndsWith(".obj") && assetPath.Contains("Assets/Scripts"))
            AssetDatabase.DeleteAsset(assetPath);
    }
}