using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FrontEnd.ModelConverter
{
    public class ObservableCollectionConverter
    {
        public static List<T> ConvertObservableCollection<T>(ObservableCollection<T> collection)
        {
            var list = new List<T>();
            foreach (var item in collection)
            {
                list.Add(item);
            }
            return list;
        }
    }
}