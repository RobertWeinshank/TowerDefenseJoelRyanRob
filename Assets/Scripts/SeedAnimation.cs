using UnityEngine;
using System;
using System.Collections;
public class SeedAnimation : MonoBehaviour
{

    private float originalY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        this.originalY = this.transform.position.y;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, originalY + (Mathf.Sin(Time.time) * 1), transform.position.z);
        Destroy(this, 3);
    }

    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        Destroy(gameObject, 3);
    }


}
