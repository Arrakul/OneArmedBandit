﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CheckWin
{
    private static int[,] MatrixSlots;
    public static int Joker;
    
    private static Image[,] ImageSlots;

    struct lineСombination
    {
        public int Value;
        public int Number;
    }
    
    struct Index
    {
        public int I;
        public int J;
    }
    
    public static void UpdateMatrix(int[] massIndexSlots, Image[] slots)
    {
        int lengthMatrix = (int) Math.Sqrt(massIndexSlots.Length);
        MatrixSlots = new int[lengthMatrix, lengthMatrix];
        ImageSlots = new Image[lengthMatrix, lengthMatrix];
        int k = 0;
        
        for (int i = 0; i < MatrixSlots.GetLength(0); i++)
        {
            for (int j = 0; j < MatrixSlots.GetLength(1); j++)
            {
                MatrixSlots[i, j] = massIndexSlots[k];
                ImageSlots[i, j] = slots[k];
                
                slots[k].color = Color.white;
                k++;
                
                //Debug.Log($"Matrix{k} : " + MatrixSlots[i, j]);
            }
        }
    }

    public static int GetWinAmount()
    {
        int prize = 0;

        prize = CheckingForJokers();
        prize += CheckFullMatrix();
        
        if(SettingsBandit.Instance.dataForFormulas.toggles[0].isOn) prize += CheckingVerticalLines();
        if(SettingsBandit.Instance.dataForFormulas.toggles[1].isOn) prize += CheckingHorizontalLines();
        if(SettingsBandit.Instance.dataForFormulas.toggles[2].isOn) prize += CheckingTheFirstCrosspiece();
        
        prize += CheckingTheSecondCrosspiece();

        return prize;
    }

    private static int CheckingForJokers()
    {
        int number = 0;
        List<Index> indexSlots = new List<Index>();

        for (int i = 0; i < MatrixSlots.GetLength(0); i++)
        {
            for (int j = 0; j < MatrixSlots.GetLength(1); j++)
            {
                if (MatrixSlots[i,j] == Joker)
                {
                    number++;
                    indexSlots.Add(new Index() {I = i, J = j});
                }
            }
        }

        if(number > 0) PaintingWinningSlots(Color.magenta, indexSlots.ToArray());
        return number * SettingsBandit.Instance.dataForFormulas.GlobalMultiplier;
    }

    private static int CheckFullMatrix()
    {
        int value = MatrixSlots[0, 0];
        int prize = 0;
        
        bool win = true;
        
        for (int i = 0; i < MatrixSlots.GetLength(0); i++)
        {
            for (int j = 0; j < MatrixSlots.GetLength(1); j++)
            {
                if (MatrixSlots[i,j] != value)
                {
                    win = false;
                    break;
                }
            }
        }

        if (win)
        {
            int numberLines = MatrixSlots.GetLength(0) + MatrixSlots.GetLength(1) + 2;
            int numberElementsCross = MatrixSlots.GetLength(0) + MatrixSlots.GetLength(1);
            
            prize = (value * MatrixSlots.GetLength(0)) * numberLines * SettingsBandit.Instance.dataForFormulas.GlobalMultiplier;
            prize += (value * numberElementsCross) * SettingsBandit.Instance.dataForFormulas.Xn * SettingsBandit.Instance.dataForFormulas.GlobalMultiplier;
            prize *= SettingsBandit.Instance.dataForFormulas.Xn;
        }
        return prize;
    }

    private static int CheckingHorizontalLines()
    {
        int prize = 0;

        for (int i = 0; i < MatrixSlots.GetLength(0); i++)
        {
            int[] line = new int[MatrixSlots.GetLength(1)];
            Index[] indexSlots = new Index[MatrixSlots.GetLength(0)];
            
            for (int j = 0; j < MatrixSlots.GetLength(1); j++)
            {
                line[j] = MatrixSlots[i, j];
                indexSlots[j].I = i;
                indexSlots[j].J = j;
            }

            var valueForPrize = CheckLine(line);
            if(valueForPrize.Number > 1) PaintingWinningSlots(Color.green, indexSlots);
            prize += valueForPrize.Value * valueForPrize.Number * SettingsBandit.Instance.dataForFormulas.GlobalMultiplier;
        }
        return prize;
    }

    private static int CheckingVerticalLines()
    {
        int prize = 0;

        for (int j = 0; j < MatrixSlots.GetLength(1); j++)
        {
            int[] line = new int[MatrixSlots.GetLength(0)];
            Index[] indexSlots = new Index[MatrixSlots.GetLength(0)];
            
            for (int i = 0; i < MatrixSlots.GetLength(0); i++)
            {
                line[i] = MatrixSlots[i, j];
                indexSlots[i].I = i;
                indexSlots[i].J = j;
            }

            var valueForPrize = CheckLine(line);
            if(valueForPrize.Number > 1) PaintingWinningSlots(Color.yellow, indexSlots);
            prize += valueForPrize.Value * valueForPrize.Number * SettingsBandit.Instance.dataForFormulas.GlobalMultiplier;
        }
        return prize;
    }
    
    private static int CheckingTheSecondCrosspiece()
    {
        int prize = 0;
        int number = 0;
        int I = MatrixSlots.GetLength(0) / 2;
        int J = MatrixSlots.GetLength(1) / 2;
        
        Index[] indexSlots = new Index[MatrixSlots.GetLength(0)];

        int value = MatrixSlots[I, J];

        for (int i = 0; i < MatrixSlots.GetLength(0); i++)
        {
            for (int j = 0; j < MatrixSlots.GetLength(1); j++)
            {
                if ((i == I || j == J) && MatrixSlots[i,j] != value)
                {
                    prize = -1;
                    break;
                }
                else if((i == I || j == J) && MatrixSlots[i,j] == value)
                {
                    indexSlots[i].I = i;
                    indexSlots[i].J = j;
                    number++;
                }
            }
        }

        if (prize != -1)
        {
            PaintingWinningSlots(Color.red, indexSlots);
            prize = value * number * SettingsBandit.Instance.dataForFormulas.Xn * SettingsBandit.Instance.dataForFormulas.GlobalMultiplier;
        }
        else
        {
            prize = 0;
        }

        return prize;
    }

    private static int CheckingTheFirstCrosspiece()
    {
        int prize = 0;
        int prize1 = 0;
        int prize2= 0;

        int value = MatrixSlots[0, 0];
        Index[] indexSlots = new Index[MatrixSlots.GetLength(0)];

        for (int i = 0; i < MatrixSlots.GetLength(0); i++)
        {
            if (value != MatrixSlots[i, i] || value != MatrixSlots[i, (MatrixSlots.GetLength(0) - 1) - i])
            {
                prize = -1;
                break;
            }
        }
        
        prize1 = CheckingTheMainDiagonal();
        prize2 = CheckingTheAuxiliaryDiagonal();
        
        if (prize != -1)
        {
            prize = prize1 + prize2;
            prize *= SettingsBandit.Instance.dataForFormulas.Xn;
        }
        else
        {
            prize = prize1 + prize2;
        }

        return prize;
    }

    private static int CheckingTheMainDiagonal()
    {
        int prize = 0;
        int[] line = new int[MatrixSlots.GetLength(0)];
        Index[] indexSlots = new Index[MatrixSlots.GetLength(0)];

        for (int i = 0; i < MatrixSlots.GetLength(0); i++)
        {
            line[i] = MatrixSlots[i, i];
            indexSlots[i].I = i;
            indexSlots[i].J = i;
        }

        var valueForPrize = CheckLine(line);
        if(valueForPrize.Number > 1) PaintingWinningSlots(Color.blue, indexSlots);
        prize += valueForPrize.Value * valueForPrize.Number * SettingsBandit.Instance.dataForFormulas.GlobalMultiplier;;
        return prize;
    }

    private static int CheckingTheAuxiliaryDiagonal()
    {
        int prize = 0;
        int[] line = new int[MatrixSlots.GetLength(0)];
        Index[] indexSlots = new Index[MatrixSlots.GetLength(0)];

        for (int i = 0; i < MatrixSlots.GetLength(0); i++)
        {
            line[i] = MatrixSlots[i, (MatrixSlots.GetLength(0)-1) - i];
            indexSlots[i].I = i;
            indexSlots[i].J = (MatrixSlots.GetLength(0)-1) - i;
        }

        var valueForPrize = CheckLine(line);
        if(valueForPrize.Number > 1) PaintingWinningSlots(Color.blue, indexSlots);
        prize += valueForPrize.Value * valueForPrize.Number * SettingsBandit.Instance.dataForFormulas.GlobalMultiplier;
        return prize;
    }

    private static lineСombination CheckLine(int[] line)
    {
        int newValue = 0;
        int newNumber = 0;

        var valueForPrize = new lineСombination();

        for (int i = 0; i < line.Length; i++)
        {
            newValue = line[i];
            newNumber = 0;
            
            for (int j = 0; j < line.Length; j++)
            {
                if (line[j] == newValue)
                {
                    newNumber++;
                }
            }

            if (newNumber >= valueForPrize.Number && newNumber > 1)
            {
                valueForPrize.Value = newValue;
                valueForPrize.Number = newNumber;
            }
        }
        
        return valueForPrize;
    }

    private static void PaintingWinningSlots(Color color, Index[] index)
    {
        for (int i = 0; i < index.Length; i++)
        {
            ImageSlots[index[i].I, index[i].J].color = color;
        }
    }
}
