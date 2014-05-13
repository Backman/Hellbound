Shader "Lux/Bumped Diffuse Tessellation" {

Properties {
	_Color ("Diffuse Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_DiffCubeIBL ("Custom Diffuse Cube", Cube) = "black" {}
	_DispTex("Displacement Texture", 2D) = "white" {}
	_Displacement("Displacement", Range(0.0, 100.0)) = 0.3
	_EdgeLength("Edge length", Range(2.0, 50.0)) = 15.0
	_Tess("Tessellation", Range(1.0, 64.0)) = 4.0
	[HideInInspector] _AO ("Ambient Occlusion Alpha (A)", 2D) = "white" {}
}

SubShader { 
	Tags { "RenderType"="Opaque" }
	LOD 400
	
	CGPROGRAM
	#pragma surface surf Lambert addshadow fullforwardshadows vertex:disp tessellate:tessDistance nolightmap
	#pragma glsl
	#pragma target 5.0
	// we use the default lighting functions
	
	#pragma multi_compile LUX_LINEAR LUX_GAMMA
	#pragma multi_compile DIFFCUBE_ON DIFFCUBE_OFF
	#pragma multi_compile LUX_AO_OFF LUX_AO_ON
 	
 	#include "Tessellation.cginc"
	
	#define LUX_DIFFUSE

	sampler2D _DispTex;
	float _Displacement;
	float _Tess;
	float4 _Color;
	sampler2D _MainTex;
	sampler2D _BumpMap;
	#ifdef DIFFCUBE_ON
		samplerCUBE _DiffCubeIBL;
	#endif
	#ifdef LUX_AO_ON
		sampler2D _AO;
	#endif
	
	// Is set by script
	float4 ExposureIBL;

	struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
		#ifdef LUX_AO_ON
			float2 uv_AO;
		#endif
		float3 viewDir;
		float3 worldNormal;
		INTERNAL_DATA
	};
	
	struct appdata {
		float4 vertex : POSITION;
		float4 tangent : TANGENT;
		float3 normal : NORMAL;
		float2 texcoord : TEXCOORD0;	
	};
	
	float _EdgeLength;
		
	float4 tessEdge(appdata v0, appdata v1, appdata v2) {
		return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
	}
	
	float4 tessDistance(appdata v0, appdata v1, appdata v2) {
		float minDist = 10.0;
		float maxDist = 25.0;
		return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
	}
	
	void disp(inout appdata v) {
		float d = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0.0, 0.0)).r * _Displacement * 0.01;
		v.vertex.xyz += v.normal * d;
	}

	void surf (Input IN, inout SurfaceOutput o) {
		fixed4 diff_albedo = tex2D(_MainTex, IN.uv_MainTex);
		// Diffuse Albedo
		o.Albedo = diff_albedo.rgb * _Color.rgb;
		o.Alpha = diff_albedo.a * _Color.a;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		
		#include "LuxCore/LuxLightingAmbient.cginc"
		
	}
ENDCG
}
FallBack "Diffuse"
CustomEditor "LuxMaterialInspector"
}
