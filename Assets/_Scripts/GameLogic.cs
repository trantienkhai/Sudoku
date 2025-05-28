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

    private SudokuObject curSudokuObject;

    void Start()
    {
        CreateFieldPrefab();
        CreateControlPrefab();
        CreateSudokuObject();
    }

    private void CreateSudokuObject()
    {
        curSudokuObject = SudokuGenerator.createSudokuObject();
        for(int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                var curValue = curSudokuObject.values[i,j];
                if(curValue != 0)
                {
                    FieldPrefab fieldObject = fieldPrefabDic[new Tuple<int, int>(i, j)];
                    fieldObject.SetNumber(curValue);
                    fieldObject.IsChangeAble = false;
                }
            }
        }
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
        if (fieldPrefab.IsChangeAble)
        {
            Debug.Log($"Clicked on Row {fieldPrefab.Row}, Column {fieldPrefab.Column} ");
            if (currentHoverFieldPrefab != null)
            {
                //currentHoverFieldPrefab.UnSetHoverMode();
                //UnHighLightRelatedFields(currentHoverFieldPrefab);
                ResetAllHighlight();
            }
            currentHoverFieldPrefab = fieldPrefab;
            HighLightRelatedFields(currentHoverFieldPrefab);
            fieldPrefab.SetHoverMode();
        }
        
    }

    private void CreateControlPrefab()
    {
        for (int i = 1; i < 10; i++)
        {
            GameObject instance = Instantiate(controlPrefab, controlPanel.transform);
            instance.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            ControlPrefab prefab = new();
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
                int curNumber = prefab.Number;
                int row = currentHoverFieldPrefab.Row;
                int column = currentHoverFieldPrefab.Column;

                if (curSudokuObject.IspossibleNumberInPos(curNumber, row, column))
                {
                    currentHoverFieldPrefab.SetNumber(prefab.Number);

                }
            }
        }
    }

    // Ham hightlight cac hang va cot, group lien quan
    private void HighLightRelatedFields(FieldPrefab fieldPrefab)
    {
        Debug.Log("Duoc goi highlight");
        // Hightlight hang va cot
        for(int i = 0; i < 9; i++)
        {
            fieldPrefabDic[new Tuple<int, int>(fieldPrefab.Row, i)].SetHighLight();
            fieldPrefabDic[new Tuple<int, int>(i, fieldPrefab.Column)].SetHighLight();
        }
        // Hightlight group
        int group = curSudokuObject.GetGroup(fieldPrefab.Row, fieldPrefab.Column);
        curSudokuObject.GetGruopIndex(group, out int startRow, out int startColumn);
        for(int i = startRow; i < startRow + 3; i++)
        {
            for(int j = startColumn; j < startColumn + 3; j++)
            {
                fieldPrefabDic[new Tuple<int, int>(i,j)].SetHighLight();
            }
        }

    }

    private void UnHighLightRelatedFields(FieldPrefab fieldPrefab)
    {
        Debug.Log("Duoc goi unhighlight");

        // UnHightlight hang va cot
        for (int i = 0; i < 9; i++)
        {
            fieldPrefabDic[new Tuple<int, int>(fieldPrefab.Row, i)].UnSetHighLight();
            fieldPrefabDic[new Tuple<int, int>(i, fieldPrefab.Column)].UnSetHighLight();
        }
        // UnHightlight group
        int group = curSudokuObject.GetGroup(fieldPrefab.Row, fieldPrefab.Column);
        curSudokuObject.GetGruopIndex(group, out int startRow, out int startColumn);
        for (int i = startRow; i < startRow + 3; i++)
        {
            for (int j = startColumn; j < startColumn + 3; j++)
            {
                fieldPrefabDic[new Tuple<int, int>(i, j)].UnSetHighLight();
            }
        }
    }

    private void ResetAllHighlight()
    {
        foreach (var field in fieldPrefabDic.Values)
        {
            field.UnSetHighLight();
        }
    }

}
