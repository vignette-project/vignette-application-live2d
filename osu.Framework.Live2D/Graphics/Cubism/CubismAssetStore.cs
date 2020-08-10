using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CubismFramework;
using osu.Framework.IO.Stores;

namespace osu.Framework.Graphics.Cubism
{
    public class CubismAssetStore : IResourceStore<CubismAsset>
    {
        private IResourceStore<byte[]> store { get; }
        private Dictionary<string, CubismAsset> assetCache = new Dictionary<string, CubismAsset>();

        public CubismAssetStore(IResourceStore<byte[]> store)
        {
            this.store = store;
        }

        public CubismAsset Get(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            lock (assetCache)
            {
                if (!assetCache.TryGetValue(name, out var asset))
                {
                    try
                    {
                        var baseDir = name.Split(".")[0];
                        assetCache[name] = asset = new CubismAsset(name, (string path) =>
                        {
                            path = path.Replace("/", ".");
                            if (!path.Contains($"{baseDir}."))
                                return GetStream($"{baseDir}.{path}");
                            else
                                return GetStream(path);
                        });

                        return asset;
                    }
                    catch
                    {
                    }
                }

                return asset;
            }
        }

        public Task<CubismAsset> GetAsync(string name) => Task.Run(() => Get(name));

        public IEnumerable<string> GetAvailableResources() => store.GetAvailableResources();

        public Stream GetStream(string name) => store.GetStream(name);

        private bool isDisposed;

        ~CubismAssetStore()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                store.Dispose();
            }
        }
    }
}