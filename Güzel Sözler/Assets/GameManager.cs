using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Security.Cryptography;
using System;

public class GameManager : MonoBehaviour
{
    static Color one = new Color(177,  197,    197);
    static Color two = new Color(153,202,172);
    static Color three = new Color(86,87,105);
    static Color four = new Color(227,138,108);
    static Color five = new Color(210,105,102);
    static Color six = new Color(219, 173, 85);
    static Color svn = new Color(216,211,120);
    static Color eight = new Color(149,143,177);
    static Color nine = new Color(199,188,188);
    static Color ten = new Color(104,136,125);
    static Color elvn = new Color(123, 184, 179);
    static Color twlv = new Color(178,195, 176);
    static Color thrteen = new Color(128,181,179);

    Color[] colors = { one, two, three, four, five, six, svn, eight, nine, ten, elvn, twlv,thrteen };

    public GameObject pre1, pre2, pre3, pre4, pre5, pre6, pre7, pre8;
    public GameObject[] parents;

    public GameObject[] txt_xs;

    // parent for all letters
    public GameObject parentLetterPrefab;

    //prefab to falling letters
    public GameObject prefabFallingLetters;

    public Sprite fallingLetterSprite, fallingLetterOutlineSprite;

    public Text txtUserInput;
    public Text txtScore;
    public GameObject dialogGameOver;
    public GameObject Options;
    public Text txt_go_score, txt_go_valid, txt_go_invalid;




    public static GameManager instance;

/*    private char [] vowels = {'A','E','I','O', 'Ö', 'Ş','Ü','U'};
    private char [] consonants = {'B','C', 'Ç', 'D','F','G','Ğ','H','İ', 'J','K','L','M','N','P','Q','R','S','T','V','W','X','Y','Z'};
*/
    private char[] vowels = { 'A', 'E', 'I', 'O', 'U'};
    private char[] consonants = { 'B', 'C', 'D', 'F', 'G',  'H',  'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z' };


    //   Ç  Ğ İ 
    // Ö Ş  Ü 



    public String candidateWord = "";
    // Start is called before the first frame update

    private float initialTimestamp,lastTimeStamp = -1;

    public int score=0;
    public int validWords = 0;
    public int invalidWords = 0;

    public float spawningRate = 5f;
    void Start()
    {
        initialTimestamp = Time.time;
        InitialSpawn();     
        
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - initialTimestamp >= 10f)
            SpawnFalling();
    }

    void InitialSpawn()
    {
        char []allLetters = generateRandomLetters();

        GameObject[] prefabs = { pre1, pre2, pre3, pre4, pre5, pre6, pre7, pre8 };

        for (int j = 1; j <= 3; j++)
        {
            for (int i = 1; i <= prefabs.Length; i++)
            {
                GameObject letterGameObject = Instantiate(prefabs[i-1], parents[i-1].transform) as GameObject;
                changeAppearance(letterGameObject, allLetters[((i)*(j)) % 24]);
            }
//            StartCoroutine(waiter());
        }

    }

    // changes the color and text of a single letter gameobject (block)
    private void changeAppearance(GameObject letterGameObject,char letter)
    {
        Text txt = (Text)letterGameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        txt.text = letter + "";

        Color blockColor = colors[(int)letter % colors.Length];
        blockColor.r = (float)blockColor.r / 255;
        blockColor.g = (float)blockColor.g / 255;
        blockColor.b = (float)blockColor.b / 255;

        letterGameObject.GetComponent<Image>().color = blockColor;

    }

    // method for changing the sprites of falling letters to circle / ice  dynamically
    private void changeSprite(GameObject letterGameObject)
    {
        letterGameObject.GetComponent<Image>().sprite = fallingLetterSprite;
        letterGameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = fallingLetterOutlineSprite;
        Text txt = (Text)letterGameObject.transform.GetChild(0).gameObject.GetComponent<Text>();

    }

    // returns an array of random characters usiung a ratio of 70% and 30% for consonants and vowels,   
    private char[] generateRandomLetters()
    {
        int vowels_count = vowels.Length;


        char[] generatedLetters = new char[24];
        int[] arr = new int[24];

        for (int i = 0; i < generatedLetters.Length;i++)
        {
            // generate random vowel
            System.Random rnd = new System.Random();
            if (i < vowels_count)
            {
                int index = rnd.Next() % vowels.Length;
                char randomVowel = vowels[index];
                arr[i] = (int)randomVowel;
            }
            else
            {
                int index = rnd.Next() % consonants.Length;
                char randomConsonant = consonants[index];
                arr[i] = (int)randomConsonant;

            }

        }

        System.Random random = new System.Random();
        arr = arr.OrderBy(x => random.Next()).ToArray();
        for (int i = 0; i < arr.Length; i++)
        {
            generatedLetters[i] = (char)arr[i];
        }
            return generatedLetters;
    }

    IEnumerator waiter()
    {
        Debug.Log("start of delay");
        yield return new WaitForSeconds(4);
        Debug.Log("end of delay");
    }

    // Spawns a falling blocks (letters) according to the score and timing conditions
    private void SpawnFalling()
    {
        if (isTimeToSpawnFalling())
        {
            lastTimeStamp = Time.time;
            GameObject[] prefabs = { pre1, pre2, pre3, pre4, pre5, pre6, pre7, pre8 };
            int randomIndex = new System.Random().Next() % 8;

            GameObject fallingLetter = Instantiate(prefabs[randomIndex], parents[randomIndex].transform ) as GameObject;

            int rnd = new System.Random().Next() % 2;
            char randomCharacter;
            if (rnd == 0)
                randomCharacter = vowels[new System.Random().Next(vowels.Length)];
            else
                randomCharacter = consonants[new System.Random().Next(consonants.Length)];

            changeSprite(fallingLetter);
            changeAppearance(fallingLetter, randomCharacter);

            if (isGameOver())
            {
                Debug.Log("GAME OVER");
                foreach(GameObject go in GameObject.FindGameObjectsWithTag("block"))
                {
                    Destroy(go);
                }
                showDialog(dialogGameOver);
                setGameOverStat();

                
            }
            

        }
    }


    private bool isTimeToSpawnFalling()
    {
        return Time.time - lastTimeStamp > spawningRate;
    }

    private void setSpawningSpeed()
    {
        if (score < 100)
            spawningRate = 5f;
        else if (score < 200)
            spawningRate = 4f;
        else if (score < 300)
            spawningRate = 3f;
        else if (score < 400)
            spawningRate = 2f;
        else
            spawningRate = 1f;

    }

    public bool isGameOver()
    {
        if (invalidWords >= 3)
            return true;
        foreach(GameObject parent in parents)
            if (parent.transform.childCount >= 10)
                return true;
        return false;
    }

    public void showDialog(GameObject go)
    {
        go.SetActive(true);
    }

    public void hideDialog(GameObject go)
    {
        go.SetActive(false);
    }
    public void setGameOverStat()
    {
        txt_go_score.text =   "score: " + score;
        txt_go_valid.text =   "valid words: " + validWords;
        txt_go_invalid.text = "invalid words: " + invalidWords;
    }

    public void resetPhase()
    {
        txtUserInput.text = "";
        candidateWord = "";
    }
}
