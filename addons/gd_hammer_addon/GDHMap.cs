using Godot;
using Godot.Collections;

public partial class MapData : GodotObject
{

    public Array<BrushData> brushes = new();

}

public partial class BrushData : GodotObject
{



}
public partial class FaceData : GodotObject
{

    public Array<int> UsedVertices = new();
    public string TexturePath;
    public Vector2 UV = Vector2.One;
    public float TextureRotation = 0f;

}
