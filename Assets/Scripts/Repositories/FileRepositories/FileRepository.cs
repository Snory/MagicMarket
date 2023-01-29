using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public enum RepositoryType { GAMEPLAY, DEFAULT}

public abstract class FileRepository<T> : Repository<T> where T: Identity
{
    [Header("FileRepository file setting")]
    [SerializeField]
    protected string _repositoryDataPath;
    [SerializeField]
    private string[] _repositoryFolderPath;
    [SerializeField] 
    private string _repositoryFileName;
    [SerializeField]
    protected RepositoryType _repositoryType;

    private void OnValidate()
    {
        if (_repositoryType == RepositoryType.DEFAULT)
        {
            _repositoryDataPath = Application.dataPath;
        }
        else if (_repositoryType == RepositoryType.GAMEPLAY)
        {
            _repositoryDataPath = Application.persistentDataPath;
        }
    }

    public override void Persist()
    {
        string folderStructure = "";

        foreach (var folder in _repositoryFolderPath)
        {
            folderStructure = Path.Combine(folderStructure,folder);
        }

        string fullPath = Path.Combine(_repositoryDataPath, folderStructure, _repositoryFileName +".data");

         try
        {
            //create folder if not exists
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //serialize json
            string dataToStore = JsonConvert.SerializeObject(_entries, Formatting.Indented);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (System.Exception e)
        {

            Debug.LogError($"Error occured when trying to save data to:" + fullPath + "\n" + e);
        }
    }

    public override void Initialize()
    {
        string folderStructure = "";

        foreach (var folder in _repositoryFolderPath)
        {
            folderStructure = Path.Combine(folderStructure, folder);
        }

        string fullPath = Path.Combine(_repositoryDataPath, folderStructure, _repositoryFileName + ".data");
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                _entries = JsonConvert.DeserializeObject<List<T>>(dataToLoad);

            } catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to load data from:" + fullPath + "\n" + e);
            }
        }
    }
}
