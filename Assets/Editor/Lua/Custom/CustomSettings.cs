﻿#define USING_DOTWEENING

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using LuaInterface;

using BindType = ToLuaMenu.BindType;
using System.Reflection;
using DG.Tweening;

public static class CustomSettings
{
    public static string saveDir = Application.dataPath + "/Lua/Source/Generate/";
    public static string toluaBaseType = Application.dataPath + "/Lua/ToLua/BaseType/";

    //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
    //unity 有些类作为sealed class, 其实完全等价于静态类
    public static List<Type> staticClassTypes = new List<Type>
    {
        typeof(UnityEngine.Application),
        typeof(UnityEngine.Time),
        typeof(UnityEngine.Screen),
        typeof(UnityEngine.SleepTimeout),
        typeof(UnityEngine.Input),
        typeof(UnityEngine.Resources),
        typeof(UnityEngine.Physics),
        typeof(UnityEngine.RenderSettings),
        typeof(UnityEngine.QualitySettings),
        typeof(UnityEngine.GL),
    };

    //附加导出委托类型(在导出委托时, customTypeList 中牵扯的委托类型都会导出， 无需写在这里)
    public static DelegateType[] customDelegateList =
    {
        _DT(typeof(Action)),
        _DT(typeof(Action<int>)),
        _DT(typeof(Action<GameObject>)),

        _DT(typeof(UnityEngine.Events.UnityAction)),
        _DT(typeof(System.Predicate<int>)),
        _DT(typeof(System.Comparison<int>)),

        _DT(typeof(DelegateHelper.VoidDelegate)),
        _DT(typeof(DelegateHelper.IntDelegate)),
        _DT(typeof(DelegateHelper.FloatDelegate)),
        _DT(typeof(DelegateHelper.StringDelegate)),
        _DT(typeof(DelegateHelper.TableDelegate)),

        _DT(typeof(UIEventListener.VoidDelegate)),
        _DT(typeof(UIEventListener.BoolDelegate)),
        _DT(typeof(UIEventListener.FloatDelegate)),
        _DT(typeof(UIEventListener.VectorDelegate)),
        _DT(typeof(UIEventListener.ObjectDelegate)),
        _DT(typeof(UIEventListener.KeyCodeDelegate)),

        _DT(typeof(TweenCallback)),
    };

    //在这里添加你要导出注册到lua的类型列表
    public static BindType[] customTypeList =
    {
        //------------------------为例子导出--------------------------------
        //_GT(typeof(TestEventListener)),
        //_GT(typeof(TestProtol)),
        //_GT(typeof(TestAccount)),
        //_GT(typeof(Dictionary<int, TestAccount>)).SetLibName("AccountMap"),
        //_GT(typeof(KeyValuePair<int, TestAccount>)),    
        //_GT(typeof(TestExport)),
        //_GT(typeof(TestExport.Space)),
        //-------------------------------------------------------------------        

        _GT(typeof(Debugger)).SetNameSpace(null),

#if USING_DOTWEENING
            _GT(typeof(DG.Tweening.DOTween)),
            _GT(typeof(DG.Tweening.Tween)).SetBaseType(typeof(System.Object)).AddExtendType(typeof(DG.Tweening.TweenExtensions)),
            _GT(typeof(DG.Tweening.Sequence)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
            _GT(typeof(DG.Tweening.Tweener)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
            _GT(typeof(DG.Tweening.LoopType)),
            _GT(typeof(DG.Tweening.PathMode)),
            _GT(typeof(DG.Tweening.PathType)),
            _GT(typeof(DG.Tweening.RotateMode)),
            _GT(typeof(Component)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(Transform)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(Light)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(Material)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(Rigidbody)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(Camera)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(AudioSource)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(LineRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
            _GT(typeof(TrailRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),    
    #else

        _GT(typeof(Component)),
        _GT(typeof(Transform)),
        _GT(typeof(Material)),
        _GT(typeof(Light)),
        _GT(typeof(Rigidbody)),
        _GT(typeof(Camera)),
        _GT(typeof(AudioSource)),
        _GT(typeof(Resources)),
        _GT(typeof(LineRenderer))
        _GT(typeof(TrailRenderer))
#endif

        _GT(typeof(Behaviour)),
        _GT(typeof(MonoBehaviour)),
        _GT(typeof(GameObject)),
        _GT(typeof(TrackedReference)),
        _GT(typeof(Application)),
        _GT(typeof(Physics)),
        _GT(typeof(Collider)),
        _GT(typeof(Time)),
        _GT(typeof(Texture)),
        _GT(typeof(Texture2D)),
        _GT(typeof(Shader)),
        _GT(typeof(Renderer)),
        _GT(typeof(WWW)),
        _GT(typeof(Screen)),
        _GT(typeof(CameraClearFlags)),
        _GT(typeof(AudioClip)),
        _GT(typeof(AssetBundle)),
        _GT(typeof(ParticleSystem)),
        _GT(typeof(AsyncOperation)).SetBaseType(typeof(System.Object)),
        _GT(typeof(LightType)),
        _GT(typeof(SleepTimeout)),
        _GT(typeof(Animator)),
        _GT(typeof(Input)),
        _GT(typeof(KeyCode)),
        _GT(typeof(SkinnedMeshRenderer)),
        _GT(typeof(Space)),

        _GT(typeof(MeshRenderer)),
        _GT(typeof(ParticleEmitter)),
        _GT(typeof(ParticleRenderer)),
        _GT(typeof(ParticleAnimator)),

        _GT(typeof(BoxCollider)),
        _GT(typeof(MeshCollider)),
        _GT(typeof(SphereCollider)),
        _GT(typeof(CharacterController)),
        _GT(typeof(CapsuleCollider)),

        _GT(typeof(Animation)),
        _GT(typeof(AnimationClip)).SetBaseType(typeof(UnityEngine.Object)),
        _GT(typeof(AnimationState)),
        _GT(typeof(AnimationBlendMode)),
        _GT(typeof(QueueMode)),
        _GT(typeof(PlayMode)),
        _GT(typeof(WrapMode)),

        _GT(typeof(QualitySettings)),
        _GT(typeof(RenderSettings)),
        _GT(typeof(BlendWeights)),
        _GT(typeof(RenderTexture)),

        //GUI
        _GT(typeof(GUILayout)),
        _GT(typeof(GUILayoutOption)),
        _GT(typeof(GUIStyle)),
        _GT(typeof(GUIContent)),

        //NGUI
        _GT(typeof(UIPanel)),
        _GT(typeof(UIScrollView)),
        _GT(typeof(UIEventListener)),
        _GT(typeof(UICamera)),
        _GT(typeof(UILabel)),
        _GT(typeof(UIGrid)),
        _GT(typeof(UIButton)),
        _GT(typeof(UIToggle)),
        _GT(typeof(UIGrid.Arrangement)),
        _GT(typeof(NGUITools)),
        _GT(typeof(EventDelegate)),

        //Lua framework
        _GT(typeof(LuaOutlet)),
        _GT(typeof(LuaHelper)),

        //Utils
        _GT(typeof(ProxyManager)),
        _GT(typeof(MediatorManager)),
        _GT(typeof(EventManager)),
        _GT(typeof(UIManager)),
        _GT(typeof(UIItem)),
        _GT(typeof(MySceneManager)),
        _GT(typeof(SceneId)),
        _GT(typeof(Game)),
        _GT(typeof(TimeHelper)),
        _GT(typeof(TimeHelper.ClockTime)),

        //Game module
        _GT(typeof(LuaTestProxy)),
        _GT(typeof(LuaTestMediator)),
        _GT(typeof(LuaTestMediator2)),
        _GT(typeof(LuaTestMediator3)),
        _GT(typeof(LuaTestMediator4)),
        _GT(typeof(MainUIMediator)),
        _GT(typeof(MainResourceMediator)),
        _GT(typeof(LoginMediator)),

        //World
        _GT(typeof(WorldMediator)),
        _GT(typeof(WorldController)),

		//City
		_GT(typeof(CameraMove)),
        _GT(typeof(HighlighterController)),

		//Dotween
		_GT(typeof(DG.Tweening.Ease))

    };

    //public static BindType[] customTypeList
    //{
    //    get
    //    {
    //        return Assembly.Load("UnityEngine").GetExportedTypes().Concat(Assembly.Load("UnityEngine.UI").GetExportedTypes()).Concat(Assembly.Load("Assembly-CSharp").GetExportedTypes()).Select(item => _GT(item)).ToArray();
    //    }
    //}

    public static List<Type> dynamicList = new List<Type>()
    {
        typeof(MeshRenderer),
        typeof(ParticleEmitter),
        typeof(ParticleRenderer),
        typeof(ParticleAnimator),

        typeof(BoxCollider),
        typeof(MeshCollider),
        typeof(SphereCollider),
        typeof(CharacterController),
        typeof(CapsuleCollider),

        typeof(Animation),
        typeof(AnimationClip),
        typeof(AnimationState),

        typeof(BlendWeights),
        typeof(RenderTexture),
        typeof(Rigidbody),
    };

    //重载函数，相同参数个数，相同位置out参数匹配出问题时, 需要强制匹配解决
    //使用方法参见例子14
    public static List<Type> outList = new List<Type>()
    {

    };

    public static BindType _GT(Type t)
    {
        return new BindType(t);
    }

    public static DelegateType _DT(Type t)
    {
        return new DelegateType(t);
    }
}
