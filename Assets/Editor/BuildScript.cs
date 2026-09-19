using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildScript
{
    private const string WebGlOutput = "BuildWebGL";
    private const string WindowsOutput = "BuildWindows/CookieClickerTest.exe";

    [MenuItem("Tools/Build/WebGL Release")]
    public static void BuildWebGL()
    {
        Build(BuildTarget.WebGL, WebGlOutput);
    }

    [MenuItem("Tools/Build/Windows Release")]
    public static void BuildWindows()
    {
        Build(BuildTarget.StandaloneWindows64, WindowsOutput);
    }

    private static void Build(BuildTarget target, string outputPath)
    {
        ApplyWebReleaseSettings(target);
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(target), target);

        var scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            throw new InvalidOperationException("No enabled scenes are configured in Build Settings.");
        }

        var absoluteOutput = Path.GetFullPath(outputPath);
        Directory.CreateDirectory(Path.GetDirectoryName(absoluteOutput));

        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = absoluteOutput,
            target = target,
            options = BuildOptions.StrictMode
        });

        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new BuildFailedException($"Build failed: {report.summary.result}; errors={report.summary.totalErrors}");
        }

        Debug.Log($"Build succeeded: {report.summary.totalSize / (1024f * 1024f):F2} MB at {absoluteOutput}");
    }

    private static void ApplyWebReleaseSettings(BuildTarget target)
    {
        if (target != BuildTarget.WebGL)
        {
            return;
        }

        var namedTarget = UnityEditor.Build.NamedBuildTarget.WebGL;
        PlayerSettings.stripEngineCode = true;
        PlayerSettings.stripUnusedMeshComponents = true;
        PlayerSettings.SetManagedStrippingLevel(namedTarget, ManagedStrippingLevel.High);
        PlayerSettings.SetIl2CppCodeGeneration(namedTarget, Il2CppCodeGeneration.OptimizeSize);
        PlayerSettings.WebGL.dataCaching = true;
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
        PlayerSettings.WebGL.decompressionFallback = false;
        PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.None;
        PlayerSettings.WebGL.debugSymbolMode = WebGLDebugSymbolMode.Off;
        PlayerSettings.WebGL.wasm2023 = true;
        UnityEditor.WebGL.UserBuildSettings.codeOptimization = UnityEditor.WebGL.WasmCodeOptimization.DiskSizeLTO;
        PlayerSettings.SetApiCompatibilityLevel(namedTarget, ApiCompatibilityLevel.NET_Standard);
        Application.targetFrameRate = -1;
        QualitySettings.vSyncCount = 0;
        AssetDatabase.SaveAssets();
    }
}
