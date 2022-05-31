using CourseWork.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWork.Common.Render;

namespace CourseWork.Core.AssetManagement
{
    internal class TextureLoader : IResourceLoader<Texture, string>
    {
        public Texture Load(string source)
        {
            return new Texture(source);
        }
    }
}
