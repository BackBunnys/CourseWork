using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Core.AssetManagement
{
    public interface IStoringStrategy
    {
        public static readonly IStoringStrategy OneTimeStoring = new OneTimeStoring();
        public static readonly IStoringStrategy PermanentStoring = new PermanentStoring();

        void Apply<T>(string name, IDictionary<string, T> items);
    }

    public class OneTimeStoring : IStoringStrategy
    {
        public void Apply<T>(string name, IDictionary<string, T> items)
        {
            items.Remove(name);
        }
    }

    public class PermanentStoring : IStoringStrategy
    {
        public void Apply<T>(string name, IDictionary<string, T> items)
        {
        }
    }
}