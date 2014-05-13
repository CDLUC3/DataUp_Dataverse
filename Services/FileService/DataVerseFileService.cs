// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Microsoft">
//   Copyright (c) 2013 Microsoft Corporation
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Microsoft.Research.DataOnboarding.DataAccessService;
using Microsoft.Research.DataOnboarding.DomainModel;
using Microsoft.Research.DataOnboarding.DomainModel.ConceptualModel;
using Microsoft.Research.DataOnboarding.FileService.Enums;
using Microsoft.Research.DataOnboarding.FileService.Interface;
using Microsoft.Research.DataOnboarding.RepositoriesService;
using Microsoft.Research.DataOnboarding.RepositoriesService.Interface;
using Microsoft.Research.DataOnboarding.RepositoryAdapters;
using Microsoft.Research.DataOnboarding.RepositoryAdapters.Interfaces;
using Microsoft.Research.DataOnboarding.Services.UserService;
using Microsoft.Research.DataOnboarding.Utilities;
using Microsoft.Research.DataOnboarding.Utilities.Model;
using Newtonsoft.Json;
using DM = Microsoft.Research.DataOnboarding.DomainModel;

namespace Microsoft.Research.DataOnboarding.FileService
{
    public class DataVerseFileService : FileServiceProvider
    {
        /// <summary>
        /// Holds the reference to userService.
        /// </summary>
        private IUserService userService;

        public DataVerseFileService(IFileRepository fileDataRepository, IBlobDataRepository blobDataRepository, IUnitOfWork unitOfWork, IRepositoryDetails repositoryDetails, IRepositoryService repositoryService, IRepositoryAdapterFactory repositoryAdapterFactory, IUserService userService) :
            base(fileDataRepository, blobDataRepository, unitOfWork, repositoryDetails, repositoryService, repositoryAdapterFactory)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Performs the necessary validations required for the file to be published in skydrive
        /// </summary>
        /// <param name="message">Publish Message</param>
        public override void ValidateForPublish(PublishMessage message)
        {
            base.ValidateForPublish(message);

            Repository repository = this.RepositoryService.GetRepositoryById(message.RepositoryId);
        }

        

        /// <summary>
        /// Method to publish file
        /// </summary>
        /// <param name="postFileData">PublishMessage object</param>
        /// <returns>returns File Identifier</returns>
        public override string PublishFile(PublishMessage postFileData)
        {
            var file = this.GetFileByFileId(postFileData.FileId);
            IEnumerable<DM.FileColumnType> fileColumnTypes = null;
            IEnumerable<DM.FileColumnUnit> fileColumnUnits = null;
            OperationStatus status = null;

            Encoding encoding = Encoding.UTF8;
            string identifier = string.Empty;


            var repository = this.RepositoryService.GetRepositoryById(postFileData.RepositoryId);

            string authorization = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(postFileData.UserName /*UserName*/ + ":" + postFileData.Password /*Password*/));

            string baseRepoName = this.RepositoryDetails.GetBaseRepositoryName(repository.BaseRepositoryId);
            this.RepositoryAdapter = new DataVerseAdapter();

            var dataFile = this.GetDataFile(file);

            var repositoryModel = GetRepositoryModel(repository, authorization);

            PublishDataVerseFileModel publishMerritFileModel = new PublishDataVerseFileModel()
            {
                File = dataFile,
                RepositoryModel = repositoryModel,
                FileColumnTypes = fileColumnTypes,
                FileColumnUnits = fileColumnUnits,
            };

            status = this.RepositoryAdapter.PostFile(publishMerritFileModel);

            return identifier;
        }

        
    }
}
