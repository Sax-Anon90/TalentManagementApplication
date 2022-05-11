using AdminPortal.Data.Data;
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

            //Courses Repositories
            CourseCategoriesRepository = new CourseCategoryRepository(_dbContext, _mapper, _configurationProvider);
            CoursesRepository = new CoursesRepository(_dbContext, _mapper, _configurationProvider);
            CourseFileAttachmentsRepository = new CourseFileAttachmentRepository(_dbContext, _mapper, _configurationProvider);
            CoursesEnrollmentsRepository = new CourseEnrollmentsRepository(_dbContext, _mapper, _configurationProvider, CoursesRepository);

            //Employee Repositories
            EmployeeRepository = new EmployeeRepository(_dbContext, _mapper, _configurationProvider);
            EmployeeFileAttachmentsRepository = new EmployeeFileAttachmentRepository(_dbContext, _mapper, _configurationProvider);

            //Other repositories
            DepartmentsRepository = new DepartmentsRepository(_dbContext, _mapper, _configurationProvider);
            GenderRepository = new GenderRepository(_dbContext, _mapper, _configurationProvider);
        }
        public ICourseCategoryRepository CourseCategoriesRepository { get; private set; }

        public ICoursesRepository CoursesRepository { get; private set; }

        public ICourseFileAttachmentRepository CourseFileAttachmentsRepository { get; private set; }

        public IEmployeeRepository EmployeeRepository { get; private set; }

        public IDepartmentsRepository DepartmentsRepository { get; private set; }

        public IGenderRepository GenderRepository { get; private set; }

        public IEmployeeFileAttachmentRepository EmployeeFileAttachmentsRepository { get; private set; }

        public ICourseEnrollmentsRepository CoursesEnrollmentsRepository { get; private set; }

        public async Task SaveChangesAsync()
        {
           await _dbContext.SaveChangesAsync();
        }
    }
}
