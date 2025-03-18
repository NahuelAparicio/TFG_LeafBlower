Shader "Shader Graphs/E4-Compiled"
{
    Properties
    {
        [KeywordEnum(Disabled, Voronoi, Normals, Depth, Distortion)]_DEBUG("Debug", Float) = 0
        _Foam_Level("_Foam_Level", Float) = 10
        _Foam_Contrast("_Foam_Contrast", Float) = 5
        _Foam_CoastDepth("_Foam_CoastDepth", Float) = 2
        _Foam_Distortion_Scale("_Foam_Distortion_Scale", Float) = 2
        _Foam_Distortion_Amount("_Foam_Distortion_Amount", Float) = 0.25
        [NoScaleOffset]_NormalMap("_NormalMap", 2D) = "white" {}
        _Normal_Scale("_Normal_Scale", Float) = 10
        _Normal_Strength("_Normal_Strength", Float) = 1
        _Normal_Animation_Speed("_Normal_Animation_Speed", Float) = 0.1
        _Normal_Animation_Radius("_Normal_Animation_Radius", Float) = 5
        _Distortion_Amount("_Distortion_Amount", Float) = 0.1
        _Depth_Range("_Depth_Range", Float) = 5
        _Depth_Tint("_Depth_Tint", Color) = (0, 1, 0.6674938, 0)
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalLitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
        // Render State
        Cull Back
        Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
        #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ SHADOWS_SHADOWMASK
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ _LIGHT_LAYERS
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _LIGHT_COOKIES
        #pragma multi_compile _ _FORWARD_PLUS
        #pragma shader_feature_local _DEBUG_DISABLED _DEBUG_VORONOI _DEBUG_NORMALS _DEBUG_DEPTH _DEBUG_DISTORTION
        
        #if defined(_DEBUG_DISABLED)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_DEBUG_VORONOI)
            #define KEYWORD_PERMUTATION_1
        #elif defined(_DEBUG_NORMALS)
            #define KEYWORD_PERMUTATION_2
        #elif defined(_DEBUG_DEPTH)
            #define KEYWORD_PERMUTATION_3
        #else
            #define KEYWORD_PERMUTATION_4
        #endif
        
        
        // Defines
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMALMAP 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMAL_DROPOFF_TS 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_NORMAL
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TANGENT
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TEXCOORD1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TEXCOORD2
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_POSITION_WS
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_NORMAL_WS
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_TANGENT_WS
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_SHADOW_COORD
        #endif
        
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_FORWARD
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _ALPHAPREMULTIPLY_ON 1
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define REQUIRE_DEPTH_TEXTURE
        #endif
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define REQUIRE_OPAQUE_TEXTURE
        #endif
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentOS : TANGENT;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 uv1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 uv2 : TEXCOORD2;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentWS;
            #endif
            #if defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 staticLightmapUV;
            #endif
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 dynamicLightmapUV;
            #endif
            #endif
            #if !defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 sh;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 fogFactorAndVertexLight;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 shadowCoord;
            #endif
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 TangentSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 WorldSpacePosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 ScreenPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 NDCPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 PixelPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 TimeParameters;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 staticLightmapUV : INTERP0;
            #endif
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 dynamicLightmapUV : INTERP1;
            #endif
            #endif
            #if !defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 sh : INTERP2;
            #endif
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 shadowCoord : INTERP3;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentWS : INTERP4;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 fogFactorAndVertexLight : INTERP5;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionWS : INTERP6;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalWS : INTERP7;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS.xyzw = input.tangentWS;
            output.fogFactorAndVertexLight.xyzw = input.fogFactorAndVertexLight;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS = input.tangentWS.xyzw;
            output.fogFactorAndVertexLight = input.fogFactorAndVertexLight.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Foam_Level;
        float _Foam_Contrast;
        float _Foam_Distortion_Amount;
        float _Normal_Scale;
        float _Normal_Strength;
        float _Normal_Animation_Speed;
        float _Normal_Animation_Radius;
        float _Foam_Distortion_Scale;
        float4 _NormalMap_TexelSize;
        float _Distortion_Amount;
        float _Depth_Range;
        float4 _Depth_Tint;
        float _Foam_CoastDepth;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_NormalMap);
        SAMPLER(sampler_NormalMap);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            if (unity_OrthoParams.w == 1.0)
            {
                Out = LinearEyeDepth(ComputeWorldSpacePosition(UV.xy, SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), UNITY_MATRIX_I_VP), UNITY_MATRIX_V);
            }
            else
            {
                Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
            }
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Lerp_float(float A, float B, float T, out float Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Cosine_float(float In, out float Out)
        {
            Out = cos(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
        {
            float x; Hash_Tchou_2_1_float(p, x);
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_Deterministic_float (float2 UV, float3 Scale, out float Out)
        {
            float2 p = UV * Scale.xy;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_NormalBlend_float(float3 A, float3 B, out float3 Out)
        {
            Out = SafeNormalize(float3(A.rg + B.rg, A.b * B.b));
        }
        
        void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
        {
            Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
        }
        
        void Vn2_float(float2 UUU, float AAA, float CCC, out float3 Out){
            float2 n = floor(UUU * CCC);
            
            float2 f = frac(UUU * CCC);
            
            float2 mg, mr;
            
            float md = 8.0;
            
            for (int j= -1; j <= 1; j++) {
            
                for (int i= -1; i <= 1; i++) {
            
                    float2 g = float2(i,j);
            
                    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
            
                    float2 UUUr = frac(sin(mul(g+n, m)) * 46839.32);
            
                    float2 o = float2(sin(UUUr.y*+AAA)*0.5+0.5, cos(UUUr.x*AAA)*0.5+0.5);
            
                    float2 r = g + o - f;
            
                    float d = dot(r,r);
            
                    if( d<md ) {
            
                        md = d;
            
                        mr = r;
            
                        mg = g;
            
                    }
            
                }
            
            }
            
            md = 8.0;
            
            for (int j= -2; j <= 2; j++) {
            
                for (int i= -2; i <= 2; i++) {
            
                    float2 g = mg + float2(i,j);
            
                    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
            
                    float2 UUUr = frac(sin(mul(g+n, m)) * 46839.32);
            
                    float2 o = float2(sin(UUUr.y*+AAA)*0.5+0.5, cos(UUUr.x*AAA)*0.5+0.5);
            
                    float2 r = g + o - f;
            
                    if ( dot(mr-r,mr-r)>0.0001 ) {
            
                        md = min(md, dot( 0.5*(mr+r), normalize(r-mr) ));
            
                    }
            
                }
            
            }
            
            Out = float3(md, mr.x, mr.y);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }
        
        void Unity_SceneColor_float(float4 UV, out float3 Out)
        {
            Out = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV.xy);
        }
        
        void Unity_Fraction_float(float In, out float Out)
        {
            Out = frac(In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalTS;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _Property_8481a4e438174b5b943f009dcab1630a_Out_0_Vector4 = _Depth_Tint;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float;
            Unity_SceneDepth_Eye_float(float4(IN.NDCPosition.xy, 0, 0), _SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4 = IN.ScreenPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_27928104217f4cf29cc2851087a2bcc9_R_1_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[0];
            float _Split_27928104217f4cf29cc2851087a2bcc9_G_2_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[1];
            float _Split_27928104217f4cf29cc2851087a2bcc9_B_3_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[2];
            float _Split_27928104217f4cf29cc2851087a2bcc9_A_4_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[3];
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float;
            Unity_Subtract_float(_SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float, _Split_27928104217f4cf29cc2851087a2bcc9_A_4_Float, _Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_2e82df2008704b02b95cfeed46b708fd_Out_0_Float = _Depth_Range;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Divide_19350d6e18284bffa24f4e82c95a8cc5_Out_2_Float;
            Unity_Divide_float(_Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float, _Property_2e82df2008704b02b95cfeed46b708fd_Out_0_Float, _Divide_19350d6e18284bffa24f4e82c95a8cc5_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float;
            Unity_Saturate_float(_Divide_19350d6e18284bffa24f4e82c95a8cc5_Out_2_Float, _Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _Lerp_8258b4c434624b60ad7eb9d848bb2bdb_Out_3_Vector4;
            Unity_Lerp_float4(float4(1, 1, 1, 1), _Property_8481a4e438174b5b943f009dcab1630a_Out_0_Vector4, (_Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float.xxxx), _Lerp_8258b4c434624b60ad7eb9d848bb2bdb_Out_3_Vector4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _ScreenPosition_a770134b04f8438682b38bf27300d909_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_112817f59e3241ecaf97d8dc07514527_Out_0_Float = _Distortion_Amount;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Lerp_b9b68aeec90e4537ba8c1221829d7af7_Out_3_Float;
            Unity_Lerp_float(0, _Property_112817f59e3241ecaf97d8dc07514527_Out_0_Float, _Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float, _Lerp_b9b68aeec90e4537ba8c1221829d7af7_Out_3_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            UnityTexture2D _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_NormalMap);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_838caa5fb6ba47cf8667c312c46234cc_R_1_Float = IN.WorldSpacePosition[0];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_G_2_Float = IN.WorldSpacePosition[1];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_B_3_Float = IN.WorldSpacePosition[2];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_A_4_Float = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2 = float2(_Split_838caa5fb6ba47cf8667c312c46234cc_R_1_Float, _Split_838caa5fb6ba47cf8667c312c46234cc_B_3_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_4bca0ae870964b80b1bf4c071a697345_Out_0_Float = _Normal_Animation_Speed;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_4bca0ae870964b80b1bf4c071a697345_Out_0_Float, _Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float;
            Unity_Sine_float(_Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float, _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float;
            Unity_Cosine_float(_Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float, _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_95a666fa09584b48aa74d064413c240b_Out_0_Vector2 = float2(_Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float, _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float = _Normal_Animation_Radius;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_95a666fa09584b48aa74d064413c240b_Out_0_Vector2, (_Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float.xx), _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2;
            Unity_Add_float2(_Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2, _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_192738b7305d464686b05d10d805ec42_Out_0_Float = _Foam_Distortion_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float;
            Unity_GradientNoise_Deterministic_float(_Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2, _Property_192738b7305d464686b05d10d805ec42_Out_0_Float, _GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_0d932574909346008e46e04609249679_Out_2_Float;
            Unity_Multiply_float_float(_Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float, -1, _Multiply_0d932574909346008e46e04609249679_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_8e409cac0d764a54a4a08573745f68fb_Out_0_Vector2 = float2(_Multiply_0d932574909346008e46e04609249679_Out_2_Float, _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_8e409cac0d764a54a4a08573745f68fb_Out_0_Vector2, (_Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float.xx), _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2;
            Unity_Add_float2(_Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2, _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float;
            Unity_Multiply_float_float(_Property_192738b7305d464686b05d10d805ec42_Out_0_Float, -1, _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float;
            Unity_GradientNoise_Deterministic_float(_Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2, _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float, _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_f26d89dcec114fbf9e11848aad395931_Out_0_Vector2 = float2(_GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float, _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_e8d3912e544a4425a4ccca0e0ec8c94c_Out_0_Float = _Foam_Distortion_Amount;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_f26d89dcec114fbf9e11848aad395931_Out_0_Vector2, (_Property_e8d3912e544a4425a4ccca0e0ec8c94c_Out_0_Float.xx), _Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2;
            Unity_Add_float2(_Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2, _Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2;
            Unity_Add_float2(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2, _Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float = _Normal_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2, (_Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float.xx), _Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.tex, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.samplerstate, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.GetTransformedUV(_Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2) );
            _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.rgb = UnpackNormal(_SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4);
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_R_4_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.r;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_G_5_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.g;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_B_6_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.b;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_A_7_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.a;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2;
            Unity_Add_float2(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2, _Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float;
            Unity_Multiply_float_float(_Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float, -1, _Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2, (_Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float.xx), _Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.tex, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.samplerstate, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.GetTransformedUV(_Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2) );
            _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.rgb = UnpackNormal(_SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4);
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_R_4_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.r;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_G_5_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.g;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_B_6_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.b;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_A_7_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.a;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3;
            Unity_NormalBlend_float((_SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.xyz), (_SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.xyz), _NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_35bdb1ec23fb4ac9beccb588d0540492_Out_0_Float = _Normal_Strength;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3;
            Unity_NormalStrength_float(_NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3, _Property_35bdb1ec23fb4ac9beccb588d0540492_Out_0_Float, _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_f0e277cb76964569ac23c9290e673186_Out_0_Float = _Foam_CoastDepth;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float;
            Unity_Divide_float(_Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float, _Property_f0e277cb76964569ac23c9290e673186_Out_0_Float, _Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float;
            Unity_Saturate_float(_Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float, _Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3;
            Vn2_float(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, IN.TimeParameters.x, 1, _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_R_1_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[0];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_G_2_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[1];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_B_3_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[2];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_A_4_Float = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float;
            Unity_Multiply_float_float(_Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float, _Split_659094672bdc4d63a53c3f0eaa743d1f_R_1_Float, _Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float;
            Unity_OneMinus_float(_Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float, _OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3;
            Unity_Lerp_float3(_NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3, float3(0, 0, 1), (_OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float.xxx), _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Multiply_545bd99a6e3847109cdc9d4d97d370da_Out_2_Vector3;
            Unity_Multiply_float3_float3((_Lerp_b9b68aeec90e4537ba8c1221829d7af7_Out_3_Float.xxx), _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3, _Multiply_545bd99a6e3847109cdc9d4d97d370da_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Add_4ff3d33383ef43ada545b8055f49fe48_Out_2_Vector3;
            Unity_Add_float3((_ScreenPosition_a770134b04f8438682b38bf27300d909_Out_0_Vector4.xyz), _Multiply_545bd99a6e3847109cdc9d4d97d370da_Out_2_Vector3, _Add_4ff3d33383ef43ada545b8055f49fe48_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _SceneColor_e76f1831af6045aaa51487c71fd5d121_Out_1_Vector3;
            Unity_SceneColor_float((float4(_Add_4ff3d33383ef43ada545b8055f49fe48_Out_2_Vector3, 1.0)), _SceneColor_e76f1831af6045aaa51487c71fd5d121_Out_1_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3;
            Unity_Multiply_float3_float3((_Lerp_8258b4c434624b60ad7eb9d848bb2bdb_Out_3_Vector4.xyz), _SceneColor_e76f1831af6045aaa51487c71fd5d121_Out_1_Vector3, _Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_6401ce34366c4dd7b7dc26e65df1fb58_Out_0_Float = _Foam_Level;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_4b76616990ac4bcc9f552ec3ed26051f_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float, _Property_6401ce34366c4dd7b7dc26e65df1fb58_Out_0_Float, _Multiply_4b76616990ac4bcc9f552ec3ed26051f_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _OneMinus_a8d37339f4c54998938ce51b9e563a00_Out_1_Float;
            Unity_OneMinus_float(_Multiply_4b76616990ac4bcc9f552ec3ed26051f_Out_2_Float, _OneMinus_a8d37339f4c54998938ce51b9e563a00_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_9220f36bb0544a169289b48cd3529146_Out_0_Float = _Foam_Contrast;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_30144d6ac6004609a47ecc162e12920f_Out_2_Float;
            Unity_Multiply_float_float(_OneMinus_a8d37339f4c54998938ce51b9e563a00_Out_1_Float, _Property_9220f36bb0544a169289b48cd3529146_Out_0_Float, _Multiply_30144d6ac6004609a47ecc162e12920f_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float;
            Unity_Saturate_float(_Multiply_30144d6ac6004609a47ecc162e12920f_Out_2_Float, _Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Lerp_de703bc50d1540859b2099644ec095da_Out_3_Vector3;
            Unity_Lerp_float3(_Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3, float3(1, 1, 1), (_Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float.xxx), _Lerp_de703bc50d1540859b2099644ec095da_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Fraction_58f43b97b1e34a7aa1d8b3cdf438e1b1_Out_1_Float;
            Unity_Fraction_float(_Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float, _Fraction_58f43b97b1e34a7aa1d8b3cdf438e1b1_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            #if defined(_DEBUG_DISABLED)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = _Lerp_de703bc50d1540859b2099644ec095da_Out_3_Vector3;
            #elif defined(_DEBUG_VORONOI)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = (_Fraction_58f43b97b1e34a7aa1d8b3cdf438e1b1_Out_1_Float.xxx);
            #elif defined(_DEBUG_NORMALS)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3;
            #elif defined(_DEBUG_DEPTH)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = (_Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float.xxx);
            #else
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = _Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            #if defined(_DEBUG_DISABLED)
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3;
            #elif defined(_DEBUG_VORONOI)
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = float3(0, 0, 1);
            #elif defined(_DEBUG_NORMALS)
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = float3(0, 0, 1);
            #elif defined(_DEBUG_DEPTH)
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = float3(0, 0, 1);
            #else
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = float3(0, 0, 1);
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Lerp_39c819c874d847ca834ce0cefb07c03a_Out_3_Float;
            Unity_Lerp_float(0.85, 0, _Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float, _Lerp_39c819c874d847ca834ce0cefb07c03a_Out_3_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            #if defined(_DEBUG_DISABLED)
            float _Debug_f9fac02476324fe5aecf8c9210c80c20_Out_0_Float = _Lerp_39c819c874d847ca834ce0cefb07c03a_Out_3_Float;
            #elif defined(_DEBUG_VORONOI)
            float _Debug_f9fac02476324fe5aecf8c9210c80c20_Out_0_Float = 0;
            #elif defined(_DEBUG_NORMALS)
            float _Debug_f9fac02476324fe5aecf8c9210c80c20_Out_0_Float = 0;
            #elif defined(_DEBUG_DEPTH)
            float _Debug_f9fac02476324fe5aecf8c9210c80c20_Out_0_Float = 0;
            #else
            float _Debug_f9fac02476324fe5aecf8c9210c80c20_Out_0_Float = 0;
            #endif
            #endif
            surface.BaseColor = _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3;
            surface.NormalTS = _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3;
            surface.Emission = float3(0, 0, 0);
            surface.Metallic = 0.1;
            surface.Smoothness = _Debug_f9fac02476324fe5aecf8c9210c80c20_Out_0_Float;
            surface.Occlusion = 1;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceNormal =                          input.normalOS;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceTangent =                         input.tangentOS.xyz;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpacePosition =                        input.positionOS;
        #endif
        
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        #endif
        
        
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.WorldSpacePosition = input.positionWS;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
        #endif
        
        
            #if UNITY_UV_STARTS_AT_TOP
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
        #endif
        
            #else
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
        #endif
        
            #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        #endif
        
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #endif
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "GBuffer"
            Tags
            {
                "LightMode" = "UniversalGBuffer"
            }
        
        // Render State
        Cull Back
        Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ SHADOWS_SHADOWMASK
        #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
        #pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma shader_feature_local _DEBUG_DISABLED _DEBUG_VORONOI _DEBUG_NORMALS _DEBUG_DEPTH _DEBUG_DISTORTION
        
        #if defined(_DEBUG_DISABLED)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_DEBUG_VORONOI)
            #define KEYWORD_PERMUTATION_1
        #elif defined(_DEBUG_NORMALS)
            #define KEYWORD_PERMUTATION_2
        #elif defined(_DEBUG_DEPTH)
            #define KEYWORD_PERMUTATION_3
        #else
            #define KEYWORD_PERMUTATION_4
        #endif
        
        
        // Defines
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMALMAP 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMAL_DROPOFF_TS 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_NORMAL
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TANGENT
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TEXCOORD1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TEXCOORD2
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_POSITION_WS
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_NORMAL_WS
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_TANGENT_WS
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_SHADOW_COORD
        #endif
        
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_GBUFFER
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _ALPHAPREMULTIPLY_ON 1
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define REQUIRE_DEPTH_TEXTURE
        #endif
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define REQUIRE_OPAQUE_TEXTURE
        #endif
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentOS : TANGENT;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 uv1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 uv2 : TEXCOORD2;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentWS;
            #endif
            #if defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 staticLightmapUV;
            #endif
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 dynamicLightmapUV;
            #endif
            #endif
            #if !defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 sh;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 fogFactorAndVertexLight;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 shadowCoord;
            #endif
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 TangentSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 WorldSpacePosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 ScreenPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 NDCPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 PixelPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 TimeParameters;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 staticLightmapUV : INTERP0;
            #endif
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 dynamicLightmapUV : INTERP1;
            #endif
            #endif
            #if !defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 sh : INTERP2;
            #endif
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 shadowCoord : INTERP3;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentWS : INTERP4;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 fogFactorAndVertexLight : INTERP5;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionWS : INTERP6;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalWS : INTERP7;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS.xyzw = input.tangentWS;
            output.fogFactorAndVertexLight.xyzw = input.fogFactorAndVertexLight;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS = input.tangentWS.xyzw;
            output.fogFactorAndVertexLight = input.fogFactorAndVertexLight.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Foam_Level;
        float _Foam_Contrast;
        float _Foam_Distortion_Amount;
        float _Normal_Scale;
        float _Normal_Strength;
        float _Normal_Animation_Speed;
        float _Normal_Animation_Radius;
        float _Foam_Distortion_Scale;
        float4 _NormalMap_TexelSize;
        float _Distortion_Amount;
        float _Depth_Range;
        float4 _Depth_Tint;
        float _Foam_CoastDepth;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_NormalMap);
        SAMPLER(sampler_NormalMap);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            if (unity_OrthoParams.w == 1.0)
            {
                Out = LinearEyeDepth(ComputeWorldSpacePosition(UV.xy, SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), UNITY_MATRIX_I_VP), UNITY_MATRIX_V);
            }
            else
            {
                Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
            }
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Lerp_float(float A, float B, float T, out float Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Cosine_float(float In, out float Out)
        {
            Out = cos(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
        {
            float x; Hash_Tchou_2_1_float(p, x);
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_Deterministic_float (float2 UV, float3 Scale, out float Out)
        {
            float2 p = UV * Scale.xy;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_NormalBlend_float(float3 A, float3 B, out float3 Out)
        {
            Out = SafeNormalize(float3(A.rg + B.rg, A.b * B.b));
        }
        
        void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
        {
            Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
        }
        
        void Vn2_float(float2 UUU, float AAA, float CCC, out float3 Out){
            float2 n = floor(UUU * CCC);
            
            float2 f = frac(UUU * CCC);
            
            float2 mg, mr;
            
            float md = 8.0;
            
            for (int j= -1; j <= 1; j++) {
            
                for (int i= -1; i <= 1; i++) {
            
                    float2 g = float2(i,j);
            
                    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
            
                    float2 UUUr = frac(sin(mul(g+n, m)) * 46839.32);
            
                    float2 o = float2(sin(UUUr.y*+AAA)*0.5+0.5, cos(UUUr.x*AAA)*0.5+0.5);
            
                    float2 r = g + o - f;
            
                    float d = dot(r,r);
            
                    if( d<md ) {
            
                        md = d;
            
                        mr = r;
            
                        mg = g;
            
                    }
            
                }
            
            }
            
            md = 8.0;
            
            for (int j= -2; j <= 2; j++) {
            
                for (int i= -2; i <= 2; i++) {
            
                    float2 g = mg + float2(i,j);
            
                    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
            
                    float2 UUUr = frac(sin(mul(g+n, m)) * 46839.32);
            
                    float2 o = float2(sin(UUUr.y*+AAA)*0.5+0.5, cos(UUUr.x*AAA)*0.5+0.5);
            
                    float2 r = g + o - f;
            
                    if ( dot(mr-r,mr-r)>0.0001 ) {
            
                        md = min(md, dot( 0.5*(mr+r), normalize(r-mr) ));
            
                    }
            
                }
            
            }
            
            Out = float3(md, mr.x, mr.y);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }
        
        void Unity_SceneColor_float(float4 UV, out float3 Out)
        {
            Out = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV.xy);
        }
        
        void Unity_Fraction_float(float In, out float Out)
        {
            Out = frac(In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalTS;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _Property_8481a4e438174b5b943f009dcab1630a_Out_0_Vector4 = _Depth_Tint;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float;
            Unity_SceneDepth_Eye_float(float4(IN.NDCPosition.xy, 0, 0), _SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4 = IN.ScreenPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_27928104217f4cf29cc2851087a2bcc9_R_1_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[0];
            float _Split_27928104217f4cf29cc2851087a2bcc9_G_2_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[1];
            float _Split_27928104217f4cf29cc2851087a2bcc9_B_3_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[2];
            float _Split_27928104217f4cf29cc2851087a2bcc9_A_4_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[3];
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float;
            Unity_Subtract_float(_SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float, _Split_27928104217f4cf29cc2851087a2bcc9_A_4_Float, _Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_2e82df2008704b02b95cfeed46b708fd_Out_0_Float = _Depth_Range;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Divide_19350d6e18284bffa24f4e82c95a8cc5_Out_2_Float;
            Unity_Divide_float(_Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float, _Property_2e82df2008704b02b95cfeed46b708fd_Out_0_Float, _Divide_19350d6e18284bffa24f4e82c95a8cc5_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float;
            Unity_Saturate_float(_Divide_19350d6e18284bffa24f4e82c95a8cc5_Out_2_Float, _Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _Lerp_8258b4c434624b60ad7eb9d848bb2bdb_Out_3_Vector4;
            Unity_Lerp_float4(float4(1, 1, 1, 1), _Property_8481a4e438174b5b943f009dcab1630a_Out_0_Vector4, (_Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float.xxxx), _Lerp_8258b4c434624b60ad7eb9d848bb2bdb_Out_3_Vector4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _ScreenPosition_a770134b04f8438682b38bf27300d909_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_112817f59e3241ecaf97d8dc07514527_Out_0_Float = _Distortion_Amount;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Lerp_b9b68aeec90e4537ba8c1221829d7af7_Out_3_Float;
            Unity_Lerp_float(0, _Property_112817f59e3241ecaf97d8dc07514527_Out_0_Float, _Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float, _Lerp_b9b68aeec90e4537ba8c1221829d7af7_Out_3_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            UnityTexture2D _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_NormalMap);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_838caa5fb6ba47cf8667c312c46234cc_R_1_Float = IN.WorldSpacePosition[0];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_G_2_Float = IN.WorldSpacePosition[1];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_B_3_Float = IN.WorldSpacePosition[2];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_A_4_Float = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2 = float2(_Split_838caa5fb6ba47cf8667c312c46234cc_R_1_Float, _Split_838caa5fb6ba47cf8667c312c46234cc_B_3_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_4bca0ae870964b80b1bf4c071a697345_Out_0_Float = _Normal_Animation_Speed;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_4bca0ae870964b80b1bf4c071a697345_Out_0_Float, _Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float;
            Unity_Sine_float(_Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float, _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float;
            Unity_Cosine_float(_Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float, _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_95a666fa09584b48aa74d064413c240b_Out_0_Vector2 = float2(_Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float, _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float = _Normal_Animation_Radius;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_95a666fa09584b48aa74d064413c240b_Out_0_Vector2, (_Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float.xx), _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2;
            Unity_Add_float2(_Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2, _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_192738b7305d464686b05d10d805ec42_Out_0_Float = _Foam_Distortion_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float;
            Unity_GradientNoise_Deterministic_float(_Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2, _Property_192738b7305d464686b05d10d805ec42_Out_0_Float, _GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_0d932574909346008e46e04609249679_Out_2_Float;
            Unity_Multiply_float_float(_Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float, -1, _Multiply_0d932574909346008e46e04609249679_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_8e409cac0d764a54a4a08573745f68fb_Out_0_Vector2 = float2(_Multiply_0d932574909346008e46e04609249679_Out_2_Float, _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_8e409cac0d764a54a4a08573745f68fb_Out_0_Vector2, (_Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float.xx), _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2;
            Unity_Add_float2(_Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2, _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float;
            Unity_Multiply_float_float(_Property_192738b7305d464686b05d10d805ec42_Out_0_Float, -1, _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float;
            Unity_GradientNoise_Deterministic_float(_Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2, _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float, _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_f26d89dcec114fbf9e11848aad395931_Out_0_Vector2 = float2(_GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float, _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_e8d3912e544a4425a4ccca0e0ec8c94c_Out_0_Float = _Foam_Distortion_Amount;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_f26d89dcec114fbf9e11848aad395931_Out_0_Vector2, (_Property_e8d3912e544a4425a4ccca0e0ec8c94c_Out_0_Float.xx), _Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2;
            Unity_Add_float2(_Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2, _Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2;
            Unity_Add_float2(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2, _Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float = _Normal_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2, (_Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float.xx), _Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.tex, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.samplerstate, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.GetTransformedUV(_Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2) );
            _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.rgb = UnpackNormal(_SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4);
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_R_4_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.r;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_G_5_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.g;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_B_6_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.b;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_A_7_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.a;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2;
            Unity_Add_float2(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2, _Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float;
            Unity_Multiply_float_float(_Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float, -1, _Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2, (_Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float.xx), _Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.tex, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.samplerstate, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.GetTransformedUV(_Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2) );
            _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.rgb = UnpackNormal(_SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4);
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_R_4_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.r;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_G_5_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.g;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_B_6_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.b;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_A_7_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.a;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3;
            Unity_NormalBlend_float((_SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.xyz), (_SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.xyz), _NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_35bdb1ec23fb4ac9beccb588d0540492_Out_0_Float = _Normal_Strength;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3;
            Unity_NormalStrength_float(_NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3, _Property_35bdb1ec23fb4ac9beccb588d0540492_Out_0_Float, _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_f0e277cb76964569ac23c9290e673186_Out_0_Float = _Foam_CoastDepth;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float;
            Unity_Divide_float(_Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float, _Property_f0e277cb76964569ac23c9290e673186_Out_0_Float, _Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float;
            Unity_Saturate_float(_Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float, _Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3;
            Vn2_float(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, IN.TimeParameters.x, 1, _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_R_1_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[0];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_G_2_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[1];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_B_3_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[2];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_A_4_Float = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float;
            Unity_Multiply_float_float(_Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float, _Split_659094672bdc4d63a53c3f0eaa743d1f_R_1_Float, _Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float;
            Unity_OneMinus_float(_Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float, _OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3;
            Unity_Lerp_float3(_NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3, float3(0, 0, 1), (_OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float.xxx), _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Multiply_545bd99a6e3847109cdc9d4d97d370da_Out_2_Vector3;
            Unity_Multiply_float3_float3((_Lerp_b9b68aeec90e4537ba8c1221829d7af7_Out_3_Float.xxx), _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3, _Multiply_545bd99a6e3847109cdc9d4d97d370da_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Add_4ff3d33383ef43ada545b8055f49fe48_Out_2_Vector3;
            Unity_Add_float3((_ScreenPosition_a770134b04f8438682b38bf27300d909_Out_0_Vector4.xyz), _Multiply_545bd99a6e3847109cdc9d4d97d370da_Out_2_Vector3, _Add_4ff3d33383ef43ada545b8055f49fe48_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _SceneColor_e76f1831af6045aaa51487c71fd5d121_Out_1_Vector3;
            Unity_SceneColor_float((float4(_Add_4ff3d33383ef43ada545b8055f49fe48_Out_2_Vector3, 1.0)), _SceneColor_e76f1831af6045aaa51487c71fd5d121_Out_1_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3;
            Unity_Multiply_float3_float3((_Lerp_8258b4c434624b60ad7eb9d848bb2bdb_Out_3_Vector4.xyz), _SceneColor_e76f1831af6045aaa51487c71fd5d121_Out_1_Vector3, _Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_6401ce34366c4dd7b7dc26e65df1fb58_Out_0_Float = _Foam_Level;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_4b76616990ac4bcc9f552ec3ed26051f_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float, _Property_6401ce34366c4dd7b7dc26e65df1fb58_Out_0_Float, _Multiply_4b76616990ac4bcc9f552ec3ed26051f_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _OneMinus_a8d37339f4c54998938ce51b9e563a00_Out_1_Float;
            Unity_OneMinus_float(_Multiply_4b76616990ac4bcc9f552ec3ed26051f_Out_2_Float, _OneMinus_a8d37339f4c54998938ce51b9e563a00_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_9220f36bb0544a169289b48cd3529146_Out_0_Float = _Foam_Contrast;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_30144d6ac6004609a47ecc162e12920f_Out_2_Float;
            Unity_Multiply_float_float(_OneMinus_a8d37339f4c54998938ce51b9e563a00_Out_1_Float, _Property_9220f36bb0544a169289b48cd3529146_Out_0_Float, _Multiply_30144d6ac6004609a47ecc162e12920f_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float;
            Unity_Saturate_float(_Multiply_30144d6ac6004609a47ecc162e12920f_Out_2_Float, _Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Lerp_de703bc50d1540859b2099644ec095da_Out_3_Vector3;
            Unity_Lerp_float3(_Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3, float3(1, 1, 1), (_Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float.xxx), _Lerp_de703bc50d1540859b2099644ec095da_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Fraction_58f43b97b1e34a7aa1d8b3cdf438e1b1_Out_1_Float;
            Unity_Fraction_float(_Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float, _Fraction_58f43b97b1e34a7aa1d8b3cdf438e1b1_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            #if defined(_DEBUG_DISABLED)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = _Lerp_de703bc50d1540859b2099644ec095da_Out_3_Vector3;
            #elif defined(_DEBUG_VORONOI)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = (_Fraction_58f43b97b1e34a7aa1d8b3cdf438e1b1_Out_1_Float.xxx);
            #elif defined(_DEBUG_NORMALS)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3;
            #elif defined(_DEBUG_DEPTH)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = (_Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float.xxx);
            #else
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = _Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            #if defined(_DEBUG_DISABLED)
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3;
            #elif defined(_DEBUG_VORONOI)
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = float3(0, 0, 1);
            #elif defined(_DEBUG_NORMALS)
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = float3(0, 0, 1);
            #elif defined(_DEBUG_DEPTH)
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = float3(0, 0, 1);
            #else
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = float3(0, 0, 1);
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Lerp_39c819c874d847ca834ce0cefb07c03a_Out_3_Float;
            Unity_Lerp_float(0.85, 0, _Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float, _Lerp_39c819c874d847ca834ce0cefb07c03a_Out_3_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            #if defined(_DEBUG_DISABLED)
            float _Debug_f9fac02476324fe5aecf8c9210c80c20_Out_0_Float = _Lerp_39c819c874d847ca834ce0cefb07c03a_Out_3_Float;
            #elif defined(_DEBUG_VORONOI)
            float _Debug_f9fac02476324fe5aecf8c9210c80c20_Out_0_Float = 0;
            #elif defined(_DEBUG_NORMALS)
            float _Debug_f9fac02476324fe5aecf8c9210c80c20_Out_0_Float = 0;
            #elif defined(_DEBUG_DEPTH)
            float _Debug_f9fac02476324fe5aecf8c9210c80c20_Out_0_Float = 0;
            #else
            float _Debug_f9fac02476324fe5aecf8c9210c80c20_Out_0_Float = 0;
            #endif
            #endif
            surface.BaseColor = _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3;
            surface.NormalTS = _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3;
            surface.Emission = float3(0, 0, 0);
            surface.Metallic = 0.1;
            surface.Smoothness = _Debug_f9fac02476324fe5aecf8c9210c80c20_Out_0_Float;
            surface.Occlusion = 1;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceNormal =                          input.normalOS;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceTangent =                         input.tangentOS.xyz;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpacePosition =                        input.positionOS;
        #endif
        
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        #endif
        
        
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.WorldSpacePosition = input.positionWS;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
        #endif
        
        
            #if UNITY_UV_STARTS_AT_TOP
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
        #endif
        
            #else
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
        #endif
        
            #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        #endif
        
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #endif
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRGBufferPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        ColorMask R
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        #pragma shader_feature_local _DEBUG_DISABLED _DEBUG_VORONOI _DEBUG_NORMALS _DEBUG_DEPTH _DEBUG_DISTORTION
        
        #if defined(_DEBUG_DISABLED)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_DEBUG_VORONOI)
            #define KEYWORD_PERMUTATION_1
        #elif defined(_DEBUG_NORMALS)
            #define KEYWORD_PERMUTATION_2
        #elif defined(_DEBUG_DEPTH)
            #define KEYWORD_PERMUTATION_3
        #else
            #define KEYWORD_PERMUTATION_4
        #endif
        
        
        // Defines
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMALMAP 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMAL_DROPOFF_TS 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_NORMAL
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TANGENT
        #endif
        
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentOS : TANGENT;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Foam_Level;
        float _Foam_Contrast;
        float _Foam_Distortion_Amount;
        float _Normal_Scale;
        float _Normal_Strength;
        float _Normal_Animation_Speed;
        float _Normal_Animation_Radius;
        float _Foam_Distortion_Scale;
        float4 _NormalMap_TexelSize;
        float _Distortion_Amount;
        float _Depth_Range;
        float4 _Depth_Tint;
        float _Foam_CoastDepth;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_NormalMap);
        SAMPLER(sampler_NormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceNormal =                          input.normalOS;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceTangent =                         input.tangentOS.xyz;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpacePosition =                        input.positionOS;
        #endif
        
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormals"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        #pragma shader_feature_local _DEBUG_DISABLED _DEBUG_VORONOI _DEBUG_NORMALS _DEBUG_DEPTH _DEBUG_DISTORTION
        
        #if defined(_DEBUG_DISABLED)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_DEBUG_VORONOI)
            #define KEYWORD_PERMUTATION_1
        #elif defined(_DEBUG_NORMALS)
            #define KEYWORD_PERMUTATION_2
        #elif defined(_DEBUG_DEPTH)
            #define KEYWORD_PERMUTATION_3
        #else
            #define KEYWORD_PERMUTATION_4
        #endif
        
        
        // Defines
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMALMAP 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMAL_DROPOFF_TS 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_NORMAL
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TANGENT
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TEXCOORD1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_POSITION_WS
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_NORMAL_WS
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_TANGENT_WS
        #endif
        
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALS
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define REQUIRE_DEPTH_TEXTURE
        #endif
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentOS : TANGENT;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 uv1 : TEXCOORD1;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentWS;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 TangentSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 WorldSpacePosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 ScreenPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 NDCPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 PixelPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 TimeParameters;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentWS : INTERP0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionWS : INTERP1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalWS : INTERP2;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.tangentWS.xyzw = input.tangentWS;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.tangentWS = input.tangentWS.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Foam_Level;
        float _Foam_Contrast;
        float _Foam_Distortion_Amount;
        float _Normal_Scale;
        float _Normal_Strength;
        float _Normal_Animation_Speed;
        float _Normal_Animation_Radius;
        float _Foam_Distortion_Scale;
        float4 _NormalMap_TexelSize;
        float _Distortion_Amount;
        float _Depth_Range;
        float4 _Depth_Tint;
        float _Foam_CoastDepth;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_NormalMap);
        SAMPLER(sampler_NormalMap);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Cosine_float(float In, out float Out)
        {
            Out = cos(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
        {
            float x; Hash_Tchou_2_1_float(p, x);
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_Deterministic_float (float2 UV, float3 Scale, out float Out)
        {
            float2 p = UV * Scale.xy;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_NormalBlend_float(float3 A, float3 B, out float3 Out)
        {
            Out = SafeNormalize(float3(A.rg + B.rg, A.b * B.b));
        }
        
        void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
        {
            Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
        }
        
        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            if (unity_OrthoParams.w == 1.0)
            {
                Out = LinearEyeDepth(ComputeWorldSpacePosition(UV.xy, SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), UNITY_MATRIX_I_VP), UNITY_MATRIX_V);
            }
            else
            {
                Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
            }
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Vn2_float(float2 UUU, float AAA, float CCC, out float3 Out){
            float2 n = floor(UUU * CCC);
            
            float2 f = frac(UUU * CCC);
            
            float2 mg, mr;
            
            float md = 8.0;
            
            for (int j= -1; j <= 1; j++) {
            
                for (int i= -1; i <= 1; i++) {
            
                    float2 g = float2(i,j);
            
                    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
            
                    float2 UUUr = frac(sin(mul(g+n, m)) * 46839.32);
            
                    float2 o = float2(sin(UUUr.y*+AAA)*0.5+0.5, cos(UUUr.x*AAA)*0.5+0.5);
            
                    float2 r = g + o - f;
            
                    float d = dot(r,r);
            
                    if( d<md ) {
            
                        md = d;
            
                        mr = r;
            
                        mg = g;
            
                    }
            
                }
            
            }
            
            md = 8.0;
            
            for (int j= -2; j <= 2; j++) {
            
                for (int i= -2; i <= 2; i++) {
            
                    float2 g = mg + float2(i,j);
            
                    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
            
                    float2 UUUr = frac(sin(mul(g+n, m)) * 46839.32);
            
                    float2 o = float2(sin(UUUr.y*+AAA)*0.5+0.5, cos(UUUr.x*AAA)*0.5+0.5);
            
                    float2 r = g + o - f;
            
                    if ( dot(mr-r,mr-r)>0.0001 ) {
            
                        md = min(md, dot( 0.5*(mr+r), normalize(r-mr) ));
            
                    }
            
                }
            
            }
            
            Out = float3(md, mr.x, mr.y);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 NormalTS;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            UnityTexture2D _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_NormalMap);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_838caa5fb6ba47cf8667c312c46234cc_R_1_Float = IN.WorldSpacePosition[0];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_G_2_Float = IN.WorldSpacePosition[1];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_B_3_Float = IN.WorldSpacePosition[2];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_A_4_Float = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2 = float2(_Split_838caa5fb6ba47cf8667c312c46234cc_R_1_Float, _Split_838caa5fb6ba47cf8667c312c46234cc_B_3_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_4bca0ae870964b80b1bf4c071a697345_Out_0_Float = _Normal_Animation_Speed;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_4bca0ae870964b80b1bf4c071a697345_Out_0_Float, _Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float;
            Unity_Sine_float(_Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float, _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float;
            Unity_Cosine_float(_Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float, _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_95a666fa09584b48aa74d064413c240b_Out_0_Vector2 = float2(_Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float, _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float = _Normal_Animation_Radius;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_95a666fa09584b48aa74d064413c240b_Out_0_Vector2, (_Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float.xx), _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2;
            Unity_Add_float2(_Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2, _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_192738b7305d464686b05d10d805ec42_Out_0_Float = _Foam_Distortion_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float;
            Unity_GradientNoise_Deterministic_float(_Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2, _Property_192738b7305d464686b05d10d805ec42_Out_0_Float, _GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_0d932574909346008e46e04609249679_Out_2_Float;
            Unity_Multiply_float_float(_Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float, -1, _Multiply_0d932574909346008e46e04609249679_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_8e409cac0d764a54a4a08573745f68fb_Out_0_Vector2 = float2(_Multiply_0d932574909346008e46e04609249679_Out_2_Float, _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_8e409cac0d764a54a4a08573745f68fb_Out_0_Vector2, (_Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float.xx), _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2;
            Unity_Add_float2(_Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2, _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float;
            Unity_Multiply_float_float(_Property_192738b7305d464686b05d10d805ec42_Out_0_Float, -1, _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float;
            Unity_GradientNoise_Deterministic_float(_Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2, _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float, _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_f26d89dcec114fbf9e11848aad395931_Out_0_Vector2 = float2(_GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float, _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_e8d3912e544a4425a4ccca0e0ec8c94c_Out_0_Float = _Foam_Distortion_Amount;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_f26d89dcec114fbf9e11848aad395931_Out_0_Vector2, (_Property_e8d3912e544a4425a4ccca0e0ec8c94c_Out_0_Float.xx), _Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2;
            Unity_Add_float2(_Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2, _Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2;
            Unity_Add_float2(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2, _Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float = _Normal_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2, (_Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float.xx), _Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.tex, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.samplerstate, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.GetTransformedUV(_Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2) );
            _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.rgb = UnpackNormal(_SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4);
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_R_4_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.r;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_G_5_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.g;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_B_6_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.b;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_A_7_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.a;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2;
            Unity_Add_float2(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2, _Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float;
            Unity_Multiply_float_float(_Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float, -1, _Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2, (_Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float.xx), _Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.tex, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.samplerstate, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.GetTransformedUV(_Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2) );
            _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.rgb = UnpackNormal(_SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4);
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_R_4_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.r;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_G_5_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.g;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_B_6_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.b;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_A_7_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.a;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3;
            Unity_NormalBlend_float((_SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.xyz), (_SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.xyz), _NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_35bdb1ec23fb4ac9beccb588d0540492_Out_0_Float = _Normal_Strength;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3;
            Unity_NormalStrength_float(_NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3, _Property_35bdb1ec23fb4ac9beccb588d0540492_Out_0_Float, _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float;
            Unity_SceneDepth_Eye_float(float4(IN.NDCPosition.xy, 0, 0), _SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4 = IN.ScreenPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_27928104217f4cf29cc2851087a2bcc9_R_1_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[0];
            float _Split_27928104217f4cf29cc2851087a2bcc9_G_2_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[1];
            float _Split_27928104217f4cf29cc2851087a2bcc9_B_3_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[2];
            float _Split_27928104217f4cf29cc2851087a2bcc9_A_4_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[3];
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float;
            Unity_Subtract_float(_SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float, _Split_27928104217f4cf29cc2851087a2bcc9_A_4_Float, _Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_f0e277cb76964569ac23c9290e673186_Out_0_Float = _Foam_CoastDepth;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float;
            Unity_Divide_float(_Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float, _Property_f0e277cb76964569ac23c9290e673186_Out_0_Float, _Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float;
            Unity_Saturate_float(_Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float, _Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3;
            Vn2_float(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, IN.TimeParameters.x, 1, _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_R_1_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[0];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_G_2_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[1];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_B_3_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[2];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_A_4_Float = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float;
            Unity_Multiply_float_float(_Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float, _Split_659094672bdc4d63a53c3f0eaa743d1f_R_1_Float, _Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float;
            Unity_OneMinus_float(_Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float, _OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3;
            Unity_Lerp_float3(_NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3, float3(0, 0, 1), (_OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float.xxx), _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            #if defined(_DEBUG_DISABLED)
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3;
            #elif defined(_DEBUG_VORONOI)
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = float3(0, 0, 1);
            #elif defined(_DEBUG_NORMALS)
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = float3(0, 0, 1);
            #elif defined(_DEBUG_DEPTH)
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = float3(0, 0, 1);
            #else
            float3 _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3 = float3(0, 0, 1);
            #endif
            #endif
            surface.NormalTS = _Debug_be74c4e3938a45a282250be83a83e730_Out_0_Vector3;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceNormal =                          input.normalOS;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceTangent =                         input.tangentOS.xyz;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpacePosition =                        input.positionOS;
        #endif
        
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        #endif
        
        
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.WorldSpacePosition = input.positionWS;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
        #endif
        
        
            #if UNITY_UV_STARTS_AT_TOP
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
        #endif
        
            #else
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
        #endif
        
            #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        #endif
        
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #endif
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "Meta"
            Tags
            {
                "LightMode" = "Meta"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma shader_feature _ EDITOR_VISUALIZATION
        #pragma shader_feature_local _DEBUG_DISABLED _DEBUG_VORONOI _DEBUG_NORMALS _DEBUG_DEPTH _DEBUG_DISTORTION
        
        #if defined(_DEBUG_DISABLED)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_DEBUG_VORONOI)
            #define KEYWORD_PERMUTATION_1
        #elif defined(_DEBUG_NORMALS)
            #define KEYWORD_PERMUTATION_2
        #elif defined(_DEBUG_DEPTH)
            #define KEYWORD_PERMUTATION_3
        #else
            #define KEYWORD_PERMUTATION_4
        #endif
        
        
        // Defines
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMALMAP 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMAL_DROPOFF_TS 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_NORMAL
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TANGENT
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TEXCOORD0
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TEXCOORD1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TEXCOORD2
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_POSITION_WS
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_TEXCOORD0
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_TEXCOORD1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_TEXCOORD2
        #endif
        
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_META
        #define _FOG_FRAGMENT 1
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define REQUIRE_DEPTH_TEXTURE
        #endif
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define REQUIRE_OPAQUE_TEXTURE
        #endif
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentOS : TANGENT;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 uv0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 uv1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 uv2 : TEXCOORD2;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 texCoord0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 texCoord1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 texCoord2;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 WorldSpacePosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 ScreenPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 NDCPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 PixelPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 TimeParameters;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 texCoord0 : INTERP0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 texCoord1 : INTERP1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 texCoord2 : INTERP2;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionWS : INTERP3;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.texCoord2.xyzw = input.texCoord2;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.texCoord2 = input.texCoord2.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Foam_Level;
        float _Foam_Contrast;
        float _Foam_Distortion_Amount;
        float _Normal_Scale;
        float _Normal_Strength;
        float _Normal_Animation_Speed;
        float _Normal_Animation_Radius;
        float _Foam_Distortion_Scale;
        float4 _NormalMap_TexelSize;
        float _Distortion_Amount;
        float _Depth_Range;
        float4 _Depth_Tint;
        float _Foam_CoastDepth;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_NormalMap);
        SAMPLER(sampler_NormalMap);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            if (unity_OrthoParams.w == 1.0)
            {
                Out = LinearEyeDepth(ComputeWorldSpacePosition(UV.xy, SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), UNITY_MATRIX_I_VP), UNITY_MATRIX_V);
            }
            else
            {
                Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
            }
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Lerp_float(float A, float B, float T, out float Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Cosine_float(float In, out float Out)
        {
            Out = cos(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
        {
            float x; Hash_Tchou_2_1_float(p, x);
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_Deterministic_float (float2 UV, float3 Scale, out float Out)
        {
            float2 p = UV * Scale.xy;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_NormalBlend_float(float3 A, float3 B, out float3 Out)
        {
            Out = SafeNormalize(float3(A.rg + B.rg, A.b * B.b));
        }
        
        void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
        {
            Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
        }
        
        void Vn2_float(float2 UUU, float AAA, float CCC, out float3 Out){
            float2 n = floor(UUU * CCC);
            
            float2 f = frac(UUU * CCC);
            
            float2 mg, mr;
            
            float md = 8.0;
            
            for (int j= -1; j <= 1; j++) {
            
                for (int i= -1; i <= 1; i++) {
            
                    float2 g = float2(i,j);
            
                    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
            
                    float2 UUUr = frac(sin(mul(g+n, m)) * 46839.32);
            
                    float2 o = float2(sin(UUUr.y*+AAA)*0.5+0.5, cos(UUUr.x*AAA)*0.5+0.5);
            
                    float2 r = g + o - f;
            
                    float d = dot(r,r);
            
                    if( d<md ) {
            
                        md = d;
            
                        mr = r;
            
                        mg = g;
            
                    }
            
                }
            
            }
            
            md = 8.0;
            
            for (int j= -2; j <= 2; j++) {
            
                for (int i= -2; i <= 2; i++) {
            
                    float2 g = mg + float2(i,j);
            
                    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
            
                    float2 UUUr = frac(sin(mul(g+n, m)) * 46839.32);
            
                    float2 o = float2(sin(UUUr.y*+AAA)*0.5+0.5, cos(UUUr.x*AAA)*0.5+0.5);
            
                    float2 r = g + o - f;
            
                    if ( dot(mr-r,mr-r)>0.0001 ) {
            
                        md = min(md, dot( 0.5*(mr+r), normalize(r-mr) ));
            
                    }
            
                }
            
            }
            
            Out = float3(md, mr.x, mr.y);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }
        
        void Unity_SceneColor_float(float4 UV, out float3 Out)
        {
            Out = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV.xy);
        }
        
        void Unity_Fraction_float(float In, out float Out)
        {
            Out = frac(In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _Property_8481a4e438174b5b943f009dcab1630a_Out_0_Vector4 = _Depth_Tint;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float;
            Unity_SceneDepth_Eye_float(float4(IN.NDCPosition.xy, 0, 0), _SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4 = IN.ScreenPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_27928104217f4cf29cc2851087a2bcc9_R_1_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[0];
            float _Split_27928104217f4cf29cc2851087a2bcc9_G_2_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[1];
            float _Split_27928104217f4cf29cc2851087a2bcc9_B_3_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[2];
            float _Split_27928104217f4cf29cc2851087a2bcc9_A_4_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[3];
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float;
            Unity_Subtract_float(_SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float, _Split_27928104217f4cf29cc2851087a2bcc9_A_4_Float, _Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_2e82df2008704b02b95cfeed46b708fd_Out_0_Float = _Depth_Range;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Divide_19350d6e18284bffa24f4e82c95a8cc5_Out_2_Float;
            Unity_Divide_float(_Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float, _Property_2e82df2008704b02b95cfeed46b708fd_Out_0_Float, _Divide_19350d6e18284bffa24f4e82c95a8cc5_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float;
            Unity_Saturate_float(_Divide_19350d6e18284bffa24f4e82c95a8cc5_Out_2_Float, _Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _Lerp_8258b4c434624b60ad7eb9d848bb2bdb_Out_3_Vector4;
            Unity_Lerp_float4(float4(1, 1, 1, 1), _Property_8481a4e438174b5b943f009dcab1630a_Out_0_Vector4, (_Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float.xxxx), _Lerp_8258b4c434624b60ad7eb9d848bb2bdb_Out_3_Vector4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _ScreenPosition_a770134b04f8438682b38bf27300d909_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_112817f59e3241ecaf97d8dc07514527_Out_0_Float = _Distortion_Amount;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Lerp_b9b68aeec90e4537ba8c1221829d7af7_Out_3_Float;
            Unity_Lerp_float(0, _Property_112817f59e3241ecaf97d8dc07514527_Out_0_Float, _Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float, _Lerp_b9b68aeec90e4537ba8c1221829d7af7_Out_3_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            UnityTexture2D _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_NormalMap);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_838caa5fb6ba47cf8667c312c46234cc_R_1_Float = IN.WorldSpacePosition[0];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_G_2_Float = IN.WorldSpacePosition[1];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_B_3_Float = IN.WorldSpacePosition[2];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_A_4_Float = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2 = float2(_Split_838caa5fb6ba47cf8667c312c46234cc_R_1_Float, _Split_838caa5fb6ba47cf8667c312c46234cc_B_3_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_4bca0ae870964b80b1bf4c071a697345_Out_0_Float = _Normal_Animation_Speed;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_4bca0ae870964b80b1bf4c071a697345_Out_0_Float, _Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float;
            Unity_Sine_float(_Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float, _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float;
            Unity_Cosine_float(_Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float, _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_95a666fa09584b48aa74d064413c240b_Out_0_Vector2 = float2(_Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float, _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float = _Normal_Animation_Radius;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_95a666fa09584b48aa74d064413c240b_Out_0_Vector2, (_Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float.xx), _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2;
            Unity_Add_float2(_Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2, _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_192738b7305d464686b05d10d805ec42_Out_0_Float = _Foam_Distortion_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float;
            Unity_GradientNoise_Deterministic_float(_Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2, _Property_192738b7305d464686b05d10d805ec42_Out_0_Float, _GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_0d932574909346008e46e04609249679_Out_2_Float;
            Unity_Multiply_float_float(_Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float, -1, _Multiply_0d932574909346008e46e04609249679_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_8e409cac0d764a54a4a08573745f68fb_Out_0_Vector2 = float2(_Multiply_0d932574909346008e46e04609249679_Out_2_Float, _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_8e409cac0d764a54a4a08573745f68fb_Out_0_Vector2, (_Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float.xx), _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2;
            Unity_Add_float2(_Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2, _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float;
            Unity_Multiply_float_float(_Property_192738b7305d464686b05d10d805ec42_Out_0_Float, -1, _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float;
            Unity_GradientNoise_Deterministic_float(_Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2, _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float, _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_f26d89dcec114fbf9e11848aad395931_Out_0_Vector2 = float2(_GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float, _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_e8d3912e544a4425a4ccca0e0ec8c94c_Out_0_Float = _Foam_Distortion_Amount;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_f26d89dcec114fbf9e11848aad395931_Out_0_Vector2, (_Property_e8d3912e544a4425a4ccca0e0ec8c94c_Out_0_Float.xx), _Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2;
            Unity_Add_float2(_Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2, _Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2;
            Unity_Add_float2(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2, _Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float = _Normal_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2, (_Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float.xx), _Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.tex, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.samplerstate, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.GetTransformedUV(_Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2) );
            _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.rgb = UnpackNormal(_SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4);
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_R_4_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.r;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_G_5_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.g;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_B_6_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.b;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_A_7_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.a;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2;
            Unity_Add_float2(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2, _Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float;
            Unity_Multiply_float_float(_Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float, -1, _Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2, (_Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float.xx), _Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.tex, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.samplerstate, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.GetTransformedUV(_Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2) );
            _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.rgb = UnpackNormal(_SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4);
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_R_4_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.r;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_G_5_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.g;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_B_6_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.b;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_A_7_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.a;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3;
            Unity_NormalBlend_float((_SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.xyz), (_SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.xyz), _NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_35bdb1ec23fb4ac9beccb588d0540492_Out_0_Float = _Normal_Strength;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3;
            Unity_NormalStrength_float(_NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3, _Property_35bdb1ec23fb4ac9beccb588d0540492_Out_0_Float, _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_f0e277cb76964569ac23c9290e673186_Out_0_Float = _Foam_CoastDepth;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float;
            Unity_Divide_float(_Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float, _Property_f0e277cb76964569ac23c9290e673186_Out_0_Float, _Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float;
            Unity_Saturate_float(_Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float, _Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3;
            Vn2_float(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, IN.TimeParameters.x, 1, _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_R_1_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[0];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_G_2_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[1];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_B_3_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[2];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_A_4_Float = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float;
            Unity_Multiply_float_float(_Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float, _Split_659094672bdc4d63a53c3f0eaa743d1f_R_1_Float, _Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float;
            Unity_OneMinus_float(_Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float, _OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3;
            Unity_Lerp_float3(_NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3, float3(0, 0, 1), (_OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float.xxx), _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Multiply_545bd99a6e3847109cdc9d4d97d370da_Out_2_Vector3;
            Unity_Multiply_float3_float3((_Lerp_b9b68aeec90e4537ba8c1221829d7af7_Out_3_Float.xxx), _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3, _Multiply_545bd99a6e3847109cdc9d4d97d370da_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Add_4ff3d33383ef43ada545b8055f49fe48_Out_2_Vector3;
            Unity_Add_float3((_ScreenPosition_a770134b04f8438682b38bf27300d909_Out_0_Vector4.xyz), _Multiply_545bd99a6e3847109cdc9d4d97d370da_Out_2_Vector3, _Add_4ff3d33383ef43ada545b8055f49fe48_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _SceneColor_e76f1831af6045aaa51487c71fd5d121_Out_1_Vector3;
            Unity_SceneColor_float((float4(_Add_4ff3d33383ef43ada545b8055f49fe48_Out_2_Vector3, 1.0)), _SceneColor_e76f1831af6045aaa51487c71fd5d121_Out_1_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3;
            Unity_Multiply_float3_float3((_Lerp_8258b4c434624b60ad7eb9d848bb2bdb_Out_3_Vector4.xyz), _SceneColor_e76f1831af6045aaa51487c71fd5d121_Out_1_Vector3, _Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_6401ce34366c4dd7b7dc26e65df1fb58_Out_0_Float = _Foam_Level;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_4b76616990ac4bcc9f552ec3ed26051f_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float, _Property_6401ce34366c4dd7b7dc26e65df1fb58_Out_0_Float, _Multiply_4b76616990ac4bcc9f552ec3ed26051f_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _OneMinus_a8d37339f4c54998938ce51b9e563a00_Out_1_Float;
            Unity_OneMinus_float(_Multiply_4b76616990ac4bcc9f552ec3ed26051f_Out_2_Float, _OneMinus_a8d37339f4c54998938ce51b9e563a00_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_9220f36bb0544a169289b48cd3529146_Out_0_Float = _Foam_Contrast;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_30144d6ac6004609a47ecc162e12920f_Out_2_Float;
            Unity_Multiply_float_float(_OneMinus_a8d37339f4c54998938ce51b9e563a00_Out_1_Float, _Property_9220f36bb0544a169289b48cd3529146_Out_0_Float, _Multiply_30144d6ac6004609a47ecc162e12920f_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float;
            Unity_Saturate_float(_Multiply_30144d6ac6004609a47ecc162e12920f_Out_2_Float, _Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Lerp_de703bc50d1540859b2099644ec095da_Out_3_Vector3;
            Unity_Lerp_float3(_Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3, float3(1, 1, 1), (_Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float.xxx), _Lerp_de703bc50d1540859b2099644ec095da_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Fraction_58f43b97b1e34a7aa1d8b3cdf438e1b1_Out_1_Float;
            Unity_Fraction_float(_Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float, _Fraction_58f43b97b1e34a7aa1d8b3cdf438e1b1_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            #if defined(_DEBUG_DISABLED)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = _Lerp_de703bc50d1540859b2099644ec095da_Out_3_Vector3;
            #elif defined(_DEBUG_VORONOI)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = (_Fraction_58f43b97b1e34a7aa1d8b3cdf438e1b1_Out_1_Float.xxx);
            #elif defined(_DEBUG_NORMALS)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3;
            #elif defined(_DEBUG_DEPTH)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = (_Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float.xxx);
            #else
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = _Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3;
            #endif
            #endif
            surface.BaseColor = _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3;
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceNormal =                          input.normalOS;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceTangent =                         input.tangentOS.xyz;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpacePosition =                        input.positionOS;
        #endif
        
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.WorldSpacePosition = input.positionWS;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
        #endif
        
        
            #if UNITY_UV_STARTS_AT_TOP
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
        #endif
        
            #else
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
        #endif
        
            #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        #endif
        
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #endif
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        #pragma shader_feature_local _DEBUG_DISABLED _DEBUG_VORONOI _DEBUG_NORMALS _DEBUG_DEPTH _DEBUG_DISTORTION
        
        #if defined(_DEBUG_DISABLED)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_DEBUG_VORONOI)
            #define KEYWORD_PERMUTATION_1
        #elif defined(_DEBUG_NORMALS)
            #define KEYWORD_PERMUTATION_2
        #elif defined(_DEBUG_DEPTH)
            #define KEYWORD_PERMUTATION_3
        #else
            #define KEYWORD_PERMUTATION_4
        #endif
        
        
        // Defines
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMALMAP 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMAL_DROPOFF_TS 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_NORMAL
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TANGENT
        #endif
        
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentOS : TANGENT;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Foam_Level;
        float _Foam_Contrast;
        float _Foam_Distortion_Amount;
        float _Normal_Scale;
        float _Normal_Strength;
        float _Normal_Animation_Speed;
        float _Normal_Animation_Radius;
        float _Foam_Distortion_Scale;
        float4 _NormalMap_TexelSize;
        float _Distortion_Amount;
        float _Depth_Range;
        float4 _Depth_Tint;
        float _Foam_CoastDepth;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_NormalMap);
        SAMPLER(sampler_NormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceNormal =                          input.normalOS;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceTangent =                         input.tangentOS.xyz;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpacePosition =                        input.positionOS;
        #endif
        
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        #pragma shader_feature_local _DEBUG_DISABLED _DEBUG_VORONOI _DEBUG_NORMALS _DEBUG_DEPTH _DEBUG_DISTORTION
        
        #if defined(_DEBUG_DISABLED)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_DEBUG_VORONOI)
            #define KEYWORD_PERMUTATION_1
        #elif defined(_DEBUG_NORMALS)
            #define KEYWORD_PERMUTATION_2
        #elif defined(_DEBUG_DEPTH)
            #define KEYWORD_PERMUTATION_3
        #else
            #define KEYWORD_PERMUTATION_4
        #endif
        
        
        // Defines
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMALMAP 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMAL_DROPOFF_TS 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_NORMAL
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TANGENT
        #endif
        
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentOS : TANGENT;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Foam_Level;
        float _Foam_Contrast;
        float _Foam_Distortion_Amount;
        float _Normal_Scale;
        float _Normal_Strength;
        float _Normal_Animation_Speed;
        float _Normal_Animation_Radius;
        float _Foam_Distortion_Scale;
        float4 _NormalMap_TexelSize;
        float _Distortion_Amount;
        float _Depth_Range;
        float4 _Depth_Tint;
        float _Foam_CoastDepth;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_NormalMap);
        SAMPLER(sampler_NormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceNormal =                          input.normalOS;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceTangent =                         input.tangentOS.xyz;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpacePosition =                        input.positionOS;
        #endif
        
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            // Name: <None>
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        #pragma shader_feature_local _DEBUG_DISABLED _DEBUG_VORONOI _DEBUG_NORMALS _DEBUG_DEPTH _DEBUG_DISTORTION
        
        #if defined(_DEBUG_DISABLED)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_DEBUG_VORONOI)
            #define KEYWORD_PERMUTATION_1
        #elif defined(_DEBUG_NORMALS)
            #define KEYWORD_PERMUTATION_2
        #elif defined(_DEBUG_DEPTH)
            #define KEYWORD_PERMUTATION_3
        #else
            #define KEYWORD_PERMUTATION_4
        #endif
        
        
        // Defines
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMALMAP 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define _NORMAL_DROPOFF_TS 1
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_NORMAL
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define ATTRIBUTES_NEED_TANGENT
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define VARYINGS_NEED_POSITION_WS
        #endif
        
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_2D
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define REQUIRE_DEPTH_TEXTURE
        #endif
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        #define REQUIRE_OPAQUE_TEXTURE
        #endif
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 tangentOS : TANGENT;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionWS;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 WorldSpacePosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 ScreenPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 NDCPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float2 PixelPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 TimeParameters;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             float3 positionWS : INTERP0;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Foam_Level;
        float _Foam_Contrast;
        float _Foam_Distortion_Amount;
        float _Normal_Scale;
        float _Normal_Strength;
        float _Normal_Animation_Speed;
        float _Normal_Animation_Radius;
        float _Foam_Distortion_Scale;
        float4 _NormalMap_TexelSize;
        float _Distortion_Amount;
        float _Depth_Range;
        float4 _Depth_Tint;
        float _Foam_CoastDepth;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_NormalMap);
        SAMPLER(sampler_NormalMap);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
        {
            if (unity_OrthoParams.w == 1.0)
            {
                Out = LinearEyeDepth(ComputeWorldSpacePosition(UV.xy, SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), UNITY_MATRIX_I_VP), UNITY_MATRIX_V);
            }
            else
            {
                Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
            }
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Lerp_float(float A, float B, float T, out float Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Cosine_float(float In, out float Out)
        {
            Out = cos(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
        {
            float x; Hash_Tchou_2_1_float(p, x);
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_Deterministic_float (float2 UV, float3 Scale, out float Out)
        {
            float2 p = UV * Scale.xy;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_NormalBlend_float(float3 A, float3 B, out float3 Out)
        {
            Out = SafeNormalize(float3(A.rg + B.rg, A.b * B.b));
        }
        
        void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
        {
            Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
        }
        
        void Vn2_float(float2 UUU, float AAA, float CCC, out float3 Out){
            float2 n = floor(UUU * CCC);
            
            float2 f = frac(UUU * CCC);
            
            float2 mg, mr;
            
            float md = 8.0;
            
            for (int j= -1; j <= 1; j++) {
            
                for (int i= -1; i <= 1; i++) {
            
                    float2 g = float2(i,j);
            
                    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
            
                    float2 UUUr = frac(sin(mul(g+n, m)) * 46839.32);
            
                    float2 o = float2(sin(UUUr.y*+AAA)*0.5+0.5, cos(UUUr.x*AAA)*0.5+0.5);
            
                    float2 r = g + o - f;
            
                    float d = dot(r,r);
            
                    if( d<md ) {
            
                        md = d;
            
                        mr = r;
            
                        mg = g;
            
                    }
            
                }
            
            }
            
            md = 8.0;
            
            for (int j= -2; j <= 2; j++) {
            
                for (int i= -2; i <= 2; i++) {
            
                    float2 g = mg + float2(i,j);
            
                    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
            
                    float2 UUUr = frac(sin(mul(g+n, m)) * 46839.32);
            
                    float2 o = float2(sin(UUUr.y*+AAA)*0.5+0.5, cos(UUUr.x*AAA)*0.5+0.5);
            
                    float2 r = g + o - f;
            
                    if ( dot(mr-r,mr-r)>0.0001 ) {
            
                        md = min(md, dot( 0.5*(mr+r), normalize(r-mr) ));
            
                    }
            
                }
            
            }
            
            Out = float3(md, mr.x, mr.y);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }
        
        void Unity_SceneColor_float(float4 UV, out float3 Out)
        {
            Out = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV.xy);
        }
        
        void Unity_Fraction_float(float In, out float Out)
        {
            Out = frac(In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _Property_8481a4e438174b5b943f009dcab1630a_Out_0_Vector4 = _Depth_Tint;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float;
            Unity_SceneDepth_Eye_float(float4(IN.NDCPosition.xy, 0, 0), _SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4 = IN.ScreenPosition;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_27928104217f4cf29cc2851087a2bcc9_R_1_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[0];
            float _Split_27928104217f4cf29cc2851087a2bcc9_G_2_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[1];
            float _Split_27928104217f4cf29cc2851087a2bcc9_B_3_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[2];
            float _Split_27928104217f4cf29cc2851087a2bcc9_A_4_Float = _ScreenPosition_a94399aa9fd142c0a323bc223ae387ad_Out_0_Vector4[3];
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float;
            Unity_Subtract_float(_SceneDepth_845b53ccfb354e95a1ad165ddf60826a_Out_1_Float, _Split_27928104217f4cf29cc2851087a2bcc9_A_4_Float, _Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_2e82df2008704b02b95cfeed46b708fd_Out_0_Float = _Depth_Range;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Divide_19350d6e18284bffa24f4e82c95a8cc5_Out_2_Float;
            Unity_Divide_float(_Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float, _Property_2e82df2008704b02b95cfeed46b708fd_Out_0_Float, _Divide_19350d6e18284bffa24f4e82c95a8cc5_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float;
            Unity_Saturate_float(_Divide_19350d6e18284bffa24f4e82c95a8cc5_Out_2_Float, _Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _Lerp_8258b4c434624b60ad7eb9d848bb2bdb_Out_3_Vector4;
            Unity_Lerp_float4(float4(1, 1, 1, 1), _Property_8481a4e438174b5b943f009dcab1630a_Out_0_Vector4, (_Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float.xxxx), _Lerp_8258b4c434624b60ad7eb9d848bb2bdb_Out_3_Vector4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _ScreenPosition_a770134b04f8438682b38bf27300d909_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_112817f59e3241ecaf97d8dc07514527_Out_0_Float = _Distortion_Amount;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Lerp_b9b68aeec90e4537ba8c1221829d7af7_Out_3_Float;
            Unity_Lerp_float(0, _Property_112817f59e3241ecaf97d8dc07514527_Out_0_Float, _Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float, _Lerp_b9b68aeec90e4537ba8c1221829d7af7_Out_3_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            UnityTexture2D _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_NormalMap);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_838caa5fb6ba47cf8667c312c46234cc_R_1_Float = IN.WorldSpacePosition[0];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_G_2_Float = IN.WorldSpacePosition[1];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_B_3_Float = IN.WorldSpacePosition[2];
            float _Split_838caa5fb6ba47cf8667c312c46234cc_A_4_Float = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2 = float2(_Split_838caa5fb6ba47cf8667c312c46234cc_R_1_Float, _Split_838caa5fb6ba47cf8667c312c46234cc_B_3_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_4bca0ae870964b80b1bf4c071a697345_Out_0_Float = _Normal_Animation_Speed;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_4bca0ae870964b80b1bf4c071a697345_Out_0_Float, _Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float;
            Unity_Sine_float(_Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float, _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float;
            Unity_Cosine_float(_Multiply_1991a497dc704ede8ab6273d3170ef13_Out_2_Float, _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_95a666fa09584b48aa74d064413c240b_Out_0_Vector2 = float2(_Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float, _Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float = _Normal_Animation_Radius;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_95a666fa09584b48aa74d064413c240b_Out_0_Vector2, (_Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float.xx), _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2;
            Unity_Add_float2(_Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Multiply_d9d92d47314f4389bffa7dc54e274228_Out_2_Vector2, _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_192738b7305d464686b05d10d805ec42_Out_0_Float = _Foam_Distortion_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float;
            Unity_GradientNoise_Deterministic_float(_Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2, _Property_192738b7305d464686b05d10d805ec42_Out_0_Float, _GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_0d932574909346008e46e04609249679_Out_2_Float;
            Unity_Multiply_float_float(_Cosine_699e6ac5a356417b912f95c5828bdf29_Out_1_Float, -1, _Multiply_0d932574909346008e46e04609249679_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_8e409cac0d764a54a4a08573745f68fb_Out_0_Vector2 = float2(_Multiply_0d932574909346008e46e04609249679_Out_2_Float, _Sine_ece36bffffdc4e1c839a035f9865fc56_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_8e409cac0d764a54a4a08573745f68fb_Out_0_Vector2, (_Property_1d5c5312946445a78ce6b829e38ff60d_Out_0_Float.xx), _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2;
            Unity_Add_float2(_Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Multiply_122a9388d23a4bb6a3408ed894c44709_Out_2_Vector2, _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float;
            Unity_Multiply_float_float(_Property_192738b7305d464686b05d10d805ec42_Out_0_Float, -1, _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float;
            Unity_GradientNoise_Deterministic_float(_Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2, _Multiply_fc9651e5b06c48ed9144f7fae6bf1218_Out_2_Float, _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Vector2_f26d89dcec114fbf9e11848aad395931_Out_0_Vector2 = float2(_GradientNoise_be11fefe4e1347feae46d698cef10cb2_Out_2_Float, _GradientNoise_b05aac0a3c984db09ede7a4da459d563_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_e8d3912e544a4425a4ccca0e0ec8c94c_Out_0_Float = _Foam_Distortion_Amount;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_f26d89dcec114fbf9e11848aad395931_Out_0_Vector2, (_Property_e8d3912e544a4425a4ccca0e0ec8c94c_Out_0_Float.xx), _Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2;
            Unity_Add_float2(_Multiply_0fd8fcb7c5a849bc9c51b63ef772eea3_Out_2_Vector2, _Vector2_3ba41b176ea04ee4b26f15e3b416a44a_Out_0_Vector2, _Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2;
            Unity_Add_float2(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, _Add_53d63cb9a6814f77bb191f211e87d2bf_Out_2_Vector2, _Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float = _Normal_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Add_7ec463904f9c4e069475dbaf272e0479_Out_2_Vector2, (_Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float.xx), _Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.tex, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.samplerstate, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.GetTransformedUV(_Multiply_9674203869ed4d679986f5716f3ab6af_Out_2_Vector2) );
            _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.rgb = UnpackNormal(_SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4);
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_R_4_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.r;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_G_5_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.g;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_B_6_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.b;
            float _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_A_7_Float = _SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.a;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2;
            Unity_Add_float2(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, _Add_7f98c5ad82c94a61922da9b5acc76247_Out_2_Vector2, _Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float;
            Unity_Multiply_float_float(_Property_19a0e9af9158470fab56b06ff7aa6580_Out_0_Float, -1, _Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float2 _Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Add_f2a1c908adba49fcaaa8bb5e84c3f8cf_Out_2_Vector2, (_Multiply_8191befbaadf44e8833da5f2e5a1ed5c_Out_2_Float.xx), _Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float4 _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.tex, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.samplerstate, _Property_2ee2c82e787140d980de89cd1558f8d8_Out_0_Texture2D.GetTransformedUV(_Multiply_8b3d273940ea4cd882f52036eb7a2309_Out_2_Vector2) );
            _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.rgb = UnpackNormal(_SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4);
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_R_4_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.r;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_G_5_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.g;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_B_6_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.b;
            float _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_A_7_Float = _SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.a;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3;
            Unity_NormalBlend_float((_SampleTexture2D_9c0820d6af9d4e998b7d0ff29f2b3610_RGBA_0_Vector4.xyz), (_SampleTexture2D_2a3d437cdcbe489bae2a22b35daa4d24_RGBA_0_Vector4.xyz), _NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_35bdb1ec23fb4ac9beccb588d0540492_Out_0_Float = _Normal_Strength;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3;
            Unity_NormalStrength_float(_NormalBlend_9e892f885aa0451092b51131f405ed1a_Out_2_Vector3, _Property_35bdb1ec23fb4ac9beccb588d0540492_Out_0_Float, _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_f0e277cb76964569ac23c9290e673186_Out_0_Float = _Foam_CoastDepth;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float;
            Unity_Divide_float(_Subtract_1f8d9bc9151c4f6c82c049da0dd248ab_Out_2_Float, _Property_f0e277cb76964569ac23c9290e673186_Out_0_Float, _Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float;
            Unity_Saturate_float(_Divide_f1698c18b27843f0b567b347c872d61c_Out_2_Float, _Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3;
            Vn2_float(_Add_18a6b4efac894519b723d6cda5bd57af_Out_2_Vector2, IN.TimeParameters.x, 1, _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_R_1_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[0];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_G_2_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[1];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_B_3_Float = _Vn2CustomFunction_f82fcebdd22b42189a237c777d364f86_Out_3_Vector3[2];
            float _Split_659094672bdc4d63a53c3f0eaa743d1f_A_4_Float = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float;
            Unity_Multiply_float_float(_Saturate_732b97b727f54713a8912173bcc1c1d1_Out_1_Float, _Split_659094672bdc4d63a53c3f0eaa743d1f_R_1_Float, _Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float;
            Unity_OneMinus_float(_Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float, _OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3;
            Unity_Lerp_float3(_NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3, float3(0, 0, 1), (_OneMinus_318d11811f2740289d8a65bc76a0a294_Out_1_Float.xxx), _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Multiply_545bd99a6e3847109cdc9d4d97d370da_Out_2_Vector3;
            Unity_Multiply_float3_float3((_Lerp_b9b68aeec90e4537ba8c1221829d7af7_Out_3_Float.xxx), _Lerp_9bc44704bfb34c048cf0a0884b7421a2_Out_3_Vector3, _Multiply_545bd99a6e3847109cdc9d4d97d370da_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Add_4ff3d33383ef43ada545b8055f49fe48_Out_2_Vector3;
            Unity_Add_float3((_ScreenPosition_a770134b04f8438682b38bf27300d909_Out_0_Vector4.xyz), _Multiply_545bd99a6e3847109cdc9d4d97d370da_Out_2_Vector3, _Add_4ff3d33383ef43ada545b8055f49fe48_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _SceneColor_e76f1831af6045aaa51487c71fd5d121_Out_1_Vector3;
            Unity_SceneColor_float((float4(_Add_4ff3d33383ef43ada545b8055f49fe48_Out_2_Vector3, 1.0)), _SceneColor_e76f1831af6045aaa51487c71fd5d121_Out_1_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3;
            Unity_Multiply_float3_float3((_Lerp_8258b4c434624b60ad7eb9d848bb2bdb_Out_3_Vector4.xyz), _SceneColor_e76f1831af6045aaa51487c71fd5d121_Out_1_Vector3, _Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_6401ce34366c4dd7b7dc26e65df1fb58_Out_0_Float = _Foam_Level;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_4b76616990ac4bcc9f552ec3ed26051f_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float, _Property_6401ce34366c4dd7b7dc26e65df1fb58_Out_0_Float, _Multiply_4b76616990ac4bcc9f552ec3ed26051f_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _OneMinus_a8d37339f4c54998938ce51b9e563a00_Out_1_Float;
            Unity_OneMinus_float(_Multiply_4b76616990ac4bcc9f552ec3ed26051f_Out_2_Float, _OneMinus_a8d37339f4c54998938ce51b9e563a00_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Property_9220f36bb0544a169289b48cd3529146_Out_0_Float = _Foam_Contrast;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Multiply_30144d6ac6004609a47ecc162e12920f_Out_2_Float;
            Unity_Multiply_float_float(_OneMinus_a8d37339f4c54998938ce51b9e563a00_Out_1_Float, _Property_9220f36bb0544a169289b48cd3529146_Out_0_Float, _Multiply_30144d6ac6004609a47ecc162e12920f_Out_2_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float;
            Unity_Saturate_float(_Multiply_30144d6ac6004609a47ecc162e12920f_Out_2_Float, _Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float3 _Lerp_de703bc50d1540859b2099644ec095da_Out_3_Vector3;
            Unity_Lerp_float3(_Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3, float3(1, 1, 1), (_Saturate_5d8909c4272746e8b7d81cbc7784b4c5_Out_1_Float.xxx), _Lerp_de703bc50d1540859b2099644ec095da_Out_3_Vector3);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            float _Fraction_58f43b97b1e34a7aa1d8b3cdf438e1b1_Out_1_Float;
            Unity_Fraction_float(_Multiply_7a1937a8a6a349eab2adbafba9d7c2f2_Out_2_Float, _Fraction_58f43b97b1e34a7aa1d8b3cdf438e1b1_Out_1_Float);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
            #if defined(_DEBUG_DISABLED)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = _Lerp_de703bc50d1540859b2099644ec095da_Out_3_Vector3;
            #elif defined(_DEBUG_VORONOI)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = (_Fraction_58f43b97b1e34a7aa1d8b3cdf438e1b1_Out_1_Float.xxx);
            #elif defined(_DEBUG_NORMALS)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = _NormalStrength_d1a4ede48fea4ee79d80db2a06623e01_Out_2_Vector3;
            #elif defined(_DEBUG_DEPTH)
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = (_Saturate_5fc570ee22ae456d9706683a26e7fbf3_Out_1_Float.xxx);
            #else
            float3 _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3 = _Multiply_08abdb0279724c19a71b1fb85ada1a8c_Out_2_Vector3;
            #endif
            #endif
            surface.BaseColor = _Debug_dc9836c66dee4b16b5207354d4442959_Out_0_Vector3;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceNormal =                          input.normalOS;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpaceTangent =                         input.tangentOS.xyz;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ObjectSpacePosition =                        input.positionOS;
        #endif
        
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.WorldSpacePosition = input.positionWS;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
        #endif
        
        
            #if UNITY_UV_STARTS_AT_TOP
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
        #endif
        
            #else
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
        #endif
        
            #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
        #endif
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        #endif
        
        
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4)
        output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #endif
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphLitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    FallBack "Hidden/Shader Graph/FallbackError"
}