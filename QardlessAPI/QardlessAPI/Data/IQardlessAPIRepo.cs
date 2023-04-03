using QardlessAPI.Data.Dtos.Admin;
using QardlessAPI.Data.Dtos.Authentication;
using QardlessAPI.Data.Dtos.Business;
using QardlessAPI.Data.Dtos.Certificate;
using QardlessAPI.Data.Dtos.Course;
using QardlessAPI.Data.Dtos.Employee;
using QardlessAPI.Data.Dtos.EndUser;
using QardlessAPI.Data.Dtos.FlaggedIssue;
using QardlessAPI.Data.Models;

namespace QardlessAPI.Data
{
    public interface IQardlessAPIRepo
    {
        bool SaveChanges();

        #region Admin
        Task<IEnumerable<Admin>> ListAllAdmins();
        Task<Admin?> GetAdminById(Guid id);
        Task<Admin?> GetAdminByEmail(LoginDto adminLoginDto);
        Task<Admin?> UpdateAdminDetails(Guid id, AdminUpdateDto adminForUpdate);
        AdminPartialDto AddNewAdmin(AdminCreateDto newAdmin);
        bool CheckAdminPassword(Admin admin, LoginDto login);
        void DeleteAdmin(Admin? admin);
        #endregion

        #region Business
        Task<IEnumerable<Business>> ListAllBusinesses();
        Task<Business?> GetBusinessById(Guid id);
        Task<Business?> GetBusinessByEmail(LoginDto businessLogin);
        Task<IEnumerable<Certificate?>> GetCertificateByBusinessId(Guid id);
        Task<Business?> UpdateBusinessDetails(Guid id, BusinessUpdateDto businessUpdate);
        BusinessReadPartialDto AddNewBusiness(BusinessCreateDto businessForCreation);
        void DeleteBusiness(Business? business);

        #endregion

        #region Certificate
        Task<IEnumerable<Certificate?>> ListAllCertificates();
        Task<IEnumerable<Certificate?>> GetCertificatesByEndUserId(Guid id);
        Task<Certificate?> GetCertificateById(Guid id);
        Task<Certificate?> FindCertificateByCertNumber(string certNumber);
        Task<Certificate?> UpdateCertificate(Guid id, CertificateUpdateDto certForUpdateDto);

        // WEB APP - GOAL 6
        Certificate AddNewCertificate(CertificateCreateDto certForCreation);
        void AssignCert(Certificate certificate);
        void UnassignCert(CertificateReadPartialDto certToUnassign);
        void FreezeCertificate(Certificate certificate);
        void UnfreezeCertificate(Certificate certificate);

        void DeleteCertificate(Certificate? certificate);
        #endregion

        #region Course 

        Task<IEnumerable<Course>> ListAllCourses();
        Task<IEnumerable<Course>> ListCoursesByBusinessId(Guid id);
        Task<Course?> GetCourseById(Guid id);
        Task<Course?> UpdateCourseDetails(Guid id, CourseReadDto courseForUpdate);
        CourseReadDto AddNewCourse(CourseReadDto newCourse);
        Task<Course?> GetCourseByTitleAndDate(CourseReadDto courseDto);
        void DeleteCourse(Course? course);

        #endregion

        #region FlaggedIssue
        Task<IEnumerable<FlaggedIssue?>> ListAllFlaggedIssues();
        Task<FlaggedIssue?> GetFlaggedIssueById(Guid id);
        Task<FlaggedIssue?> UpdateFlaggedIssueWasRead(Guid id, FlaggedIssueUpdateDto flaggedIssueDto);
        void PostFlaggedIssue(FlaggedIssue? flaggedIssue);
        void DeleteFlaggedIssue(FlaggedIssue? flaggedIssue);
        #endregion

        #region Employee
        Task<IEnumerable<Employee>> ListAllEmployees();
        Task<IEnumerable<Employee?>> GetEmployeesByBusinessId(Guid id);
        Task<Employee?> GetEmployeeById(Guid id);
        Task<Employee?> GetEmployeeByEmail(LoginDto empLoginDto);
        Task<Employee?> UpdateEmployee(Guid id, EmployeeUpdateDto employeeUpdateDto);
        EmployeeReadPartialDto AddNewEmployee(EmployeeCreateDto newEmp);
        bool CheckEmpPassword(Employee emp, LoginDto login);
        void DeleteEmployee(Employee? employee);
        #endregion

        #region EndUser
        Task<IEnumerable<EndUser>> ListAllEndUsers();
        Task<EndUser?> GetEndUserById(Guid id);
        Task<EndUser?> FindEndUserByEmail(string email);
        Task<EndUser?> GetEndUserByEmail(LoginDto endUserLoginDto);
        Task<EndUser?> UpdateEndUserDetails(Guid id, EndUserUpdateDto endUserUpdateDto);
        EndUserReadPartialDto AddNewEndUser(EndUserCreateDto endUserForCreation);
        bool CheckEndUserPassword(EndUser endUser, LoginDto login);
        void DeleteEndUser(EndUser endUser);
        #endregion

        #region Login
        EndUserReadPartialDto SendEndUserForProps(EndUser endUser);
        EmployeeReadPartialDto SendEmpForProps(Employee emp);
        AdminPartialDto SendAdminForProps(Admin admin);

        #endregion

        #region Security
        public string HashPassword(string Password);
        #endregion

    }
}
