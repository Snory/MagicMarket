using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public enum RepositoryType { RESOURCE, RUNTIME}

public abstract class Repository<T> : MonoBehaviour, IPersistableRepository where T : class
{

    [Header("Repository file setting")]
    [SerializeField]
    private string _repositoryDataPath;
    [SerializeField] 
    private string _repositoryFolderName;
    [SerializeField] 
    private string _repositoryFileName;
    [SerializeField] 
    protected List<T> _entries;
    [SerializeField]
    private RepositoryType _repositoryType;

    private void OnValidate()
    {
        switch (_repositoryType)
        {
            case RepositoryType.RUNTIME:
                _repositoryDataPath = Application.persistentDataPath;
                break;
            case RepositoryType.RESOURCE:
                _repositoryDataPath = Application.dataPath;
                break;
        }
    }
    private void Awake()
    {
        Initialize();
    }

    public virtual void Persist()
    {
        string fullPath = Path.Combine(_repositoryDataPath, "Resources", "Repositories", _repositoryFolderName, _repositoryFileName +".data");

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

    public virtual void Initialize()
    {
        string fullPath = Path.Combine(_repositoryDataPath, "Resources", "Repositories", _repositoryFolderName, _repositoryFileName + ".data");
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

    public abstract void CreateEntry();
}
