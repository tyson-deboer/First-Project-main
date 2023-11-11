using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class WebShooter : MonoBehaviour
{
    [Header("Core Variables")]
    public Transform shooterTip;
    public Rigidbody player;
    public GameObject webEnd;
    public LineRenderer lineRenderer;
    public XRController controller;

    [Header("Web Settings")]
    public float webStrength = 8.5f;
    public float webDamper = 7f;
    public float webMassScale = 4.5f;
    public float webZipStrength = 5f;
    public float maxDistance;
    public LayerMask webLayers;

    private SpringJoint joint;
    private FixedJoint endJoint;
    private Vector3 webPoint;
    private float distanceFromPoint;
    private bool webShot;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        webEnd.transform.parent = null;
    }

    private void Update()
    {
        HandleInput();
        if (webShot && joint)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, shooterTip.position);
            lineRenderer.SetPosition(1, webEnd.transform.position);
        }
    }

    private void HandleInput()
    {
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed))
        {
            if (isPressed && !webShot)
            {
                webShot = true;
                ShootWeb();
            }
            else if (!isPressed && webShot)
            {
                webShot = false;
                StopWeb();
            }
        }
    }
    private void ShootWeb()
    {
        RaycastHit hit;
        if (Physics.Raycast(shooterTip.position, shooterTip.forward, out hit, maxDistance, webLayers))
        {
            webPoint = hit.point;
            webEnd.transform.position = webPoint;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = webPoint;

            if (hit.transform.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                if (rigidbody)
                {
                    joint.connectedAnchor = new Vector3(0, 0, 0);

                    webEnd.GetComponent<Rigidbody>().isKinematic = false;
                    endJoint = webEnd.AddComponent<FixedJoint>();
                    endJoint.connectedBody = rigidbody;

                    joint.connectedBody = webEnd.GetComponent<Rigidbody>();
                }
            }

            if (!rigidbody)
            {
                webEnd.GetComponent<Rigidbody>().isKinematic = true;
            }

            distanceFromPoint = Vector3.Distance(player.transform.position, webPoint) * .75f;
            joint.minDistance = 0;
            joint.maxDistance = distanceFromPoint;

            joint.spring = webStrength;
            joint.damper = webDamper;
            joint.massScale = webMassScale;

        }
    }

    private void StopWeb()
    {
        Destroy(joint);
        if (endJoint) Destroy(endJoint);
        lineRenderer.positionCount = 0;
    }

}
