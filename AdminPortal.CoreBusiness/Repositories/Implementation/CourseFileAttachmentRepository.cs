using AdminPortal.Data.Data;
using AdminPortal.Common.Models.CoursesViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using AdminPortal.CoreBusiness.Repositories.Contracts;

namespace AdminPortal.CoreBusiness.Repositories.Implementation
{
    public class CourseFileAttachmentRepository : ICourseFileAttachmentRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        public CourseFileAttachmentRepository(SaxTalentManagementContext _dbContext, IMapper _mapper,
            AutoMapper.IConfigurationProvider _configurationProvider)
        {
            this._dbContext = _dbContext;
            this._mapper = _mapper;
            this._configurationProvider = _configurationProvider;
        }

        public async Task<ICollection<CourseFileAttachmentViewVM>> GetAllCourseFileAttachmentsById(int? CourseId)
        {
            var courseFileAttachments = await _dbContext.CourseFileAttachments
                .Where(x => x.CourseId == CourseId)
                .ProjectTo<CourseFileAttachmentViewVM>(_configurationProvider)
                .ToListAsync();
            return courseFileAttachments;
        }

        public async Task<CourseFileAttachmentViewVM> GetCourseFileAttachment(int Id)
        {
            var courseFileAttachment = await _dbContext.CourseFileAttachments
                 .ProjectTo<CourseFileAttachmentViewVM>(_configurationProvider)
                 .FirstOrDefaultAsync(x => x.Id == Id);
            return courseFileAttachment;
        }

        public async Task<bool> UploadCourseFileAttachment(CoursesAndCourseCategoriesVM courseFileAttachment, int courseId)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                await courseFileAttachment.CourseFileAttachmentVM.courseFileAttachment.CopyToAsync(stream);
                var fileContent = stream.ToArray();
                CourseFileAttachmentViewVM fileAttachmentVM = new()
                {
                    FileName = courseFileAttachment.CourseFileAttachmentVM.courseFileAttachment.FileName,
                    FileType = courseFileAttachment.CourseFileAttachmentVM.courseFileAttachment.ContentType,
                    FileContent = fileContent,
                    CourseId = courseId
                };
                var fileAttachment = _mapper.Map<CourseFileAttachment>(fileAttachmentVM);
                await _dbContext.CourseFileAttachments.AddAsync(fileAttachment);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task DeleteCourseFileAttachment(int courseFileAttachmentId)
        {
            var courseFileAttachment = await _dbContext.CourseFileAttachments.FirstOrDefaultAsync(x => x.Id == courseFileAttachmentId);
            _dbContext.CourseFileAttachments.Remove(courseFileAttachment);
        }

        public async Task<int> GetTotalNumberOfCourseFiles()
        {
           var totalNumberOfCourseFiles = await _dbContext.CourseFileAttachments.CountAsync();
            return totalNumberOfCourseFiles;
        }

        public async Task<int> GetTotalNumberofCourseFilesById(int CourseId)
        {
            var totalCourseFiles = await _dbContext.CourseFileAttachments
                .CountAsync(x => x.CourseId == CourseId);
            return totalCourseFiles;
        }
    }
}
