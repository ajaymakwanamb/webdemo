using System.IO;
using System.Text;
using iText.Html2pdf;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
namespace webdemo.Models {
    public class class_export {

        #region GenerateHTML
        public static string CreateTable(JArray jsonData, string tableId) {
            StringBuilder tableHtml = new StringBuilder();
            tableHtml.Append($"<table class='table table-striped table-bordered' id='{tableId}'>");

            StringBuilder thead = new StringBuilder();
            StringBuilder tbody = new StringBuilder();
            StringBuilder tfoot = new StringBuilder();

            foreach (JObject row in jsonData) {
                StringBuilder tr = new StringBuilder();
                string rowClass = row["row_class"]?.ToString() ?? string.Empty;

                foreach (var property in row.Properties()) {
                    string key = property.Name;

                    if (key.StartsWith("col") && int.TryParse(key.Substring(3), out _)) {
                        int columnIndex = int.Parse(key.Substring(3));
                        string colType = row[$"col{columnIndex}_type"]?.ToString() ?? "text";
                        string colClass = row[$"col{columnIndex}_class"]?.ToString() ?? string.Empty;

                        string content = row[key]?.ToString() ?? string.Empty;
                        string cellHtml = string.Empty;

                        if (row["row_type"] != null) {
                            if (row["row_type"].ToString() == "head") {
                                if (colType == "html") {
                                    continue;
                                } else {
                                    tr.Append($"<th class='{colClass}'>{content}</th>");
                                }
                            } else if (row["row_type"].ToString() == "body") {
                                if (colType == "html") {
                                    continue;
                                } else {
                                    cellHtml = $"<td class='{colClass}'>{content}</td>";
                                }
                                tr.Append(cellHtml);
                            } else if (row["row_type"].ToString() == "foot") {
                                tr.Append($"<td class='{colClass}'>{content}</td>");
                            }
                        }
                    }
                }

                if (row["row_type"] != null) {
                    if (row["row_type"].ToString() == "head") {
                        thead.Append($"<tr class='{rowClass}'>{tr}</tr>");
                    } else if (row["row_type"].ToString() == "body") {
                        tbody.Append($"<tr class='{rowClass}'>{tr}</tr>");
                    } else if (row["row_type"].ToString() == "foot") {
                        tfoot.Append($"<tr class='{rowClass}'>{tr}</tr>");
                    }
                }
            }

            if (thead.Length > 0)
                tableHtml.Append($"<thead>{thead}</thead>");
            if (tbody.Length > 0)
                tableHtml.Append($"<tbody>{tbody}</tbody>");
            if (tfoot.Length > 0)
                tableHtml.Append($"<tfoot>{tfoot}</tfoot>");

            tableHtml.Append("</table>");
            return tableHtml.ToString();
        }
        #endregion

        #region PDF
        public static byte[] generatepdf(string htmlContent) {
            htmlContent = "<!DOCTYPE html><html><head><meta charset=\"utf-8\" /><title></title></head><body>" + htmlContent + "</body></html>";
            try {
                using (var memoryStream = new MemoryStream()) {
                    HtmlConverter.ConvertToPdf(htmlContent, memoryStream);
                    return memoryStream.ToArray();
                }
            } catch (Exception ex) {
                string msg = ex.Message;
            }
            return null;
        }
        #endregion

        #region Excel
        public static byte[] generateexcel(JArray data) {
            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage()) {
                    // Add a worksheet
                    var worksheet = package.Workbook.Worksheets.Add("Employee Data");

                    int rowIndex = 1;
                    int colIndex = 1;

                    foreach (JObject row in data) {
                        StringBuilder tr = new StringBuilder();
                        string rowClass = row["row_class"]?.ToString() ?? string.Empty;

                        foreach (var property in row.Properties()) {
                            string key = property.Name;

                            if (key.StartsWith("col") && int.TryParse(key.Substring(3), out _)) {
                                int columnIndex = int.Parse(key.Substring(3));
                                string colType = row[$"col{columnIndex}_type"]?.ToString() ?? "text";
                                string colClass = row[$"col{columnIndex}_class"]?.ToString() ?? string.Empty;
                                string content = row[key]?.ToString() ?? string.Empty;

                                if (row["row_type"] != null) {
                                    if (row["row_type"].ToString() == "head") {
                                        if (colType == "html") {
                                            continue;
                                        } else {
                                            worksheet.Cells[rowIndex, colIndex].Value = content;
                                            worksheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
                                            worksheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                            worksheet.Cells[rowIndex, colIndex].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                            worksheet.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            worksheet.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray); 
                                        }
                                    } else if (row["row_type"].ToString() == "body") {
                                        if (colType == "html") {
                                            continue;
                                        } else {
                                            worksheet.Cells[rowIndex, colIndex].Value = content;
                                        }


                                    } else if (row["row_type"].ToString() == "foot") {
                                        worksheet.Cells[rowIndex, colIndex].Value = content;
                                        worksheet.Cells[rowIndex, colIndex].Style.Font.Italic = true;
                                        worksheet.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow); 
                                    }
                                }

                                colIndex++;
                            }
                        }

                        if (row["row_type"] != null) {
                            if (row["row_type"].ToString() == "head" || row["row_type"].ToString() == "foot") {
                                rowIndex++;
                            } else if (row["row_type"].ToString() == "body") {
                                rowIndex++;
                            }
                        }

                        colIndex = 1;
                    }

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    return package.GetAsByteArray();
                }
            } catch (Exception ex) {
                string msg = ex.Message;
            }
            return null;
        }
        #endregion

    }
}

