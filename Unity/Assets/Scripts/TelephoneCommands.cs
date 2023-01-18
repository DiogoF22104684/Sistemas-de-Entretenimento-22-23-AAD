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
    public char[] charInput, randomSeq;
    private int score = 0;
    private bool phonePutDownCheck = true, runningTimeWaitCoroutine = false, gameStart = false;

    [SerializeField] private GameObject yourCodeText, combinationCodeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    [SerializeField] private int maxInput = 8;

    // Start is called before the first frame update
    void Start()
    {
        charInput = new char[maxInput];
        randomSeq = new char[maxInput];
        ArrayClear(); 
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

    private void MenuSound()
    {

    }

    private void KeyInputSound()
    {

    }

    private void CorrectSound()
    {

    }

    private void IncorrectSound()
    {

    }

    private void NewCodeSound()
    {

    }

}
