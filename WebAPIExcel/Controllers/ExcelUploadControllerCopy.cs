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
    public class ExcelUploadControllerCopy : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExcelUploadControllerCopy(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/ExcelUpload
        [HttpPost]
        public async Task<ActionResult> PostExcelItem(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var items = new List<ExcelFileItem>();

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

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var clave = worksheet.Cells[row, headerRow["CLAVE"]].Text.Trim();
                        var concepto = worksheet.Cells[row, headerRow["CONCEPTO"]].Text.Trim();

                        if (string.IsNullOrEmpty(clave) || string.IsNullOrEmpty(concepto) || clave == "CLAVE" ||
                            concepto == "CONCEPTO")
                        {
                            continue;
                        }

                        var excelFileItem = new ExcelFileItem
                        {
                            Clave = clave,
                            Concepto = concepto,
                            Un = worksheet.Cells[row, headerRow["UN"]].Text.Trim(),
                            Cant = decimal.TryParse(worksheet.Cells[row, headerRow["CANT."]].Text, out var cant)
                                ? cant
                                : 0,
                            PU = decimal.TryParse(worksheet.Cells[row, headerRow["P.U."]].Text, out var pu) ? pu : 0,
                            Importe = decimal.TryParse(worksheet.Cells[row, headerRow["IMPORTE"]].Text, out var importe)
                                ? importe
                                : 0
                        };

                        items.Add(excelFileItem);
                    }
                }
            }

            // Now use the CreateHierarchy method to generate the hierarchical structure
            var hierarchy = CreateHierarchy(items);

            return Ok(new List<Dictionary<string, object>> { hierarchy });
        }

        private Dictionary<string, object> CreateHierarchy(List<ExcelFileItem> items)
        {
            var root = new Dictionary<string, object>
            {
                { "clave", "ROOT" },
                { "concepto", "Conceptos Raíz" },
                { "hijos", new List<Dictionary<string, object>>() }
            };

            // Agrupar los elementos en un diccionario por su clave padre
            foreach (var item in items)
            {
                // Separar la clave en partes para determinar la jerarquía
                var keyParts = item.Clave.Split(new[] { '/', '-', '.', '_' }, StringSplitOptions.RemoveEmptyEntries);

                // Comenzar en el nodo raíz
                var currentParent = root;

                // Recorrer las partes de la clave, creando nodos para cada nivel
                foreach (var part in keyParts)
                {
                    var children = currentParent["hijos"] as List<Dictionary<string, object>>;
                    var existingNode = children.FirstOrDefault(c => c["clave"].ToString() == part);

                    if (existingNode == null)
                    {
                        // Si no existe, crear el nodo
                        existingNode = new Dictionary<string, object>
                        {
                            { "clave", part },
                            { "concepto", GetConceptoForKey(part) }, // Obtener el concepto adecuado para el clave
                            { "hijos", new List<Dictionary<string, object>>() }
                        };

                        // Agregar el nodo al nivel superior
                        children.Add(existingNode);
                    }

                    // Mover el puntero al siguiente nivel
                    currentParent = existingNode;
                }

                // Finalmente, agregar el item al nodo correcto (hoja de la jerarquía)
                var itemToAdd = new Dictionary<string, object>
                {
                    { "clave", item.Clave },
                    { "concepto", item.Concepto },
                    { "un", item.Un },
                    { "cant", item.Cant },
                    { "pu", item.PU },
                    { "importe", item.Importe }
                };

                // Añadir el item al nodo correspondiente
                (currentParent["hijos"] as List<Dictionary<string, object>>).Add(itemToAdd);
            }

            return root;
        }
        
        private string GetConceptoForKey(string clave)
        {
            // Lógica para determinar un "concepto" adecuado basado en la clave.
            return clave;
        }

        private string GetParentKey(string clave)
        {
            // Si la clave tiene una barra (/), usamos el primer segmento como clave de agrupación
            if (clave.Contains("/"))
            {
                var parts = clave.Split('/');
                return parts[0]; // Utiliza el primer segmento como "clave padre"
            }

            // Si la clave tiene más de un segmento (por ejemplo, "PRE/001"), usa la primera parte como clave padre
            var segments = clave.Split(new[] { '-', '.', '_' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length > 1)
            {
                return segments[0]; // El primer segmento podría ser el "padre"
            }

            // Si no se encuentra un patrón específico, usamos un fallback
            return clave.Length > 2 ? clave.Substring(0, 3) : "ROOT";
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
                string missingColumns = string.Join(", ",
                    new[] { "CLAVE", "CONCEPTO", "UN", "CANT.", "P.U.", "IMPORTE" }.Except(headerRow.Keys));
                Console.WriteLine($"Faltan las siguientes columnas: {missingColumns}");
                return null;
            }
        }
    }
}