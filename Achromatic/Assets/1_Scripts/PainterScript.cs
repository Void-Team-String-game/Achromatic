using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Generate paint decals
/// </summary>
public class PainterScript : MonoBehaviour
{
    public static PainterScript Instance;

    public Transform PaintPrefab;
    
    private float SplashRange = 2f;

    private float MinScale = 0.25f;
    private float MaxScale = 0.01f; // 최대 크기를 0.01로 설정

    private bool mDrawDebug;
    private Vector3 mHitPoint;
    private List<Ray> mRaysDebug = new List<Ray>();

    void Awake()
    {
        if (Instance != null)
            Debug.LogError("More than one Painter has been instanciated in this scene!");
        Instance = this;

        if (PaintPrefab == null)
            Debug.LogError("Missing Paint decal prefab!");
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Paint(hit.point + hit.normal * 0.01f, hit.normal);
            }
        }
    }

    public void Paint(Vector3 location, Vector3 normal)
    {
        mHitPoint = location;
        mRaysDebug.Clear();
        mDrawDebug = true;

        RaycastHit hit;
        int drops = 1; // Generate only one decal

        for (int n = 0; n < drops; n++)
        {
            var fwd = transform.TransformDirection(Random.onUnitSphere * SplashRange);
            mRaysDebug.Add(new Ray(location, fwd));

            if (Physics.Raycast(location, fwd, out hit, SplashRange))
            {
                var paintSplatter = GameObject.Instantiate(PaintPrefab, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(normal)) as Transform;

                var scaler = Random.Range(MinScale, MaxScale);
                scaler = Mathf.Clamp(scaler, MinScale, MaxScale);

                paintSplatter.localScale = Vector3.one * scaler;
                paintSplatter.up = hit.normal;

                Destroy(paintSplatter.gameObject, 5); // Destroy the decal after 5 seconds

                // 출력
                Debug.Log("Hit Point Euler Angles: " + paintSplatter.eulerAngles);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (mDrawDebug)
        {
            Gizmos.DrawSphere(mHitPoint, 0.2f);
            foreach (var r in mRaysDebug)
            {
                Gizmos.DrawRay(r);
            }
        }
    }
}
