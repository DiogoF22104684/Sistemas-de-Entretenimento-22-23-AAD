using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TelephoneCommands : MonoBehaviour
{
    private bool Key1, Key2, Key3, Key4, Key5, Key6, Key7, Key8, Key9, Key0, KeyA, KeyH;
    private int Keys = 0;
    public char[] charInput, randomSeq; 
    private char input;
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
        RandomSequence();
    }

    // Update is called once per frame
    void Update()
    {
        TestInput();   
    }

    private void ArrayInput(char input)
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

    void TestInput()
    {
        if(Input.inputString.Length > 0)
            ArrayInput(Input.inputString.ToCharArray()[0]);

    }

    void RandomSequence()
    {
        for (int i = 0; i < maxInput; i++)
        {
            randomSeq[i] = (char)Mathf.RoundToInt(Random.Range(48f, 57f));
        }
    }

    void OnMessageArrived(string msg)
    {
        if(msg == "Banana")
        { 
        }
        else if(msg == "Amoras")
        {
        }
        else
        ArrayInput(msg.ToCharArray()[0]);
    }
}
