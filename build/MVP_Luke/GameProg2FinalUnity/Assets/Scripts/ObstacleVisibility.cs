using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstacleVisibility : MonoBehaviour
{
    MeshRenderer m_Renderer;
    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
    }

    private void OnBecameVisible()
    {
        this.gameObject.SetActive(true);
    }

    private void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
