using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Core.AssetManagement
{
    internal class SoundLoader : IResourceLoader<AudioFileReader, string>
    {
        public AudioFileReader Load(string source)
        {
            return new AudioFileReader(source);
        }
    }
}
