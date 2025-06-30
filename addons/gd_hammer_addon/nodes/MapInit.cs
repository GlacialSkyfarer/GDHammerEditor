using Godot;
using Godot.Collections;

[Tool]
public partial class MapInit : Node3D
{

    public GDHMap map = new();

    public override void _Ready()
    {

        map.brushes.Add(new BrushData(new Array<BrushFace> {
                new BrushFace(new Plane(Vector3.Up, 5)),
                new BrushFace(new Plane(Vector3.Forward, 1)),
                new BrushFace(new Plane(Vector3.Right, 1)),
                new BrushFace(new Plane(Vector3.Left, 1)),
                new BrushFace(new Plane(Vector3.Down, 1)),
                new BrushFace(new Plane(Vector3.Back, 1)),
                new BrushFace(new Plane(new Vector3(1, 0, 1), 0.75f))
            }));

        MeshInstance3D test = new();
        AddChild(test);
        test.Mesh = map.brushes[0].BuildMesh();

    }

}
