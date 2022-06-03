using AdminPortal.Data.Data;
using AutoMapper;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using AdminPortal.CoreBusiness.Services;

namespace AdminPortal.CoreBusiness.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly IExcelFileService _excelFileService;
        private readonly IPasswordHashService _passwordHashService;
        public UnitOfWork(SaxTalentManagementContext _dbContext, IMapper _mapper,
            AutoMapper.IConfigurationProvider _configurationProvider, IExcelFileService _excelFileService,
            IPasswordHashService _passwordHashService)
        {
            this._mapper = _mapper;
            this._configurationProvider = _configurationProvider;
            this._dbContext = _dbContext;
            this._excelFileService = _excelFileService;
            this._passwordHashService = _passwordHashService;

            //Courses Repositories
            CourseCategoriesRepository = new CourseCategoryRepository(_dbContext, _mapper, _configurationProvider, _excelFileService);
            CoursesRepository = new CoursesRepository(_dbContext, _mapper, _configurationProvider, _excelFileService);
            CourseFileAttachmentsRepository = new CourseFileAttachmentRepository(_dbContext, _mapper, _configurationProvider);
            CoursesEnrollmentsRepository = new CourseEnrollmentsRepository(_dbContext, _mapper, _configurationProvider, CoursesRepository, _excelFileService);

            //Employee Repositories
            EmployeeRepository = new EmployeeRepository(_dbContext, _mapper, _configurationProvider, _excelFileService);
            EmployeeFileAttachmentsRepository = new EmployeeFileAttachmentRepository(_dbContext, _mapper, _configurationProvider);

            //User and Roles Repositories
            UserRepository = new UserRepository(_dbContext, _mapper, _configurationProvider, _passwordHashService);
            RoleRepository = new RolesRepository(_dbContext, _mapper, _configurationProvider);
            UserRolesRepository = new UserRolesRepository(_dbContext, _mapper, _configurationProvider, RoleRepository);

            //Authentication Repository
            AuthenticationRepository = new AuthenticationRepository(_passwordHashService, _dbContext);

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

        public IUserRepository UserRepository { get; private set; }

        public IRolesRepository RoleRepository { get; private set; }

        public IUserRolesRepository UserRolesRepository { get; private set; }

        public IAuthenticationRepository AuthenticationRepository { get; private set; }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
