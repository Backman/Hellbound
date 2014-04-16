Shader "Tessellation/Edge Based Diffuse" {
	Properties {
		_EdgeLength("Edge length", Range(2.0, 50.0)) = 15.0
		_Color("Color", Color) = (1.0, 0.0, 0.0, 1.0)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DispTex("Displacement Texture", 2D) = "white" {}
		_NormalTex("Normal Texture", 2D) = "bump" {}
		_Displacement("Displacement", Range(0.0, 10.0)) = 0.3
		_SpecColor ("Spec color", color) = (0.5,0.5,0.5,0.5)
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 300
		
		CGPROGRAM
		#pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessEdge nolightmap
		#pragma target 5.0
		#include "Tessellation.cginc"
		
		struct appdata {
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};
		
		struct Input {
			float2 uv_MainTex;
		};
		
		
		float _EdgeLength;
		
		float4 tessEdge(appdata v0, appdata v1, appdata v2) {
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		sampler2D _MainTex;
		sampler2D _DispTex;
		sampler2D _NormalTex;
		float _Displacement;
		float4 _Color;
		
		void disp(inout appdata v) {
			float d = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0.0, 0.0)).r * _Displacement * 0.01;
			v.vertex.xyz += v.normal * d;
		}


		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Specular = 0.2;
			o.Gloss = 1.0;
			o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_MainTex));
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
