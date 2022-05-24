using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class SaveSystem
{
    public static void SaveCurrency ()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/currency.datajuli";
        FileStream stream = new FileStream (path, FileMode.Create);

        PlayerData data = new PlayerData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadCurrency()
    {
        string path = Application.persistentDataPath + "/currency.datajuli";
        if (!File.Exists(path))
            return null;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream (path, FileMode.Open);

        PlayerData data = formatter.Deserialize(stream) as PlayerData;
        stream.Close();

        return data;
    }
}
