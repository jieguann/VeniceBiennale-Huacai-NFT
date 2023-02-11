// Original reference: https://www.shadertoy.com/view/MdlBDn

Shader "Basic Feedback"
{
	Properties 
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        //_Color ("Color (RGBA)", Color) = (1, 1, 1, 1) // add _Color property
		_Transparency ("Transparency", Range(0.0, 0.5 )) = 0.25

		COL ("Color (RGBA)", Color) = (1,1,1,1)
		//_alpha ("ALPHA", Float) = COL.a
		//_alpha ("ALPHA", Float) = 0.5 
    }

	SubShader
	{ 

		
//-------------------------------------------------------------------------------------------
	
		CGINCLUDE
		#pragma vertex VSMain
		#pragma fragment PSMain

		sampler2D _BufferA, _Video;	

		float4 VSMain (in float4 vertex:POSITION, inout float2 uv:TEXCOORD0) : SV_POSITION
		{
			return UnityObjectToClipPos(vertex);
		}

		ENDCG

//-------------------------------------------------------------------------------------------
		
		Pass
		{

			CGPROGRAM
			
			void PSMain (float4 vertex:SV_POSITION, float2 uv:TEXCOORD0, out float4 fragColor:SV_TARGET)
			{
				float2 tc = uv;
				float2 uv0 = tc;  
				uv0 *= 0.999;   
				float4 sum = tex2D(_BufferA, uv0);
				float4 src = tex2D(_Video, tc); 
				sum.rgb = lerp(sum.rbg, src.rgb, 0.03);
				fragColor = sum;
				//return sum;
				
			}
			
			ENDCG

	//Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

	//Tags {"Queue"="Transparent" "RenderType"="Transparent"}

	//ZWrite Off

    //Cull Off

	//Blend SrcAlpha OneMinusSrcAlpha		// Traditional transparency
	//Blend OneMinusDstColor One			// Soft additive

	//Blend One One							// Additive

    //Blend One OneMinusSrcAlpha			// Premultiplied transparency

		}



//-------------------------------------------------------------------------------------------


		Pass
		{


			CGPROGRAM

			//float _Transparency;
			//float _alpha;
			
			void PSMain (float4 vertex:SV_POSITION, float2 uv:TEXCOORD0, out float4 fragColor:SV_TARGET)
			{
				fragColor = tex2D(_BufferA, uv);

				//float3 col = fragColor.rgb

				//fragColor.a = 1.0 - col.r ; // make white transparent

				//fragColor.a = _alpha ;

				//fragColor = float4(col, _alpha);
				
				
			}
			
			ENDCG


		}

//-------------------------------------------------------------------------------------------
	}
}