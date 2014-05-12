// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Microsoft">
//   Copyright (c) 2013 Microsoft Corporation
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.Research.DataOnboarding.DomainModel;
using Microsoft.Research.DataOnboarding.RepositoryAdapters.Interfaces;
using Microsoft.Research.DataOnboarding.Utilities;
using Microsoft.Research.DataOnboarding.Utilities.Enums;
using Microsoft.Research.DataOnboarding.Utilities.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Microsoft.Research.DataOnboarding.RepositoryAdapters
{
    public class DataVerseAdapter : BaseAdapter, IRepositoryAdapter
    {
        private readonly DiagnosticsProvider diagnostics;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataVerseAdapter"/> class.
        /// </summary>
        public DataVerseAdapter()
        {
            diagnostics = new DiagnosticsProvider(this.GetType());
        }

        #region public methods

        /// <summary>
        /// Method to get the identifier 
        /// </summary>
        /// <param name="queryData">query data</param>
        /// <param name="repositoryModel">repository model</param>
        /// <returns>returns string</returns>
        public string GetIdentifier(MerritQueryData queryData, RepositoryModel repositoryModel)
        {
            throw new NotImplementedException();
        }

        public OperationStatus CreateSubRepository(string subRepositoryName)
        {
            throw new NotImplementedException();

            //Here we have to call data verse to create a study. A study can be seen as a repository in the context of dataup.
            //We need to figure out how to substitute the following Curl Command with an http request sent using the .Net Framework Libraries
            //curl --data-binary "@atom-entry-study.xml" -H "Content-Type: application/atom+xml" https://$USERNAME:$PASSWORD@$DVN_SERVER/dvn/api/data-deposit/v1/swordv2/collection/dataverse/$DATAVERSE_ALIAS
        }

        /// <summary>
        /// method to post the file to repository
        /// </summary>
        /// <param name="request">request object</param>
        /// <param name="repositoryModel">repository model</param>
        /// <param name="file">file</param>
        /// <returns>returns File Identifier.</returns>
        public OperationStatus PostFile(PublishFileModel publishFileModel)
        {
            PublishDataVerseFileModel publishDataVerseFileModel = (PublishDataVerseFileModel)publishFileModel;
            Check.IsNotNull(publishDataVerseFileModel.File, "File");
            Check.IsNotEmptyOrWhiteSpace(publishDataVerseFileModel.File.FileName, "File name");
            Check.IsNotNull(publishDataVerseFileModel.File.FileContent, "File content");
            if (publishDataVerseFileModel.File.FileContent.Length == 0)
            {
                throw new ArgumentException("File content is of zero length.");
            }

            this.UploadFile(publishDataVerseFileModel);
            OperationStatus status = OperationStatus.CreateSuccessStatus();
            return status;
        }

        /// <summary>
        /// Retrives the Access token by passing refresh token
        /// </summary>
        /// <param name="refreshToken">refresh Token</param>
        /// <returns></returns>
        public AuthToken RefreshToken(string refreshToken)
        {
            return null;
        }

        /// <summary>
        /// Method to download the file from the repository.
        /// </summary>
        /// <param name="downloadInput">Input data required to download the file from the repository.</param>
        /// <returns>Downloaded file data.</returns>
        public DataFile DownloadFile(string downloadUrl, string authorization, string fileName)
        {
            throw new NotImplementedException();
            //Dataverse does not offer an API that allows you to download a File. 
            //We have to work with the Dataverse team to figure this out
        }

        /// <summary>
        /// Verifies if the file exists in the repository
        /// </summary>
        /// <param name="fileIdentifier">File Identifier</param>
        /// <param name="authorization">AccessToken in case of DataVerse and username password for Merrit</param>
        /// <returns>OperationStatus returns Success if the file exists </returns>
        public OperationStatus CheckIfFileExists(string downloadURL, string fileIdentifier, string authorization)
        {
            throw new NotImplementedException();
            //Dataverse does not offer an API that allows you to check for the presence of a File. 
            //We have to work with the Dataverse team to figure this out
        }

        #endregion

        #region private methods

        /// <summary>
        /// uploads the file to DataVerse
        /// </summary>
        /// <param name="publishDataVerseFileModel">PublishDataVerseFileModel </param>
        /// <returns>File Id</returns>
        private void UploadFile(PublishDataVerseFileModel publishDataVerseFileModel)
        {
            string repositoryName = publishDataVerseFileModel.RepositoryModel.RepositoryName;
            DataFile dataFile = publishDataVerseFileModel.File;
            int fileId = publishDataVerseFileModel.File.FileInfo.FileId;
            string fileName = Path.GetFileNameWithoutExtension(dataFile.FileName);

            //We need to figure out how to substitute the following Curl Command with an http request sent using the .Net Framework Libraries
            //curl --data-binary @example.zip -H "Content-Disposition: filename=example.zip" -H "Content-Type: application/zip" -H "Packaging: http://purl.org/net/sword/package/SimpleZip" https://$USERNAME:$PASSWORD@$DVN_SERVER/dvn/api/data-deposit/v1/swordv2/edit-media/study/hdl:TEST/12345
        }

        #endregion
    }
}
