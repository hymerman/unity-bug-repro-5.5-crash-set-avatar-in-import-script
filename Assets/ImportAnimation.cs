using UnityEditor;
using UnityEngine;

public class SetupAnimationProcessor : AssetPostprocessor
{
	private static bool _runSetup;

	[MenuItem( "Assets/Setup Animation" )]
	private static void SetupAnimation()
	{
		Debug.LogFormat( "Setting up animation {0}...", Selection.activeObject );

		_runSetup = true;
		AssetDatabase.ImportAsset( AssetDatabase.GetAssetPath( Selection.activeObject ) );
		_runSetup = false;
	}

	[MenuItem( "Assets/Setup Animation", true )]
	private static bool SetupAnimationValidation()
	{
		return Selection.activeObject is GameObject;
	}

	private void OnPreprocessAnimation()
	{
		if ( _runSetup )
		{
			ModelImporter importer = assetImporter as ModelImporter;

			if ( importer != null )
			{
				importer.animationType = ModelImporterAnimationType.Human;

				Debug.LogFormat( "Setup {0} successfully", importer.assetPath );
			}
		}
	}
}
