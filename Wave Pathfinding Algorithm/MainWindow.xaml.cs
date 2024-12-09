using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Wave_Pathfinding_Algorithm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Matrix matrix;
        int xSize = 0;
        int nSize = 0;
        private bool isColorizingEnabled = false;
        private bool isDeletingEnabled = false;
        private bool isSelectingStart = false;
        private bool isSelectingEnd = false;
        private (int, int) startPosition;
        private (int, int) endPosition;
        private Grid myGrid;
        private bool isDiagonal = false;
        int delay = 400;
        public MainWindow()
        {
            InitializeComponent();
            this.SizeChanged += SizeChangedHandler;
        }

        private void GenerateMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                xSize = int.Parse(XSizeBox.Text); // x представляет количество строк
                nSize = int.Parse(NSizeBox.Text); // n представляет количество столбцов
                if (xSize <= 0 || nSize <= 0)
                {
                    MessageBox.Show("Размеры должны быть больше нуля.");
                    return;
                }
                matrix = new Matrix(xSize, nSize); // Создаем матрицу с правильными размерами
                                                   // Очистка предыдущих элементов матрицы
                matrixContentPresenter.Content = null;

                for (int i = 0; i < xSize; i++) // Используем xSize для прохода по строкам
                {
                    for (int j = 0; j < nSize; j++) // Используем nSize для прохода по столбцам
                    {
                        matrix.CellStates[(i, j)] = Brushes.White; // Все клетки изначально доступны
                    }
                }
                // Отображение матрицы
                DisplayMatrix(matrix, xSize, nSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при генерации матрицы: " + ex.Message);
            }
        }

        private void DisplayMatrix(Matrix matrix, int xSize, int nSize)
        {
            var grid = new Grid();

            double cellSize = Math.Min(matrixContentPresenter.ActualWidth / xSize, matrixContentPresenter.ActualHeight / nSize);


            for (int i = 0; i < xSize; i++) // Используем nSize для прохода по строкам
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(cellSize);
                grid.RowDefinitions.Add(rowDef);

                for (int j = 0; j < nSize; j++) // Используем xSize для прохода по столбцам
                {
                    ColumnDefinition columnDef = new ColumnDefinition();
                    columnDef.Width = new GridLength(cellSize);
                    grid.ColumnDefinitions.Add(columnDef);

                    Border cellBorder = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1),
                        Child = new Rectangle() { Fill = Brushes.White, Width = cellSize, Height = cellSize }
                    };
                    Grid.SetRow(cellBorder, i);
                    Grid.SetColumn(cellBorder, j);
                    grid.Children.Add(cellBorder);

                    // Установка цвета клетки согласно сохраненному состоянию
                    if (matrix.CellStates.TryGetValue((i, j), out var brush))
                    {
                        ((Rectangle)cellBorder.Child).Fill = brush;
                    }

                    // Локальные переменные для координат внутри обработчика событий
                    int localI = i;
                    int localJ = j;

                    // Добавление обработчика событий для изменения фона при клике
                    cellBorder.PreviewMouseLeftButtonDown += (sender, e) =>
                    {
                        if (!(sender is Border border)) return;

                        var rectangle = border.Child as Rectangle;
                        if (rectangle != null)
                        {
                            if (!isColorizingEnabled && !isDeletingEnabled && !isSelectingStart && !isSelectingEnd) return;

                            if (isColorizingEnabled)
                            {
                                // Изменение фона выбранной клетки на черный для добавления препятствия
                                rectangle.Fill = Brushes.Black;
                                // Обновление состояния клетки
                                matrix.CellStates[(localI, localJ)] = Brushes.Black;
                            }
                            else if (isDeletingEnabled)
                            {
                                // Изменение фона выбранной клетки на белый для удаления препятствия
                                rectangle.Fill = Brushes.White;
                                // Обновление состояния клетки
                                matrix.CellStates[(localI, localJ)] = Brushes.White;
                            }
                            else if (isSelectingStart)
                            {
                                rectangle.Fill = Brushes.Orange;
                                matrix.CellStates[(localI, localJ  )] = Brushes.Orange;
                                startPosition = (localI, localJ);
                                isSelectingStart = false;
                                // Сохраняем координаты начала пути
                            }
                            else if (isSelectingEnd)
                            {
                                rectangle.Fill = Brushes.Green;
                                matrix.CellStates[(localI, localJ)] = Brushes.Green;
                                endPosition = (localI, localJ);
                                isSelectingEnd = false;
                                // Сохраняем координаты конца пути
                            }
                        }
                    };
                }
            }

            matrixContentPresenter.Content = grid;
            myGrid = grid;
        }
        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            if (matrix != null)
            {
                DisplayMatrix(matrix, xSize, nSize); // Используем сохраненное состояние матрицы

            }
        }

        private void WallButton_Click(object sender, RoutedEventArgs e)
        {
            isColorizingEnabled = !isColorizingEnabled;
            isDeletingEnabled = false;
            RemoveWallButton.Content = "Режим удаления";
            WallButton.Content = isColorizingEnabled ? "Завершить" : "Добавить препятствие";
        }

        private void RemoveWallButton_Click(object sender, RoutedEventArgs e)
        {
            isDeletingEnabled = !isDeletingEnabled;
            isColorizingEnabled = false;
            WallButton.Content = "Добавить препятствие";
            RemoveWallButton.Content = isDeletingEnabled ? "Завершить" : "Режим удаления";
        }
        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            isSelectingStart = !isSelectingStart;
            StartBtn.Background = Brushes.Red;
            StartBtn.IsEnabled = false;
            isDeletingEnabled = false;
            isColorizingEnabled = false;
            RemoveWallButton.Content = isDeletingEnabled ? "Завершить" : "Режим удаления";
            WallButton.Content = isColorizingEnabled ? "Завершить" : "Добавить препятствие";
        }

        private void EndBtn_Click(object sender, RoutedEventArgs e)
        {
            isSelectingEnd = !isSelectingEnd;
            EndBtn.Background = Brushes.Red;
            EndBtn.IsEnabled = false;
            isDeletingEnabled = false;
            isColorizingEnabled = false;
            
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = true;
            EndBtn.IsEnabled = true;
            myGrid.Children.Clear();
            matrix.IsAccessibleCells.Clear();
            matrix.StepsToEachCell.Clear();
            matrix.VisitedCells.Clear();
            StartBtn.IsEnabled = true;
            EndBtn.IsEnabled = true;
            RemoveWallButton.IsEnabled = true;
            WallButton.IsEnabled = true;
            ResizeMode = ResizeMode.CanResize;

            // Отображение матрицы
            DisplayMatrix(matrix, xSize, nSize);


        }

        private void TraceBtn_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = false;
            EndBtn.IsEnabled = false;
            RemoveWallButton.IsEnabled = false;
            WallButton.IsEnabled = false; 
            _ = AsyncMethod();
            ResizeMode = ResizeMode.NoResize;
        }
        private async Task AsyncMethod()
        {
            (int steps, List<(int, int)> path) = await WaveAlgorithmAsync(startPosition.Item1, startPosition.Item2, endPosition.Item1, endPosition.Item2, xSize, nSize, matrix, isDiagonal);

            if (steps != -1)
            {
                UpdateUIWithSteps(steps, path);
            }
            else
            {
                MessageBox.Show("Путь не найден");
            }
        }

        private async Task<(int Steps, List<(int, int)>)> WaveAlgorithmAsync(int startX, int startY, int endX, int endY, int xSize, int nSize, Matrix matrix, bool useDiagonalMovement)

        {
            Queue<(int, int, int, List<(int, int)>, Dictionary<(int, int), bool>)> queue = new Queue<(int, int, int, List<(int, int)>, Dictionary<(int, int), bool>)>();
            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            queue.Enqueue((startX, startY, 0, new List<(int, int)> { (startX, startY) }, new Dictionary<(int, int), bool>()));

            while (queue.Count > 0)
            {
                int currentCount = queue.Count;
                for (int i = 0; i < currentCount; i++)
                {
                    var current = queue.Dequeue();
                    int x = current.Item1;
                    int y = current.Item2;
                    int steps = current.Item3;
                    List<(int, int)> path = current.Item4;
                    Dictionary<(int, int), bool> visitedCells = current.Item5;

                    if (x == endX && y == endY)
                    {
                        return (steps, path);
                    }

                    var directions = useDiagonalMovement ? new[] { (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1) } :
                                            new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

                    foreach (var direction in directions)
                    {
                        int newX = x + direction.Item1;
                        int newY = y + direction.Item2;

                        if (newX >= 0 && newX < xSize && newY >= 0 && newY < nSize && !visited.Contains((newX, newY)))
                        {
                            if (matrix.CellStates.TryGetValue((newX, newY), out var cellState) && cellState != Brushes.Black)
                            {
                                queue.Enqueue((newX, newY, steps + 1, path.Concat(new List<(int, int)> { (newX, newY) }).ToList(), visitedCells.Concat(new Dictionary<(int, int), bool> { { (newX, newY), true } }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)));
                                visited.Add((newX, newY));
                                matrix.VisitedCells[(newX, newY)] = true;
                                matrix.StepsToEachCell[(newX, newY)] = steps + 1;
                                await Task.Delay(delay);
                                UpdateUIWithSteps(steps + 1, path.Concat(new List<(int, int)> { (newX, newY) }).ToList());
                            }
                        }
                    }
                }
            }

            return (-1, null);
        }





        private void UpdateUIWithSteps(int steps, List<(int, int)> path)
        {
           
            if (path == null || path.Count == 0) return;

            // Цвет для самого коротшего пути
            Brush pathColor = Brushes.Yellow;

            // Цвет для посещенных клеток
            Brush visitedColor = Brushes.LightBlue;

            for (int i = 0; i < nSize; i++) // Используем nSize для прохода по строкам
            {
                for (int j = 0; j < xSize; j++) // Используем xSize для прохода по столбцам
                {
                    var cellBorder = myGrid.Children.Cast<Border>().FirstOrDefault(b => Grid.GetRow(b) == j && Grid.GetColumn(b) == i);
                    if (cellBorder != null)
                    {
                        var rectangle = cellBorder.Child as Rectangle;
                        if (rectangle != null)
                        {
                            // Проверяем, находится ли клетка на самом коротком пути
                            bool isOnShortestPath = path.Contains((j, i));
                            // Проверяем, была ли клетка посещена
                            bool wasVisited = matrix.VisitedCells.TryGetValue((j, i), out bool _) ? true : false;

                            // Проверяем, является ли клетка начальной или конечной точкой
                            bool isStartPosition = (j, i) == startPosition;
                            bool isEndPosition = (j, i) == endPosition;

                            if (isOnShortestPath && !isStartPosition && !isEndPosition)
                            {
                                // Если клетка на самом коротком пути и не является начальной или конечной, окрашиваем её в желтый цвет
                                rectangle.Fill = pathColor;
                            }
                            else if (wasVisited && !isStartPosition && !isEndPosition)
                            {
                                // Если клетка была посещена и не является начальной или конечной, окрашиваем её в голубой цвет
                                rectangle.Fill = visitedColor;
                            }
                            else
                            {
                                // Если клетка не на пути, не была посещена, или является начальной/конечной, восстанавливаем её исходный цвет
                                if (matrix.CellStates.TryGetValue((j, i), out var originalColor))
                                {
                                    rectangle.Fill = originalColor;
                                }
                            }

                            // Добавляем текстовый блок для отображения количества шагов
                            if (!isStartPosition)
                            {
                                TextBlock textBlock = new TextBlock
                                {
                                    Text = matrix.StepsToEachCell.ContainsKey((j, i)) ? matrix.StepsToEachCell[(j, i)].ToString() : "",
                                    Foreground = Brushes.Black,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    FontSize = 20
                                };
                                Grid.SetRow(textBlock, j);
                                Grid.SetColumn(textBlock, i);
                                myGrid.Children.Add(textBlock);
                            }
                        }
                    }
                }
            }
        }

        private void OrtogonBtn_Click(object sender, RoutedEventArgs e)
        {
            isDiagonal = false;
            OrtogonBtn.IsEnabled = false;
            DiagonBtn.IsEnabled = true;
        }

        private void DiagonBtn_Click(object sender, RoutedEventArgs e)
        {
            isDiagonal = true;
            DiagonBtn.IsEnabled = false;
            OrtogonBtn.IsEnabled = true;
        }

        private void AboutBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Программу выполнил студент группы ПКТб-21-1 Абрамов Руслан Закирович");
        }

        private void AnimCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            delay = 400;
        }

        private void AnimCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            delay = 0;
        }
    }
}
