Shader "Special FX/FX Dissolve" {
Properties {
	_TintColor ("Tint Color(RGBA)", Color) = (0.5,0.5,0.5,0.5)
	_Bri("Brightness", Range(-1,1) ) = 0
	_Con("Contrast", Range(0,2) ) = 1
	[Enum(Multiply Multiply,0,Add Add,1, Multiply Add,2,Add Multiply,3)] _rgbc("Blending Texture(RGB)",  Float) = 0
	[Enum(Multiply Multiply,0,Add Add,1, Multiply Add,2,Add Multiply,3)] _ac("Blending Alpha(A)", Float) = 0
	[Enum(One,0,Two,1,Three,2)] _numberTex("Number of Texture", Float) = 0
	
	_DslTex("Dissolve Texture", 2D) = "gray" {}
	_DslGl("Global Dissolve", Float) = 0.0	
	_DslEC ("Dissolve Edge Color", Color) = (1, 1, 1, 1)
	_DslEI ("Dissolve Edge Intensity", Range(0,1)) = 1
	_DslER ("Dissolve Edge Range", Range(0,0.2)) = 0.08
	_DslEA ("Dissolve Edge Alpha", Range(0,1)) = 0
	_ScaleDsl("Scale(XY), PivotXY(ZW))", Vector) = (1,1,0.5,0.5)
	[Enum(Off,0,On,1)] _dslTAE("Animation <1>", Float) = 0.0			//togle dissolveTextureAnimationEnable
	_TileDsl("Tile Dissolve Columns(X), Rows(Y), FPS(Z), Frame(W)", Vector) = (1,1,0,0)
	_PanDsl("Pan Dissolve (Speed(XY))", Vector) = (0,0,0,0)
	_RotDsl("Rot Dissolve (Pivot(XY), Angle Speed(Z), Angle(W))", Vector) = (0.5,0.5,0,0)
	
	_MainTex ("Specia FX Texture(RGBA) <1>", 2D) = "white" {}
	_inv1("Invert Texture(RGB) <1>", Range(0,1) ) = 0
	_Alpha1("Alpha Texture(A) <1> [On - Off]", Range(0,1) ) = 0
	_invAlpha1("Invert Alpha Texture(A) <1>", Range(0,1) ) = 0
	_Scale("Scale(XY) <1>, PivotXY(ZW))", Vector) = (1,1,0.5,0.5)
	[Enum(Off,0,On,1)] _fTAE("Animation <1>", Float) = 0.0 			//togle firstTextureAnimationEnable
	_Tile1("Tile <1> Columns(X), Rows(Y), FPS(Z), Frame(W)", Vector) = (1,1,0,0)
	_Pan1("Pan <1> (Speed(XY))", Vector) = (0,0,0,0)
	_Rot1("Rot <1> (Pivot(XY), Angle Speed(Z), Angle(W))", Vector) = (0.5,0.5,0,0)
	
	_MainTex2 ("Specia FX Texture(RGBA) <2>", 2D) = "white" {}
	_inv2("Invert Texture(RGB) <2>", Range(0,1) ) = 0
	_Alpha2("Alpha Texture(A) <2> (On - Off)", Range(0,1) ) = 0
	_invAlpha2("Invert Alpha Texture(A) <2>", Range(0,1) ) = 0
	_Scale2("Scale(XY) <2>, PivotXY(ZW))", Vector) = (1,1,0.5,0.5)
	[Enum(Off,0,On,1)] _sTAE ("Animation <2>", Float) = 0.0 		//togle secondTextureAnimationEnable
	_Tile2("Tile <2> Columns(X), Rows(Y), FPS(Z), Frame(W)", Vector) = (1,1,0,0)
	_Pan2("Pan <2> (Speed(XY))", Vector) = (0,0,0,0)
	_Rot2("Rot <2> (Pivot(XY), Angle Speed(Z), Angle(W))", Vector) = (0.5,0.5,0,0)
	
	_MainTex3 ("Specia FX Texture(RGBA) <3>", 2D) = "white" {}
	_inv3("Invert Texture(RGB) <3>", Range(0,1) ) = 0
	_Alpha3("Alpha Texture(A) <3> (On - Off)", Range(0,1) ) = 0
	_invAlpha3("Invert Alpha Texture(A) <3>", Range(0,1) ) = 0
	_Scale3("Scale(XY) <3>, PivotXY(ZW))", Vector) = (1,1,0.5,0.5)
	[Enum(Off,0,On,1)] _tTAE ("Animation <3>", Float) = 0.0 		//togle thirdTextureAnimationEnable
	_Tile3("Tile <3> Columns(X), Rows(Y), FPS(Z), Frame(W)", Vector) = (1,1,0,0)
	_Pan3("Pan <3> (Speed(XY))", Vector) = (0,0,0,0)
	_Rot3("Rot <3> (Pivot(XY), Angle Speed(Z), Angle(W))", Vector) = (0.5,0.5,0,0)
	
	_InvFade ("Soft Particles Factor", Range(0.01,5.0)) = 5.0
		
	//----------------------------------------------------
	[HideInInspector] _Mode ("__mode", Float) = 0.0 //save blending mode
	[HideInInspector] _SrcBlend ("__src", Float) = 1.0 
	[HideInInspector] _DstBlend ("__dst", Float) = 1.0
	[HideInInspector] _Queue ("__queue", Float) = 3000.0 //save render queue
	
	[HideInInspector] _dslT ("__dslt", Float) = 1.0  	//foldout dissolveTexture
	[HideInInspector] _dslTA("__dslta", Float) = 0.0 	//foldout dissolveTextureAnimation
	[HideInInspector] _fT ("__ft", Float) = 1.0  	//foldout firstTexture
	[HideInInspector] _fTA("__fta", Float) = 0.0 	//foldout firstTextureAnimation
	[HideInInspector] _sT ("__st", Float) = 1.0 	//foldout secondTexture
	[HideInInspector] _sTA ("__sta", Float) = 0.0 	//foldout secondTextureAnimation
	[HideInInspector] _tT ("__st", Float) = 1.0		//foldout thirdTexture
	[HideInInspector] _tTA ("__sta", Float) = 0.0	//foldout thirdTextureAnimation
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend [_SrcBlend] [_DstBlend]
	Cull Off //Back | Front | Off
	ZWrite Off //On | Off

	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			half _Mode;
			fixed _dslTAE;
			fixed _fTAE;
			fixed _sTAE;
			fixed _tTAE;
			
			half _rgbc;
			half _ac;
			fixed _numberTex;
			fixed _Bri;
			fixed _Con;
			
			sampler2D _DslTex;
			half _DslGl;
			fixed4 _DslEC;	
			fixed _DslER;
			fixed _DslEA;
			fixed _DslEI;
			float4 _TileDsl;
			float4 _PanDsl;
			float4 _RotDsl;
			
			sampler2D _MainTex;
			fixed _inv1;
			fixed _Alpha1;
			fixed _invAlpha1;
			float4 _Tile1;
			float4 _Pan1;
			float4 _Rot1;
			
			sampler2D _MainTex2;
			fixed _inv2;
			fixed _Alpha2;
			fixed _invAlpha2;
			float4 _Tile2;
			float4 _Pan2;
			float4 _Rot2;
			
			sampler2D _MainTex3;
			fixed _inv3;
			fixed _Alpha3;
			fixed _invAlpha3;
			float4 _Tile3;
			float4 _Pan3;
			float4 _Rot3;
			
			fixed4 _TintColor;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord2 : TEXCOORD1;
				float2 texcoord3 : TEXCOORD2;
				float2 dslcoord4 : TEXCOORD3;
				UNITY_FOG_COORDS(4)
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD5;
				#endif
			};
			
			float4 _DslTex_ST;
			
			float4 _MainTex_ST;
			float4 _MainTex2_ST;
			float4 _MainTex3_ST;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.texcoord2 = TRANSFORM_TEX(v.texcoord,_MainTex2);
				o.texcoord3 = TRANSFORM_TEX(v.texcoord,_MainTex3);
				o.dslcoord4 = TRANSFORM_TEX(v.texcoord,_DslTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			sampler2D_float _CameraDepthTexture;
			float _InvFade;
			//u = UV, t = Tile, p = Pan, r = Rot
			float2 UV_TPR(float2 u, float4 t, float4 p, float4 r)			
			{
				//Panoraming & Rotating Animation
				float2 v = float2((r.x-((u.x-r.x)*cos((r.z*_Time.y)+(3.14159265359f*(1+(r.w/180))))-(u.y-r.y)*sin((r.z*_Time.y)+(3.14159265359f*(1+(r.w/180))))))+(p.x*_Time.y),(r.y-((u.x-r.x)*sin((r.z*_Time.y)+(3.14159265359f*(1+(r.w/180))))+(u.y-r.y)*cos((r.z*_Time.y)+(3.14159265359f*(1+(r.w/180))))))+(p.y*_Time.y));
				//Tile Animation
				v = float2(v.x/t.x+floor(_Time.y*t.z+t.w)/t.x,(v.y+t.y-1)/t.y-floor(floor(_Time.y*t.z+t.w)/t.x)/t.y);
				return  v;
			}

			//Scale
			float4 _Scale;
			float4 _Scale2;
			float4 _Scale3;
			float4 _ScaleDsl;

			float2 Scale(float2 uv, float4 scale)
			{
				return ((uv - scale.zw)/scale.xy)+scale.zw;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));
				i.color.a *= fade;
				#endif
				
				fixed4 c;
				fixed4 d=0;
				fixed4 e=0;
				
				i.texcoord = Scale(i.texcoord,_Scale);
				if(_fTAE > 0.5)
					c = tex2D(_MainTex,UV_TPR(i.texcoord,_Tile1,_Pan1,_Rot1));
					else c = tex2D(_MainTex,i.texcoord);
				c.rgb = lerp(c.rgb,1-c.rgb,_inv1).rgb; //invert texture
				c.a = lerp(c.a,1-c.a,_invAlpha1); //invert alpha channel
				c.a = lerp(c.a,1,_Alpha1); //On - Off alpha channel
				
				
				if(_numberTex > 0.5)
				{
					i.texcoord2 = Scale(i.texcoord2,_Scale2);
					if(_sTAE > 0.5)				
						d = tex2D(_MainTex2,UV_TPR(i.texcoord2,_Tile2,_Pan2,_Rot2));
						else d = tex2D(_MainTex2,i.texcoord2);
					d.rgb = lerp(d.rgb,1-d.rgb,_inv2).rgb; //invert texture
					d.a = lerp(d.a,1-d.a,_invAlpha2); //invert alpha channel
					d.a = lerp(d.a,1,_Alpha2); //On - Off alpha channel
				}
				
				if(_numberTex > 1.5)
				{
					i.texcoord3 = Scale(i.texcoord3,_Scale3);
					if(_tTAE > 0.5)				
						e = tex2D(_MainTex3,UV_TPR(i.texcoord3,_Tile3,_Pan3,_Rot3));
						else e = tex2D(_MainTex3,i.texcoord3);
					e.rgb = lerp(e.rgb,1-e.rgb,_inv3).rgb; //invert texture
					e.a = lerp(e.a,1-e.a,_invAlpha3); //invert alpha channel
					e.a = lerp(e.a,1,_Alpha3); //On - Off alpha channel
				}
												
				//mul mul		
				if(_rgbc == 0)	{if(_numberTex > 0.5){c.rgb *= d.rgb;}	if(_numberTex > 1.5){c.rgb *= e.rgb;}}
				//add add
				if(_rgbc == 1)	{if(_numberTex > 0.5){c.rgb += d.rgb;}	if(_numberTex > 1.5){c.rgb += e.rgb;}}
				//mul add
				if(_rgbc == 2)	{if(_numberTex > 0.5){c.rgb *= d.rgb;}	if(_numberTex > 1.5){c.rgb += e.rgb;}}
				//add mul
				if(_rgbc == 3)	{if(_numberTex > 0.5){c.rgb += d.rgb;}	if(_numberTex > 1.5){c.rgb *= e.rgb;}}
				
				//mul mul
				if(_ac == 0)	{if(_numberTex > 0.5){c.a *= d.a;}		if(_numberTex > 1.5){c.a *= e.a;}}
				//add add
				if(_ac == 1)	{if(_numberTex > 0.5){c.a += d.a;}		if(_numberTex > 1.5){c.a += e.a;}}
				//mul add
				if(_ac == 2)	{if(_numberTex > 0.5){c.a *= d.a;}		if(_numberTex > 1.5){c.a += e.a;}}
				//add mul
				if(_ac == 3)	{if(_numberTex > 0.5){c.a += d.a;}		if(_numberTex > 1.5){c.a *= e.a;}}
																																																																			
				if(_Mode == 0)//Additive
				{
					c = 2.0f * i.color * _TintColor * c;
					UNITY_APPLY_FOG_COLOR(i.fogCoord, c, fixed4(0,0,0,0));
				}
				if (_Mode == 1)//Aditive Multiply
				{
					c.rgb = i.color.rgb * _TintColor.rgb * c.rgb * 2.0f;
					c.a = (1-c.a) * (_TintColor.a * i.color.a * 2.0f);
					UNITY_APPLY_FOG_COLOR(i.fogCoord, c, fixed4(0,0,0,0));
				}
				if (_Mode == 2)//Additive Soft
				{ 
					c = 2.0f * i.color * _TintColor * c;
					c.rgb *= c.a;
					UNITY_APPLY_FOG_COLOR(i.fogCoord, c, fixed4(0,0,0,0));
				}
				if (_Mode == 3)//Alpha Blend
				{
					c = 2.0f * i.color * _TintColor * c;
					UNITY_APPLY_FOG(i.fogCoord, c);
				}
				if (_Mode == 4)//Blend
				{
					c = i.color * _TintColor * c;
					UNITY_APPLY_FOG_COLOR(i.fogCoord, c, fixed4(0,0,0,0));
				}
				if (_Mode == 5)//Multiply
				{
					c = i.color * _TintColor * c;
					c = lerp(half4(1,1,1,1), c, c.a);
					UNITY_APPLY_FOG_COLOR(i.fogCoord, c, fixed4(1,1,1,1));
				}
				if (_Mode == 6)//Multiply Double
				{
					c.rgb = 2.0f * i.color.rgb * _TintColor.rgb * c.rgb;
					c.a = i.color.a * _TintColor.a * c.a;
					c = lerp(fixed4(0.5f,0.5f,0.5f,0.5f), c, c.a);
					UNITY_APPLY_FOG_COLOR(i.fogCoord, c, fixed4(0.5,0.5,0.5,0.5));
				}
				if (_Mode == 7)//Alpha Blended Premultiply
				{
					c = i.color * _TintColor * c * i.color.a;
				}
								
				//Dissolve
				float4 dt;
				float er;
				i.dslcoord4 = Scale(i.dslcoord4,_ScaleDsl);
				if(_dslTAE > 0.5)
					dt = tex2D(_DslTex,UV_TPR(i.dslcoord4,_TileDsl,_PanDsl,_RotDsl));
					else dt = tex2D(_DslTex, i.dslcoord4);
					
				dt.a = er = (dt.r+dt.g+dt.b)/3;
				dt.a = _DslGl >= dt.a ? 0 : 1;//Calculate dissolve
				er = _DslGl+_DslER >= er ? 0 : 1;//Calculate dissolve edge range
				//Apply Dissolve Edge & intensity				
				c.rgb = lerp(lerp(c.rgb,_DslEC.rgb*lerp(c.a,1,_DslEA),_DslEI),c.rgb,er) * dt.a;
				c.a *= dt.a;
				
				c.rgb = (1-(1-(c.rgb*_Con))*_Con + _Bri) * c.a;//Brightness, Contrast
				return c;
			}
			ENDCG 
		}
	}	
}
CustomEditor "FxShaderGUI"
}
