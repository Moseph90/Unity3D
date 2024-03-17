using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayer(PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.bum";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.bum";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save File Not Found In " + path);
            return null;
        }
    }
}
