using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Core.AssetManagement
{
    internal interface IResourceLoader<out T, in TK> 
    {
        T Load(TK source);
    }
}
