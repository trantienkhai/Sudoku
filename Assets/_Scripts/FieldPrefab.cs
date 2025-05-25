using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldPrefab
{
    private int row;
    private int column;
    private GameObject instance;

    public FieldPrefab(GameObject instance, int row, int column)
    {
        this.instance = instance;
        this.Row = row;
        this.Column = column;
    }

    public bool TryGetTextByName(string name, out TextMeshProUGUI text)
    {
        text = null;
        TextMeshProUGUI[] texts = instance.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(var curText in texts)
        {
            if (curText.name.Equals(name))
            {
                text = curText;
                return true;
            }
        }
        return false;
    }

    public int Row { get => row; set => row = value; }
    public int Column { get => column; set => column = value; }

    public void SetHoverMode()
    {
        instance.GetComponent<Image>().color = new Color(0.7f, 0.99f, 0.99f);
    }

    public void UnSetHoverMode()
    {
        instance.GetComponent<Image>().color = new Color(1f, 1f, 1f);

    }

    public void SetSmallNumber(int number)
    {
        if (TryGetTextByName($"Number{number}", out TextMeshProUGUI text))
        {
            text.text = number.ToString();
            if(TryGetTextByName("Value", out TextMeshProUGUI textValue))
            {
                textValue.text = "";
            }
        }
    }

    public void SetNumber(int number)
    {
        if (TryGetTextByName("Value", out TextMeshProUGUI text))
        {
            text.text = number.ToString();
            for (int i = 1; i < 10; i++)
            {
                if(TryGetTextByName($"Number{i}", out TextMeshProUGUI textNumber))
                {
                    textNumber.text = "";
                }
            }
        }
    }
}
