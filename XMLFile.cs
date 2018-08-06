using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityHelper.Model
{
    public class XMLFile<T> // : INotifyPropertyChanged
    {
        public DateTime Creation { get; set; }
        public System.IO.FileInfo FileInfo { get; set; }
        public T Object { get; set; }

    }
}
