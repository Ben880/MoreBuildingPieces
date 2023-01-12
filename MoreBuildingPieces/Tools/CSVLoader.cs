using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MoreBuildingPieces.Tools {

    public interface ICSV {
        void Add(string key, string val);
    }

    class CSVLoader<T> where T : ICSV, new() {

        private string filePath;
        private Dictionary<string, int> Headers = new Dictionary<string, int>();
        private Dictionary<string, int> ColumnIndex = new Dictionary<string, int>();
        private string[,] data = new string[500,100];

        public Dictionary<string, T> DO = new Dictionary<string, T>();

        public CSVLoader(string fileName) {
            this.filePath = Path.Combine(Generic.GetPathToMod(), fileName);
            if (Path.GetFileName(fileName) != fileName) {
                throw new Exception("'fileName' is invalid!");
            }
        }
        
        public void Load() {
            string[] lines = File.ReadAllLines(filePath);

            string[] fields;
            for (int i = 0; i < lines.Length; i++) {
                fields = lines[i].Split(',');
                for (int j = 0; j < fields.Length; j++) {
                    if (j == 0)
                        ColumnIndex.Add(fields[0], j);
                    if (i == 0)
                        Headers.Add(fields[j], i);
                    data[i, j] = fields[j];
                }
            }
        }

        public Dictionary<string, T> LoadToObjectDict() {
            Dictionary<string, T> data = new Dictionary<string, T>();
            string[] lines = File.ReadAllLines(filePath);
            string[] headers = lines[0].Split(',');
            T obj;
            foreach (string line in lines.Skip(1)) {
                obj = new T();
                string[] values = line.Split(',');
                for (int i = 0; i < values.Length; i++) {
                    obj.Add(headers[i], values[i]);
                }
                data.Add(values[0], obj);
            }
            return data;
        }
    }
}
