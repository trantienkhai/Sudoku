using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;

using Random = UnityEngine.Random;


public class SudokuGenerator
{
    public static void createSudokuObject(out SudokuObject finalObject, out SudokuObject gameObject )
    {
        finalObject = null;
        SudokuObject sudokuObject = null;
        bool success = false;
        while (!success)
        {
            sudokuObject = new SudokuObject();
            CreateRandomGruops(sudokuObject);

            if (TryToSolve(sudokuObject))
            {
                sudokuObject = finalSudokuObject;
                finalObject = sudokuObject;
                success = true;
            }
        }
        gameObject = RemoveNumbers(sudokuObject);
    }

    //private static SudokuObject RemoveNumbers(SudokuObject sudokuObject)
    //{
    //    SudokuObject newSudokuObject = new SudokuObject();
    //    newSudokuObject.values = (int[,])sudokuObject.values.Clone();
    //    ////
    //    //List<int> values = GetValue();
    //    List<Tuple<int, int>> values = GetValue();
    //    int valueIndex = 10;
    //    if(GameSetting.EasyMiddleHardManager == 1) valueIndex = 71;
    //    if (GameSetting.EasyMiddleHardManager == 1) valueIndex = 61;
    //    bool isFinish = false;
    //    while (!isFinish)
    //    {
    //        int index = Random.Range(0, values.Count);
    //        int searchIndex = values[index];
    //        bool isFirst = true;
    //        for(int i = 1; i < 10; i++)
    //        {
    //            for(int j = 1; j < 10; j++)
    //            {
    //                if(i*j == searchIndex && isFirst)
    //                {
    //                    isFirst = false;
    //                    values.RemoveAt(index);
    //                    SudokuObject nextSudokuObject = new SudokuObject();
    //                    nextSudokuObject.values = (int[,])newSudokuObject.values.Clone();
    //                    nextSudokuObject.values[i - 1, j - 1] = 0;
    //                    if (TryToSolve(nextSudokuObject, true))
    //                    {
    //                        newSudokuObject = nextSudokuObject;
    //                    }
    //                }
    //            }
    //        }
    //        // < 30
    //        if(values.Count < valueIndex)
    //        {
    //            isFinish = true;
    //        }
    //    }
    //    return newSudokuObject;
    //}

    private static SudokuObject RemoveNumbers(SudokuObject sudokuObject)
    {
        SudokuObject newSudokuObject = new SudokuObject();
        newSudokuObject.values = (int[,])sudokuObject.values.Clone();
        ////
        //List<int> values = GetValue();
        List<Tuple<int, int>> values = GetValue();
        int valueIndex = 10;
        if (GameSetting.EasyMiddleHardManager == 1) valueIndex = 71;
        if (GameSetting.EasyMiddleHardManager == 1) valueIndex = 61;
        bool isFinish = false;
        while (!isFinish)
        {
            int index = Random.Range(0, values.Count);
            var searchIndex = values[index];
            SudokuObject nextSudokuObject = new SudokuObject();
            nextSudokuObject.values = (int[,])newSudokuObject.values.Clone();
            nextSudokuObject.values[searchIndex.Item1,searchIndex.Item2] = 0;

            if (TryToSolve(nextSudokuObject, true))
            {
                newSudokuObject = nextSudokuObject;
            }
            values.RemoveAt(index);

            if (values.Count < valueIndex)
            {
                isFinish = true;
            }
            //values.RemoveAt(index);

        }
        return newSudokuObject;
    }


    //private static List<int> GetValue()
    //{
    //    List<int> values = new List<int>();
    //    for (int i = 1; i < 10; i++)
    //    {
    //        for (int j = 0; j < 10; j++)
    //        {
    //            values.Add(i * j);
    //        }
    //    }
    //    return values;
    //}

    private static List<Tuple<int, int>> GetValue()
    {
        List<Tuple<int, int>> values = new List<Tuple<int, int>>();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                values.Add(new Tuple<int, int>(i, j));
            }
        }
        return values;
    }

    private static SudokuObject finalSudokuObject;

    private static bool TryToSolve(SudokuObject sudokuObject, bool onlyOne = false)
    {
        if (HasEmptyFieldsToFill(sudokuObject, out int row, out int column, onlyOne))
        {
            List<int> possibleValues = GetPossibleValues(sudokuObject, row, column);
            foreach (var possibleValue in possibleValues)
            {
                SudokuObject nextSudokuObject = new SudokuObject();
                nextSudokuObject.values = (int[,])sudokuObject.values.Clone();
                nextSudokuObject.values[row, column] = possibleValue;
                if (TryToSolve(nextSudokuObject, onlyOne))
                {
                    return true;
                }
            }
        }

        if (HasEmptyFields(sudokuObject))
        {
            return false;
        }
        finalSudokuObject = sudokuObject;
        return true;
    }

    private static bool HasEmptyFields(SudokuObject sudokuObject)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudokuObject.values[i, j] == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private static List<int> GetPossibleValues(SudokuObject sudokuObject, int row, int column)
    {
        List<int> possibleValues = new List<int>();
        for (int value = 1; value < 10; value++)
        {
            if (sudokuObject.IspossibleNumberInPos(value, row, column))
            {
                possibleValues.Add(value);
            }
        }
        return possibleValues;
    }

    private static bool HasEmptyFieldsToFill(SudokuObject sudokuObject, out int row, out int column, bool onlyOne = false)
    {
        row = 0; column = 0;
        int amountOfPossibaleValues = 10;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudokuObject.values[i, j] == 0)
                {
                    int curAmount = GetPossibleAmountOfValues(sudokuObject, i, j);
                    if (curAmount != 0)
                    {
                        if (curAmount < amountOfPossibaleValues)
                        {
                            amountOfPossibaleValues = curAmount;
                            row = i;
                            column = j;
                        }
                    }


                }
            }
        }
        if (onlyOne)
        {
            if (amountOfPossibaleValues == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (amountOfPossibaleValues == 10)
        {
            return false;
        }
        return true;
    }

    private static int GetPossibleAmountOfValues(SudokuObject sudokuObject, int row, int column)
    {
        int amount = 0;
        for (int k = 1; k < 10; k++)
        {
            if (sudokuObject.IspossibleNumberInPos(k, row, column))
            {
                amount++;
            }
        }
        return amount;
    }
    public static void CreateRandomGruops(SudokuObject sudokuObject)
    {
        List<int> values = new List<int>() { 0, 1, 2 };
        int index = Random.Range(0, values.Count);
        InsertRandomGroup(sudokuObject, 1 + values[index]);
        values.RemoveAt(index);

        index = Random.Range(0, values.Count);
        InsertRandomGroup(sudokuObject, 4 + values[index]);
        values.RemoveAt(index);

        index = Random.Range(0, values.Count);
        InsertRandomGroup(sudokuObject, 7 + values[index]);
        values.RemoveAt(index);
    }

    public static void InsertRandomGroup(SudokuObject sudokuObject, int group)
    {
        sudokuObject.GetGruopIndex(group, out int startRow, out int startColumn);
        List<int> values = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        for (int i = startRow; i < startRow + 3; i++)
        {
            for (int j = startColumn; j < startColumn + 3; j++)
            {
                int index = Random.Range(0, values.Count);
                sudokuObject.values[i, j] = values[index];
                values.RemoveAt(index);
            }
        }
    }
}
