using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PainterScript : MonoBehaviour
{
    public static PainterScript Instance;

    public Transform PaintPrefab;
    public GameObject HeadMuzzlePrefab;
    public GameObject MuzzlePrefab;
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

    public void ShootPaint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // PaintManager 오브젝트의 CreateDecal 함수 호출
            PaintManager paintManager = FindObjectOfType<PaintManager>();
            if (paintManager != null)
            {
                if (hit.transform.GetComponent<Mob>() != null)
                {
                    bool head = false;
                    RaycastHit[] headhitfinder = { };
                    headhitfinder = Physics.RaycastAll(ray, Mathf.Infinity);

                    for (int i = 0; i < headhitfinder.Length; i++)
                    {
                        if (headhitfinder[i].transform.name == "Head")
                        {
                            head = true;
                        }
                    }
                    GameObject ImpactGo;
                    if (head == true)
                    {
                        Debug.Log("Head Effect");
                        ImpactGo = Instantiate(HeadMuzzlePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                    else ImpactGo = Instantiate(MuzzlePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(ImpactGo, 0.5f);
                }
                else
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