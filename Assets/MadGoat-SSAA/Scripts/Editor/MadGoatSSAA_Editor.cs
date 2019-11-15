using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace MadGoat_SSAA
{
    [CustomEditor(typeof(MadGoatSSAA))]
    public class MadGoatSSAA_Editor : Editor
    {
        SerializedObject serObj;
        SerializedProperty Mode;
        SerializedProperty FilterType;
        SerializedProperty Sharpness;
        SerializedProperty sampleDistance;
        SerializedProperty Multiplier;
        SerializedProperty UseShader;
        SerializedProperty mouseCompatiblity;
        //SerializedProperty vrCompatibility;

        SerializedProperty SSAA_HALF;
        SerializedProperty SSAA_X2;
        SerializedProperty SSAA_X4;
        private string[] ssaaModes = new string[] { "Off", "0.5x", "2x", "4x" };


        ScreenshotSettings settings = new ScreenshotSettings();
        int tab;
        private bool Extend;
        private int scale;
        private int mode;
        void OnEnable()
        {
            serObj = new SerializedObject(target);
            Mode = serObj.FindProperty("renderMode");
            FilterType = serObj.FindProperty("filterType");
            Sharpness = serObj.FindProperty("sharpness");
            sampleDistance = serObj.FindProperty("sampleDistance");
            Multiplier = serObj.FindProperty("multiplier");
            UseShader = serObj.FindProperty("useShader");
            mouseCompatiblity = serObj.FindProperty("mouseCompatibilityMode");
            //vrCompatibility = serObj.FindProperty("useVR");

            SSAA_HALF = serObj.FindProperty("SSAA_HALF");
            SSAA_X2 = serObj.FindProperty("SSAA_X2");
            SSAA_X4 = serObj.FindProperty("SSAA_X4");
        }
        public override void OnInspectorGUI()
        {
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serObj.Update();
            GUIStyle s = new GUIStyle();
            s.normal.textColor = new Color(0.5f, 0.1f, 0.1f);
            s.fontSize = 16;
            EditorGUILayout.Separator();
            GUILayout.Label("MadGoat SuperSampling", s);
            EditorGUILayout.Separator();
            tab = GUILayout.Toolbar(tab, new string[] { "SSAA", "Screenshot", "Misc" });
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            switch (tab)
            {
                case 0:
                    s.fontSize = 12;
                    s.normal.textColor = new Color(0, 0.2f, 1);
                    s.normal.textColor = new Color(0.5f, 0.1f, 0.1f);
                    EditorGUILayout.PropertyField(Mode, new GUIContent("Operation mode"), true);
                    if (Mode.intValue == 1) // Resolution scale
                    {
                        EditorGUILayout.HelpBox("Rise or lower the render resolution by percent", MessageType.Info);
                        Multiplier.floatValue = EditorGUILayout.Slider("Resolution Scale (%)", Multiplier.floatValue * 100f, 50, 200) / 100f;
                    }
                    else if (Mode.intValue == 0) // SSAA presets
                    {
                        mode = getmode();
                        EditorGUILayout.HelpBox("Conventional SSAA settings. Higher settings produces better quality at the cost of performance. x0.5 boost the performance, but reduces the resolution.", MessageType.Info);
                        mode = EditorGUILayout.Popup("SSAA Mode", mode, ssaaModes);
                        switch (mode)
                        {
                            case 0: // off
                                (target as MadGoatSSAA).SetAsSSAA(SSAAMode.SSAA_OFF);
                                break;
                            case 1: // x0.5
                                (target as MadGoatSSAA).SetAsSSAA(SSAAMode.SSAA_HALF);
                                break;
                            case 2: // x2
                                (target as MadGoatSSAA).SetAsSSAA(SSAAMode.SSAA_X2);
                                break;
                            case 3: // x4
                                (target as MadGoatSSAA).SetAsSSAA(SSAAMode.SSAA_X4);
                                break;
                        }
                        EditorGUILayout.Separator();
                        s.fontSize = 12;
                        GUILayout.Label("Edit SSAA Presets", s);
                        EditorGUILayout.Separator();
                        EditorGUILayout.PropertyField(SSAA_HALF, new GUIContent("SSAA x0.5"), true);
                        EditorGUILayout.PropertyField(SSAA_X2, new GUIContent("SSAA x2"), true);
                        EditorGUILayout.PropertyField(SSAA_X4, new GUIContent("SSAA x4"), true);

                        if (GUILayout.Button("Reset SSAA preset to defaults"))
                        {
                            // Reset
                            (target as MadGoatSSAA).SSAA_X2 = new SsaaProfile(1.5f, true, Filter.BILINEAR, 0.8f, 0.5f);
                            (target as MadGoatSSAA).SSAA_X4 = new SsaaProfile(2f, true, Filter.BICUBIC, 0.725f, .95f);
                            (target as MadGoatSSAA).SSAA_HALF = new SsaaProfile(.5f, false);
                        }
                    }
                    else // Custom 
                    {
                        EditorGUILayout.HelpBox("Experimental. Only use if you know what you're doing.\nValues over 4 not recommended, higher values (depending on current screen size) may cause system instability or engine crashes.", MessageType.Warning);

                        Extend = EditorGUILayout.Toggle("Don't limit the multiplier", Extend);
                        if (Extend) EditorGUILayout.PropertyField(Multiplier, new GUIContent("Resolution Multiplier"), true);
                        else Multiplier.floatValue = EditorGUILayout.Slider("Resolution Multiplier", Multiplier.floatValue, 0.2f, 4f);
                    }
                    // Draw the shader stuff
                    if (Mode.intValue != 0)
                    {
                        EditorGUILayout.Separator();
                        s.fontSize = 12;
                        GUILayout.Label("Downsampling", s);
                        EditorGUILayout.Separator();

                        EditorGUILayout.HelpBox("If using image filtering, the render image will be passed through a custom downsampling filter. If not, it will be resized as is.", MessageType.Info);
                        UseShader.boolValue = EditorGUILayout.Toggle("Use Filter", UseShader.boolValue);
                        if (UseShader.boolValue)
                        {
                            EditorGUILayout.PropertyField(FilterType);
                            Sharpness.floatValue = EditorGUILayout.Slider("Downsample Sharpness", Sharpness.floatValue, 0f, 1f);
                            sampleDistance.floatValue = EditorGUILayout.Slider("Distance between samples", sampleDistance.floatValue, 0.5f, 2f);
                        }
                    }
                    break;
                case 1:
                    // the screenshot module
                    settings.outputResolution = EditorGUILayout.Vector2Field("Screenshot Resolution", settings.outputResolution);
                    settings.screenshotMultiplier = EditorGUILayout.IntSlider("Render Resolution Multiplier", settings.screenshotMultiplier, 1, 4);
                    s.fontSize = 12;
                    s.normal.textColor = new Color(0, 0.2f, 1);
                    GUILayout.Label("*Render Resolution: " + settings.outputResolution * settings.screenshotMultiplier, s);
                    EditorGUILayout.Separator();
                    settings.screenshotPath = EditorGUILayout.TextField("Save path", settings.screenshotPath);
                    settings.namePrefix = EditorGUILayout.TextField("File Name Prefix", settings.namePrefix);
                    settings.useFilter = EditorGUILayout.Toggle("Use downsampling shader", settings.useFilter);
                    if (settings.useFilter)
                    {
                        settings.sharpness = EditorGUILayout.Slider("   Sharpness", settings.sharpness, 0, 1);
                    }
                    if (GUILayout.Button(Application.isPlaying ? "Take Screenshot" : "Only available in play mode"))
                    {
                        if (Application.isPlaying)
                            (target as MadGoatSSAA).TakeScreenshot(
                                settings.screenshotPath,
                                settings.outputResolution,
                                settings.screenshotMultiplier,
                                settings.sharpness
                                );
                    }
                    break;
                case 2:
                    EditorGUILayout.HelpBox("Enables compatibility with OnClick() and other mouse events, at the cost of performance impact (scene is rendered second time for collider lookup).", MessageType.Info);
                    mouseCompatiblity.boolValue = EditorGUILayout.Toggle("OnClick() Compatibility Mode", mouseCompatiblity.boolValue);
                    break;
            }
            s.normal.textColor = new Color(0.5f, 0.1f, 0.1f);
            s.fontSize = 8;
            EditorGUILayout.Separator();
            GUILayout.Label("Version: " + MadGoatSSAA_Utils.ssaa_version, s);
            // Apply modifications
            serObj.ApplyModifiedProperties();
        }
        private int getmode()
        {
            return (int)(target as MadGoatSSAA).ssaaMode;
        }
    }
}