using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        Save s = new Save();

        s.score = 15;
        s.inventory = new List<string>();
        s.inventory.Add("GUS");

        SaveGame(s);

        Save load = LoadGame();


        
    }

    public void SaveGame(Save s)
    {
        BinaryFormatter bft = new BinaryFormatter();

        string path = Application.persistentDataPath;//AppData/LocalLow/SeuNome

        FileStream file = File.Create(path + "/savegame.save");

        bft.Serialize(file, s);
        file.Close();

        Debug.Log("Jogo Salvo!");
    }

    public Save LoadGame()
    {
        BinaryFormatter bft = new BinaryFormatter();
        string path = Application.persistentDataPath;
        FileStream file;

        if(File.Exists(path * "/savegame.save"))
        {

            file = File.Open(path + "/savegame.save", FileMode.Open);
            Save load = (Save)bft.Deserialize(file);
            file.Close();

            Debug.Log("Jogo Carregando!");

            return load;
        }

        return null;
    }
}
