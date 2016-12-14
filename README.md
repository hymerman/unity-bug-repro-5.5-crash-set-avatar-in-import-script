# What is this?
This is a project to reproduce a bug in Unity to do with a crash when setting the avatar type in OnPreprocessAnimation of an AssetPostprocessor.

This project contains a single FBX file with a mesh, rig and animation, and an AssetPostprocessor script that demonstrates the problem when run.

The script is set up to run on demand only, by right clicking the asset and choosing 'Setup Animation'.

Note that it happens whatever the model's current avatar type is, and whatever the type it's being changed to.

It doesn't crash if the change is done at another stage of the asset import, but then there are other problems - our full script also sets the reference avatar, and the asset being set up doesn't copy the avatar definition fully unless run in the OnPreprocessAnimation stage, so this isn't a workaround.

It seems to be a regression from Unity 5.4 as it started happening after upgrading to 5.5.0p1 from 5.4.0f3.

An example editor log file is included in this repository.

# Repro steps
- Right click the single model file
- Choose 'Setup Animation'

# Expected behaviour?
I would expect Unity to set the avatar type successfully.

# Actual behaviour
The Unity editor crashes.

Relevant log lines:

    Invalid OffsetPtr access! Pointer is NULL
    UnityEditor.AssetDatabase:ImportAsset(String, ImportAssetOptions)
    UnityEditor.AssetDatabase:ImportAsset(String) (at C:\buildslave\unity\build\artifacts\generated\common\editor\AssetDatabaseBindings.gen.cs:113)
    ImportAnimation:SetupAnimation() (at Assets\ImportAnimation.cs:18)
    
    [C:\buildslave\unity\build\Runtime/Serialize/Blobification/offsetptr.h line 51] 
    (Filename: Assets/ImportAnimation.cs Line: 18)
    
    Crash!!!

Relevant stack trace from log:

    ========== OUTPUTING STACK TRACE ==================
    
    0x00000001416323F4 (Unity) mecanim::skeleton::CreateSkeletonPose<math::trsX>
    0x000000014018592F (Unity) GenerateMecanimClipsCurves
    0x000000014130CA40 (Unity) ModelImporter::ImportMuscleClip
    0x00000001413239E7 (Unity) ModelImporter::GenerateAnimationClips
    0x000000014133C620 (Unity) ModelImporter::GenerateAll
    0x000000014133D410 (Unity) ModelImporter::GenerateAssetData
    0x000000014013E6F1 (Unity) AssetDatabase::ImportAsset
    0x000000014014A09B (Unity) AssetDatabase::UpdateAsset
    0x000000014014CA54 (Unity) AssetInterface::ProcessAssetsImplementation
    0x0000000140151292 (Unity) AssetInterface::StopAssetEditing
    0x0000000140151D53 (Unity) AssetInterface::ImportAtPath
    0x0000000141055A57 (Unity) AssetDatabase_CUSTOM_ImportAsset
    0x0000000011F88AAD (Mono JIT Code) (wrapper managed-to-native) UnityEditor.AssetDatabase:ImportAsset (string,UnityEditor.ImportAssetOptions)
    0x0000000011F889AE (Mono JIT Code) [C:\buildslave\unity\build\artifacts\generated\common\editor\AssetDatabaseBindings.gen.cs:113] UnityEditor.AssetDatabase:ImportAsset (string) 
    0x0000000011F821D5 (Mono JIT Code) [C:\PersonalProjects\unity-bug-repro-5.5-crash-set-avatar-in-import-script\Assets\ImportAnimation.cs:14] SetupAnimationProcessor:SetupAnimation () 
    0x000000001F260A8E (Mono JIT Code) (wrapper runtime-invoke) object:runtime_invoke_void (object,intptr,intptr,intptr)
    0x00007FFF763354A3 (mono) [c:\buildslave\mono\build\mono\mini\mini.c:4937] mono_jit_runtime_invoke 
    0x00007FFF762883F1 (mono) [c:\buildslave\mono\build\mono\metadata\object.c:2623] mono_runtime_invoke 
    0x00000001411FEB8F (Unity) scripting_method_invoke
    0x0000000140E325F5 (Unity) ScriptingInvocation::Invoke
    0x0000000140E32BAD (Unity) ScriptingInvocation::InvokeChecked
    0x00000001412567FA (Unity) ScriptCommands::InvokeMenuItemWithContext
    0x0000000141256A4F (Unity) ScriptCommands::Execute
    0x00000001414BB7C2 (Unity) MenuController::ExecuteMenuItem
    0x000000014154302F (Unity) ShowDelayedContextMenu
    0x000000014153388D (Unity) GUIView::OnInputEvent
    0x0000000140079C13 (Unity) GUIView::ProcessInputEvent
    0x000000014152EDE2 (Unity) GUIView::ProcessEventMessages
    0x0000000141534D3A (Unity) GUIView::GUIViewWndProc
    0x00007FFF9B301C24 (USER32) CallWindowProcW
    0x00007FFF9B30156C (USER32) DispatchMessageW
    0x000000014154DE59 (Unity) FindMonoBinaryToUse
    0x000000014154F4E1 (Unity) WinMain
    0x000000014186A914 (Unity) strnlen
    0x00007FFF9B918364 (KERNEL32) BaseThreadInitThunk
    0x00007FFF9C455E91 (ntdll) RtlUserThreadStart
    
    ========== END OF STACKTRACE ===========
