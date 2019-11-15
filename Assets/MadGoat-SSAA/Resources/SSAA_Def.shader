Shader "Hidden/SSAA_Def" 
{
	Properties 
	{
		_MainTex ("Texture", 2D) = "" {} 
	}
	SubShader {
		
		Pass {
 			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			// include the unityCG.cginc
#include "UnityCG.cginc"

			#include "SSAA_Utils.cginc"

			#pragma vertex vert_img
			#pragma fragment frag

			sampler2D _MainTex;

			fixed4 frag(v2f i) : COLOR
			{
				return tex2D(_MainTex, i.texcoord).rgba;
			}
			ENDCG 

		}
	}
	Fallback Off 
}
