using System.Collections;
using System.Collections.Generic;
using ARobj;
using System.Xml.Serialization;
using System.IO;

namespace ARIO
{
    public class ObjectSaver
    {
        private List<ObjectData> dataToSave;
        private readonly string filepath = "";

        public ObjectSaver()
        {
            dataToSave = new List<ObjectData>();
        }

        /// <summary>
        /// Add a datum to the data saver
        /// </summary>
        /// <param name="textToRecognize">A word that will be recognized</param>
        /// <param name="objFileName">The name of the respective .obj file</param>
        public void AddData(string textToRecognize, string objFileName)
        {
            ObjectData obj = new ObjectData();
            obj.Text = textToRecognize;
            obj.FileName = objFileName;
            dataToSave.Add(obj);
        }

        /// <summary>
        /// Add multiple datums to the data saver
        /// </summary>
        /// <param name="datas"></param>
        public void AddData(params ObjectData[] datas)
        {
            dataToSave.AddRange(datas);
        }

        /// <summary>
        /// Delete a datums from the data saver
        /// </summary>
        /// <param name="selectiveIndex">Starts from 0, indicates which datums would be deleted</param>
        public void DeleteData(int selectiveIndex)
        {
            dataToSave.Remove(dataToSave[selectiveIndex]);
        }

        /// <summary>
        /// Write and save the data into a xml file
        /// </summary>
        /// <returns></returns>
        public bool SaveData()
        {
            try
            {
                if (dataToSave.Count != 0)
                {
                    ObjectData[] dataArray = dataToSave.ToArray();
                    XmlSerializer serializer = new XmlSerializer(typeof(ObjectData[]));
                    using (StreamWriter writer = new StreamWriter(filepath))
                    {
                        serializer.Serialize(writer, dataArray);
                        writer.Close();
                    }
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }

        }
    }

    public class ObjectLoader
    {
        private ObjectData[] loadedData;
        /// <summary>
        /// Load XML data
        /// </summary>
        /// <param name="filepath">Path to the target XML file</param>
        /// <returns></returns>
        public ObjectData[] LoadXML(string filepath)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(ObjectData[]));
                using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    loadedData = deserializer.Deserialize(fs) as ObjectData[];
                    return loadedData;
                }
            }
            catch
            {
                return null;
            }
        }
            
    }
}
