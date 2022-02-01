using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class DataManager : MonoBehaviour {
   private static string dataToDelete;
   
   public static void Save<T>(T objectToSave, string choreography, string key) {
      String path = Application.persistentDataPath + "/saves/" + choreography + "/";
      Directory.CreateDirectory(path);
      
      BinaryFormatter formatter = new BinaryFormatter();
      using (FileStream fileStream = new FileStream(path + key + ".jd", FileMode.Create)) {
         formatter.Serialize(fileStream, objectToSave);
      }
   }

   public static T Load<T>(string choreography, string key) {
      string path = Application.persistentDataPath + "/saves/" + choreography + "/";
      BinaryFormatter formatter = new BinaryFormatter();
      T loadedData;
      using (FileStream fileStream = new FileStream(path + key + ".jd", FileMode.Open)) {
         loadedData = (T) formatter.Deserialize(fileStream);
      }
      return loadedData;
   }
   
   public static void SaveConfig<T>(T objectToSave, string key) {
      String path = Application.persistentDataPath + "/config/";
      Directory.CreateDirectory(path);
      
      BinaryFormatter formatter = new BinaryFormatter();
      using (FileStream fileStream = new FileStream(path + key + ".conf", FileMode.Create)) {
         formatter.Serialize(fileStream, objectToSave);
      }
   }
   
   public static T LoadConfig<T>(string key) {
      string path = Application.persistentDataPath + "/config/";
      BinaryFormatter formatter = new BinaryFormatter();
      T loadedData;
      using (FileStream fileStream = new FileStream(path + key + ".conf", FileMode.Open)) {
         loadedData = (T) formatter.Deserialize(fileStream);
      }
      return loadedData;
   }
   
   public static bool DataExists(string choreography) {
      string path = Application.persistentDataPath + "/saves/" + choreography + "/";
      return Directory.Exists(path);
   }
   
   public static bool SaveDataFileExists(string choreography, string key) {
      string path = Application.persistentDataPath + "/saves/" + choreography + "/" + key + ".jd";
      return File.Exists(path);
   }
   
   public static bool ConfigDataFileExists(string key) {
      string path = Application.persistentDataPath + "/config/" + key + ".conf";
      return File.Exists(path);
   }

   public static void DeleteSavedData(string choreography) {
      if (DataExists(choreography)) {
         string path = Application.persistentDataPath + "/saves/" + choreography + "/";
         ClearDirectory(path);
         MenuEvents.Instance.LoadSaves();
      }
      else {
         Debug.LogError("Data " + choreography + " don't exists!");
      }
   }
   
   private static void ClearDirectory(string target_dir) {
      string[] files = Directory.GetFiles(target_dir);
      string[] dirs = Directory.GetDirectories(target_dir);

      foreach (string file in files) {
         File.SetAttributes(file, FileAttributes.Normal);
         File.Delete(file);
      }

      foreach (string dir in dirs) {
         ClearDirectory(dir);
      }
      Directory.Delete(target_dir, false);
   }

   public void SaveToCSV(StepsToExport droneSteps) {
      List<string[]> rowData = new List<string[]>();
      
      foreach (Vector3 coordinate in droneSteps.DroneCoordinates) {
         string[] rowDataTemp = new string[3];
         rowDataTemp[0] = coordinate.x.ToString();
         rowDataTemp[1] = coordinate.y.ToString();
         rowDataTemp[2] = coordinate.z.ToString();
         rowData.Add(rowDataTemp);
      }

      string[][] output = new string[rowData.Count][];

      for(int i = 0; i < output.Length; i++){
         output[i] = rowData[i];
      }

      int length = output.GetLength(0);
      string delimiter = "|";

      StringBuilder sb = new StringBuilder();
        
      for (int index = 0; index < length; index++)
         sb.AppendLine(string.Join(delimiter, output[index]));


      string masterDirectory = Application.persistentDataPath + "/data/";
      string directoryPath = masterDirectory + droneSteps.ChoreographyName;
      string filePath = directoryPath + "/" + droneSteps.DroneName + ".csv";

      if (!Directory.Exists(masterDirectory)) { Directory.CreateDirectory(masterDirectory); }
      if (!Directory.Exists(directoryPath)) { Directory.CreateDirectory(directoryPath); }
      
      if (File.Exists(filePath)) {
         File.Delete(filePath);
      }
      
      StreamWriter outStream = File.CreateText(filePath);
      outStream.WriteLine(sb);
      outStream.Close();
   }

   public static string DataToDelete {
      get => dataToDelete;
      set => dataToDelete = value;
   }
}
