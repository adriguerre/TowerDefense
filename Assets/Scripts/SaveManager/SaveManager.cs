using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using GameResources;
using UnityEngine;

namespace SaveManager
{
    public class SaveManager : ISingleton<SaveManager>
    {
        private string _dataDirPath;
        private FileStream dataStream;
        private ResourcesDataDTO resourcesDataDto;
        private Aes iAes;
        
        
        private string resourcesDataFileName = "resources.txt";

        
        // Save the generated key
        private byte[] savedKey = { 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15 };

        
        public event EventHandler<ResourcesDataDTO> onDataLoaded;
        //public event EventHandler<PlayerData.PlayerData> onPlayerDataLoaded;
        
        public void OnApplicationQuit()
        {
            //QUITTING GAME - SAVING
            //SaveResourcesData();
            SaveEncryptedResourcesData();
            //SaveEncryptedMilitaryUpgradesData();
            //SaveUpgradeWithoutEncryption();
            //SaveEncryptedFactoryUpgradesData();
            //SaveFactoryUpgradeWithoutEncryption();
            //SaveEncryptedPlayerData();
            //SavePlayerDataWithoutEncryption();
        }
        
        
        #region Helper Methods 
        
        private bool CheckIfFileIsEmpty(string fullPath)
        {
            using (StreamReader streamReader = new StreamReader(fullPath))
            {
                string jsonFromFile = streamReader.ReadToEnd();

                if (string.IsNullOrEmpty(jsonFromFile))
                {
                    return true;
                }
            }
            return false;
        }
        
        #endregion
        
        
        #region Resources Data
        
        public void LoadResourcesDataEncrypted()
        {
            _dataDirPath = Application.persistentDataPath;
            string fullPath = Path.Combine(_dataDirPath, resourcesDataFileName);
            if (File.Exists(fullPath))
            {
                // Create FileStream for opening files.
                dataStream = new FileStream(fullPath, FileMode.Open);

                // Create new AES instance.
                Aes oAes = Aes.Create();
                // Create an array of correct size based on AES IV.
                byte[] outputIV = new byte[oAes.IV.Length];
                // Read the IV from the file.
                dataStream.Read(outputIV, 0, outputIV.Length);

                // Create CryptoStream, wrapping FileStream
                CryptoStream oStream = new CryptoStream(
                    dataStream,
                    oAes.CreateDecryptor(savedKey, outputIV),
                    CryptoStreamMode.Read);

                // Create a StreamReader, wrapping CryptoStream
                StreamReader reader = new StreamReader(oStream);

                // Read the entire file into a String value.
                string text = reader.ReadToEnd();
                // Always close a stream after usage.
                reader.Close();

                // Deserialize the JSON data 
                //  into a pattern matching the GameData class.
                resourcesDataDto = JsonUtility.FromJson<ResourcesDataDTO>(text);
            
            }
            else
            {
                //First time loading resources
                resourcesDataDto = new ResourcesDataDTO(0,0,0,
                    0,0,0,0,
                    0,0,0);
            }
            
            onDataLoaded?.Invoke(this, resourcesDataDto);
        }
        public void SaveEncryptedResourcesData()
        {
            iAes = Aes.Create();
            
            _dataDirPath = Application.persistentDataPath;
            //_dataDirPath = "C:/Users/adria/AppData/LocalLow/DefaultCompany/CatalystDefender";
            string fullPath = Path.Combine(_dataDirPath, resourcesDataFileName);

            // Create a FileStream for creating files.
            dataStream = new FileStream(fullPath, FileMode.Create);

            // Save the new generated IV.
            byte[] inputIV = iAes.IV;

            // Write the IV to the FileStream unencrypted.
            dataStream.Write(inputIV, 0, inputIV.Length);

            // Create CryptoStream, wrapping FileStream.
            CryptoStream iStream = new CryptoStream(
                dataStream,
                iAes.CreateEncryptor(savedKey, iAes.IV),
                CryptoStreamMode.Write);

            // Create StreamWriter, wrapping CryptoStream.
            StreamWriter sWriter = new StreamWriter(iStream);

            // Serialize the object into JSON and save string.
            ResourcesDataDTO data = ResourcesManager.Instance.GetResourcesDataDTO();
            string jsonString = JsonUtility.ToJson(data);

            // Write to the innermost stream (which will encrypt).
            sWriter.Write(jsonString);

            // Close StreamWriter.
            sWriter.Close();

            // Close CryptoStream.
            iStream.Close();

            // Close FileStream.
            dataStream.Close();
        }
        #endregion
    }
}