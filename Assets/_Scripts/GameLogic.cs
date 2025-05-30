using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public GameObject mainPanel;

    public GameObject sudokuFieldPanel;

    public GameObject fieldPrefab;

    public GameObject controlPanel;

    public GameObject controlPrefab;

    public Button informationButton;

    private SudokuObject _gameObject;

    private SudokuObject _finalObject;

    public Button Back;

    public Button finish;

    private bool IsInformationActive = false;


    public void ClickOnFinsih()
    {
        for (int i = 0; i < 9; i++) 
        {
            for(int j = 0; j < 9; j++)
            {
                FieldPrefab fieldObject = fieldPrefabDic[new Tuple<int, int>(i, j)];
                if (fieldObject.IsChangeAble)
                {
                    if (_finalObject.values[i,j] == fieldObject.number)
                    {
                        fieldObject.ChangeColorToGreen();
                    }
                    else
                    {
                        fieldObject.ChangeColorToRed();
                    }
                }
            }
        }
    }

    public void OnClickBack()
    {
        SceneManager.LoadScene("Level");
    }

    void Start()
    {
        CreateFieldPrefab();
        CreateControlPrefab();
        CreateSudokuObject();
    }

    private void CreateSudokuObject()
    {
        SudokuGenerator.createSudokuObject(out SudokuObject finalObject, out SudokuObject gameObject);
        _gameObject = gameObject;
        _finalObject = finalObject;
        for(int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                var curValue = this._gameObject.values[i, j];
                if(curValue != 0)
                {
                    FieldPrefab fieldObject = fieldPrefabDic[new Tuple<int, int>(i, j)];
                    fieldObject.SetNumber(curValue);
                    fieldObject.IsChangeAble = false;
                }
            }
        }
    }

    public void ClickOnInformationButton()
    {
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
        if (currentHoverFieldPrefab != null)
        {
            if (IsInformationActive)
            {
                if (!currentHoverFieldPrefab.numberInControlPrefab[prefab.Number]) 
                {
                    currentHoverFieldPrefab.numberInControlPrefab[prefab.Number] = true;
                    currentHoverFieldPrefab.SetSmallNumber(prefab.Number);
                }
                else
                {
                    currentHoverFieldPrefab.numberInControlPrefab[prefab.Number] = false;
                    currentHoverFieldPrefab.UnSetSmallNumber(prefab.Number);
                }
            }
            else
            {
                currentHoverFieldPrefab.SetNumber(prefab.Number);
                if(currentHoverFieldPrefab.number != _finalObject.values[currentHoverFieldPrefab.Row, currentHoverFieldPrefab.Column])
                {
                    currentHoverFieldPrefab.ChangeColorToRed();
                }
                else
                {
                    currentHoverFieldPrefab.ChangeColorToGreen();
                    RemoveWhenFillCorrect(currentHoverFieldPrefab);
                }
            }
        }
    }

    private void HighLightRelatedFields(FieldPrefab fieldPrefab)
    {
        for(int i = 0; i < 9; i++)
        {
            fieldPrefabDic[new Tuple<int, int>(fieldPrefab.Row, i)].SetHighLight();
            fieldPrefabDic[new Tuple<int, int>(i, fieldPrefab.Column)].SetHighLight();
        }
        int group = _gameObject.GetGroup(fieldPrefab.Row, fieldPrefab.Column);
        _gameObject.GetGruopIndex(group, out int startRow, out int startColumn);
        for(int i = startRow; i < startRow + 3; i++)
        {
            for(int j = startColumn; j < startColumn + 3; j++)
            {
                fieldPrefabDic[new Tuple<int, int>(i,j)].SetHighLight();
            }
        }
    }

    private void RemoveWhenFillCorrect(FieldPrefab fieldPrefab)
    {
        for (int i = 0; i < 9; i++)
        {
            fieldPrefabDic[new Tuple<int, int>(fieldPrefab.Row, i)].RemoveNumbersRelated(fieldPrefab.number);
            fieldPrefabDic[new Tuple<int, int>(i, fieldPrefab.Column)].RemoveNumbersRelated(fieldPrefab.number);
        }
        int group = _gameObject.GetGroup(fieldPrefab.Row, fieldPrefab.Column);
        _gameObject.GetGruopIndex(group, out int startRow, out int startColumn);
        for (int i = startRow; i < startRow + 3; i++)
        {
            for (int j = startColumn; j < startColumn + 3; j++)
            {
                fieldPrefabDic[new Tuple<int, int>(i, j)].RemoveNumbersRelated(fieldPrefab.number);
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
        int group = _gameObject.GetGroup(fieldPrefab.Row, fieldPrefab.Column);
        _gameObject.GetGruopIndex(group, out int startRow, out int startColumn);
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
