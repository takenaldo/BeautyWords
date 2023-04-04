using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Helper : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    void showDialog(GameObject go)
    {
        go.SetActive(true);
    }

    void hideDialog(GameObject go)
    {
        go.SetActive(false);
    }
}
