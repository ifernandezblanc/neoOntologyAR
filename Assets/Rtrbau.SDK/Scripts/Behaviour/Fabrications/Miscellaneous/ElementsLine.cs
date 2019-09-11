/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 29/08/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections.Generic;
using UnityEngine;
#endregion NAMESPACES

public class ElementsLine : MonoBehaviour
{
    #region INITIALISATION_VARIABLES
    public GameObject start;
    public GameObject end;
    public Material lineMaterial;
    #endregion INITIALISATION_VARIABLES

    #region MONOBEHAVIOUR_METHODS
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start != null && end != null)
        {
            start.gameObject.GetComponent<LineRenderer>().positionCount = 2;
            Vector3 startPoint = start.transform.position;
            Vector3 endPoint = end.transform.position;
            start.gameObject.GetComponent<LineRenderer>().SetPosition(0, startPoint);
            start.gameObject.GetComponent<LineRenderer>().SetPosition(1, endPoint);
        }
        else
        {
            start.gameObject.GetComponent<LineRenderer>().positionCount = 0;
        }
    }
    #endregion MONOBEHAVIOUR_METHODS

    #region INITIALISATION_METHODS
    public void Initialise(GameObject lineStart, GameObject lineEnd, Material lineMaterial)
    {
        if (lineStart != null && lineEnd != null)
        {
            // Reference line starting and ending game objects
            start = lineStart;
            end = lineEnd;
            // Set line widht at 10% of starting element consult panel
            // float width = start.transform.localScale.x * 0.01f;
            float width = 0.005f;
            // Add line renderer to starting game object
            start.gameObject.AddComponent<LineRenderer>();
            start.gameObject.GetComponent<LineRenderer>().useWorldSpace = true;
            start.gameObject.GetComponent<LineRenderer>().material = lineMaterial;
            start.gameObject.GetComponent<LineRenderer>().startWidth = width;
            start.gameObject.GetComponent<LineRenderer>().endWidth = width;
        }
    }

    public void UpdateLineEnd(GameObject lineEnd)
    {
        end = lineEnd;
    }
    #endregion INITIALISATION_METHODS
}
