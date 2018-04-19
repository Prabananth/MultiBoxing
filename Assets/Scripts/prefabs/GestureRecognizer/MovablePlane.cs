using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlane : MonoBehaviour {

    private string TagName = "SkeletonBody";
    private void OnTriggerEnter(Collider other)
    {;
        if (other.CompareTag(TagName))
        {
            StartCircle.CanMove = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagName))
        {
            StartCircle.CanMove = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TagName) && !StartCircle.CanMove)
        {
            StartCircle.CanMove = true;
        }
    }
}
