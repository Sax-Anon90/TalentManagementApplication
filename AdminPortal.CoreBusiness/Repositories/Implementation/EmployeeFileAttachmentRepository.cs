using AdminPortal.Common.Models.EmployeesViewModels;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using AdminPortal.Data.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Repositories.Implementation
{
    public class EmployeeFileAttachmentRepository : IEmployeeFileAttachmentRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        public EmployeeFileAttachmentRepository(SaxTalentManagementContext _dbContext, IMapper _mapper,
             AutoMapper.IConfigurationProvider _configurationProvider)
        {
            this._dbContext = _dbContext;
            this._mapper = _mapper;
            this._configurationProvider = _configurationProvider;
        }
        public async Task<EmployeeFileAttachmentsVM> GetEmployeeFileAttachment(int Id)
        {
            var employeeFileAttachment = await _dbContext.EmployeeFileAttachments
                .ProjectTo<EmployeeFileAttachmentsVM>(_configurationProvider)
                .FirstOrDefaultAsync(x => x.Id == Id);
            return employeeFileAttachment;
        }
        public async Task<ICollection<EmployeeFileAttachmentsVM>> GetAllEmployeeFileAttachmentsById(int employeeId)
        {
            var employeeFileAttachments = await _dbContext.EmployeeFileAttachments
                .Where(x => x.EmployeeId == employeeId)
                .ProjectTo<EmployeeFileAttachmentsVM>(_configurationProvider)
                .ToListAsync();
            return employeeFileAttachments;
        }
        public async Task<bool> UploadEmployeeFileAttachment(EmployeeAndEmployeeTrainingVM employeeFileAttachmentModel, int employeeId)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                await employeeFileAttachmentModel.EmployeeFileAttachment.employeeFileAttachment.CopyToAsync(stream);
                var fileContent = stream.ToArray();
                EmployeeFileAttachmentsVM fileAttachmentsVM = new()
                {
                    FileName = employeeFileAttachmentModel.EmployeeFileAttachment.employeeFileAttachment.FileName,
                    FileType = employeeFileAttachmentModel.EmployeeFileAttachment.employeeFileAttachment.ContentType,
                    FileContent = fileContent,
                    EmployeeId = employeeId
                };
                var employeeFileAttachment = _mapper.Map<EmployeeFileAttachment>(fileAttachmentsVM);
                await _dbContext.EmployeeFileAttachments.AddAsync(employeeFileAttachment);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task DeleteEmployeeFileAttachment(int employeeFileAttachmentId)
        {
            var employeeFileAttachment = await _dbContext.EmployeeFileAttachments.FirstOrDefaultAsync(x => x.Id == employeeFileAttachmentId);
            _dbContext.EmployeeFileAttachments.Remove(employeeFileAttachment);
        }

        public async Task<int> GetTotalNumberOfEmployeeFiles(int employeeId)
        {
            var totalNumberOfEmployeeFiles = await _dbContext.EmployeeFileAttachments
                .Where(x => x.EmployeeId == employeeId).CountAsync();
            return totalNumberOfEmployeeFiles;
        }

        public async Task<int> GetTotalOfAllEmployeeFiles()
        {
            var totalFiles = await _dbContext.EmployeeFileAttachments.CountAsync();
            return totalFiles;
        }


    }
}
