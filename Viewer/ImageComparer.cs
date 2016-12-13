using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Viewer
{
    class ImageComparer : IEqualityComparer<Image>
    {
        public bool Equals(Image x, Image y)
        {
            if (x.Name == y.Name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(Image obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
