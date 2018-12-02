using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintText : MonoBehaviour {

    public Text textbox;
    bool text_complete = true;
    private string leftover_text = "";

    public bool IsTextComplete()
    {
        return text_complete;
    }

    private IEnumerator writeText()
    {
        textbox.text += leftover_text[0];
        leftover_text = leftover_text.Substring(1);
        yield return new WaitForSeconds(0.0225f);

        if (leftover_text.Length > 0)
        {
            yield return writeText();
        }
        else
        { 
            text_complete = true;
        }
    }

    public void SetText(string txt)
    {
        if (!text_complete)
        {
            StopCoroutine("writeText");
            textbox.text += leftover_text;
            leftover_text = "";
            text_complete = true;
        }
        else
        {
            leftover_text = txt;
            textbox.text = "";
            text_complete = false;
            StartCoroutine("writeText");
        }
     }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SetText("beeadtwefsdr3egfbert54t54gfadfadffeay7fuierjtuirgauitehbityeouitbriuhjoirs");
        }
    }

}
