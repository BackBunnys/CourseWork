using System;
using System.Collections.Generic;
using CourseWork.Common;
using CourseWork.Common.OpenGL;
using CourseWork.Common.Render;
using NAudio.Wave;
using OpenTK.Graphics.OpenGL;

namespace CourseWork.Core.AssetManagement
{
    public static class AssetManager
    {
        private static readonly ShaderLoader ShaderLoader = new();

        private static readonly IDictionary<string, Asset<ShaderProgram>> Shaders =
            new Dictionary<string, Asset<ShaderProgram>>();

        private static readonly IDictionary<string, Asset<Texture>> Textures = new Dictionary<string, Asset<Texture>>();

        private static readonly IDictionary<string, Asset<AudioFileReader>> Sounds =
            new Dictionary<string, Asset<AudioFileReader>>();

        public static void LoadShader(string name, ShaderProgramInfo shaderInfo,
            IStoringStrategy storingStrategy = null)
        {
            Shaders[name] = new Asset<ShaderProgram>(ShaderLoader.Load(shaderInfo), storingStrategy);
        }

        public static ShaderProgram GetShader(string name)
        {
            if (!Shaders.TryGetValue(name, out var asset))
                throw new ArgumentException("Shader with specified name is not exists");

            asset.StoringStrategy.Apply(name, Shaders);

            return asset.Item;
        }

        public static void LoadTexture(string name, string path, IStoringStrategy storingStrategy = null,
            TextureTarget target = TextureTarget.Texture2D)
        {
            Textures[name] = new Asset<Texture>(new Texture(path, target), storingStrategy);
        }

        public static void LoadTextureSet(string name, TextureSetInfo textureSetInfo,
            IStoringStrategy storingStrategy = null, TextureTarget target = TextureTarget.Texture2D)
        {
            var mainTexture = new Texture(textureSetInfo.ColorPath, target);
            mainTexture.SubTextures.Add(new Texture(textureSetInfo.NormalPath, target));
            Textures[name] = new Asset<Texture>(mainTexture, storingStrategy);
        }

        public static Texture GetTexture(string name)
        {
            if (!Textures.TryGetValue(name, out var asset))
                throw new ArgumentException("Texture with specified name is not exists");

            asset.StoringStrategy.Apply(name, Textures);

            return asset.Item;
        }

        public static void LoadSound(string name, string path, IStoringStrategy storingStrategy = null)
        {
            Sounds[name] = new Asset<AudioFileReader>(new AudioFileReader(path), storingStrategy);
        }

        public static AudioFileReader GetSound(string name)
        {
            if (!Sounds.TryGetValue(name, out var asset))
                throw new ArgumentException("Texture with specified name is not exists");

            asset.StoringStrategy.Apply(name, Sounds);

            return asset.Item;
        }

        private struct Asset<T>
        {
            public readonly T Item;
            public readonly IStoringStrategy StoringStrategy;

            public Asset(T item, IStoringStrategy storingStrategy)
            {
                Item = item;
                StoringStrategy = storingStrategy ?? IStoringStrategy.OneTimeStoring;
            }
        }
    }
}