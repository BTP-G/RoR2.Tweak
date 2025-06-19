using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BTP.RoR2Plugin {

    public interface ISaveData {

        void SaveData();

        void LoadData();
    }

    public static class ProperSaveSupport {
        private static readonly List<Type> _saveDataTypes = [];

        public static void AddSaveDataType(Type type) {
            if (_saveDataTypes.Contains(type)) {
                $"此类型('{type.FullName}')已添加！请勿重复添加！".LogError();
                return;
            }
            if (!type.ImplementInterface(typeof(ISaveData))) {
                $"此类型('{type.FullName}')不实现接口('{typeof(ISaveData).FullName}')！请实现此接口！".LogError();
                return;
            }
            _saveDataTypes.Add(type);
            $"存档类型('{type.FullName}')已添加。".LogMessage();
        }

        public static void AddSaveDataType<T>() where T : ISaveData {
            var type = typeof(T);
            if (_saveDataTypes.Contains(type)) {
                $"此类型('{type.FullName}')已添加！请勿重复添加！".LogError();
                return;
            }
            _saveDataTypes.Add(type);
            $"存档类型('{type.FullName}')已添加。".LogMessage();
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Init() {
            ProperSave.SaveFile.OnGatherSaveData += SaveFile_OnGatherSaveData;
            ProperSave.Loading.OnLoadingEnded += Loading_OnLoadingEnded;
        }

        private static void SaveFile_OnGatherSaveData(Dictionary<string, object> obj) {
            foreach (var type in _saveDataTypes) {
                var saveData = (ISaveData)Activator.CreateInstance(type);
                saveData.SaveData();
                obj.Add(type.FullName, saveData);
                $"存档类型('{type.FullName}')已存入。".LogMessage();
            }
        }

        private static void Loading_OnLoadingEnded(ProperSave.SaveFile obj) {
            foreach (var type in _saveDataTypes) {
                obj.GetModdedData<ISaveData>(type.FullName)?.LoadData();
                $"存档类型('{type.FullName}')已载入。".LogMessage();
            }
        }
    }
}