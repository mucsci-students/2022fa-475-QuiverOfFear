using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delete());
    }

    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
