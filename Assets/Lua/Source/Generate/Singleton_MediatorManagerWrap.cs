//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class Singleton_MediatorManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(Singleton<MediatorManager>), typeof(System.Object), "Singleton_MediatorManager");
		L.RegFunction("GetInstance", GetInstance);
		L.RegFunction("New", _CreateSingleton_MediatorManager);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSingleton_MediatorManager(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				Singleton<MediatorManager> obj = new Singleton<MediatorManager>();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: Singleton<MediatorManager>.New");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInstance(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			MediatorManager o = Singleton<MediatorManager>.GetInstance();
			ToLua.PushObject(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

