using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockController : MonoBehaviour
{
    public bool activationStatus = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addToCandidateWord()
    {
        Text txt = (Text)gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();

        if (activationStatus == true)
        {
            GameManager.instance.candidateWord += txt.text;
            GameManager.instance.txtUserInput.text = GameManager.instance.candidateWord;
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            activationStatus = false;
            Debug.Log("must be : " + GameManager.instance.candidateWord);
        }
        else
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            activationStatus = true;
            string str = GameManager.instance.txtUserInput.text;
//            str =  str.Replace(txt.text, "");
            str = remove(str, txt.text[0]);
            Debug.Log("str is : " + str);
            GameManager.instance.candidateWord = str;
            GameManager.instance.txtUserInput.text = str;

        }
    }

    private string remove(string inputStr, char key)
    {
        char[] ch_arr = inputStr.ToCharArray();
        string str = "";
        int last_index = inputStr.LastIndexOf(key);
        for(int i = 0; i < inputStr.ToCharArray().Length; i++)
        {
            if(i!=last_index)
                str += ch_arr[i];
        }
        return str;
    }
}
