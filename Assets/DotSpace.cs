using System;
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


    private Button saveBtn;
    private string ouputFile;

    private bool isDemo = false;
    private float startTime;
    private float nextTime = 0;

    void Start()
    {
        points = new List<GameObject>();
        rayLaser.SetActive(false);
        pointer.SetActive(true);
        saveBtn = GameObject.Find("Button").GetComponent<Button>();
        saveBtn.interactable = false;

        if (isDemo)
        {
            startTime = Time.time;
            nextTime = nextTime + startTime;
            Debug.Log("Demo started at " + startTime);
            rightController.position = Vector3.forward / 4f;
            leftController.position = Vector3.forward / 5f;
        }
    }

    private void FixedUpdate()
    {
        if(isDemo)
        {
            Debug.Log("Demo time: " + Time.time);
            rightController.transform.Translate(Vector3.right * 0.1f * Time.fixedDeltaTime);

            float val = Mathf.Floor(10 * (Time.time - startTime)) / 10;
            if(nextTime < Time.time)
            {
                // TODO create point
                nextTime = Time.time + 0.4f;
                CreatePoint(pointer.transform.position);
            }

            if(Time.time > startTime + 3.5f)
            {
                isDemo = false;
            }

        } else
        {
            if (Time.time > startTime + 4.5f)
            {
                Button btn = GameObject.Find("Button").GetComponent<Button>();
                btn.interactable = true;
            }
            Debug.Log("Demo was stopped");
        }
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
        rayLaser.SetActive(!rayLaser.activeSelf);
        pointer.SetActive(!pointer.activeSelf);
        Button btn = GameObject.Find("Button").GetComponent<Button>();
        btn.interactable = rayLaser.activeSelf;
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
