using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InteractionManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private ParticleController psController;
    [Header("Distance Control")]
    //Distance between User and Particles
    [SerializeField] private Transform userPosition;
    [SerializeField] private Transform particlePosition;
    [SerializeField] private float distanceAR;
    [SerializeField] private float initDistanceAR;

    [Header("Particle Width")]
    [SerializeField] private float minUser = 2;
    [SerializeField] private float maxUser = 10;
    [SerializeField] private float minParticleWidth = 0.1f;
    [SerializeField] private float maxParticleWidth = 1f;
    public float oututWidth;
    [Header("Object Movement")]
    [SerializeField] private Camera graCamera;
    [SerializeField] private float distanceForMoveAway = 5f;
    [SerializeField] private float timeMoveToward = 10f;
    [SerializeField] private float moveTimer = 0;
    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] private float resetDistance = 0.5f;
    [SerializeField] private float backgroundStartChange = 6f;
    [SerializeField] private bool timeFlag = false;


    


    [Header("Finger Interaction")]
    [SerializeField] private float maxSelector = 0.5f;
    [SerializeField] private float minSelector = 0.1f;
    [SerializeField] private float defaultSelector = 0.1f;
    [SerializeField] private float interactionSpeed = 0.2f;
    //Vetext Displace Mesh
    [SerializeField] private GameObject VetexDisplaceMesh;
    private Material VetexMaterial;


    [Header("Time Controller")]
    [SerializeField] private float timeNow;


    // Start is called before the first frame update
    void Start()
    {
        
        VetexMaterial = VetexDisplaceMesh.GetComponent<Renderer>().material; 
        initDistanceAR = Vector3.Distance(userPosition.position, particlePosition.position);
    }

    // Update is called once per frame
    void Update()
    {
        screenInteraction();
        particleTrailController();
        timeController();
        particleMoveAway();
    }


    
    private void screenInteraction()
    {
        
        if (Input.GetMouseButton(0))

        {   
            //Depth Selector 0.1 - 0.5, default 0.3
            float swipeY = Input.GetAxis("Mouse Y");

            //psController.depthSelector 
            float newDepthSelector = psController.depthSelector + swipeY * interactionSpeed * Time.deltaTime;

            newDepthSelector = Mathf.Clamp(newDepthSelector, minSelector, maxSelector);
            psController.depthSelector = newDepthSelector;

            //Vetex
            
        }

        //VetexMaterial.SetVector("MinMax", new Vector2(-0.2f, -0.2f));

    }

    
    private void particleTrailController()
    {
        distanceAR = Vector3.Distance(userPosition.position, particlePosition.position);
        if (distanceAR < minUser)
        {
            oututWidth = minParticleWidth;
        }
        else if (distanceAR > maxUser)
        {
            oututWidth = maxParticleWidth;
        }
        else
        {
            oututWidth = ExtensionMethods.Remap(distanceAR, minUser, maxUser, minParticleWidth, maxParticleWidth);
        }

        var psTraild = ps.trails;
        psTraild.widthOverTrail = oututWidth;
    }


    private void particleMoveAway()
    {   moveTimer += Time.deltaTime;

        //look at
        particlePosition.LookAt(userPosition);
        var step = moveSpeed * Time.deltaTime;
        if (moveTimer > 10)
        {
            timeFlag = true;
        }

        if (distanceAR < 3f && timeFlag == false)
        {
            
            particlePosition.position  -= particlePosition.forward* step;
            moveTimer = 0;
        }

        if(timeFlag == true)
        {
            particlePosition.position += particlePosition.forward * step;
        }

        if(distanceAR < resetDistance)
        {
            timeFlag = false;
            moveTimer = 0;
            particlePosition.position = userPosition.forward * initDistanceAR;

        }



    
        if (distanceAR > backgroundStartChange)
        {
            graCamera.backgroundColor = new Color(1, 1, 1, 1);
        }
        else
        {
            var colorValue = ExtensionMethods.Remap(distanceAR, backgroundStartChange, resetDistance, 1, 0);
            graCamera.backgroundColor = new Color(colorValue, colorValue, colorValue, colorValue);
        }




    }


    private void timeController()
    {
        float hour = DateTime.Now.Hour;
        float minute = DateTime.Now.Minute;
        float second = DateTime.Now.Second;
        timeNow =  hour * 3600 + minute * 60 + second;
        //Debug.Log(timeNow);
    }
    

}


public static class ExtensionMethods
{
    public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }

}