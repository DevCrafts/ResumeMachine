using ResumeMachine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace ResumeMachine.Helper
{
  public static class WordWriter
  {
    public static async Task<string> WriteToWordTemplate(ResumeData cvData, string destinationPath, string templateLocationPath)
    {
      string fullPDFDestinationPath = Path.Combine(destinationPath, $"CV {cvData.FirstName} {cvData.LastName}.pdf");
      string fullWordDestinationPath = Path.Combine(destinationPath, $"CV {cvData.FirstName} {cvData.LastName}.docx");

      if (FileAccessProvider.IsLocked(fullWordDestinationPath))
      {
        return "Some of files in output directory is in use, please close and try again!";
      }

      if (!File.Exists(fullWordDestinationPath))
      {
        return "No template file has been selected!";
      }

      await System.Threading.Tasks.Task.Run(() =>
      {
        Word._Application wApp = new Word.Application();
        Word.Documents wDocs = wApp.Documents;
        Word._Document wDoc = wDocs.Open(templateLocationPath, ReadOnly: false);

        wDoc.Activate();

        Word.Bookmarks wBookmarks = wDoc.Bookmarks;
        Word.Bookmark dateTodayBookmark = wBookmarks["DateToday"];
        Word.Range wDateToday = dateTodayBookmark.Range;
        Word.Bookmark firstNameBookmark = wBookmarks["FirstName"];
        Word.Range wFirstName = firstNameBookmark.Range;
        Word.Bookmark lastNameBookmark = wBookmarks["LastName"];
        Word.Range wLastName = lastNameBookmark.Range;
        Word.Bookmark dateOfBirthBookmark = wBookmarks["DateOfBirth"];
        Word.Range wDateOfBirth = dateOfBirthBookmark.Range;
        Word.Bookmark nationalityBookmark = wBookmarks["Nationality"];
        Word.Range wNationality = nationalityBookmark.Range;
        Word.Bookmark presentPositionBookmark = wBookmarks["PresentPosition"];
        Word.Range wPresentPosition = presentPositionBookmark.Range;
        Word.Bookmark currentCompanyBookmark = wBookmarks["CurrentCompany"];
        Word.Range wCurrentCompany = currentCompanyBookmark.Range;
        Word.Bookmark yearsWithCompanyBookmark = wBookmarks["YearsWithCompany"];
        Word.Range wYearsWithCompany = yearsWithCompanyBookmark.Range;

        wDateToday.Text = DateTime.Today.ToString("dd.MM.yyyy");
        wFirstName.Text = cvData.FirstName;
        wLastName.Text = cvData.LastName;
        wDateOfBirth.Text = cvData.DateOfBirth.ToString("dd.MM.yyyy");
        wNationality.Text = cvData.Nationality;
        wPresentPosition.Text = cvData.PresentPosition;
        wCurrentCompany.Text = cvData.CurrentCompany;
        wYearsWithCompany.Text = $"Since {cvData.SinceWithCompany.ToString("dd.MM.yyyy")}";

        object oMissing = System.Reflection.Missing.Value;
        object oEndOfDoc = "\\endofdoc"; // "endofdoc" is a predefined bookmark
        object oRng = wDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

        // Let's write Educations
        int numOfEducationRows = cvData.Educations.Count();

        Word.Table oTable1;
        Word.Range wrdRng1 = wDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
        oTable1 = wDoc.Tables.Add(wrdRng1, numOfEducationRows, 2, ref oMissing, ref oMissing);
        oTable1.Range.ParagraphFormat.SpaceAfter = 4;
        int r1, c1;

        for (r1 = 1; r1 <= numOfEducationRows; r1++)
        {
          oTable1.Cell(r1, 2).Range.Text = cvData.Educations[r1 - 1].Description;
        }

        oTable1.Columns[1].Width = wApp.CentimetersToPoints(5);
        oTable1.Columns[2].Width = wApp.CentimetersToPoints(18);

        // Insert "Work history" paragraph
        Word.Paragraph oPara2;
        oRng = wDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
        oPara2 = wDoc.Content.Paragraphs.Add(ref oRng);
        oPara2.Range.Text = "Work history";
        oPara2.Range.Font.Bold = 1;
        oPara2.Format.SpaceAfter = 6;
        oPara2.Range.InsertParagraphAfter();

        oPara2.Range.Font.Bold = 0;

        foreach (var item in cvData.JobRecords)
        {
          Word.Table oTable2;
          Word.Range wrdRng2 = wDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
          oTable2 = wDoc.Tables.Add(wrdRng2, 3, 2, ref oMissing, ref oMissing);
          oTable2.Range.ParagraphFormat.SpaceAfter = 4;
          int r2;

          for (r2 = 1; r2 <= 3; r2++)
          {
            oTable2.Cell(1, 1).Range.Text = item.IsPresent ? String.Format("{0}.{1} - Present", item.FromDate.Month, item.FromDate.Year)
            : String.Format("{0}.{1} - {2}.{3}", item.FromDate.Month, item.FromDate.Year, item.ToDate.Month, item.ToDate.Year);
            oTable2.Cell(1, 2).Range.Text = item.CompanyName;
            oTable2.Cell(2, 2).Range.Text = item.PositionTitle;
            oTable2.Cell(3, 2).Range.Text = item.JobDescription;
          }

          oTable2.Columns[1].Width = wApp.CentimetersToPoints(5);
          oTable2.Columns[2].Width = wApp.CentimetersToPoints(18);

          oTable2.Rows[1].Range.Font.Bold = 1;
          //oTable.Rows[1].Range.Font.Italic = 1;

          //Add some text after the table.
          Word.Paragraph oPara4;
          oRng = wDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
          oPara4 = wDoc.Content.Paragraphs.Add(ref oRng);
        }

        //Insert another paragraph.
        Word.Paragraph oPara3;
        oRng = wDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
        oPara3 = wDoc.Content.Paragraphs.Add(ref oRng);
        oPara3.Range.Text = "Languages";
        oPara3.Range.Font.Bold = 1;
        oPara3.Format.SpaceAfter = 6;
        oPara3.Range.InsertParagraphAfter();

        oPara2.Range.Font.Bold = 0;

        //Insert another paragraph.
        var numOfLanguageRows = cvData.Languages.Count();

        Word.Table oTable;
        Word.Range wrdRng = wDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
        oTable = wDoc.Tables.Add(wrdRng, numOfLanguageRows, 3, ref oMissing, ref oMissing);
        oTable.Range.ParagraphFormat.SpaceAfter = 4;
        int r, c;

        for (r = 1; r <= numOfLanguageRows; r++)
        {
          oTable.Cell(r, 2).Range.Text = cvData.Languages[r - 1].Name;
          oTable.Cell(r, 3).Range.Text = cvData.Languages[r - 1].Level;
        }

        oTable.Columns[1].Width = wApp.CentimetersToPoints(5);
        oTable.Columns[2].Width = wApp.CentimetersToPoints(4);
        oTable.Columns[3].Width = wApp.CentimetersToPoints(5);

        wDoc.SaveAs2(fullPDFDestinationPath, Word.WdSaveFormat.wdFormatPDF);
        wDoc.SaveAs2(fullWordDestinationPath);

        wDoc.Close(false); // Close the Word Document.
        wApp.Quit(false, false, false);

        wDoc = null;
        wApp = null;
      });

      return "PDF and Word files has been successfully created";
    }
  }
}

