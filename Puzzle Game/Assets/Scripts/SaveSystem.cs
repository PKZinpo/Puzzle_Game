using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    
    public static void SaveGameData(GameObjectData gameData) {
        string path = Application.persistentDataPath + "/level.data";
        using (FileStream stream = new FileStream(path, FileMode.Create)) {
            BinaryFormatter formatter = new BinaryFormatter();
            GameData data = new GameData(gameData);
            formatter.Serialize(stream, data);
        }
    }

    public static GameData LoadGameData() {
        string path = Application.persistentDataPath + "/level.data";
        if (File.Exists(path)) {
            using (FileStream stream = new FileStream(path, FileMode.Open)) {
                BinaryFormatter formatter = new BinaryFormatter();
                GameData data = formatter.Deserialize(stream) as GameData;
                return data;
            }
        }
        else {
            Debug.LogError("NO FILE FOUND IN " + path);
            return null;
        }

    }
}
