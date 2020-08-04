using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using MVVM_Example.Model;

namespace MVVM_Example.ViewModel
{
    public class JsonFileService : IFileService
    {
        public List<Phone> Open(string fileName)
        {
            List<Phone> phones = new List<Phone>();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Phone>));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                phones = jsonFormatter.ReadObject(fs) as List<Phone>;
            }

            return phones;
        }

        public void Save(string fileName, List<Phone> phoneList)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Phone>));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, phoneList);
            }
        }
    }
}