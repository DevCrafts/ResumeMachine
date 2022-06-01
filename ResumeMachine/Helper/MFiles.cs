using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeMachine.Helper
{
  public class MFiles
  {
    private void CreateObject()
    {
      //// Which vault are we connecting to?
      //var vaultGuid = "{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}";

      //// Connect to the vault using a client connection.
      //// ref: http://developer.m-files.com/APIs/COM-API/#api-modes-client-vs-server
      //// Note: this code will just return the first connection to the vault.
      //var clientApplication = new MFilesClientApplication();
      //var vault =
      //	clientApplication
      //			.GetVaultConnectionsWithGUID(vaultGuid)
      //			.Cast<VaultConnection>()
      //			.FirstOrDefault()?
      //			.BindToVault(IntPtr.Zero, true, true);
      //if (null == vault)
      //{
      //	throw new NotImplementedException("Vault connection not found");
      //}

      //// Define the property values for the new object.
      //var propertyValues = new MFilesAPI.PropertyValues();

      //// Class.
      //var classPropertyValue = new MFilesAPI.PropertyValue()
      //{
      //	PropertyDef = (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefClass
      //};
      //classPropertyValue.Value.SetValue(
      //	MFDataType.MFDatatypeLookup,  // This must be correct for the property definition.
      //	(int)MFBuiltInDocumentClass.MFBuiltInDocumentClassOtherDocument
      //	);
      //propertyValues.Add(-1, classPropertyValue);

      //// Name or title.
      //var nameOrTitlePropertyValue = new MFilesAPI.PropertyValue()
      //{
      //	PropertyDef = (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle
      //};
      //nameOrTitlePropertyValue.Value.SetValue(
      //	MFDataType.MFDatatypeText,  // This must be correct for the property definition.
      //	"My document"
      //);
      //propertyValues.Add(-1, nameOrTitlePropertyValue);

      //// Define the source files to add.
      //var sourceFiles = new MFilesAPI.SourceObjectFiles();

      //// Add one file.
      //var myFile = new MFilesAPI.SourceObjectFile();
      //myFile.SourceFilePath = @"C:\temp\test.txt";
      //myFile.Title = "My test document"; // For single-file-documents this is ignored.
      //myFile.Extension = "txt";
      //sourceFiles.Add(-1, myFile);

      //// What object type is being created?
      //var objectTypeID = (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument;

      //// A "single file document" must be both a document and contain exactly one file.
      //var isSingleFileDocument =
      //	objectTypeID == (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument && sourceFiles.Count == 1;

      //// Create the object and check it in.
      //var objectVersion = vault.ObjectOperations.CreateNewObjectEx(
      //	objectTypeID,
      //	propertyValues,
      //	sourceFiles,
      //	SFD: isSingleFileDocument,
      //	CheckIn: true);
    }

    private void LoadListOfEmployees()
    {
      //// Which vault are we connecting to?
      //var vaultGuid = "{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}";

      //// Connect to the vault using a client connection.
      //// ref: http://developer.m-files.com/APIs/COM-API/#api-modes-client-vs-server
      //// Note: this code will just return the first connection to the vault.
      //var clientApplication = new MFilesClientApplication();
      //var vault =
      //	clientApplication
      //			.GetVaultConnectionsWithGUID(vaultGuid)
      //			.Cast<VaultConnection>()
      //			.FirstOrDefault()?
      //			.BindToVault(IntPtr.Zero, true, true);
      //if (null == vault)
      //{
      //	throw new NotImplementedException("Vault connection not found");
      //}

      //// Define the property values for the new object.
      //var propertyValues = new MFilesAPI.PropertyValues();

      //// Class.
      //var classPropertyValue = new MFilesAPI.PropertyValue()
      //{
      //	PropertyDef = (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefClass
      //};
      //classPropertyValue.Value.SetValue(
      //	MFDataType.MFDatatypeLookup,  // This must be correct for the property definition.
      //	(int)MFBuiltInDocumentClass.MFBuiltInDocumentClassOtherDocument
      //	);
      //propertyValues.Add(-1, classPropertyValue);

      //// Name or title.
      //var nameOrTitlePropertyValue = new MFilesAPI.PropertyValue()
      //{
      //	PropertyDef = (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle
      //};
      //nameOrTitlePropertyValue.Value.SetValue(
      //	MFDataType.MFDatatypeText,  // This must be correct for the property definition.
      //	"My document"
      //);
      //propertyValues.Add(-1, nameOrTitlePropertyValue);

      //// Define the source files to add.
      //var sourceFiles = new MFilesAPI.SourceObjectFiles();

      //// Add one file.
      //var myFile = new MFilesAPI.SourceObjectFile();
      //myFile.SourceFilePath = @"C:\temp\test.txt";
      //myFile.Title = "My test document"; // For single-file-documents this is ignored.
      //myFile.Extension = "txt";
      //sourceFiles.Add(-1, myFile);

      //// What object type is being created?
      //var objectTypeID = (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument;

      //// A "single file document" must be both a document and contain exactly one file.
      //var isSingleFileDocument =
      //	objectTypeID == (int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument && sourceFiles.Count == 1;

      //// Create the object and check it in.
      //var objectVersion = vault.ObjectOperations.CreateNewObjectEx(
      //	objectTypeID,
      //	propertyValues,
      //	sourceFiles,
      //	SFD: isSingleFileDocument,
      //	CheckIn: true);
    }
  }

}
