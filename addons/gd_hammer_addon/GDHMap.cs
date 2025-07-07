using Godot;
using Godot.Collections;

public partial class MapData : GodotObject
{

    public Array<BrushData> brushes = new();

}

public partial class BrushData : GodotObject
{

    public BrushData(Array<FaceData> faces) {

        this.Faces = faces;

    }

    public Array<FaceData> Faces = new();

}
public partial class FaceData : GodotObject
{

    public Array<Vector3> Vertices = new();
    public string TexturePath;
    public Vector2 UV = Vector2.One;
    public float TextureRotation = 0f;

}
