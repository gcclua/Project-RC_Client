﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class LuaTestProxyWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(LuaTestProxy), typeof(System.Object));
		L.RegFunction("StartUpdateData", StartUpdateData);
		L.RegFunction("OnDestroy", OnDestroy);
		L.RegFunction("OnInit", OnInit);
		L.RegFunction("New", _CreateLuaTestProxy);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("OnUpdateData", get_OnUpdateData, set_OnUpdateData);
		L.RegVar("BagItemDataList", get_BagItemDataList, set_BagItemDataList);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateLuaTestProxy(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				LuaTestProxy obj = new LuaTestProxy();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: LuaTestProxy.New");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StartUpdateData(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			LuaTestProxy obj = (LuaTestProxy)ToLua.CheckObject(L, 1, typeof(LuaTestProxy));
			obj.StartUpdateData();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDestroy(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			LuaTestProxy obj = (LuaTestProxy)ToLua.CheckObject(L, 1, typeof(LuaTestProxy));
			obj.OnDestroy();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnInit(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			LuaTestProxy obj = (LuaTestProxy)ToLua.CheckObject(L, 1, typeof(LuaTestProxy));
			obj.OnInit();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_OnUpdateData(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			LuaTestProxy obj = (LuaTestProxy)o;
			DelegateUtil.VoidDelegate ret = obj.OnUpdateData;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index OnUpdateData on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BagItemDataList(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			LuaTestProxy obj = (LuaTestProxy)o;
			System.Collections.Generic.List<int> ret = obj.BagItemDataList;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index BagItemDataList on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_OnUpdateData(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			LuaTestProxy obj = (LuaTestProxy)o;
			DelegateUtil.VoidDelegate arg0 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg0 = (DelegateUtil.VoidDelegate)ToLua.CheckObject(L, 2, typeof(DelegateUtil.VoidDelegate));
			}
			else
			{
				LuaFunction func = ToLua.ToLuaFunction(L, 2);
				arg0 = DelegateFactory.CreateDelegate(typeof(DelegateUtil.VoidDelegate), func) as DelegateUtil.VoidDelegate;
			}

			obj.OnUpdateData = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index OnUpdateData on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_BagItemDataList(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			LuaTestProxy obj = (LuaTestProxy)o;
			System.Collections.Generic.List<int> arg0 = (System.Collections.Generic.List<int>)ToLua.CheckObject(L, 2, typeof(System.Collections.Generic.List<int>));
			obj.BagItemDataList = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index BagItemDataList on a nil value" : e.Message);
		}
	}
}
