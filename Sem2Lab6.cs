namespace ConsoleApp5
{

    class SquareMatrix
    {
        public int[,] Matrix;
        public int Size;

        public SquareMatrix()
        {
            Random random = new Random();
            Size = random.Next(2, 10);
            Matrix = new int[Size, Size];

            for (int rowIndex = 0; rowIndex < Size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < Size; ++columnIndex)
                {
                    Matrix[rowIndex, columnIndex] = random.Next(1, 10);
                }
            }
        }

        public SquareMatrix(int size)
        {
            this.Size = size;
            Random random = new Random();
            Matrix = new int[size, size];

            for (int rowIndex = 0; rowIndex < size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < size; ++columnIndex)
                {
                    Matrix[rowIndex, columnIndex] = random.Next(1, 10);
                }
            }
        }

        public static SquareMatrix operator +(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            SquareMatrix result = new SquareMatrix(matrix1.Matrix.GetLength(0));

            for (int rowIndex = 0; rowIndex < matrix1.Matrix.GetLength(0); ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < matrix1.Matrix.GetLength(1); ++columnIndex)
                {
                    result.Matrix[rowIndex, columnIndex] = matrix1.Matrix[rowIndex, columnIndex] + matrix2.Matrix[rowIndex, columnIndex];
                }
            }

            return result;
        }

        public static SquareMatrix operator *(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            int size = matrix1.Matrix.GetLength(0);
            SquareMatrix result = new SquareMatrix(size);

            for (int rowIndex = 0; rowIndex < size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < size; ++columnIndex)
                {
                    for (int anotherIndexIDK = 0; anotherIndexIDK < size; ++anotherIndexIDK)
                    {
                        result.Matrix[rowIndex, columnIndex] += matrix1.Matrix[rowIndex, anotherIndexIDK] * matrix2.Matrix[anotherIndexIDK, columnIndex];
                    }
                }
            }

            return result;
        }

        public SquareMatrix GetStepaMatrix(int row, int column)
        {
            SquareMatrix result = new SquareMatrix(Size - 1);

            for (int columnIndex = 0; columnIndex < Size - 1; ++columnIndex)
            {
                for (int rowIndex = 0; rowIndex < Size - 1; ++rowIndex)
                {
                    result.Matrix[rowIndex, columnIndex] = columnIndex < column ?
                        rowIndex < row ?
                        Matrix[rowIndex, columnIndex] :
                        Matrix[rowIndex + 1, columnIndex] :
                        rowIndex < row ?
                        Matrix[rowIndex, columnIndex + 1] :
                        Matrix[rowIndex + 1, columnIndex + 1];
                }
            }

            return result;
        }

        public int FindMatrixTrace()
        {
            int trace = 0;

            for (int index = 0; index < Size; ++index)
            {
                trace += Matrix[index, index];
            }

            return trace;
        }

        public SquareMatrix FindDiagonal(SquareMatrix matrix1)
        {
            var result = matrix1;

            for (int rowIndex = 0; rowIndex < matrix1.Size; ++rowIndex) 
            {
                for (int columnIndex = 0; columnIndex < matrix1.Size; ++columnIndex)
                {
                    if (rowIndex != columnIndex)
                    {
                        result.Matrix[rowIndex, columnIndex] = 0;
                    }
                }
            }

            return result;
        }

        public int FindDeterminant()
        {
            if (Size == 2)
            {
                int determinant = Matrix[0, 0] * Matrix[1, 1] - Matrix[1, 0] * Matrix[0, 1];
                return determinant;
            }

            int result = 0;

            for (int columnIndex = 0; columnIndex < Size; ++columnIndex)
            {

                result += (columnIndex % 2 == 1 ? 1 : -1) * Matrix[1, columnIndex] * GetStepaMatrix(1, columnIndex).FindDeterminant();
            }

            return result;
        }

        public SquareMatrix FindTransposed()
        {
            var result = new SquareMatrix(Size);

            for (int rowIndex = 0; rowIndex < Size; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < Size; ++columnIndex)
                {
                    result.Matrix[rowIndex, columnIndex] = Matrix[columnIndex, rowIndex];
                }
            }

            return result;
        }

        public void PrintMatrix()
        {
            for (int rowIndex = 0; rowIndex < Matrix.GetLength(0); ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < Matrix.GetLength(1); ++columnIndex)
                {
                    Console.Write(Matrix[rowIndex, columnIndex] + " ");
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

        public class MatrixDiagonalHandler : Handler
        {
            override public void handleRequest(int request, SquareMatrix matrix1, SquareMatrix matrix2)
            {
                if (request == 6)
                {
                    var result = matrix1.FindDiagonal(matrix1);
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
                if (request == 7)
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
                Console.WriteLine("1. +; 2. *; 3. Find Determinant; 4. Find Matrix Trace; 5. Find Transposed " +
                                  "\n6.Find Diagonal Matrix; 7. Close this app");

                while (true)
                {
                    int userRequest = int.Parse(Console.ReadLine());

                    Handler matrixSummHandler = new MatrixSummHandler();
                    Handler matrixMultiplicationHandler = new MatrixMultiplicationHandler();
                    Handler matrixTraceHandler = new MatrixTraceHandler();
                    Handler matrixDeterminantHandler = new MatrixDeterminantHandler();
                    Handler matrixTransposedHandler = new MatrixTransposedHandler();
                    Handler matrixDiagonalHandler = new MatrixDiagonalHandler();
                    Handler closeAppHandler = new CloseAppHandler();


                    matrixSummHandler.setNext(matrixMultiplicationHandler);
                    matrixMultiplicationHandler.setNext(matrixTraceHandler);
                    matrixTraceHandler.setNext(matrixDeterminantHandler);
                    matrixDeterminantHandler.setNext(matrixTransposedHandler);
                    matrixTransposedHandler.setNext(matrixDiagonalHandler);
                    matrixDiagonalHandler.setNext(closeAppHandler);

                    matrixSummHandler.handleRequest(userRequest, matrix1, matrix2);
                }
            }
        }
    }
}
