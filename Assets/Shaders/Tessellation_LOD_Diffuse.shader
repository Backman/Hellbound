Shader "Tessellation/Distance Based Diffuse" {
	Properties {
		_Tess("Tessellation", Range(1.0, 32.0)) = 4.0
		_Color("Main Color", Color) = (1.0, 0.0, 0.0, 1.0)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DispTex("Displacement Texture", 2D) = "white" {}
		_NormalTex("Normal Texture", 2D) = "bump" {}
		_Displacement("Displacement", Range(0.0, 0.1)) = 0.05
		_SpecColor ("Specular Color", color) = (0.5,0.5,0.5,0.5)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 300
		
		CGPROGRAM
		#pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessDistance nolightmap
		#pragma target 5.0
		#include "Tessellation.cginc"

		struct Input {
			float2 uv_MainTex;
		};
		
		struct appdata {
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;	
		};
		
		sampler2D _MainTex;
		sampler2D _DispTex;
		sampler2D _NormalTex;
		float4 _Color;
		float _Tess;
		float _Displacement;
		
		float4 tessDistance(appdata v0, appdata v1, appdata v2) {
			float minDist = 10.0;
			float maxDist = 25.0;
			return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
		}
		
		void disp(inout appdata v) {
			float d = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0.0, 0.0)).r * _Displacement;
			v.vertex.xyz += v.normal * d;
		}
		
		


		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Specular = 0.2;
			o.Gloss = 1.0;
			o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_MainTex));
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
