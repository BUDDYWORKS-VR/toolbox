//2018/10/27/hhotatea
//This Shader is released under the MIT License, see LICENSE.txt.

Shader "BUDDYWORKS/WireFrameNoCamera" {
	Properties {
        _delta ("サンプル間隔",Range(0,2)) = 0.831
        _dist ("精度",Range(0,2)) = 0.001
        _BGC ("背景色", Color) = (1,1,1,1)
        _LineC ("線の色", Color) = (0,0,0,1)
   	}
    SubShader {
        Tags { "Queue"="Transparent+10"	"RenderType"="Transparent" }
        LOD 200 
        //Cull off  //ここを有効にすると板の裏面が有効になります。
        //Ztest always  //ここを有効にすると板より手前もワイヤーフレーム描写されます。
        Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM

            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            uniform float _delta;
            uniform float _dist;
            uniform float4 _BGC;
            uniform float4 _LineC;
            uniform float4x4 _InverseProjection;
			
            sampler2D _CameraDepthTexture;
            float4 _CameraDepthTexture_ST;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
				float4 screenpos : TEXCOORD1;
            };

            v2f vert(appdata_img v)
            {
                v2f o = (v2f)0;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord.xy;
				o.screenpos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float ep = _delta*1.2/_ScreenParams.xy;
                float eq = _delta*1.0/_ScreenParams.xy;
				float2 screenuv = UnityStereoTransformScreenSpaceTex(float2(i.screenpos.x/i.screenpos.w,i.screenpos.y*_ProjectionParams.x/i.screenpos.w) * 0.5f + 0.5f);
                float2 xy1[3];float2 xy2[3];float2 xy3[3];
                float3 n[3];

                xy1[0] = float2(screenuv.x          , screenuv.y          );xy1[1] = float2(screenuv.x + eq     , screenuv.y          );xy1[2] = float2(screenuv.x          , screenuv.y + eq     );
                xy2[0] = float2(screenuv.x      + ep, screenuv.y          );xy2[1] = float2(screenuv.x + eq + ep, screenuv.y          );xy2[2] = float2(screenuv.x      + ep, screenuv.y + eq     );
                xy3[0] = float2(screenuv.x          , screenuv.y      + ep);xy3[1] = float2(screenuv.x + eq     , screenuv.y      + ep);xy3[2] = float2(screenuv.x          , screenuv.y + eq + ep);

                for(int j=0;j<3;j++){

                	float z1 = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,xy1[j]));
                	float z2 = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,xy2[j]));
                	float z3 = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,xy3[j]));

               		float4 pPos1 = float4(xy1[j] , z1, 1.0);
                	float4 p1 = mul(_InverseProjection, pPos1);
                	p1.xyz /= p1.w;

                	float4 pPos2 = float4(xy2[j] , z2, 1.0);
                	float4 p2 = mul(_InverseProjection, pPos2);
                	p2.xyz /= p2.w;

                	float4 pPos3 = float4(xy3[j] , z3, 1.0);
                	float4 p3 = mul(_InverseProjection, pPos3);
                	p3.xyz /= p3.w;

                	n[j] = normalize(cross(normalize(p2.xyz - p1.xyz), normalize(p3.xyz - p1.xyz)));

                }
                float3 one = float3(1, 1, 1);
				float w = dot(one, abs(n[1] - n[0])) + dot(one, abs(n[2] - n[0]));
				float4 col = _BGC;
                if(w>_dist){col = _LineC;}
				
                return col;
            }
            ENDCG
        }
    }
    //FallBack "Diffuse"
}