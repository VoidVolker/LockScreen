using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Lib.DataTypes.Structures
{
    /// <summary>
    /// JSONFile interface
    /// </summary>
    /// <remarks>
    /// Create new JSON file
    /// </remarks>
    /// <param name="filePath">Path to file</param>
    [JsonObject(MemberSerialization.OptIn)]
    public class JSONFile(string filePath) : Json<JSONFile>
    {
        const string DefaultFilePath = "./settings.json";

        /// <summary>
        /// Path to JSON file
        /// </summary>
        public string FilePath = filePath;

        public new JsonSerializerSettings ToSettings = JsonSettings.SettingsWithNull;
        public new JsonSerializerSettings FromSettings = JsonSettings.SettingsWithNull;

        public JSONFile() : this(DefaultFilePath)
        {

        }

        ///// <summary>
        ///// Load data to variable
        ///// </summary>
        ///// <param name="varReference">Reference to variable</param>
        //public void LoadTo(ref JSONFile varReference)
        //{            
        //    varReference = Load();
        //}

        /// <summary>
        /// Load data from file and deserialize it
        /// </summary>
        public bool TryLoadData<T>(out T data) where T : JSONFile, new()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath, Encoding.UTF8);
                try
                {
                    data = JsonConvert.DeserializeObject<T>(json, FromSettings);
                    return true;
                }
                catch (Exception)
                {
                    Clear<T>();
                }
            }
            else
            {
                Save();
            }
            data = default;
            return false;
        }

        /// <summary>
        /// Save current object to JSON file
        /// </summary>
        public void Save()
        {
            File.WriteAllText(
                FilePath,
                ToJson(ToSettings),
                Encoding.UTF8
            );
        }

        /// <summary>
        /// Clear file
        /// </summary>
        public void Clear<T>() where T : JSONFile, new()
        {
            T data = new() { FilePath = FilePath };
            File.WriteAllText(
                FilePath,
                data.ToJson(ToSettings),
                Encoding.UTF8
            );
        }
    }
}
