﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
[CustomEditor(typeof(LuaOutlet))]
public class LuaOutletEditor : Editor
{
    /// <summary>
    /// mark all Game objects that has Outlet, for GUIhierarchyWindow mark
    /// </summary>
    static Dictionary<GameObject, string> _outletObjects = new Dictionary<GameObject, string>();

    static LuaOutletEditor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
    }

    private static void HierarchyItemCB(int instanceid, Rect selectionrect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
        if (obj != null)
        {
            if (_outletObjects.ContainsKey(obj))
            {
                Rect r = new Rect(selectionrect);
                r.x = r.width - 80;
                r.width = 80;
                var style = new GUIStyle();
                style.normal.textColor = Color.green;
                style.hover.textColor = Color.cyan;
                GUI.Label(r, string.Format("=>'{0}'", _outletObjects[obj]), style);
            }
        }
    }

    private Object SettedLuaFile;

    private void SetLuaName()
    {
        var outlet = target as LuaOutlet;
        var luaPath = AssetDatabase.GetAssetPath(outlet.m_LuaFile);
        if (!string.IsNullOrEmpty(luaPath))
        {
            var index = luaPath.LastIndexOf("/");
            if (index >= 0)
            {
                luaPath = luaPath.Substring(index + 1);
            }
            if (luaPath.EndsWith(".lua"))
            {
                luaPath = luaPath.Substring(0, luaPath.Length - ".lua".Length);
            }

            outlet.m_LuaName = luaPath;
            SettedLuaFile = outlet.m_LuaFile;

            if (PrefabUtility.GetPrefabType(outlet) == PrefabType.None)
            {
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            else
            {
                EditorUtility.SetDirty(outlet);
            }

            return;
        }

        outlet.m_LuaName = "";
    }

    GUIStyle GreenFont;
    GUIStyle RedFont;
    private static string LuaFilePathSymbol = "Lua/";

    private HashSet<string> _cachedPropertyNames = new HashSet<string>();

    void OnEnable()
    {
        GreenFont = new GUIStyle();
        GreenFont.fontStyle = FontStyle.Bold;
        GreenFont.fontSize = 11;
        GreenFont.normal.textColor = Color.green;
        RedFont = new GUIStyle();
        RedFont.fontStyle = FontStyle.Bold;
        RedFont.fontSize = 11;
        RedFont.normal.textColor = Color.red;
    }

    public override void OnInspectorGUI()
    {
        _cachedPropertyNames.Clear();

        EditorGUI.BeginChangeCheck();

        var outlet = target as LuaOutlet;

        //Show outlet name.
        outlet.m_LuaName = EditorGUILayout.TextField("Lua name:", outlet.m_LuaName);

        var outletLuaFile = (Object)EditorGUILayout.ObjectField("Lua file:", outlet.m_LuaFile, typeof(Object), true);
        if (outletLuaFile != null && outlet.m_LuaFile != outletLuaFile)
        {
            outlet.m_LuaFile = outletLuaFile;
            SetLuaName();
        }

        if (outlet.OutletInfos == null || outlet.OutletInfos.Count == 0)
        {
            if (GUILayout.Button("Add New Outlet"))
            {
                if (outlet.OutletInfos == null)
                    outlet.OutletInfos = new List<LuaOutlet.OutletInfo>();

                Undo.RecordObject(target, "Add OutletInfo");
                outlet.OutletInfos.Add(new LuaOutlet.OutletInfo());
            }
        }
        else
        {
            // outlet ui edit
            for (var j = outlet.OutletInfos.Count - 1; j >= 0; j--)
            {
                var currentTypeIndex = -1;
                var outletInfo = outlet.OutletInfos[j];
                string[] typesOptions = new string[0];

                var isValid = outletInfo.Object != null && !_cachedPropertyNames.Contains(outletInfo.Name);
                // check duplicate property name
                _cachedPropertyNames.Add(outletInfo.Name);

                if (outletInfo.Object != null)
                {
                    if (outletInfo.Object is GameObject)
                    {
                        currentTypeIndex = 0;// give it default
                        var gameObj = outletInfo.Object as GameObject;
                        var components = gameObj.GetComponents<Component>();

                        _outletObjects[gameObj] = outletInfo.Name;

                        typesOptions = new string[components.Length + 1];
                        for (var i = 0; i < components.Length; i++)
                        {
                            var com = components[i];
                            var typeName = typesOptions[i] = com.GetType().Name;
                            if (typeName == outletInfo.ComponentType)
                            {
                                currentTypeIndex = i;
                            }
                        }

                        var gbType = typesOptions[components.Length] = "GameObject";
                        if (gbType == outletInfo.ComponentType)
                        {
                            currentTypeIndex = components.Length;
                        }
                    }
                }

                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(string.Format("Lua Property: '{0}'", outletInfo.Name), isValid ? GreenFont : RedFont);
                EditorGUILayout.Space();
                if (GUILayout.Button("+"))
                {
                    Undo.RecordObject(target, "Insert OutletInfo");
                    outlet.OutletInfos.Insert(j, new LuaOutlet.OutletInfo());
                }
                if (GUILayout.Button("-"))
                {

                    Undo.RecordObject(target, "Remove OutletInfo");
                    outlet.OutletInfos.RemoveAt(j);
                }
                EditorGUILayout.EndHorizontal();

                outletInfo.Name = EditorGUILayout.TextField("Name:", outletInfo.Name);
                var outletObject = (GameObject)EditorGUILayout.ObjectField("GameObject:", outletInfo.Object, typeof(UnityEngine.GameObject), true);

                if (outletObject != null && outletObject != outletInfo.Object)
                {
                    outletInfo.Object = outletObject;
                    outletInfo.Name = outletObject.name;
                }

                if (currentTypeIndex >= 0)
                {
                    var typeIndex = EditorGUILayout.Popup("Component:", currentTypeIndex, typesOptions);
                    outletInfo.ComponentType = typesOptions[typeIndex].ToString();
                }
            }
        }
        //base.OnInspectorGUI ();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "GUI Change Check");
        }
    }
}
