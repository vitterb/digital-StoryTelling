using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour
{
    public int counter = 0;
    public int scene;
    // Update is called once per frame
    void Update()
    {
        if (counter == 0)
            counter = 3000;
        counter -= 1;
        if (counter <= 0)
            SceneManager.LoadScene(scene);
    }
}
