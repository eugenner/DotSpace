using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DotSpace : MonoBehaviour
{
    public Transform leftController;
    public Transform rightController;

    public Material newPointMaterialRef;
    private List<GameObject> points;

    public GameObject rayLaser;
    public GameObject pointer;

    private bool isLaserOn = false;

    private Button saveBtn;
    private string ouputFile;

    private LineRenderer lr;
    void Start()
    {
        points = new List<GameObject>();
        rayLaser.SetActive(isLaserOn);
        lr = rayLaser.GetComponent<LineRenderer>();
        pointer.SetActive(!isLaserOn);
        saveBtn = GameObject.Find("Button").GetComponent<Button>();
        saveBtn.interactable = false;
    }

    void LateUpdate()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger | OVRInput.Button.SecondaryIndexTrigger))
        {
            if (pointer.activeSelf)
                CreatePoint(pointer.transform.position);
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight))
        {
            RemoveLastPoint();
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp))
        {
            ToggleRayLaser();
        }
    }

    private void ToggleRayLaser()
    {
        isLaserOn = !isLaserOn;

        lr.enabled = false;
        rayLaser.SetActive(isLaserOn);
        if (isLaserOn) StartCoroutine(enableLaserRay(lr));

        pointer.SetActive(!isLaserOn);
        Button btn = GameObject.Find("Button").GetComponent<Button>();
        btn.interactable = isLaserOn;
    }

    IEnumerator enableLaserRay(LineRenderer lr)
    {
        yield return new WaitForSeconds(0.1f);
        lr.enabled = true;
    }

    private void RemoveLastPoint()
    {
        GameObject go = points[points.Count - 1];
        Destroy(go);
        points.RemoveAt(points.Count - 1);
    }

    private void CreatePoint(Vector3 newPointPosition)
    {
        GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        point.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        point.transform.position = newPointPosition;
        MeshRenderer meshRenderer = point.GetComponent<MeshRenderer>();
        meshRenderer.material = newPointMaterialRef;

        points.Add(point);
    }

    public void OnClickSaveBtn()
    {
        ouputFile = Path.Combine(Application.persistentDataPath, "points.obj");
        StreamWriter writer = new StreamWriter(ouputFile, false);
        foreach (GameObject point in points)
        {
            writer.WriteLine("v {0} {1} {2}", point.transform.position.x,
                point.transform.position.z, point.transform.position.y);
        }
        writer.Close();
        ToggleRayLaser();
    }

}
