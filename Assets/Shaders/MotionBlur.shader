Shader "Hidden/MotionBlur" {
	Properties {
		_MainTex ("Source (RGBA)", 2D) = "white" {}
		_VelocityTex("Velocity Field(RG)", 2D) = "black" {}
		_AspectRatio("Camera aspect ratio", float) = 1.0
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		
		 Pass {
		 	Blend One Zero
		 	CGPROGRAM
		 	#pragma vertex vert_img
		 	#pragma fragment frag
		 	#pragma fragmentoption ARB_precision_hint_fastest
		 	#pragma exclude_renderers flash
		 	#pragma target 5.0
		 	#include "UnityCG.cginc"
		 	
		 	uniform sampler2D _MainTex;
		 	uniform sampler2D _VelocityTex;
		 	uniform float _VelocityScale;
		 	
		 	#define MAX_SAMPLES 10
		 	
		 	uniform float2 _MainTex_TexelSize;
		 	
		 	float4 motionBlur(float2 texCoord) {
		 		float2 velocityUV = texCoord;
		 		float4 v = tex2D(_VelocityTex, velocityUV);
		 		float2 velocity = float2(DecodeFloatRG(v.rg), DecodeFloatRG(v.ba));
		 		
		 		// current fps / target fps
				//velocity *= uMotionScale;
				
				
				//	compute number of blur samples to take:
				float2 texelSize = 1.0 / _MainTex_TexelSize;;
				float speed = length(velocity / texelSize);
				int nSamples = clamp(speed, 1, MAX_SAMPLES);
				float4 result = float4(0.0, 0.0, 0.0, 0.0);
				
//				This approach samples at even intervals along the velocity direction. This
//				ensures the proper amount of blur, but causes a degredation in blur 
//				quality (banding aretfacts) when when speed > uMaxMotionBlurSamples.
				result = tex2D(_MainTex, texCoord);
				for (int j = 1; j < nSamples; ++j) {
					float2 offset = velocity * (float(j) / float(nSamples - 1) - 0.5);
					result += tex2D(_MainTex, texCoord + offset);
				}
			
				result /= float(nSamples);
				
				return result;
		 	}
		 	
		 	float4 frag(v2f_img i) : COLOR {
		 		float4 outCol = float4(0.0, 0.0, 0.0, 0.0);
		 		float2 velocityUV = i.uv;
		 		
		 		
		 		
		 		#if SHADER_API_D3D9
		 		if(_MainTex_TexelSize.y < 0) {
		 			velocityUV.y = 1.0 - velocityUV.y;
		 		}
		 		#endif
		 		
		 		float4 v2 = tex2D(_VelocityTex, velocityUV);
		 		float2 v = float2(DecodeFloatRG(v2.rg), DecodeFloatRG(v2.ba));
		 		//v *= _VelocityScale;
		 		
		 		v = (v - 0.5) * 4.0;
		 		
		 		#if SHADER_API_D3D9
		 		if(_MainTex_TexelSize.y >= 0) {
		 			v.y = -v.y;
		 		}
		 		#endif
		 		
		 		float speed = length(v / _MainTex_TexelSize);
		 		float samples = clamp(speed, 1, MAX_SAMPLES);
		 		
		 		for(int j = 0; j <= MAX_SAMPLES; j++) {
		 			//float2 offset = v * (float(j) / (MAX_SAMPLES - 1) - 0.5);
		 			float stepIntensity = 1.0 / (MAX_SAMPLES + 1);
		 			float stepLength = (0.5 / MAX_SAMPLES);
		 			float2 offset = v*j*stepLength;
		 			float4 stepSample = tex2D(_MainTex, i.uv + offset);
		 			
		 			outCol += stepSample * stepIntensity;
		 		}
		 		
		 		return outCol;// motionBlur(i.uv);
		 	}
		 	ENDCG
		 } 
	} 
	FallBack Off
}

