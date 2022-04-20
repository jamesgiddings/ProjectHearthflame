using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GramophoneUtils.SavingLoading
{
    public class SavingSystem : MonoBehaviour
    {

        private string saveDirectory;

        private void Awake()
        {
            saveDirectory = Application.persistentDataPath;
        }

        public string GetPathFromName(string fileName)
		{
            return saveDirectory + "/" + fileName + ".sav";
        }

        public void Save(string fileName)
        {
            string filePath = GetPathFromName(fileName);
            var state = LoadFile(filePath);
            CaptureState(state);
            SaveFile(state, filePath);
        }

        public void Load(string fileName)
        {
            string filePath = GetPathFromName(fileName);
            RestoreState(LoadFile(filePath));
        }

        private Dictionary<string, object> LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Dictionary<string, object>();
            }

            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void SaveFile(object state, string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.Id] = saveable.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                if (state.TryGetValue(saveable.Id, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }
    }
}