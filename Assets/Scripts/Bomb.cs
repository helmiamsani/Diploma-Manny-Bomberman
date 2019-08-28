using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab;
    public LayerMask levelMask;

    private bool exploded = false;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explode", 3f);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        StartCoroutine(CreateExplosion(Vector3.forward));
        StartCoroutine(CreateExplosion(Vector3.right));
        StartCoroutine(CreateExplosion(Vector3.back));
        StartCoroutine(CreateExplosion(Vector3.left));

        GetComponent<MeshRenderer>().enabled = false;
        exploded = true;
        transform.Find("Collider").gameObject.SetActive(false);
        Destroy(gameObject, .3f);
    }

    IEnumerator CreateExplosion(Vector3 direction)
    {
        //return null;

        for (int i = 1; i < 3; i++)
        {
            RaycastHit hit;

            Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), direction, out hit, i, levelMask);


            if (!hit.collider)
            {
                Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(!exploded && other.CompareTag("Explosion"))
        {
            CancelInvoke("Explode");
            Explode();
        }
    }
}
