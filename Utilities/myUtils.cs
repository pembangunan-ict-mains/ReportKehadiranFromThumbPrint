using ClosedXML.Excel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class myUtils : ImyUtils
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly myUtils _util;

        public myUtils(IJSRuntime jSRuntime)
        {
            _jsRuntime = jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime)); ;
        }

        public async Task DownloadFile(string fileName, byte[] content, string contentType)
        {
            await _jsRuntime.InvokeVoidAsync("downloadFile", fileName, content, contentType);
        }

        public byte[] ExportToExcel<T>(IEnumerable<T> data)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");
                var properties = typeof(T).GetProperties();

                // Add headers
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = properties[i].Name;
                }

                // Add data
                int row = 2;
                foreach (var item in data)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        var value = properties[i].GetValue(item);
                        if (value != null)
                        {
                            worksheet.Cell(row, i + 1).Value = value.ToString();
                        }
                        else
                        {
                            worksheet.Cell(row, i + 1).Value = "";
                        }
                    }
                    row++;
                }

                // Save the Excel file to a memory stream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public byte[] ExportToCSV<T>(IEnumerable<T> data)
        {
            var csvContent = new StringBuilder();

            var properties = typeof(T).GetProperties();

            // Write headers
            foreach (var prop in properties)
            {
                csvContent.Append($"{prop.Name},");
            }
            csvContent.AppendLine(); // Move to the next line after writing headers

            // Write data rows
            foreach (var item in data)
            {
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(item);
                    csvContent.Append($"{(value != null ? value.ToString() : "")},");
                }
                csvContent.AppendLine(); // Move to the next line after writing a row
            }

            // Convert the CSV content to byte array
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                streamWriter.Write(csvContent.ToString());
                streamWriter.Flush();
                return memoryStream.ToArray();
            }
        }

        public byte[] ExportMultipleToExcel(Dictionary<string, object> datasets)
        {
            using (var workbook = new XLWorkbook())
            {
                foreach (var entry in datasets)
                {
                    string sheetName = entry.Key;
                    object data = entry.Value;

                    if (data == null) continue;

                    var dataType = data.GetType();
                    var itemType = dataType.GetGenericArguments()[0];
                    var props = itemType.GetProperties();

                    var worksheet = workbook.Worksheets.Add(sheetName);

                    // Add headers
                    for (int i = 0; i < props.Length; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = props[i].Name;
                    }

                    // Add rows
                    int row = 2;
                    foreach (var item in (IEnumerable)data) // guna IEnumerable, bukan IEnumerable<object>
                    {
                        for (int i = 0; i < props.Length; i++)
                        {
                            var value = props[i].GetValue(item);
                            worksheet.Cell(row, i + 1).Value = value?.ToString() ?? string.Empty;
                        }
                        row++;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }



    }
}
