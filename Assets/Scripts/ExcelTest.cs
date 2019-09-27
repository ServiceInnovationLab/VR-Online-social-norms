using UnityEngine;
using System.IO;
using ExcelDataReader;

public class ExcelTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        /*
        using (var stream = File.Open("D:\\Sample Excel.xlsx", FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();

                var sms = result.Tables["SMS"];
                var twitter = result.Tables["Twitter"];

                foreach (System.Data.DataRow row in sms.Rows)
                {
                    int i = 0;
                }
            }
        }*/
    }
}
