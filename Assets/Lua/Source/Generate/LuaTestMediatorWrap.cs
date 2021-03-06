//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class LuaTestMediatorWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(LuaTestMediator), typeof(System.Object));
		L.RegFunction("UpdateItems", UpdateItems);
		L.RegFunction("OnInit", OnInit);
		L.RegFunction("GetBagItemData", GetBagItemData);
		L.RegFunction("UseItem", UseItem);
		L.RegFunction("UseAllItems", UseAllItems);
		L.RegFunction("OnDestroy", OnDestroy);
		L.RegFunction("New", _CreateLuaTestMediator);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateLuaTestMediator(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				LuaTestMediator obj = new LuaTestMediator();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: LuaTestMediator.New");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UpdateItems(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			LuaTestMediator obj = (LuaTestMediator)ToLua.CheckObject(L, 1, typeof(LuaTestMediator));
			object arg0 = ToLua.ToVarObject(L, 2);
			obj.UpdateItems(arg0);
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
			LuaTestMediator obj = (LuaTestMediator)ToLua.CheckObject(L, 1, typeof(LuaTestMediator));
			obj.OnInit();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetBagItemData(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			LuaTestMediator obj = (LuaTestMediator)ToLua.CheckObject(L, 1, typeof(LuaTestMediator));
			LuaInterface.LuaTable o = obj.GetBagItemData();
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UseItem(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			LuaTestMediator obj = (LuaTestMediator)ToLua.CheckObject(L, 1, typeof(LuaTestMediator));
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.UseItem(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UseAllItems(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			LuaTestMediator obj = (LuaTestMediator)ToLua.CheckObject(L, 1, typeof(LuaTestMediator));
			obj.UseAllItems();
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
			LuaTestMediator obj = (LuaTestMediator)ToLua.CheckObject(L, 1, typeof(LuaTestMediator));
			obj.OnDestroy();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

