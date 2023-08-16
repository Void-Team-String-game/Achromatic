using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PainterScript : MonoBehaviour
{
    public static PainterScript Instance;

    public Transform PaintPrefab;
    public int MinSplashs = 5;
    public int MaxSplashs = 15;
    public float SplashRange = 2f;
    public float MinScale = 0.25f;
    public float MaxScale = 0.01f;

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
        if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼을 누르는 동안 계속해서 실행
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // PaintManager 오브젝트의 CreateDecal 함수 호출
                PaintManager paintManager = FindObjectOfType<PaintManager>();
                if (paintManager != null)
                {
                    paintManager.CreateDecal(hit.point, hit.normal);
                }
            }
        }
    }

    public void Paint(Vector3 location, Vector3 normal)
    {
        mHitPoint = location;
        mRaysDebug.Clear();
        mDrawDebug = true;

        int drops = 1;

        for (int n = 0; n < drops; n++)
        {
            var fwd = transform.TransformDirection(Random.onUnitSphere * SplashRange);
            mRaysDebug.Add(new Ray(location, fwd));

            RaycastHit hit;
            if (Physics.Raycast(location, fwd, out hit, SplashRange))
            {
                var paintManager = hit.collider.GetComponent<PaintManager>();
                if (paintManager != null)
                {
                    paintManager.CreateDecal(hit.point, hit.normal);
                }
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