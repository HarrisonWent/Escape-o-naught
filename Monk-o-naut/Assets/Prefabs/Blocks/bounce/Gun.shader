Shader "Custom/Gun"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)        
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

        _OcclusionTex("Occlusion (RGB)", 2D) = "white" {}
        _OcclusionStrength("Occlusion Strength", Range(0,1)) = 1.0

        _NormalMap("NormalMap", 2D) = "bump" {}
        //_DetailNormalMap("DetailNormalMap", 2D) = "bump" {}

        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.25

        _RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
        _RimPower("Rim Power", Range(0.5,8.0)) = 3.0

    }
    SubShader
        {

            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Standard fullforwardshadows

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0

            sampler2D _MainTex;
            sampler2D _SpecularTex;
            sampler2D _OcclusionTex;
            sampler2D _NormalMap;
            sampler2D _DetailNormalMap;

            struct Input
            {
                float2 uv_MainTex;

                float2 uv_SpecularTex;

                float2 uv_OcclusionTex;

                float2 uv_NormalMap;
                float2 uv_DetailNormalMap;

                float3 viewDir;                
            };

            half _Glossiness;
            half _Metallic;
            fixed4 _Color;
            float4 _RimColor;
            float _RimPower;
            float _OcclusionStrength;

            // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
            // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
            // #pragma instancing_options assumeuniformscaling
            UNITY_INSTANCING_BUFFER_START(Props)
                // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                // Albedo comes from a texture tinted by color
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

                fixed3 Occ = tex2D(_OcclusionTex, IN.uv_OcclusionTex);
                o.Albedo = c.rgb ;
                o.Occlusion = lerp(1.0, Occ.rgb, _OcclusionStrength);
                
           /*     o.Normal =
                    UnpackNormal(
                        tex2D(_NormalMap, IN.uv_NormalMap).rgba
                        + tex2D(_DetailNormalMap, IN.uv_DetailNormalMap).rgba
                    );*/
                o.Normal =
                    UnpackNormal(
                        tex2D(_NormalMap, IN.uv_NormalMap));

                // Metallic and smoothness come from slider variables
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = c.a;

                half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
                o.Emission = _RimColor.rgb * pow(rim, _RimPower);
            }
            ENDCG
        }
    FallBack "Diffuse"
}
