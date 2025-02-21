using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using WebAPIExcel.Models.Contexts;
using WebAPIExcel.Models.Items;
using System.IO;

namespace WebAPIExcel.Controllers
{
     [Route("api/[controller]")]
     [ApiController]
    public class ExcelUploadController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExcelUploadController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/ExcelUpload
        [HttpPost]
        public async Task<ActionResult> PostExcelItem(IFormFile file)
        {
            // Validar que el archivo Excel no esté vacío
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Almacenar los datos del Excel
            var items = new List<Dictionary<string, object>>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.First();
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    var headerRow = GetHeaderRow(worksheet, rowCount, colCount);
                    if (headerRow == null)
                    {
                        return BadRequest("No se encontraron las columnas necesarias en el archivo.");
                    }

                    // Iterar sobre las filas del archivo Excel (desde la fila 2)
                    for (int row = 2; row <= rowCount; row++)
                    {
                        // Verifica que la "Clave" de la primera columna no sea vacía
                        var clave = worksheet.Cells[row, headerRow["CLAVE"]].Text;
                        var concepto = worksheet.Cells[row, headerRow["CONCEPTO"]].Text;
                        
                        if (string .IsNullOrEmpty(clave) || string.IsNullOrEmpty(concepto))
                        {
                            // Ignorar la fila si "Clave" está vacía
                            continue;
                        }

                        var excelFileItem = new ExcelFileItem
                        {
                            Clave = clave,
                            Concepto = worksheet.Cells[row, headerRow["CONCEPTO"]].Text,
                            Un = worksheet.Cells[row, headerRow["UN"]].Text,
                            Cant = decimal.TryParse(worksheet.Cells[row, headerRow["CANT."]].Text, out var cant) ? cant : 0,
                            PU = decimal.TryParse(worksheet.Cells[row, headerRow["P.U."]].Text, out var pu) ? pu : 0,
                            Importe = decimal.TryParse(worksheet.Cells[row, headerRow["IMPORTE"]].Text, out var importe) ? importe : 0
                        };

                        // Solo agregar el item si tiene al menos un valor significativo
                        var itemToAdd = new Dictionary<string, object>();

                        // Validamos que el valor no sea el valor predeterminado (vacío o cero) antes de agregarlo al JSON
                        if (!string.IsNullOrEmpty(excelFileItem.Clave) && excelFileItem.Clave != "CLAVE") itemToAdd["clave"] = excelFileItem.Clave;
                        if (!string.IsNullOrEmpty(excelFileItem.Concepto) && excelFileItem.Concepto != "CONCEPTO") itemToAdd["concepto"] = excelFileItem.Concepto;
                        if (!string.IsNullOrEmpty(excelFileItem.Un) && excelFileItem.Un != "UN") itemToAdd["un"] = excelFileItem.Un;
                        if (excelFileItem.Cant != 0) itemToAdd["cant"] = excelFileItem.Cant;
                        if (excelFileItem.PU != 0) itemToAdd["pu"] = excelFileItem.PU;
                        if (excelFileItem.Importe != 0) itemToAdd["importe"] = excelFileItem.Importe;
                        
                        // Si hay algo en itemToAdd, lo agregamos a la lista de items
                        if (itemToAdd.Any())
                        {
                            items.Add(itemToAdd);
                        }
                    }
                }
            }
            return Ok(items);
        }

        private Dictionary<string, int> GetHeaderRow(ExcelWorksheet worksheet, int rowCount, int colCount)
        {
            Dictionary<string, int> headerRow = new Dictionary<string, int>();

            // Recorre todas las filas y columnas del documento
            for (int row = 1; row <= rowCount; row++)
            {
                for (int col = 1; col <= colCount; col++)
                {
                    string cellValue = worksheet.Cells[row, col].Text.Trim().ToUpper(); 

                    // Compara con las cabeceras conocidas
                    if (cellValue == "CLAVE" && !headerRow.ContainsKey("CLAVE"))
                        headerRow["CLAVE"] = col;
                    else if (cellValue == "CONCEPTO" && !headerRow.ContainsKey("CONCEPTO"))
                        headerRow["CONCEPTO"] = col;
                    else if (cellValue == "UN" && !headerRow.ContainsKey("UN"))
                        headerRow["UN"] = col;
                    else if (cellValue == "CANT." && !headerRow.ContainsKey("CANT."))
                        headerRow["CANT."] = col;
                    else if (cellValue == "P.U." && !headerRow.ContainsKey("P.U."))
                        headerRow["P.U."] = col;
                    else if (cellValue == "IMPORTE" && !headerRow.ContainsKey("IMPORTE"))
                        headerRow["IMPORTE"] = col;
                }
            }

            // Verifica si todas las cabeceras fueron encontradas
            if (headerRow.Count == 6)
            {
                return headerRow;
            }
            else
            {
                string missingColumns = string.Join(", ", new[] 
                    { "CLAVE", "CONCEPTO", "UN", "CANT.", "P.U.", "IMPORTE" }.Except(headerRow.Keys));
                Console.WriteLine($"Faltan las siguientes columnas: {missingColumns}");
                return null;
            }
        }
    }
}
