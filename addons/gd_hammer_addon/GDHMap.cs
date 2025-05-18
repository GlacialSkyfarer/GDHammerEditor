using Godot;
public class GDHMap
{

    protected Godot.Collections.Array<BrushData> brushes;

    public GDHMap()
    {

        this.brushes = new();
        this.brushes.Add(new BrushData(Geometry3D.BuildBoxPlanes(Vector3.One)));

    }

}

public class BrushData
{

    public Godot.Collections.Array<Plane> planes;

    public BrushData(Godot.Collections.Array<Plane> planes)
    {

        this.planes = planes;

    }

    public Vector3[] getVertices()
    {

        return Geometry3D.ComputeConvexMeshPoints(planes);

    }

}
