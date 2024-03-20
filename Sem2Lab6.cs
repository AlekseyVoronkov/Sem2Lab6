namespace ConsoleApp5
{

    class SquareMatrix
    {
        public int[,] matrix;
        public int size;

        public SquareMatrix()
        {
            Random random = new Random();
            size = random.Next(2, 10);
            matrix = new int[size, size];

            for (int RowIndex = 0; RowIndex < size; ++RowIndex)
            {
                for (int ColumnIndex = 0; ColumnIndex < size; ++ColumnIndex)
                {
                    matrix[RowIndex, ColumnIndex] = random.Next(1, 10);
                }
            }
        }

        public SquareMatrix(int size)
        {
            this.size = size;
            Random random = new Random();
            matrix = new int[size, size];

            for (int RowIndex = 0; RowIndex < size; ++RowIndex)
            {
                for (int ColumnIndex = 0; ColumnIndex < size; ++ColumnIndex)
                {
                    matrix[RowIndex, ColumnIndex] = random.Next(1, 10);
                }
            }
        }

        public static SquareMatrix operator +(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            SquareMatrix result = new SquareMatrix(matrix1.matrix.GetLength(0));

            for (int rowIndex = 0; rowIndex < matrix1.matrix.GetLength(0); ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < matrix1.matrix.GetLength(1); ++columnIndex)
                {
                    result.matrix[rowIndex, columnIndex] = matrix1.matrix[rowIndex, columnIndex] + matrix2.matrix[rowIndex, columnIndex];
                }
            }

            return result;
        }

        public static SquareMatrix operator *(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            int size = matrix1.matrix.GetLength(0);
            SquareMatrix result = new SquareMatrix(size);

            for (int rowIndex = 0; rowIndex < size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < size; ++columnIndex)
                {
                    for (int anotherIndexIDK = 0; anotherIndexIDK < size; anotherIndexIDK++)
                    {
                        result.matrix[rowIndex, columnIndex] += matrix1.matrix[rowIndex, anotherIndexIDK] * matrix2.matrix[anotherIndexIDK, columnIndex];
                    }
                }
            }

            return result;
        }

        public SquareMatrix GetStepaMatrix(int row, int column)
        {
            SquareMatrix result = new SquareMatrix(size - 1);

            for (int columnIndex = 0; columnIndex < size - 1; ++columnIndex)
            {
                for (int rowIndex = 0; rowIndex < size - 1; ++rowIndex)
                {
                    result.matrix[rowIndex, columnIndex] = columnIndex < column ?
                        rowIndex < row ?
                        matrix[rowIndex, columnIndex] :
                        matrix[rowIndex + 1, columnIndex] :
                        rowIndex < row ?
                        matrix[rowIndex, columnIndex + 1] :
                        matrix[rowIndex + 1, columnIndex + 1];
                }
            }

            return result;
        }

        public int FindMatrixTrace()
        {
            int trace = 0;

            for (int index = 0; index < size; ++index)
            {
                trace += matrix[index, index];
            }

            return trace;
        }

        public int FindDeterminant()
        {
            if (size == 2)
            {
                int determinant = matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
                return determinant;
            }

            int result = 0;

            for (int columnIndex = 0; columnIndex < size; ++columnIndex)
            {

                result += (columnIndex % 2 == 1 ? 1 : -1) * matrix[1, columnIndex] * GetStepaMatrix(1, columnIndex).FindDeterminant();
            }

            return result;
        }

        public SquareMatrix FindTransposed()
        {
            var result = new SquareMatrix(size);

            for (int rowIndex = 0; rowIndex < size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < size; ++columnIndex)
                {
                    result.matrix[rowIndex, columnIndex] = matrix[columnIndex, rowIndex];
                }
            }

            return result;
        }

        public void PrintMatrix()
        {
            for (int RowIndex = 0; RowIndex < matrix.GetLength(0); ++RowIndex)
            {
                for (int ColumnIndex = 0; ColumnIndex < matrix.GetLength(1); ++ColumnIndex)
                {
                    Console.Write(matrix[RowIndex, ColumnIndex] + " ");
                }
              
                Console.WriteLine();
            }
        }

        public abstract class Handler
        {
            protected Handler next;

            public void setNext(Handler next)
            {
                this.next = next;
            }

            public abstract void handleRequest(int request, SquareMatrix matrix1, SquareMatrix matrix2);
        }

        public class MatrixSummHandler : Handler
        {
            override public void handleRequest(int request, SquareMatrix matrix1, SquareMatrix matrix2)
            {
                if (request == 1)
                {
                    var result = matrix1 + matrix2;
                    result.PrintMatrix();
                }
              
                else if (next != null)
                {
                    next.handleRequest(request, matrix1, matrix2);
                }
            }
        }

        public class MatrixMultiplicationHandler : Handler
        {
            override public void handleRequest(int request, SquareMatrix matrix1, SquareMatrix matrix2)
            {
                if (request == 2)
                {
                    var result = matrix1 * matrix2;
                    result.PrintMatrix();
                }
              
                else if (next != null)
                {
                    next.handleRequest(request, matrix1, matrix2);
                }
            }
        }

        public class MatrixTraceHandler : Handler
        {
            override public void handleRequest(int request, SquareMatrix matrix1, SquareMatrix matrix2)
            {
                if (request == 3)
                {
                    Console.WriteLine(matrix1.FindMatrixTrace());
                }
              
                else if (next != null)
                {
                    next.handleRequest(request, matrix1, matrix2);
                }
            }
        }

        public class MatrixDeterminantHandler : Handler
        {
            override public void handleRequest(int request, SquareMatrix matrix1, SquareMatrix matrix2)
            {
                if (request == 4)
                {
                    Console.WriteLine(matrix1.FindDeterminant());
                }
              
                else if (next != null)
                {
                    next.handleRequest(request, matrix1, matrix2);
                }
            }
        }

        public class MatrixTransposedHandler : Handler
        {
            override public void handleRequest(int request, SquareMatrix matrix1, SquareMatrix matrix2)
            {
                if (request == 5)
                {
                    var result = matrix1.FindTransposed();
                    result.PrintMatrix();
                }
              
                else if (next != null)
                {
                    next.handleRequest(request, matrix1, matrix2);
                }
            }
        }

        public class CloseAppHandler : Handler
        {
            public override void handleRequest(int request, SquareMatrix matrix1, SquareMatrix matrix2)
            {
                if (request == 6)
                {
                    Environment.Exit(0);
                }
              
                else if (next != null)
                {
                    next.handleRequest(request, matrix1, matrix2);
                }
            }
        }

        class Program
        {
            public delegate int MyDelegate();
            static void Main(string[] args)
            {
                Console.WriteLine("Type in size of the first matrix: ");

                int firstMatrixSize = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Type in size of the second matrix: ");

                int secondMatrixSize = Convert.ToInt32(Console.ReadLine());

                SquareMatrix matrix1 = new SquareMatrix(firstMatrixSize);
                Thread.Sleep(1);
                SquareMatrix matrix2 = new SquareMatrix(secondMatrixSize);

                MyDelegate findMatrixTrace = delegate ()
                {
                    int matrixTrace = matrix1.FindMatrixTrace();
                    return matrixTrace;
                };

                Console.WriteLine("First matrix");
                matrix1.PrintMatrix();
                Console.WriteLine("Second matrix");
                matrix2.PrintMatrix();
                Console.WriteLine();

                Console.WriteLine("Your options: ");
                Console.WriteLine("1. +; 2. *; 3. FindDeterminant; 4. FindMatrixTrace; 5. FindTransposed " +
                                  "\n6. Close this app");

                while (true)
                {
                    int userRequest = int.Parse(Console.ReadLine());

                    Handler matrixSummHandler = new MatrixSummHandler();
                    Handler matrixMultiplicationHandler = new MatrixMultiplicationHandler();
                    Handler matrixTraceHandler = new MatrixTraceHandler();
                    Handler matrixDeterminantHandler = new MatrixDeterminantHandler();
                    Handler matrixTransposedHandler = new MatrixTransposedHandler();
                    Handler closeAppHandler = new CloseAppHandler();

                    matrixSummHandler.setNext(matrixMultiplicationHandler);
                    matrixMultiplicationHandler.setNext(matrixTraceHandler);
                    matrixTraceHandler.setNext(matrixDeterminantHandler);
                    matrixDeterminantHandler.setNext(matrixTransposedHandler);
                    matrixTransposedHandler.setNext(closeAppHandler);

                    matrixSummHandler.handleRequest(userRequest, matrix1, matrix2);
                }
            }
        }
    }
}
