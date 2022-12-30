using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace GramophoneUtils.SavingLoading
{    
    public class SavingSystem : MonoBehaviour
    {
        [SerializeField] private string statePath = "state";

        public static string GetPathFromName(string fileName)
		{
            return Application.persistentDataPath + "/" + fileName + ".sav";
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
            ServiceLocator.Instance.GameStateManager.ChangeState(ServiceLocator.Instance.LoadingState);
            RestoreState(LoadFile(filePath));
        }

        public void SaveOnSceneChange()
        {
            string filePath = statePath;
            var state = LoadFile(filePath);
            CaptureState(state);
            SaveFile(state, filePath);
        }

        public void LoadOnSceneChange()
        {
            Debug.Log("Load on change scene");
            string filePath = statePath;
            RestoreState(LoadFile(filePath), false);
        }

        public void Delete(string fileName)
        {
            string filePath = GetPathFromName(fileName);
            if (!File.Exists(filePath))
            {
                return;
            }
            File.Delete(filePath);
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
            foreach (var saveable in FindObjectsOfType<SaveableEntity>(true))
            {
                state[saveable.Id] = saveable.CaptureState();
            }
            
            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private async void RestoreState(Dictionary<string, object> state, bool changeScene = true)
        {
            if (changeScene)
            {
                // Restore the scene first:
                int lastSceneBuildIndex = (int)state["lastSceneBuildIndex"];
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(lastSceneBuildIndex);
                while (asyncLoad.isDone == false)
                {
                    await Task.Yield();
                }
            }
            // Rest of state to restore:

            foreach (var saveable in FindObjectsOfType<SaveableEntity>(true))
            {
                if (state.TryGetValue(saveable.Id, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }
    }
}