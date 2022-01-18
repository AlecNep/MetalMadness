using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadDataUtil : MonoBehaviour
{
    private static string SavePath => $"{Application.persistentDataPath}/save.txt";

    [ContextMenu("Save")]
    public void Save()
    {
        var state = LoadFile();
        CaptureState(state);
        SaveFile(state);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        var state = LoadFile();
        RestoreState(state);
    }

    private void SaveFile(object state)
    {
        using (var stream = File.Open(SavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private Dictionary<string, object> LoadFile()
    {
        if (!File.Exists(SavePath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(SavePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    private void CaptureState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>(true))
        {
            print("capturing " + saveable.name);
            state[saveable.Id] = saveable.CaptureState();
        }
    }

    private void RestoreState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>(true))
        {
            if (state.TryGetValue(saveable.Id, out object value))
            {
                print("restoring " + saveable.name);
                saveable.RestoreState(value);
            }
        }
    }
}
