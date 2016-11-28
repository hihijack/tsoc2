Shader "Custom/FadeInOut" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Base", 2D) = "white" {}
		 
	}
	SubShader 
	{
		Tags { "Queue" = "Transparent" } 
		Pass
		{
			//ZWrite Off // don't write to depth buffer 
            // in order not to occlude other objects

			Blend SrcAlpha OneMinusSrcAlpha // use alpha blending

			CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag

					uniform float4 _Color;
					uniform sampler2D _MainTex;

					void vert(float4 pos:POSITION, float2 texcoord:TEXCOORD0,
							   out float4 oPos:POSITION, out float4 oColor:COLOR, out float2 oTexCoord : TEXCOORD0)
					{
			
						oPos = mul(UNITY_MATRIX_MVP, pos);
						oColor = _Color;
						
						oTexCoord = texcoord;
					}

					void frag(float4 color:COLOR, float2 texcoord:TEXCOORD0, out float4 oColor:COLOR)
					{
						oColor = tex2D(_MainTex, texcoord) * _Color;
						oColor.w = smoothstep(1, 0.5, _Time.y);
					}

			ENDCG
		}	
	} 
}
	
