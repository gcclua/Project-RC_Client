//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class LoginMediatorWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(LoginMediator), typeof(System.Object));
		L.RegFunction("ConnectToServer", ConnectToServer);
		L.RegFunction("EnterGame", EnterGame);
		L.RegFunction("OnInit", OnInit);
		L.RegFunction("OnDestroy", OnDestroy);
		L.RegFunction("New", _CreateLoginMediator);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("m_View", get_m_View, set_m_View);
		L.RegVar("m_UIName", get_m_UIName, set_m_UIName);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateLoginMediator(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				LoginMediator obj = new LoginMediator();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: LoginMediator.New");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ConnectToServer(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			LoginMediator obj = (LoginMediator)ToLua.CheckObject(L, 1, typeof(LoginMediator));
			string arg0 = ToLua.CheckString(L, 2);
			string arg1 = ToLua.CheckString(L, 3);
			string arg2 = ToLua.CheckString(L, 4);
			obj.ConnectToServer(arg0, arg1, arg2);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EnterGame(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			LoginMediator obj = (LoginMediator)ToLua.CheckObject(L, 1, typeof(LoginMediator));
			obj.EnterGame();
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
			LoginMediator obj = (LoginMediator)ToLua.CheckObject(L, 1, typeof(LoginMediator));
			obj.OnInit();
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
			LoginMediator obj = (LoginMediator)ToLua.CheckObject(L, 1, typeof(LoginMediator));
			obj.OnDestroy();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_View(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			LoginMediator obj = (LoginMediator)o;
			LuaInterface.LuaTable ret = obj.m_View;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index m_View on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_UIName(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			LoginMediator obj = (LoginMediator)o;
			string ret = obj.m_UIName;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index m_UIName on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_View(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			LoginMediator obj = (LoginMediator)o;
			LuaTable arg0 = ToLua.CheckLuaTable(L, 2);
			obj.m_View = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index m_View on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_UIName(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			LoginMediator obj = (LoginMediator)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.m_UIName = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index m_UIName on a nil value" : e.Message);
		}
	}
}

