using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrail : MonoBehaviour
{
    List<GameObject> trailParts = new List<GameObject>();

    void Start()
    {

    }

    public void StartTrail()
    {
        InvokeRepeating("SpawnTrailPart", 0, 0.02f); // replace 0.2f with needed repeatRate
    }

    public void StopTrail()
    {
        CancelInvoke();
    }

    void SpawnTrailPart()
    {
        GameObject trailPart = new GameObject();
        SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
        trailPartRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        trailPart.transform.position = transform.position;
        trailPart.transform.rotation = transform.rotation;
        trailParts.Add(trailPart);

        StartCoroutine(FadeTrailPart(trailPartRenderer));
        StartCoroutine(DestroyTrailPart(trailPart, 0.2f)); // replace 0.5f with needed lifeTime
    }

    IEnumerator FadeTrailPart(SpriteRenderer trailPartRenderer)
    {
        Color color = trailPartRenderer.color;
        color.a -= 0.6f; // replace 0.5f with needed alpha decrement
        trailPartRenderer.color = color;

        yield return new WaitForEndOfFrame();
    }

    IEnumerator DestroyTrailPart(GameObject trailPart, float delay)
    {
        yield return new WaitForSeconds(delay);

        trailParts.Remove(trailPart);
        Destroy(trailPart);
    }
}
