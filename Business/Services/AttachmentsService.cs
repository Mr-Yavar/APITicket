using AutoMapper;
using Contracts.IDataShaper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Models;
using Features.RequestFeatures;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AttachmentsService : ServiceBase, IAttachmentsService
    {
        
        ILoggerManager _logger;
        private readonly IRepositoryManager _repositoryManager;

        public AttachmentsService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger) : base(repositoryManager, mapper)
        {
            _logger = logger;
            _repositoryManager= repositoryManager;
        }

        public async Task<string> AddToStore(string fileName, string FileCategory, string FileType, Guid ObjectId)
        {
            Guid RowId = Guid.NewGuid();

            _repositoryManager.IAttachmentsRepository.Create(new()
            {
                Id = RowId,
                CreatedDate = DateTime.Now,
                FileCategory = FileCategory,
                FileName = fileName,
                FileType = FileType,
                ObjectId = ObjectId
            });
            _repositoryManager.IAttachmentsRepository.SaveChanges();
            return RowId.ToString();
        }

        public async Task<Attachments> GetFromStore(Guid AttachId)
        {
            return await _repositoryManager.IAttachmentsRepository.FindByCondition(x=>x.Id==AttachId,false).FirstOrDefaultAsync();
        }

        public async Task RemoveFromStore(Guid AttachId)
        {
            _repositoryManager.IAttachmentsRepository.Delete(_repositoryManager.IAttachmentsRepository.FindByCondition(x => x.Id == AttachId, false).FirstOrDefault());
        }
    }
}
