using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ParticleController : MonoBehaviour
{
    [SerializeField] InteractionManager interactionManager; 

    public ParticleSystem ps;
    [SerializeField] ParticleSystem.Particle[] particles;
    
    
    [Range(0, 1000f)] [SerializeField] private float pointDepth;
    [Range(0, 1f)] [SerializeField] public float depthSelector;
    
    [SerializeField] DepthVideo videoTexture;

    private void Setup()
    {
        var main = ps.main;
        main.maxParticles = videoTexture.resolution;
        var emission = ps.emission;
        emission.rateOverTime = videoTexture.resolution; 
    }

  

    

    private void Start()
    {
        Setup();

        particles = new ParticleSystem.Particle[videoTexture.resolution];
        ps.Emit(videoTexture.resolution);
        ps.GetParticles(particles);

    }

    private void Update()
    {
        
        for (int index_dst = 0, depth_y = 0; depth_y < videoTexture.height_depth; depth_y++)
        {
            for (int depth_x = 0; depth_x < videoTexture.width_depth; depth_x++, index_dst++)
            {



                
                //Mesh
                float depth = videoTexture.colorPixels[index_dst].g * pointDepth;
                particles[index_dst].position = new Vector3(depth_x, depth_y, depth);
                
                
                
                if(videoTexture.colorPixels[index_dst].g > depthSelector)
                {
                    
                    particles[index_dst].startColor = videoTexture.colorPixels[index_dst];
                   
                }

                else
                {   
                    
                    particles[index_dst].startColor = new Color(videoTexture.colorPixels[index_dst].r, videoTexture.colorPixels[index_dst].g, videoTexture.colorPixels[index_dst].b, 0);
                }

                
                





            }
        }
        
        
        ps.SetParticles(particles, videoTexture.resolution);
        
    }

}