/////////////////      Indicate ghost location

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject tarGhost;

    private void Update()
    {
        // transform.LookAt(tarGhost.transform); 
        transform.right = tarGhost.transform.position - transform.position; 
    }
}
