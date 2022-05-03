using AdminPortal.Data;
using AdminPortal.CoreBusiness.Repositories.Implementation;
using AutoMapper;
using AdminPortal.CoreBusiness.Repositories.Contracts;

namespace AdminPortal.CoreBusiness.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        public UnitOfWork(SaxTalentManagementContext _dbContext, IMapper _mapper,
            AutoMapper.IConfigurationProvider _configurationProvider)
        {
            this._mapper = _mapper;
            this._configurationProvider = _configurationProvider;
            this._dbContext = _dbContext;
            CourseCategoriesRepository = new CourseCategoryRepository(_dbContext, _mapper, _configurationProvider);
            CoursesRepository = new CoursesRepository(_dbContext, _mapper, _configurationProvider);
            CourseFileAttachmentsRepository = new CourseFileAttachmentRepository(_dbContext, _mapper, _configurationProvider);
        }
        public ICourseCategoryRepository CourseCategoriesRepository { get; private set; }

        public ICoursesRepository CoursesRepository { get; private set; }

        public ICourseFileAttachmentRepository CourseFileAttachmentsRepository { get; private set; }
    }
}
