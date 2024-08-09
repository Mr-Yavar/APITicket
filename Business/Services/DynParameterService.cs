using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.DynParameters;
using Entities.Enum;
using Entities.Exceptions;
using Entities.Models;
using Entities.Utils;
using Features.CustomRequest;
using Features.RequestFeatures;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class DynParameterService : ServiceBase, IDynParameterService
    {
        private IAttachmentsService _attachments;
        private DBContextProvider _context;
        public DynParameterService(
                                IRepositoryManager repositoryManager,
                              IMapper mapper,
                              DBContextProvider context,
                              IAttachmentsService attachments) : base(repositoryManager, mapper)
        {
            _context = context;
            _attachments = attachments;
        }
        public async Task<List<DynParametersDto>> GetParents()
        {
            var result = await _repositoryManager.IDynParametersRepository.FindByCondition(x => x.ParentId == null, false).ToListAsync();
            return _mapper.Map<List<DynParametersDto>>(result);
        }
        public async Task<DynParametersDto> GetChildrenByParentId(Guid Id)
        {
            var result = await _repositoryManager.IDynParametersRepository.FindByCondition(x => x.Id == Id, false).Include(x => x.Children).FirstOrDefaultAsync();
            var res = result != null ? _mapper.Map<DynParametersDto>(result) : new DynParametersDto() ;
            res.Values = result != null ?  _mapper.Map<List<DynParamterValueDto>>(result.Children ) : new();
            return res;
        }
        public async Task<DynParametersDto> GetChildrenByParentKey(string Key)
        {
            var result = await _repositoryManager.IDynParametersRepository.FindByCondition(x => x.Value == Key && x.ParentId == null, false).Include(x => x.Children).FirstOrDefaultAsync();
            var res = _mapper.Map<DynParametersDto>(result);
            res.Values = _mapper.Map<List<DynParamterValueDto>>(result.Children);
            return res;
        }

        public async Task<DynParametersDto> ManipulateChildren(DynParametersDto dto, FilesDto files)
        {
            var todelte = await _repositoryManager.IDynParametersRepository.FindByCondition(x => dto.Garbage.Contains(x.Id), false).ToListAsync();
            todelte.ForEach(item =>
            {
                _repositoryManager.IDynParametersRepository.Delete(item);
            });
            _repositoryManager.IDynParametersRepository.SaveChanges();

            var OldDynParameters = _repositoryManager.IDynParametersRepository.FindByCondition(x => x.ParentId == dto.ParentId, true).ToList();

            foreach (var item in dto.Values)
            {

                if (item.Id != null)
                {
                    var oldDyn = OldDynParameters.FirstOrDefault(x => x.Id == item.Id);
                    if (oldDyn == null)
                        continue;

                    oldDyn.ValueType = ValueTypeEnum.STRING;
                    oldDyn.Value = item.Value;
                    ///UPDATE
                    if (item.ValueType == ValueTypeEnum.FILE)
                    {


                        var file = files.Files.FirstOrDefault(x => x.Key == item.Value);
                        string tempName = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.DynParam]);
                        oldDyn.Value = await _attachments.AddToStore(tempName, FileType.FileTypes[FileTypeEnum.DynParam], file.File.ContentType, item.Id.Value);
                        oldDyn.ValueType = ValueTypeEnum.FILE;

                    }




                    _repositoryManager.IDynParametersRepository.Update(oldDyn);

                }
                else
                {
                    DynParameters model = new DynParameters()
                    {
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        ParentId = dto.ParentId,
                        Value = item.Value,
                        ValueType = ValueTypeEnum.STRING,

                    };
                    ///Create
                    if (item.ValueType == ValueTypeEnum.FILE)
                    {
                        //Saving File
                        var file = files.Files.FirstOrDefault(x => x.Key == item.Value);
                        if (file == null)
                            continue;
                        string tempName = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.DynParam]);
                        model.Value = await _attachments.AddToStore(tempName, FileType.FileTypes[FileTypeEnum.DynParam], file.File.ContentType, model.Id);
                        model.ValueType = ValueTypeEnum.FILE;
                    }

                    _repositoryManager.IDynParametersRepository.Create(model);
                }

              
            }
            _repositoryManager.IDynParametersRepository.SaveChanges();
            

            dto.Values = _mapper.Map<List<DynParamterValueDto>>(OldDynParameters);
            return dto;
        }
        public async Task DeleteChild(Guid Id)
        {

        }


    }
}