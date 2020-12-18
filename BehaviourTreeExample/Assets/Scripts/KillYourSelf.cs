using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillYourSelf : MonoBehaviour
{
    [Range(1, 10)]
    public float lifetime;
    public delegate void Died();
    public static event Died Die;
    void Start()
    {
       
        StartCoroutine(kill(lifetime));
    }

    IEnumerator kill(float x)
    {
     
 
        yield return new WaitForSeconds(x);

        Destroy(this.gameObject);
    }
    
}
