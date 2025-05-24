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

    public void SetNumber(int number)
    {
        instance.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();
    }
}
