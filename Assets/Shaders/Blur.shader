Shader "Hidden/Blur" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		
	 	
		 Pass {
		 	Blend One Zero
		 	CGPROGRAM
		 	#pragma vertex vert_img
		 	#pragma fragment frag
		 	#pragma fragmentoption ARB_precision_hint_fastest
		 	#pragma target 3.0
		 	#include "UnityCG.cginc"
		 	
		 	uniform sampler2D _MainTex;
		 	uniform float _BlurSize;
			 	
		 	float4 frag(v2f_img i) : COLOR {
		 		float4 sum = float4(0.0, 0.0, 0.0, 0.0);
		 		float2 vTexCoord = i.uv;
		 		
				sum += tex2D(_MainTex, float2(vTexCoord.x - 4.0*_BlurSize, vTexCoord.y)) * 0.05;
				sum += tex2D(_MainTex, float2(vTexCoord.x - 3.0*_BlurSize, vTexCoord.y)) * 0.09;
				sum += tex2D(_MainTex, float2(vTexCoord.x - 2.0*_BlurSize, vTexCoord.y)) * 0.12;
				sum += tex2D(_MainTex, float2(vTexCoord.x - _BlurSize, vTexCoord.y)) * 0.15;
				sum += tex2D(_MainTex, float2(vTexCoord.x, vTexCoord.y)) * 0.16;
				sum += tex2D(_MainTex, float2(vTexCoord.x + _BlurSize, vTexCoord.y)) * 0.15;
				sum += tex2D(_MainTex, float2(vTexCoord.x + 2.0*_BlurSize, vTexCoord.y)) * 0.12;
				sum += tex2D(_MainTex, float2(vTexCoord.x + 3.0*_BlurSize, vTexCoord.y)) * 0.09;
				sum += tex2D(_MainTex, float2(vTexCoord.x + 4.0*_BlurSize, vTexCoord.y)) * 0.05;
		 	
		 		return sum;
		 	}
		 	
		 	ENDCG
		 }
		 Pass {
		 	Blend One Zero
		 	CGPROGRAM
		 	#pragma vertex vert_img
		 	#pragma fragment frag
		 	#pragma fragmentoption ARB_precision_hint_fastest
		 	#pragma target 3.0
		 	#include "UnityCG.cginc"
		 	
		 	uniform sampler2D _MainTex;
		 	uniform float _BlurSize;
		 	
		 	float4 frag(v2f_img i) : COLOR {
		 		float4 sum = float4(0.0, 0.0, 0.0, 0.0);
		 		float2 vTexCoord = i.uv;
		 		
				sum += tex2D(_MainTex, float2(vTexCoord.x, vTexCoord.y - 4.0*_BlurSize)) * 0.05;
				sum += tex2D(_MainTex, float2(vTexCoord.x, vTexCoord.y - 3.0*_BlurSize)) * 0.09;
				sum += tex2D(_MainTex, float2(vTexCoord.x, vTexCoord.y - 2.0*_BlurSize)) * 0.12;
				sum += tex2D(_MainTex, float2(vTexCoord.x, vTexCoord.y - _BlurSize)) * 0.15;
				sum += tex2D(_MainTex, float2(vTexCoord.x, vTexCoord.y)) * 0.16;
				sum += tex2D(_MainTex, float2(vTexCoord.x, vTexCoord.y + _BlurSize)) * 0.15;
				sum += tex2D(_MainTex, float2(vTexCoord.x, vTexCoord.y + 2.0*_BlurSize)) * 0.12;
				sum += tex2D(_MainTex, float2(vTexCoord.x, vTexCoord.y + 3.0*_BlurSize)) * 0.09;
				sum += tex2D(_MainTex, float2(vTexCoord.x, vTexCoord.y + 4.0*_BlurSize)) * 0.05;
		 		
		 		return sum;
		 	}
		 	
		 	ENDCG
		 }
	} 
	FallBack Off
}
