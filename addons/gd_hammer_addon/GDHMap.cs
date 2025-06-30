using Godot;
using Godot.Collections;

public partial class GDHMap : GodotObject
{

    public Array<BrushData> brushes = new();

}

public partial class BrushData : GodotObject
{


    public Array<BrushFace> Faces;

    public BrushData(Array<BrushFace> faces)
    {

        this.Faces = faces;

    }

    public ArrayMesh BuildMesh()
    {

        SurfaceTool st = new();
        ArrayMesh result = new();

        for (int i = 0; i < Faces.Count; i++)
        {

            BrushFace face = Faces[i];

            Array<Vector3> winding = face.GetBaseWinding();

            for (int k = 0; k < Faces.Count; k++)
            {

                winding = ClipWinding(winding, Faces[k].Plane);

            }

            Vector3 commonPoint = winding[0];

            st.Begin(Mesh.PrimitiveType.Triangles);

            for (int k = 1; k < winding.Count - 1; k++)
            {

                st.SetNormal(face.Plane.Normal);
                st.AddVertex(commonPoint);
                st.SetNormal(face.Plane.Normal);
                st.AddVertex(winding[k + 1]);
                st.SetNormal(face.Plane.Normal);
                st.AddVertex(winding[k]);

            }

            st.Commit(result);

        }

        return result;

    }

    public static Array<Vector3> ClipWinding(Array<Vector3> winding, Plane plane)
    {

        const int SIDE_BACK = 0;
        const int SIDE_FRONT = 1;
        const int SIDE_ON = 2;

        int[] sides = new int[winding.Count + 1];
        float[] distances = new float[winding.Count + 1];

        int i = 0;
        for (; i < winding.Count; i++)
        {

            float distance = plane.Normal.Dot(winding[i]) - plane.D;
            distances[i] = distance;


            if (distance > float.Epsilon)
            {

                sides[i] = SIDE_BACK;

            }
            else if (distance < -float.Epsilon)
            {

                sides[i] = SIDE_FRONT;

            }
            else
            {

                sides[i] = SIDE_ON;

            }

        }

        Array<Vector3> finalWinding = new();

        sides[i] = sides[0];
        distances[i] = distances[0];

        for (int k = 0; k < winding.Count; k++)
        {

            Vector3 current = winding[k];

            if (sides[k] == SIDE_ON)
            {
                finalWinding.Add(current);
                continue;
            }
            if (sides[k] == SIDE_FRONT)
            {
                finalWinding.Add(current);
            }

            if (sides[k + 1] == SIDE_ON || sides[k] == sides[k + 1])
            {
                continue;
            }

            Vector3 next = winding[(k + 1) % winding.Count];
            float t = distances[k] / (distances[k] - distances[k + 1]);

            Vector3 between = next - current;
            between *= t;

            Vector3 split = current + between;
            finalWinding.Add(split);

        }

        return finalWinding;

    }

}

public partial class BrushFace : GodotObject
{

    //TODO: This should be replaced with actual map size bounds.
    const int TEMP_BOUNDS = 10000;

    public Plane Plane;
    //TODO: actual texture handling
    public string Texture;
    public UVAxis UAxis;
    public UVAxis VAxis;
    public bool Draw;

    public BrushFace(Plane plane, string texture = "nil", UVAxis uAxis = null, UVAxis vAxis = null, bool draw = true)
    {

        this.Plane = plane;
        this.Texture = texture;
        this.UAxis = uAxis;
        this.VAxis = vAxis;
        this.Draw = draw;

    }

    public Array<Vector3> GetBaseWinding()
    {

        Vector3 upVector;
        Vector3 rightVector;
        Vector3 normalVector;

        (rightVector, upVector, normalVector) = GetPlaneAxes(Plane.Normal);

        Vector3 offset = Plane.Normal * Plane.D;

        Array<Vector3> winding = new();

        winding.Add(-rightVector * TEMP_BOUNDS + upVector * TEMP_BOUNDS + offset);
        winding.Add(-rightVector * TEMP_BOUNDS - upVector * TEMP_BOUNDS + offset);
        winding.Add(rightVector * TEMP_BOUNDS - upVector * TEMP_BOUNDS + offset);
        winding.Add(rightVector * TEMP_BOUNDS + upVector * TEMP_BOUNDS + offset);

        return winding;

    }


    protected Vector3 GetGlobalUp(Vector3 normal)
    {

        const int Y_AXIS = 1;

        int axis = -1;
        float max = -float.MaxValue;

        for (int i = 0; i < 3; i++)
        {

            float absolute = Mathf.Abs(normal[i]);

            if (absolute > max)
            {

                max = absolute;
                axis = i;

            }

        }

        if (axis == -1) return Vector3.Zero;
        if (axis == Y_AXIS) return Vector3.Forward;
        return Vector3.Up;

    }

    protected (Vector3, Vector3, Vector3) GetPlaneAxes(Vector3 normal)
    {

        Vector3 upVector;
        Vector3 rightVector;
        Vector3 normalVector;

        normalVector = normal;
        rightVector = normal.Cross(GetGlobalUp(normal));
        upVector = normal.Cross(rightVector);

        return (rightVector, upVector, normalVector);

    }

}

public class UVAxis
{

    public Vector3 Axis;
    public float Scale;
    public float Offset;

}
