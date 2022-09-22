using System;
using System.Linq;

namespace Matrix_Sort;

internal delegate void Sort(ref double[][] matrix, SwapCondition swapCondition, bool descending);

internal delegate bool SwapCondition(double[] a, double[] b, bool descending);

public static class Program
{
    public static void Main()
    {
        var matrix = new[] {new double[] {1, 2, 3}, new double[] {1, 10, 2}, new double[] {9, 8, 0}};
        Sort sort = BubbleSort;

        while (true)
        {
            SwapCondition condition;
            bool descending;

            Console.Clear();
            Console.WriteLine("Матрица: ");
            DisplayMatrix(matrix);

            Console.WriteLine("Выберите тип сортировки: ");
            Console.WriteLine("1 - Сортировка по сумме элементов строк матриц ");
            Console.WriteLine("2 - Сортировка по максимальному элементу в строке матрицы ");
            Console.WriteLine("3 - Сортировка по минимальному элементу в строке матрицы ");
            Console.WriteLine("0 - Выход");
            
            var input = Console.ReadKey(true).KeyChar;
            switch (input)
            {
                case '1':
                    condition = RowSumSwapCondition;
                    break;
                case '2':
                    condition = RowMaxSwapCondition;
                    break;
                case '3':
                    condition = RowMinSwapCondition;
                    break;
                case '0': return;
                default: continue;
            }

            Console.WriteLine("Выберите тип сравнения: ");
            Console.WriteLine("1 - По возрастанию ");
            Console.WriteLine("2 - По убыванию ");
            input = Console.ReadKey(true).KeyChar;
            switch (input)
            {
                case '1':
                    descending = false;
                    break;
                case '2':
                    descending = true;
                    break;
                default: continue;
            }
            sort(ref matrix, condition, descending);
        }
    }

    private static void BubbleSort(ref double[][] matrix, SwapCondition swapCondition, bool descending = false)
    {
        for (var i = 0; i < matrix.Length - 1; ++i)
        {
            for (var j = i + 1; j < matrix.Length; ++j)
                if (swapCondition(matrix[i], matrix[j], descending))
                    SwapRows(ref matrix[i], ref matrix[j]);
        }
    }

    private static void SwapRows(ref double[] a, ref double[] b)
    {
        for (var i = 0; i < a.Length; ++i)
            (a[i], b[i]) = (b[i], a[i]);
    }

    private static void DisplayMatrix(double[][] matrix)
    {
        foreach (var row in matrix)
        {
            foreach (var item in row)
            {
                Console.Write(item + "\t");
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private static bool RowSumSwapCondition(double[] a, double[] b, bool descending) =>
        a.Sum().CompareTo(b.Sum()) == (descending ? -1 : 1);

    private static bool RowMaxSwapCondition(double[] a, double[] b, bool descending) =>
        a.Max().CompareTo(b.Max()) == (descending ? -1 : 1);

    private static bool RowMinSwapCondition(double[] a, double[] b, bool descending) =>
        a.Min().CompareTo(b.Min()) == (descending ? -1 : 1);
}