using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]



public class DepthVideo : MonoBehaviour
{
    public RenderTexture videoTexture;


    //public RawImage raw;

    [SerializeField] private ParticleSystem psSystem;

    [SerializeField] private GameObject particle;
    private GameObject[] particles;
    public Texture2D m_DepthTexture_Float;

    //video parameter
    public int width_depth;
    public int height_depth;
    public int resolution;
    public Color[] colorPixels;
    // Start is called before the first frame update
    void Awake()
    {
        
        m_DepthTexture_Float = toTexture2D(videoTexture);
        width_depth = m_DepthTexture_Float.width;
        height_depth = m_DepthTexture_Float.height;
        resolution = width_depth * height_depth;
        colorPixels = m_DepthTexture_Float.GetPixels();
        //particles = new GameObject[resolution];

        //assign particle texture
        //var sh = psSystem.shape;
        //sh.shapeType = ParticleSystemShapeType.Rectangle;
        //sh.texture = m_DepthTexture_Float;

        //ReprojectPointCloud();
    }

    // Update is called once per frame
    void Update()
    {
        
        m_DepthTexture_Float = toTexture2D(videoTexture);
        colorPixels = m_DepthTexture_Float.GetPixels();
        /*
        m_DepthTexture_Float = toTexture2D(videoTexture);
        int width_depth = m_DepthTexture_Float.width;
        int height_depth = m_DepthTexture_Float.height;
        Color[] depthPixels = m_DepthTexture_Float.GetPixels();
        for (int index_dst = 0, depth_y = 0; depth_y < height_depth; depth_y++)
        {
            //index_dst = depth_y * width_depth;
            for (int depth_x = 0; depth_x < width_depth; depth_x++, index_dst++)
            {
                float pointDepth = -100;
                float depth = depthPixels[index_dst].g * pointDepth;
                
                particles[index_dst].GetComponent<Renderer>().material.color = depthPixels[index_dst];
                particles[index_dst].transform.position = new Vector3(depth_x, depth_y, depth);
            }
        }
        */

    }

    void ReprojectPointCloud()
    {   //Call the convert video to texture2d function
        m_DepthTexture_Float = toTexture2D(videoTexture);
        //raw.texture = m_DepthTexture_Float;



        int width_depth = m_DepthTexture_Float.width;
        int height_depth = m_DepthTexture_Float.height;
        //int width_camera = m_CameraTexture.width;




        Color[] depthPixels = m_DepthTexture_Float.GetPixels();


        //int index_dst;
        float depth;

        for (int index_dst = 0, depth_y = 0; depth_y < height_depth; depth_y++)
        {
            //index_dst = depth_y * width_depth;
            for (int depth_x = 0; depth_x < width_depth; depth_x++, index_dst++)
            {
                float pointDepth = 100;
                depth = depthPixels[index_dst].g * pointDepth;
                particles[index_dst] = Instantiate(particle, new Vector3(depth_x, depth_y, depth), new Quaternion(0, 0, 0, 0));
                particles[index_dst].GetComponent<Renderer>().material.color = depthPixels[index_dst];
            }
        }

    }







    //Function For convert RenderTexture(Video File) to Texture2d
    //https://stackoverflow.com/questions/44264468/convert-rendertexture-to-texture2d
    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.width, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}