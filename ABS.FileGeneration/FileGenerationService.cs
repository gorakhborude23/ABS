using ClosedXML.Excel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ABS.FileGeneration
{
    public class FileGenerationService
    {
        public async Task<FileStream> GenerateFile()
        {
            return await Task.Run(() => CreateFile());
        }

        private FileStream CreateFile()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), $"employees_{Guid.NewGuid()}.xlsx");
            try
            {
                // Create a new workbook and worksheet
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Employees");
                    worksheet.Cell(1, 1).Value = "EmployeeID";
                    worksheet.Cell(1, 2).Value = "Name";
                    worksheet.Cell(1, 3).Value = "Department";

                    // Sample data
                    worksheet.Cell(2, 1).Value = "1";
                    worksheet.Cell(2, 2).Value = "Lorem";
                    worksheet.Cell(2, 3).Value = "Sales";

                    worksheet.Cell(3, 1).Value = "2";
                    worksheet.Cell(3, 2).Value = "Ipsum";
                    worksheet.Cell(3, 3).Value = "Marketing";

                    worksheet.Cell(4, 1).Value = "3";
                    worksheet.Cell(4, 2).Value = "Dummy";
                    worksheet.Cell(4, 3).Value = "Finance";

                    // Save the workbook to the file
                    workbook.SaveAs(tempFilePath);
                }

                // Return a stream to the generated file
                return new FileStream(tempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.DeleteOnClose);
            }

            catch (UnauthorizedAccessException ex)
            {
                throw new InvalidOperationException("Access to the file is denied. Please check the file permissions.", ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new InvalidOperationException("The specified directory was not found.", ex);
            }
            catch (PathTooLongException ex)
            {
                throw new InvalidOperationException("The specified path is too long.", ex);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException("A general file I/O error occurred. Please check disk space and ensure the file is not locked.", ex);
            }
            catch (NotSupportedException ex)
            {
                throw new InvalidOperationException("The file path provided is invalid.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }
        }
    }
}


