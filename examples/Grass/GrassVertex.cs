using System.Numerics;

namespace Grass;

struct GrassVertex
{
    public Vector3 Position;
    public float RandomValue;
    public Vector2 Pivot;
    public float PivotWeight;
    public float height;

    public GrassVertex(Vector3 position, float randomValue, Vector2 pivot, float pivotWeight, float height)
    {
        this.Position = position;
        this.RandomValue = randomValue;
        this.Pivot = pivot;
        this.PivotWeight = pivotWeight;
        this.height = height;
    }
}