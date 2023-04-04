using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsHelper : MonoBehaviour
{
    private string[] dictionary = { "HE", "SHE", "ME", "HH","EE","OO","II","UI" };


    SortedList<string, int> letterValues = new SortedList<string, int>();

    private void Start()
    {
        letterValues.Add("A", 1);
        letterValues.Add("B", 3);
        letterValues.Add("C", 4);
        letterValues.Add("Ç", 4);
        letterValues.Add("D", 3);
        letterValues.Add("E", 1);
        letterValues.Add("F", 7);
        letterValues.Add("G", 5);
        letterValues.Add("Ğ", 8);
        letterValues.Add("H", 5);
        letterValues.Add("I", 2);
        letterValues.Add("İ", 1);
        letterValues.Add("J", 10);
        letterValues.Add("K", 1);
        letterValues.Add("L", 1);


        letterValues.Add("M", 2);
        letterValues.Add("N", 1);
        letterValues.Add("O", 2);
        letterValues.Add("Ö", 7);
        letterValues.Add("P", 5);

        letterValues.Add("R", 1);
        letterValues.Add("S", 2);
        letterValues.Add("Ş", 4);
        letterValues.Add("T", 1);
        letterValues.Add("U", 2);
        letterValues.Add("Ü", 3);
        letterValues.Add("V", 7);
        letterValues.Add("Y", 3);
        letterValues.Add("Z", 4);

        letterValues.Add("Q", 0);
        letterValues.Add("X", 0);

    }

    public void confirmProcessCandidate()
    {

        string candidateStr = GameManager.instance.candidateWord;
        if (isValidWord(candidateStr))
        {
            DestroySelectedBlocks();
            GameManager.instance.validWords++;
            GameManager.instance.score += calculateScore(candidateStr);
            GameManager.instance.txtScore.text = GameManager.instance.score+"";

            GameManager.instance.candidateWord = "";
            GameManager.instance.txtUserInput.text = GameManager.instance.candidateWord;
        }
        else
        {
            GameManager.instance.invalidWords++;
            if (GameManager.instance.isGameOver())
            {
                DestroyAllBlocks();
                GameManager.instance.Options.SetActive(false);
                GameManager.instance.showDialog(GameManager.instance.dialogGameOver);
                GameManager.instance.setGameOverStat();
            }
            GameManager.instance.txt_xs[GameManager.instance.invalidWords-1].SetActive(true);
        }

        GameManager.instance.resetPhase();
        

    }



    public bool isValidWord(string candidateStr)
    {
        bool found = false;
        for(int i=0; i< dictionary.Length; i++)
            if (dictionary[i] == candidateStr)
                found = true;
        return found;
    }

    private int calculateScore(string candidateStr)
    {
        int sum = 0;
        for(int i = 0; i < candidateStr.Length; i++)
            sum += letterValues[candidateStr[i]+""];
        return sum;
    }

    public void DestroySelectedBlocks()
    {
        GameObject []blocks = GameObject.FindGameObjectsWithTag("block");

        foreach(GameObject go in blocks){
            bool markedStatusActive = go.GetComponent<BlockController>().activationStatus;
            if (!markedStatusActive)
            {
                Destroy(go);
            }    
        }
    }

    public void DestroyAllBlocks()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");

        foreach (GameObject go in blocks)
        {
                Destroy(go);
        }
    }

}
