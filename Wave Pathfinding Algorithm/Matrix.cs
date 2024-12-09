using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Wave_Pathfinding_Algorithm
{
    public class Matrix
    {
        public int[,] Values { get; set; }
        public List<Point> Walls { get; set; } // Список препятствий
        public Dictionary<(int, int), Brush> CellStates { get; set; } // Состояние каждой клетки
        public Dictionary<(int, int), bool> IsAccessibleCells { get; set; } = new Dictionary<(int, int), bool>();
        public Dictionary<(int, int), int> StepsToEachCell { get; set; } = new Dictionary<(int, int), int>();
        public Dictionary<(int, int), bool> VisitedCells { get; set; } = new Dictionary<(int, int), bool>();

        public Matrix(int xSize, int nSize)
        {
            Values = new int[xSize, nSize];
            Walls = new List<Point>();
            CellStates = new Dictionary<(int, int), Brush>(); // Инициализируем словарь состояний клеток
                                                            
        }
    }
}
