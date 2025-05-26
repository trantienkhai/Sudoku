using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuObject 
{
    public int[,] values = new int[9, 9];
    public void GetGruopIndex(int group,out int startRow, out int startColumn)
    {
        startRow = 0; startColumn = 0;
        switch (group)
        {
            case 1:
                startRow = 0; startColumn = 0;
                break;
            case 2:
                startRow = 0; startColumn = 3;
                break;
            case 3:
                startRow = 0; startColumn = 6;
                break;
            case 4:
                startRow = 3; startColumn = 0;
                break;
            case 5:
                startRow = 3; startColumn = 3;
                break;
            case 6:
                startRow = 3; startColumn = 6;
                break;
            case 7:
                startRow = 6; startColumn = 0;
                break;
            case 8:
                startRow = 6; startColumn = 3;
                break;
            case 9:
                startRow = 6; startColumn = 6;
                break;

        }
    }

    public bool IspossibleNumberInPos(int number, int row, int column)
    {
        if(IsPossibleNumberInRow(number, row) && IsPossibleNumberInColumn(number, column))
        {
            if(IsPossibleNumberInGroup(number, GetGroup(row, column)))
            {
                return true;
            }
        }
        return false;
    }

    private int GetGroup(int row, int column)
    {
        if(row < 3)
        {
            if (column < 3) return 1;
            if(column < 6) return 2;
            return 3;
        }
        if(row < 6)
        {
            if (column < 3) return 4;
            if (column < 6) return 5;
            return 6;
        }
        else
        {
            if (column < 3) return 7;
            if (column < 6) return 8;
            return 9;
        }
    }
    private bool IsPossibleNumberInRow(int number, int row)
    {
        for(int i = 0; i < 9; i++)
        {
            if(values[row, i] == number)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsPossibleNumberInColumn(int number, int column)
    {
        for (int i = 0; i < 9; i++)
        {
            if (values[i, column] == number)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsPossibleNumberInGroup(int number, int group)
    {
        GetGruopIndex(group, out int startRow, out int startColumn);
        for(int i = startRow; i < startRow + 3; i++)
        {
            for(int j = startColumn; j < startColumn + 3; j++)
            {
                if (values[i,j] == number)
                {
                    return false;
                }
            }
        }
        return true ;
    }
}
