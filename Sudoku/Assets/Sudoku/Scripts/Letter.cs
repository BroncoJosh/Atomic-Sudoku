using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour
{
    public TextMesh letter;
    public int value = 0;
    public int input = 0;
    public bool modify;
    public bool visible;

    void Update()
    {
        if (!visible)
        {
            if (letter.GetComponent<Renderer>().enabled)
                letter.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            if (!letter.GetComponent<Renderer>().enabled)
                letter.GetComponent<Renderer>().enabled = true;
        }
    }

    public void Erase()
    {
        if (!modify) return;
        input = 0;
        letter.text = "";
        visible = false;
    }

    public void Decrease()
    {
        if (!modify) return;

        input--;
        if (input < 1)
            input = 9;
        
        letter.text = input.ToString();
        visible = true;
    }

    public void Increase()
    {
        if (!modify) return;

        input++;
        if (input > 9)
            input = 1;

        letter.text = input.ToString();
        visible = true;
    }

    public void setValue(int number)
    {
        if (!modify) return;
        input = number;
        letter.text = number.ToString();
        visible = true;
    }
}
