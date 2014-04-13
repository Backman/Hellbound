Shader "Hidden/Motion Vectors" {
	SubShader {
		Tags { "RenderType" = "Moving" }
		Pass {
			Fog { Mode Off }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_fog_exp2
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			
			struct appdata_simple {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				float4 curPos : TEXCOORD0;
				float4 prevPos : TEXCOORD1;
				float3 normal : TEXCOORD2;
				float4 col : COLOR;
			};
			
			uniform float4x4 _ModelView;
			uniform float4x4 _prevModelView;
			uniform float4x4 _prevModelViewProjection;
			uniform float4x4 _ModelViewInverseTranspose;
			
			v2f vert(appdata_simple v) {
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
//				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
//				o.curPos = o.pos;
//				o.prevPos = mul(_prevModelViewProjection, v.vertex);
				
				float4 currPos = mul(_ModelView, v.vertex);
				float4 prevPos = mul(_prevModelView, v.vertex);
				
				float3 N = (float3)mul(_ModelViewInverseTranspose, float4(v.normal, 1.0));
				
				float3 eyeMotion = currPos.xyz - prevPos.xyz;
				
				float dotMN = dot(eyeMotion, N);
				float4 posStretch = dotMN > 0 ? currPos : prevPos;
				
				o.pos = posStretch;
				
				currPos.xyz = currPos.xyz / currPos.w;
				prevPos.xyz = prevPos.xyz / prevPos.w;
				
				float2 screenMotion = currPos.xy - prevPos.xy;
				
				screenMotion.xy = screenMotion.xy * 0.25 + 0.5;
				
				o.col = float4(EncodeFloatRG(screenMotion.x), EncodeFloatRG(screenMotion.y));
				
				return o;
			}
			
			float4 frag(v2f i) : COLOR {
			
//				float3 eyeMotion = i.curPos.xyz - i.prevPos.xyz;
//				
//				float dotMN = dot(eyeMotion, i.normal);
//				float4 posStretch = dotMN > 0 ? i.curPos : i.prevPos;
//				
//				float4 pos = posStretch;
//				
//				i.curPos.xyz = i.curPos.xyz / i.curPos.w;
//				i.prevPos.xyz = i.prevPos.xyz / i.prevPos.w;
//				
//				float2 screenMotion = i.curPos.xy - i.prevPos.xy;
//				
//				screenMotion.xy = screenMotion.xy * 0.25 + 0.5;
//				
//				float4 col = float4(EncodeFloatRG(screenMotion.x), EncodeFloatRG(screenMotion.y));
				
//				float2 a = (i.curPos.xy / i.curPos.w ) * 0.5 + 0.5;
//				float2 b = (i.prevPos.xy / i.prevPos.w) * 0.5 + 0.5;
//				float2 velocity = a - b;
				return i.col;
			}
			ENDCG
		}
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		Pass {
			Fog { Mode Off }
			Color (0.4980392, 0.5, 0.4980392, 0.5)
		}
	}
}

