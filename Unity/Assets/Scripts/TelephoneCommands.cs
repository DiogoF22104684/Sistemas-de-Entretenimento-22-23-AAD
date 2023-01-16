using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TelephoneCommands : MonoBehaviour
{
    private bool Key1, Key2, Key3, Key4, Key5, Key6, Key7, Key8, Key9, Key0, KeyA, KeyH;
    private int Keys = 0;
    public char[] charInput, randomSeq;
    private char input;
    private int score = 0;
    private bool phonePutDownCheck = true, runningTimeWaitCoroutine = false, gameStart = false;

    private SerialController controller;

    [SerializeField] private GameObject yourCodeText, combinationCodeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    [SerializeField] private int maxInput = 8;
    /*
    #region Phone Layout Editor
    [ButtonGroup("FirstLine")]
    [LabelText("1")]
    private void One()
    {
        ArrayInput('1');
    }

    [ButtonGroup("FirstLine")]
    [LabelText("2")]
    private void Two()
    {
        ArrayInput((char)2);
    }

    [ButtonGroup("FirstLine")]
    [LabelText("3")]
    private void Three()
    {
        ArrayInput((char)3);
    }

    [ButtonGroup("SecondLine")]
    [LabelText("4")]
    private void Four()
    {
        ArrayInput((char)4);
    }

    [ButtonGroup("SecondLine")]
    [LabelText("5")]
    private void Five()
    {
        ArrayInput((char)5);
    }

    [ButtonGroup("SecondLine")]
    [LabelText("6")]
    private void Six()
    {
        ArrayInput((char)6);
    }

    [ButtonGroup("ThirdLine")]
    [LabelText("7")]
    private void Seven()
    {
        ArrayInput((char)7);
    }

    [ButtonGroup("ThirdLine")]
    [LabelText("8")]
    private void Eight()
    {
        ArrayInput((char)8);
    }

    [ButtonGroup("ThirdLine")]
    [LabelText("9")]
    private void Nine()
    {
        ArrayInput((char)9);
    }

    [ButtonGroup("FourthLine")]
    [LabelText("*")]
    private void Asterix()
    {
        ArrayInput((char)11);
    }

    [ButtonGroup("FourthLine")]
    [LabelText("0")]
    private void Zero()
    {
        ArrayInput((char)10);
    }

    [ButtonGroup("FourthLine")]
    [LabelText("#")]
    private void Hashtag()
    {
        ArrayInput((char)12);
    }
    #endregion
    */

    // Start is called before the first frame update
    void Start()
    {
        charInput = new char[maxInput];
        randomSeq = new char[maxInput];
        ArrayClear(); 

        controller = GetComponent<SerialController>();
    }


    // Update is called once per frame
    void Update()
    {        
        if(phonePutDownCheck && !runningTimeWaitCoroutine)
            TestInput();

        PlaceInputInText(charInput);
        PlaceRandomInText(randomSeq);

        if (IsArrayFilled(charInput))
        {
            ArrayCheck();            
            ArrayClear();
        }

        scoreText.text = "Score: " + score; 
    }

    private void ArrayClear()
    {
        Debug.Log("Array Clear");
        for (int i = 0; i < maxInput; i++)
            charInput[i] = 'ª';
    }

    private void ArrayCheck()
    {
        Debug.Log("Array Check");
        if (charInput.SequenceEqual(randomSeq))
        {
            phonePutDownCheck = false;
            score += 100;
        }
            Array.Clear(randomSeq, 0, maxInput);
            StartCoroutine(TimeWait(3f));
    }

    private void ArrayInput(char input)
    {
        if (!IsArrayFilled(charInput))
        {
            // input char with the value of the number
            for (int i = 0; i < maxInput; i++)
            {
                if (i == (maxInput - 1))
                {
                    charInput[i] = input;
                }
                else
                {
                    charInput[i] = charInput[i + 1];
                }
            }

        
        }
    }

    void TestInput()
    {
        if (Input.inputString.Length > 0)
        {
            ArrayInput(Input.inputString.ToCharArray()[0]);
        }
    }

    void RandomSequence()
    {
        if(gameStart && phonePutDownCheck)
        {
            Debug.Log("Random Sequence gotten");
            combinationCodeText.GetComponent<TextMeshProUGUI>().text = "";

            for (int i = 0; i < maxInput; i++)
            {
                randomSeq[i] = (char)Mathf.RoundToInt(UnityEngine.Random.Range(48f, 57f));
            }
            GetComponent<SerialController>().SendSerialMessage("ring");
        }
        else
            Debug.Log("Can't get random sequence");
    }

    private bool IsArrayFilled(char[] array)
    {
        foreach (char element in array)
        {
            if (element == 'ª')
            {
                return false;
            }
        }
        return true;
    }

    void OnMessageArrived(string msg)
    {
        if (msg == "ring" || msg == "message recieved") { }

        else if(msg == "up")
        {
            phonePutDownCheck = false;
        }
        else if (msg == "start")
        {
            gameStart = true;
            Debug.Log("Game Started");
            RandomSequence();
        }
        else if(msg == "down")
        {
            phonePutDownCheck = true;
            ArrayCheck();
            ArrayClear();
        }
        else
            ArrayInput(msg.ToCharArray()[0]);
    }
    
    void PlaceInputInText(char[] array)
    {
        yourCodeText.GetComponent<TextMeshProUGUI>().text = "";
        for (int i = 0; i < maxInput; i++)
        {
            if (array[i] != 'ª')
            {
            yourCodeText.GetComponent<TextMeshProUGUI>().text += array[i].ToString() + " ";
            }
        }
    }

    void PlaceRandomInText(char[] array)
    {
        combinationCodeText.GetComponent<TextMeshProUGUI>().text = "";

        for (int i = 0; i < maxInput; i++)
            combinationCodeText.GetComponent<TextMeshProUGUI>().text += array[i].ToString() + " ";
    }

    private IEnumerator TimeWait(float time)
    {
        Debug.Log("Coroutine run");
        runningTimeWaitCoroutine = true;
        yield return new WaitForSeconds(time);
        RandomSequence();
        runningTimeWaitCoroutine = false;
        phonePutDownCheck = false;
    }

}
