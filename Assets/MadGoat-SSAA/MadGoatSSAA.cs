using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadGoat_SSAA
{
    [RequireComponent(typeof(Camera))]
    public class MadGoatSSAA : MonoBehaviour
    {
        // Renderer
        public Mode renderMode = Mode.SSAA;
        public float multiplier = 1f;

        // SSAA Profiles
        public SsaaProfile SSAA_X2 = new SsaaProfile(1.5f, true, Filter.BILINEAR, .8f, .5f);
        public SsaaProfile SSAA_X4 = new SsaaProfile(2f, true, Filter.BICUBIC, .725f, .95f);
        public SsaaProfile SSAA_HALF = new SsaaProfile(.5f, true, Filter.NEAREST_NEIGHBOR, 0, 0);
        public SSAAMode ssaaMode = SSAAMode.SSAA_OFF;

        // Downsampler
        public bool useShader = false;
        public Filter filterType = Filter.BILINEAR;
        public float sharpness = 0.8f;
        public float sampleDistance = 1f;

        // Misc
        [SerializeField]
        private Camera currentCamera;
        private Camera renderCamera;
        private GameObject renderCameraObject;
        private MadGoatSSAA_InternalRenderer SSAA_Internal;
        private Rect tempRect;

        // Misc settings
        public bool mouseCompatibilityMode;

        // Screenshot Module
        public ScreenshotSettings screenshtSettings = new ScreenshotSettings();

        //********* Private functions *********
        private void OnEnable()
        {
            currentCamera = GetComponent<Camera>();
            if (renderCameraObject == null)
            {
                //Setup new high resolution camera
                renderCameraObject = new GameObject("RenderCameraObject");
                renderCameraObject.transform.SetParent(transform);
                renderCameraObject.transform.position = Vector3.zero;
                renderCameraObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                renderCameraObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

                // Setup components of new camera
                renderCamera = renderCameraObject.AddComponent<Camera>();
                SSAA_Internal = renderCameraObject.AddComponent<MadGoatSSAA_InternalRenderer>();
                SSAA_Internal.current = renderCamera;
                SSAA_Internal.main = currentCamera;
                SSAA_Internal.enabled = true;

                // Copy settings from current camera
                renderCamera.CopyFrom(currentCamera);

                // Disable rendering on internal cam.
                // Nothing is drawn on main camera, performance hit is minimal
                renderCamera.cullingMask = 0;
                renderCamera.clearFlags = CameraClearFlags.Nothing;
            }
            else
                SSAA_Internal.enabled = true;

            currentCamera.targetTexture = new RenderTexture(
                (int)(Screen.width * multiplier),
                (int)(Screen.height * multiplier),
                24,
                RenderTextureFormat.ARGB32);
            currentCamera.targetTexture.Create();
        }
        private void Update()
        {
            renderCamera.enabled = currentCamera.enabled;
            renderCamera.CopyFrom(currentCamera, null);

            // Nothing is drawn on output camera, so the performance hit is minimal, we only need it to output the render (Graphics.Blit)
            renderCamera.cullingMask = mouseCompatibilityMode? -1 : 0;
            renderCamera.clearFlags = CameraClearFlags.Color;

            // Set render settings
            SSAA_Internal.multiplier = multiplier;
            SSAA_Internal.sharpness = sharpness;
            SSAA_Internal.useShader = useShader;
            SSAA_Internal.sampleDistance = sampleDistance;

            SSAA_Internal.ChangeMaterial(filterType);
        }
        private void OnDisable()
        {
            SSAA_Internal.enabled = false;
            currentCamera.targetTexture.Release();
            currentCamera.targetTexture = null;
        }
        private void OnPreRender()
        {
            // Setup the aspect ratio
            currentCamera.aspect = (Screen.width*currentCamera.rect.width) / (Screen.height*currentCamera.rect.height) ;
            // If a screenshot is queued 
            if (screenshtSettings.takeScreenshot)
            {   // Setup for the screenshot and stop there.
                SetupScreenshotRender(screenshtSettings.screenshotMultiplier, false);
                return;
            }

            if (Screen.width * multiplier != currentCamera.targetTexture.width || Screen.height * multiplier != currentCamera.targetTexture.height)
                SetupRender(multiplier);

            // Cache current camera rect and set it to fullscreen
            // Render Texture doesn't seem to like incomplete camera renders for some reason.
            tempRect = currentCamera.rect;
            currentCamera.rect = new Rect(0, 0, 1, 1);
        }
        private void OnPostRender()
        {
            // Reset the camera rect (to be used used by our internal camera in final output)
            currentCamera.rect = tempRect;
        }

        // Setup for SSAA renderer
        private void SetupRender(float mul)
        {
            try
            {
                currentCamera.targetTexture.Release();
                currentCamera.targetTexture.width = (int)(Screen.width * mul);
                currentCamera.targetTexture.height = (int)(Screen.height * mul);
                currentCamera.targetTexture.Create();
            }
            catch (Exception ex)
            {
                Debug.LogError("Something went wrong. SSAA has been set to off");
                Debug.LogError(ex);
                SetAsSSAA(SSAAMode.SSAA_OFF);
            }
        }
        // Setup for ScreenShot Render
        private void SetupScreenshotRender(float mul, bool compatibilityMode)
        {
            try
            {
                // If taking a screenshot, the aspect ratio should be given by the screenshot resolution, not the screenres.
                currentCamera.aspect = screenshtSettings.outputResolution.x / screenshtSettings.outputResolution.y;

                currentCamera.targetTexture.Release();
                currentCamera.targetTexture.width = (int)(screenshtSettings.outputResolution.x * mul);
                currentCamera.targetTexture.height = (int)(screenshtSettings.outputResolution.y * mul);
                currentCamera.targetTexture.Create();
            }
            catch (Exception ex) { Debug.LogError(ex.ToString()); }
        }


        /************************************************************************
         *                            PUBLIC API                                *
         ************************************************************************/
        /// <summary>
        /// Set rendering mode to given SSAA mode
        /// </summary>
        public void SetAsSSAA(SSAAMode mode)
        {
            renderMode = Mode.SSAA;
            ssaaMode = mode;
            switch (mode)
            {
                case SSAAMode.SSAA_OFF:
                    multiplier = 1f;
                    useShader = false;
                    break;
                case SSAAMode.SSAA_HALF:
                    multiplier = SSAA_HALF.multiplier;
                    useShader = SSAA_HALF.useFilter;
                    sharpness = SSAA_HALF.sharpness;
                    filterType = SSAA_HALF.filterType;
                    sampleDistance = SSAA_HALF.sampleDistance;
                    break;
                case SSAAMode.SSAA_X2:
                    multiplier = SSAA_X2.multiplier;
                    useShader = SSAA_X2.useFilter;
                    sharpness = SSAA_X2.sharpness;
                    filterType = SSAA_X2.filterType;
                    sampleDistance = SSAA_X2.sampleDistance;
                    break;
                case SSAAMode.SSAA_X4:
                    multiplier = SSAA_X4.multiplier;
                    useShader = SSAA_X4.useFilter;
                    sharpness = SSAA_X4.sharpness;
                    filterType = SSAA_X4.filterType;
                    sampleDistance = SSAA_X4.sampleDistance;
                    break;
            }
        }
        /// <summary>
        /// Set the resolution scale to a given percent
        /// </summary>
        public void SetAsScale(int percent)
        {
            renderMode = Mode.ResolutionScale;
            multiplier = percent / 100f;

            SetDownsamplingSettings(false);
        }
        /// <summary>
        /// Set the resolution scale to a given percent, and use custom downsampler settings
        /// </summary>
        public void SetAsScale(int percent, Filter FilterType, float sharpnessfactor, float sampledist)
        {
            renderMode = Mode.ResolutionScale;
            multiplier = percent / 100f;

            SetDownsamplingSettings(FilterType, sharpnessfactor, sampledist);
        }
        /// <summary>
        /// Set a custom resolution multiplier
        /// </summary>
        public void SetAsCustom(float Multiplier)
        {
            renderMode = Mode.Custom;
            multiplier = Multiplier;

            SetDownsamplingSettings(false);
        }
        /// <summary>
        /// Set a custom resolution multiplier, and use custom downsampler settings
        /// </summary>
        public void SetAsCustom(float Multiplier, Filter FilterType, float sharpnessfactor, float sampledist)
        {
            renderMode = Mode.Custom;
            multiplier = Multiplier;

            SetDownsamplingSettings(FilterType, sharpnessfactor, sampledist);
        }
        /// <summary>
        /// Set the downsampling shader parameters. If the case, this should be called after setting the mode, otherwise it might get overrided. (ex: SSAA)
        /// </summary>
        public void SetDownsamplingSettings(bool use)
        {
            useShader = use;
            filterType = use ? Filter.BILINEAR : Filter.NEAREST_NEIGHBOR;
            sharpness = use ? 0.85f : 0; // 0.85 should work fine for any resolution 
            sampleDistance = use ? 0.9f : 0; // 0.9 should work fine for any res
        }
        /// <summary>
        /// Set the downsampling shader parameters. If the case, this should be called after setting the mode, otherwise it might get overrided. (ex: SSAA)
        /// </summary>
        public void SetDownsamplingSettings(Filter FilterType, float sharpnessfactor, float sampledist)
        {
            useShader = true;
            filterType = FilterType;
            sharpness = Mathf.Clamp(sharpnessfactor, 0, 1);
            sampleDistance = Mathf.Clamp(sampledist, 0.5f, 1.5f);
        }
        /// <summary>
        /// Take a screenshot of resolution Size (x is width, y is height) rendered at a higher resolution given by the multiplier. The screenshot is saved at the given path in PNG format.
        /// </summary>
        public void TakeScreenshot(string path, Vector2 Size, int multiplier)
        {
            // Take screenshot with default settings
            screenshtSettings.takeScreenshot = true;
            screenshtSettings.outputResolution = Size;
            screenshtSettings.screenshotMultiplier = multiplier;
            screenshtSettings.screenshotPath = path;
            screenshtSettings.useFilter = false;
        }
        /// <summary>
        /// Take a screenshot of resolution Size (x is width, y is height) rendered at a higher resolution given by the multiplier and use the bicubic downsampler. The screenshot is saved at the given path in PNG format. 
        /// </summary>
        public void TakeScreenshot(string path, Vector2 Size, int multiplier, float sharpness)
        {
            // Take screenshot with custom settings
            screenshtSettings.takeScreenshot = true;
            screenshtSettings.outputResolution = Size;
            screenshtSettings.screenshotMultiplier = multiplier;
            screenshtSettings.screenshotPath = path;
            screenshtSettings.useFilter = true;
            screenshtSettings.sharpness = Mathf.Clamp(sharpness, 0, 1);
        }
        /// <summary>
        /// Return string with current internal resolution
        /// </summary>
        /// <returns></returns>
        public string GetRes()
        {
            return Screen.width * multiplier + ":" + Screen.height * multiplier;
        }
      
        /// <summary>
        /// Returns a ray from a given screenpoint
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Ray ScreenPointToRay(Vector3 position)
        {
            return renderCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
 