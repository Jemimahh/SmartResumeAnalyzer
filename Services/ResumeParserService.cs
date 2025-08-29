using System;
using System.IO;
using System.Text;
using UglyToad.PdfPig;
using DocumentFormat.OpenXml.Packaging;

public class ResumeParserService
{
    public string ExtractText(Stream fileStream, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();

        return extension switch
        {
            ".pdf" => ExtractTextFromPdf(fileStream),
            ".docx" => ExtractTextFromDocx(fileStream),
            _ => throw new NotSupportedException("Only PDF and DOCX files are supported.")
        };
    }

    private string ExtractTextFromPdf(Stream fileStream)
    {
        using var memoryStream = new MemoryStream();
        fileStream.CopyTo(memoryStream);
        memoryStream.Position = 0;

        var sb = new StringBuilder();
        using var pdf = PdfDocument.Open(memoryStream);
        foreach (var page in pdf.GetPages())
        {
            sb.AppendLine(page.Text);
        }
        return sb.ToString();
    }

    private string ExtractTextFromDocx(Stream fileStream)
    {
        using var memoryStream = new MemoryStream();
        fileStream.CopyTo(memoryStream);
        memoryStream.Position = 0;

        using var wordDoc = WordprocessingDocument.Open(memoryStream, false);
        return wordDoc.MainDocumentPart?.Document.Body?.InnerText ?? string.Empty;
    }
}
