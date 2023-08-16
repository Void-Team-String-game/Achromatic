using UnityEngine;

public class PaintManager : MonoBehaviour
{
    public Transform PaintPrefab;
    public float MaxDecalSize = 0.5f;

    public void CreateDecal(Vector3 position, Vector3 normal)
    {
        Debug.Log("CreateDecal function called!");
        var paintSplatter = GameObject.Instantiate(PaintPrefab, position + normal * 0.01f, Quaternion.LookRotation(normal)) as Transform;

        var scaler = Random.Range(PainterScript.Instance.MinScale, MaxDecalSize);
        scaler = Mathf.Clamp(scaler, PainterScript.Instance.MinScale, MaxDecalSize);

        paintSplatter.localScale = Vector3.one * scaler;
        paintSplatter.up = normal;

        Destroy(paintSplatter.gameObject, 5);

        Debug.Log("Decal created at position: " + position);
    }
}