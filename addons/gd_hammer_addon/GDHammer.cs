#if TOOLS
using Godot;
using System;

[Tool]
public partial class GDHammer : EditorPlugin
{

    const string MAP_INIT_SCRIPT_PATH = "res://addons/gd_hammer_addon/nodes/MapInit.cs";
    const string MAP_INIT_ICON_PATH = "res://addons/gd_hammer_addon/icons/icn_mapInit.svg";

    public override void _EnterTree()
    {

        Script mapInitScript = GD.Load<Script>(MAP_INIT_SCRIPT_PATH);
        Texture2D mapInitIcon = GD.Load<Texture2D>(MAP_INIT_ICON_PATH);
        AddCustomType("MapInit", "Node3D", mapInitScript, mapInitIcon);

    }

    public override void _ExitTree()
    {
        RemoveCustomType("MapInit");
    }

}
#endif
