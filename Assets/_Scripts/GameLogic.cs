using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public GameObject mainPanel;

    public GameObject sudokuFieldPanel;

    public GameObject fieldPrefab;

    public GameObject controlPanel;

    public GameObject controlPrefab;

    public Button informationButton;

    void Start()
    {
        CreateFieldPrefab();
        CreateControlPrefab();
    }

    private bool IsInformationActive = false;

    public void ClickOnInformationButton()
    {
        Debug.Log($"Click on info");
        if(IsInformationActive)
        {
            IsInformationActive = false;
            informationButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);

        }
        else
        {
            IsInformationActive=true;
            informationButton.GetComponent<Image>().color = new Color(0.7f, 0.99f, 0.99f);
        }
    }

    private Dictionary<Tuple<int, int>, FieldPrefab> fieldPrefabDic = new();
    private void CreateFieldPrefab()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                GameObject cell = Instantiate(fieldPrefab, sudokuFieldPanel.transform);
                FieldPrefab prefab = new(cell, i, j);
                fieldPrefabDic.Add(new Tuple<int, int>(i, j), prefab);
                cell.GetComponent<Button>().onClick.AddListener(() => OnClickFieldPrefab(prefab));
            }
        }
    }

    private FieldPrefab currentHoverFieldPrefab;
    private void OnClickFieldPrefab(FieldPrefab fieldPrefab)
    {
        Debug.Log($"Clicked on Row {fieldPrefab.Row}, Column {fieldPrefab.Column} ");
        if(currentHoverFieldPrefab != null)
        {
            currentHoverFieldPrefab.UnSetHoverMode();
        }
        currentHoverFieldPrefab = fieldPrefab;
        fieldPrefab.SetHoverMode();
    }

    private void CreateControlPrefab()
    {
        for (int i = 1; i < 10; i++)
        {
            GameObject instance = Instantiate(controlPrefab, controlPanel.transform);
            instance.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            ControlPrefab prefab = new();
            //controlPrefabDic.Add()
            prefab.Number = i;
            instance.GetComponent<Button>().onClick.AddListener(() => OnClickControlPrefab(prefab));

        }
    }

    private void OnClickControlPrefab(ControlPrefab prefab)
    {
        Debug.Log($"Click on ControlPrefab: {prefab.Number}");
        if (currentHoverFieldPrefab != null)
        {
            if (IsInformationActive)
            {
                currentHoverFieldPrefab.SetSmallNumber(prefab.Number);
            }
            else
            {
                currentHoverFieldPrefab.SetNumber(prefab.Number);
            }
        }
    }
}
