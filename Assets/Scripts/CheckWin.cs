using System;
using UnityEngine;

public static class CheckWin
{
    private static int[,] MatrixSlots;
    public static int Joker;

    struct MyStruct
    {
        public int Value;
        public int Number;
    }
    
    public static void UpdateMatrix(params int[] massIndexSlots)
    {
        int lengthMatrix = (int) Math.Sqrt(massIndexSlots.Length);
        MatrixSlots = new int[lengthMatrix, lengthMatrix];
        int k = 0;
        
        for (int i = 0; i < MatrixSlots.GetLength(0); i++)
        {
            for (int j = 0; j < MatrixSlots.GetLength(1); j++)
            {
                MatrixSlots[i, j] = massIndexSlots[k];
                //Debug.Log($"Matrix{k} : " + MatrixSlots[i, j]);
                k++;
            }
        }
    }

    public static int GetWinAmount()
    {
        int prize = 0;

        prize = CheckingForJokers();
        prize += CheckFullMatrix();
        prize += CheckingHorizontalLines();
        prize+= CheckingVerticalLines();
        prize += CheckingTheFirstCrosspiece();
        prize += CheckingTheSecondCrosspiece();

        return prize;
    }

    private static int CheckingForJokers()
    {
        int number = 0;

        for (int i = 0; i < MatrixSlots.GetLength(0); i++)
        {
            for (int j = 0; j < MatrixSlots.GetLength(1); j++)
            {
                if (MatrixSlots[i,j] == Joker)
                {
                    number++;
                }
            }
        }
        
        return number * 10;
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
            prize = (value * 3) * 8 * 10;
            prize += (value * 6) * 10 * 2;
            prize *= 2;
        }
        return prize;
    }

    private static int CheckingHorizontalLines()
    {
        int prize = 0;

        for (int i = 0; i < MatrixSlots.GetLength(0); i++)
        {
            int[] line = new int[MatrixSlots.GetLength(1)];
            
            for (int j = 0; j < MatrixSlots.GetLength(1); j++)
            {
                line[j] = MatrixSlots[i, j];
            }

            var valueForPrize = CheckLine(line);
            prize += valueForPrize.Value * valueForPrize.Number * 10;
        }
        return prize;
    }

    private static int CheckingVerticalLines()
    {
        int prize = 0;

        for (int j = 0; j < MatrixSlots.GetLength(1); j++)
        {
            int[] line = new int[MatrixSlots.GetLength(0)];
            
            for (int i = 0; i < MatrixSlots.GetLength(0); i++)
            {
                line[i] = MatrixSlots[i, j];
            }

            var valueForPrize = CheckLine(line);
            prize += valueForPrize.Value * valueForPrize.Number * 10;
        }
        return prize;
    }
    
    private static int CheckingTheSecondCrosspiece()
    {
        int prize = 0;
        int number = 0;
        int I = MatrixSlots.GetLength(0) / 2;
        int J= MatrixSlots.GetLength(1) / 2;;

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
                    number++;
                }
            }
        }

        if (prize != -1)
        {
            prize = value * number * 10 * 2;
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
            prize *= 2;
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

        for (int i = 0; i < MatrixSlots.GetLength(0); i++)
        {
            line[i] = MatrixSlots[i, i];
        }

        var valueForPrize = CheckLine(line);
        prize += valueForPrize.Value * valueForPrize.Number * 10;
        return prize;
    }

    private static int CheckingTheAuxiliaryDiagonal()
    {
        int prize = 0;
        int[] line = new int[MatrixSlots.GetLength(0)];

        for (int i = 0; i < MatrixSlots.GetLength(0); i++)
        {
            line[i] = MatrixSlots[i, (MatrixSlots.GetLength(0)-1) - i];
        }

        var valueForPrize = CheckLine(line);
        prize += valueForPrize.Value * valueForPrize.Number * 10;
        return prize;
    }

    private static MyStruct CheckLine(params int[] line)
    {
        int newValue = 0;
        int newNumber = 0;

        var valueForPrize = new MyStruct();

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
}
