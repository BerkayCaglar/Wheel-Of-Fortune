using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Modules.AddressableSystem
{
    public class AddressablesManager : MonoBehaviour
    {
        public static async UniTask<T> LoadAsset<T>(string key) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded) return handle.Result;
            else
            {
                Debug.LogError($"Failed to load asset with key: {key}");
                return null;
            }
        }

        public static async UniTask<List<T>> LoadAssets<T>(string key) where T : Object
        {
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(key, null);
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded) return new List<T>(handle.Result);
            else
            {
                Debug.LogError($"Failed to load assets with key: {key}");
                return null;
            }
        }

        public static async UniTask UnloadAsset<T>(T asset) where T : Object
        {
            Addressables.Release(asset);
            await UniTask.CompletedTask;
        }
    }
}
