using UnityEngine;

public class PaintManager : MonoBehaviour
{
    public Transform PaintPrefab;
    public float MaxDecalSize = 0.5f;

    public void CreateDecal(Vector3 position, Vector3 normal)
    {
        Debug.Log("CreateDecal function called!");
        var paintSplatter = GameObject.Instantiate(PaintPrefab, position + normal * 0.01f, Quaternion.identity) as Transform;

        var scaler = Random.Range(PainterScript.Instance.MinScale, MaxDecalSize);
        scaler = Mathf.Clamp(scaler, PainterScript.Instance.MinScale, MaxDecalSize);

        paintSplatter.localScale = Vector3.one * scaler;

        // normal 벡터가 x축 방향인 경우
        if (normal == Vector3.right)
        {
            // 데칼을 x축 180도 회전, y축 90도 회전
            paintSplatter.eulerAngles = new Vector3(180f, 90f, paintSplatter.eulerAngles.z);
        }
        else
        {
            // 기존 코드대로 데칼을 normal 방향으로 회전
            paintSplatter.LookAt(position + normal);
        }

        Destroy(paintSplatter.gameObject, 5);

        Debug.Log("Decal created at position: " + position);
    }
}
