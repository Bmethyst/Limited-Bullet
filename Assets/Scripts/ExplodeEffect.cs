using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("destroyEffect", 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void destroyEffect() {
        Destroy(gameObject);
    }
}
