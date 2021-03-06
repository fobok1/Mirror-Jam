﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MadGoat_SSAA
{
    public class MadGoatSSAA_InternalRenderer : MonoBehaviour
    {
        [HideInInspector()]
        public float multiplier;

        // Shader Pramaters
        [HideInInspector()]
        public float sharpness;
        [HideInInspector()]
        public bool useShader;
        [HideInInspector()]
        public float sampleDistance;

        // Cameras
        [HideInInspector()]
        public Camera main;
        [HideInInspector()]
        public Camera current;

        // Shader Setup
        [SerializeField]
        private Shader _bilinearshader;
        public Shader bilinearshader
        {
            get
            {
                if (_bilinearshader == null)
                    _bilinearshader = Shader.Find("Hidden/SSAA_Bilinear");

                return _bilinearshader;
            }
        }
        [SerializeField]
        private Shader _bicubicshader;
        public Shader bicubicshader
        {
            get
            {
                if (_bicubicshader == null)
                    _bicubicshader = Shader.Find("Hidden/SSAA_Bicubic");

                return _bicubicshader;
            }
        }
        [SerializeField]
        private Shader _neighborshader;
        public Shader neighborshader
        {
            get
            {
                if (_neighborshader == null)
                {
                    _neighborshader = Shader.Find("Hidden/SSAA_Nearest");
                }
                return _neighborshader;
            }
        }
        [SerializeField]
        private Shader _defshader;
        public Shader defshader
        {
            get
            {
                if (_defshader == null)
                {
                    _defshader = Shader.Find("Hidden/SSAA_Def");
                }
                return _defshader;
            }
        }

        private Material material_bl; // Bilinear Material
        private Material material_bc; // Bicubic
        private Material material_nn; // Nearest Neighbor
        private Material material_def; // Default

        private Material material_current;

        MadGoatSSAA mainComponent;
        private void Start()
        {
            mainComponent = main.GetComponent<MadGoatSSAA>();
            material_bl = new Material(bilinearshader);
            material_bc = new Material(bicubicshader);
            material_nn = new Material(neighborshader);
            material_def = new Material(defshader);
            material_current = material_bc;
        }
        /// <summary>
        /// Change the shader to use in the internal renderer
        /// </summary>
        public void ChangeMaterial(Filter Type)
        {
            // Point material_current to the given material
            switch (Type)
            {
                case Filter.NEAREST_NEIGHBOR:
                    material_current = material_nn;
                    break;
                case Filter.BILINEAR:
                    material_current = material_bl;
                    break;
                case Filter.BICUBIC:
                    material_current = material_bc;
                    break;
            }
        }
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // is a screenshot queued?
            if (mainComponent.screenshtSettings.takeScreenshot)
            {
                // Default material for screenshots is bicubic (we don't care about performance here, so we use whats best)
                Material material = new Material(bicubicshader);
                
                // buffer to store texture
                RenderTexture buff = new RenderTexture((int)mainComponent.screenshtSettings.outputResolution.x, (int)mainComponent.screenshtSettings.outputResolution.y, 24, RenderTextureFormat.ARGB32);

                // setup shader
                if (mainComponent.screenshtSettings.useFilter)
                {
                    material.SetFloat("_ResizeWidth", (int)mainComponent.screenshtSettings.outputResolution.x);
                    material.SetFloat("_ResizeHeight", (int)mainComponent.screenshtSettings.outputResolution.y);
                    material.SetFloat("_Sharpness", 0.85f);
                    Graphics.Blit(main.targetTexture, buff, material, 0);
                }
                else // or blit as it is
                {
                    Graphics.Blit(main.targetTexture, buff);
                }
                DestroyImmediate(material);
                RenderTexture.active = buff;

                // Copy from active texture to buffer
                Texture2D screenshotBuffer = new Texture2D(RenderTexture.active.width, RenderTexture.active.height, TextureFormat.RGB24, false);
                screenshotBuffer.ReadPixels(new Rect(0, 0, RenderTexture.active.width, RenderTexture.active.height), 0, 0);

                // Create path if not available and write the screenshot to disk
                (new FileInfo(mainComponent.screenshtSettings.screenshotPath)).Directory.Create();
                File.WriteAllBytes(mainComponent.screenshtSettings.screenshotPath + getName, screenshotBuffer.EncodeToPNG());

                // Clean stuff
                RenderTexture.active = null;
                buff.Release();

                DestroyImmediate(screenshotBuffer);
                mainComponent.screenshtSettings.takeScreenshot = false;
            }

            // Effect is disabled or we don't use custom downsampling
            if (!useShader || multiplier == 1f)
            {
                // Just copy pixels without alteration
                Graphics.Blit(main.targetTexture, destination, material_def, 0);
            }
            else // Setup the custom downsampler and output
            {
                material_current.SetFloat("_ResizeWidth", Screen.width);
                material_current.SetFloat("_ResizeHeight", Screen.height);
                material_current.SetFloat("_Sharpness", sharpness);
                material_current.SetFloat("_SampleDistance", sampleDistance);
                Graphics.Blit(main.targetTexture, destination, material_current, 0);
            }
        }
        private string getName // generate a string for the filename of the screenshot
        {
            get
            {
                return mainComponent.screenshtSettings.namePrefix + "_" +
                    DateTime.Now.Year.ToString() +
                    DateTime.Now.Month.ToString() +
                    DateTime.Now.Day.ToString() + "_" +
                    DateTime.Now.Hour.ToString() +
                    DateTime.Now.Minute.ToString() +
                    DateTime.Now.Second.ToString() +
                    DateTime.Now.Millisecond.ToString() + "_" +
                    mainComponent.screenshtSettings.outputResolution.y.ToString() + "p.png";
            }
        }
    }
}