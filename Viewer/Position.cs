using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Viewer
{
    //класс отвечает за положение контрола Image  при добавлении его на сетку.
    public static class Position
    {
        static int row = 0;
        static int column = 0;
        static public int ROW
        {
            get
            {
                return row;
            }
        }
        static public int COLUMN
        {
            get
            {
                return column;
            }
        }
        static public void NextPosition(Grid grid)
        {
            column++;
            //окно вьювера будет показывать только по 4 картинки в ряду
            if(column==4)
            {
                row++;
                column = 0;
                grid.RowDefinitions.Add(new RowDefinition() { MinHeight = 150});
            }
        }
    }
}
