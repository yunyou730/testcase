Shader "ayy/ClipColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor("Main Color",Color) = (1,1,1,1)
        _TestX("Test X",Range(0,1)) = 0.5
        
        _BoundsMin("Bounds Min",Vector) = (0,0,0,0)
        _BoundsMax("Bounds Max",Vector) = (0,0,0,0)
        
        
    }
    SubShader
    {
        Tags 
        {
            "RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }
        // Cull Off 
        // ZTest Off 
        // ZWrite Off        
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            struct Attributes
            {
                float4 positionOS : POSITION;   // OS:Object Space
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionHCS :SV_POSITION;    // HCS: Homogeneous Clipping Space
                float2 uv : TEXCOORD0;
                float4 positionOS : TEXCOORD1;
            };

        CBUFFER_START(UnityPerMaterial)
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            float4 _MainColor;
            float _TestX;


            float4 _BoundsMin;
            float4 _BoundsMax;
        CBUFFER_END            

            Varyings vert(Attributes IN)
            {
                float3 localPos = IN.positionOS;
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(localPos);
                OUT.uv = IN.uv;
                OUT.positionOS = float4(localPos,1.0);
                return OUT;
            }
            
            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                float4 col = _MainColor;//tex2D(_MainTex,uv);
                
                float4 testPos = IN.positionOS;

                float len = _BoundsMax.x - _BoundsMin.x;
                float pct = (testPos.x - _BoundsMin.x) / len;

                col = _MainColor;
                if(pct > _TestX)
                {
                    col = float4(1,0,1,1);
                }

                return col;
                //return float4(pct,pct,pct,1.0);
            }
            ENDHLSL
        }
    }
}
