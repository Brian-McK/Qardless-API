using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data.Dtos.Admin;
using QardlessAPI.Data.Dtos.Authentication;
using QardlessAPI.Data.Dtos.Business;
using QardlessAPI.Data.Dtos.Certificate;
using QardlessAPI.Data.Dtos.Course;
using QardlessAPI.Data.Dtos.Employee;
using QardlessAPI.Data.Dtos.EndUser;
using QardlessAPI.Data.Dtos.FlaggedIssue;
using QardlessAPI.Data.Models;
using System.Composition;
using System.Security.Cryptography;
using System.Text;

namespace QardlessAPI.Data
{
    public class QardlessAPIRepo : IQardlessAPIRepo
    {
        private readonly ApplicationDbContext _context;

        public QardlessAPIRepo(ApplicationDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context)); 
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        #region Admin
        public async Task<IEnumerable<Admin>> ListAllAdmins()
        {
            return await _context.Admins.ToListAsync();
        }

        public async Task<Admin?> GetAdminById(Guid id)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Id == id);
        }

        //for login
        public async Task<Admin?> GetAdminByEmail(LoginDto adminLoginDto)
        {
            return await _context.Admins.FirstOrDefaultAsync(
                e => e.Email == adminLoginDto.Email);
        }

        public async Task<Admin?> UpdateAdminDetails(Guid id, AdminUpdateDto adminForUpdate)
        {
            Admin? admin = await _context.Admins.FirstOrDefaultAsync(e => e.Id == id);

            admin.Name = adminForUpdate.Name;
            admin.Email = adminForUpdate.Email;
            admin.ContactNumber = adminForUpdate.ContactNumber;

            _context.SaveChanges();
            _context.Admins.Add(admin);

            return await _context.Admins.FirstOrDefaultAsync(e => e.Id == id);
        }

        public AdminPartialDto AddNewAdmin(AdminCreateDto newAdmin)
        {
            if (newAdmin == null)
                throw new ArgumentNullException(nameof(newAdmin));

            Admin admin = new Admin
            {
                Id = new Guid(),
                Name = newAdmin.Name,
                Email = newAdmin.Email,
                PasswordHash = HashPassword(newAdmin.Password),
                ContactNumber = newAdmin.ContactNumber,
                CreatedAt = DateTime.Now,
                LastLoginDate = DateTime.Now
            };

            _context.Admins.Add(admin);
            _context.SaveChanges();

            AdminPartialDto adminPartial = new AdminPartialDto
            {
                Id = admin.Id,
                Name = admin.Name,
                Email = admin.Email,
                ContactNumber = admin.ContactNumber,
                IsLoggedIn = false
            };

            return adminPartial;
        }

        // Password check for login
        public bool CheckAdminPassword(Admin admin, LoginDto login)
        {
            if(admin.PasswordHash == HashPassword(login.Password))
                return true;
            return false;
        }

        public void DeleteAdmin(Admin? admin)
        {
            if (admin == null)
                throw new ArgumentNullException(nameof(admin));

            _context.Admins.Remove(admin);
            _context.SaveChanges();
        }
        #endregion

        #region Business
        public async Task<IEnumerable<Business>> ListAllBusinesses()
        {
            return await _context.Businesses.ToListAsync();
        }

        public async Task<Business?> GetBusinessById(Guid id)
        {
            return await _context.Businesses.FirstOrDefaultAsync(b => b.Id == id);
        }

        // For login
        public async Task<Business?> GetBusinessByEmail(LoginDto businessLogin)
        {
            return await _context.Businesses.FirstOrDefaultAsync(
                e => e.Email == businessLogin.Email);
        }

        public async Task<Business?> UpdateBusinessDetails(Guid id, BusinessUpdateDto businessUpdate)
        {
            Business? business = await _context.Businesses.FirstOrDefaultAsync(b => b.Id == id);

            business.Name = businessUpdate.Name;
            business.Email = businessUpdate.Email;
            business.Contact = businessUpdate.Contact;

            _context.SaveChanges();
            _context.Businesses.Add(business);

            return await _context.Businesses.FirstOrDefaultAsync(b => b.Id == id);
        }

        public BusinessReadPartialDto AddNewBusiness(BusinessCreateDto businessForCreation)
        {
            if (businessForCreation == null)
                throw new ArgumentNullException(nameof(businessForCreation));

            Business business = new Business()
            {
                Id = new Guid(),
                Name = businessForCreation.Name,
                Email = businessForCreation.Email,
                Contact = businessForCreation.Contact,
                CreatedAt = DateTime.Now
            };

            _context.Businesses.Add(business);
            _context.SaveChanges();

            BusinessReadPartialDto businessReadPartialDto = new BusinessReadPartialDto
            {
                Id = business.Id,
                Name = business.Name,
                Email = business.Email,
                Contact = business.Contact
            };

            return businessReadPartialDto;
        }

        public void DeleteBusiness(Business? business)
        {
            if (business == null)
                throw new ArgumentNullException(nameof(business));

            _context.Businesses.Remove(business);
            _context.SaveChanges();
        }
        #endregion

        #region Certificate
        public async Task<IEnumerable<Certificate?>> ListAllCertificates()
        {
            return await _context.Certificates
                .Include(c => c.Course)
                .ToListAsync();
        }

        public async Task<IEnumerable<Certificate?>> GetCertificatesByEndUserId(Guid id)
        {
            return await _context.Certificates
                .Include(c => c.Course)
                .Where(c => c.EndUserId == id).ToListAsync();
        }

        public async Task<IEnumerable<CertificatesForExport>> GetCertificateByBusinessId(Guid id)
        {
            return await _context.Certificates
                .Include(c => c.Course)
                .Where(c => c.Course.BusinessId == id)
                .Select(c => new CertificatesForExport
                {
                    Id = c.Id,
                    CertNumber = c.CertNumber,
                    Course = _context.Courses
                        .Where(c => c.Id == c.Id)
                        .Select(co => new Course
                        {
                            Id = c.CourseId,
                            BusinessId = co.BusinessId,
                            Title = co.Title,
                            CourseDate = co.CourseDate,
                            Expiry = co.Expiry,
                        }).FirstOrDefault(),
                    PdfUrl = c.PdfUrl,
                    CreatedAt = c.CreatedAt,
                    EndUserName = _context.EndUsers
                        .Where(e => e.Id == c.EndUserId)
                        .Select(e => e.Name)
                        .FirstOrDefault(),
                    EndUserEmail = _context.EndUsers
                        .Where(e => e.Id == c.EndUserId)
                        .Select(e => e.Email)
                        .FirstOrDefault(),
                    IsFrozen = c.IsFrozen,
                    })
                .ToListAsync();

        }

        public async Task<Certificate?> GetCertificateById(Guid id)
        {
            return await _context.Certificates
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Certificate?> FindCertificateByCertNumber(string certNumber)
        {
            return await _context.Certificates
                .FirstOrDefaultAsync(c => c.CertNumber == certNumber);
        }

        public async Task<Certificate?> UpdateCertificate(Guid id, CertificateUpdateDto certForUpdateDto)
        {
            Certificate? cert = await _context.Certificates.FirstOrDefaultAsync(c => c.Id == id);

            cert.CourseId = certForUpdateDto.CourseId;
            cert.EndUserId = certForUpdateDto.EndUserId;
            cert.CertNumber = certForUpdateDto.CertNumber;
            cert.PdfUrl = certForUpdateDto.PdfUrl;
            cert.CreatedAt = DateTime.Now;
            
            _context.SaveChanges();
            _context.Certificates.Add(cert);

            return await _context.Certificates.FirstOrDefaultAsync(c => c.Id == id);
        }

        // WEB APP  - CREATE CERT
        public Certificate AddNewCertificate(CertificateCreateDto certForCreation)
        {
            if (certForCreation == null)
                throw new ArgumentNullException(nameof(certForCreation));
            
            var endUser = FindEndUserByEmail(certForCreation.EndUserEmail).Result;

            if (endUser == null)
                throw new ArgumentNullException(nameof(endUser));

            var cert = new Certificate
            {
                Id = Guid.NewGuid(),
                CourseId = certForCreation.CourseId,
                EndUserId = endUser.Id,
                CertNumber = certForCreation.CertNumber,
                PdfUrl = certForCreation.PdfUrl,
                CreatedAt = DateTime.Now,
                IsFrozen = false
            };

            _context.Certificates.Add(cert);
            
            _context.SaveChanges();

            return cert;
        }

        // WEB APP - ASSIGN CERT
        public void AssignCert(Certificate certificate)
        {
            var cert = _context.Certificates.Find(certificate.Id);

            if (cert == null)
                throw new ArgumentNullException(nameof(cert));

            if (!certificate.Equals(cert))
                throw new Exception("Created certificate does not match");

            var endUser = _context.EndUsers.Include(i => i.EndUserCerts).FirstOrDefault(e => e.Id == cert.EndUserId);

            if (endUser == null)
                throw new ArgumentNullException(nameof(endUser));

            endUser.EndUserCerts.Add(cert);

            _context.SaveChanges();
        }

        // WEB APP - UNASSIGN CERT
        public void UnassignCert(CertificateReadPartialDto certToUnassign)
        {
            var cert =  _context.Certificates.Find(certToUnassign.Id);
            var endUser = _context.EndUsers.Find(certToUnassign.EndUserId);

            if (cert == null)
                throw new ArgumentNullException(nameof(cert));

            _context.EndUsers.Include(i => i.EndUserCerts)
                .FirstOrDefault(e => e.Id == cert.EndUserId);
            
            if(endUser == null)
                throw new ArgumentNullException(nameof(endUser));
            
            endUser.EndUserCerts.Remove(cert);
            
            _context.Certificates.Remove(cert); // Remove certificate from db 
            _context.SaveChanges();
        }

        // WEB APP - FREEZE CERT
        public void FreezeCertificate(Certificate certificate)
        {
            if(certificate == null) throw new ArgumentNullException(nameof(certificate));

            certificate.IsFrozen = true;

            _context.SaveChanges();
        }

        // WEB APP - UNFREEZE CERT
        public void UnfreezeCertificate(Certificate certificate)
        {
            if (certificate == null) throw new ArgumentNullException(nameof(certificate));

            certificate.IsFrozen = false;

            _context.SaveChanges();
        }

        public void DeleteCertificate(Certificate? certificate)
        {
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));

            _context.Certificates.Remove(certificate);
            _context.SaveChanges();
        }
        #endregion

        #region Course 

        public async Task<IEnumerable<Course>> ListAllCourses()
        {
            return await _context.Courses.ToListAsync();
        }
        
        public async Task<IEnumerable<Course>> ListCoursesByBusinessId(Guid id)
        {
            return await _context.Courses
                .Where(c => c.BusinessId == id)
                .OrderBy(c => c.CourseDate)
                .ToListAsync();
        }

        public async Task<Course?> GetCourseById(Guid id)
        {
            return await _context.Courses
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Course?> UpdateCourseDetails(Guid id, CourseReadDto courseForUpdate)
        {
            Course? course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);

            course.Title = courseForUpdate.Title;
            course.CourseDate = DateTime.ParseExact(courseForUpdate.CourseDate, "dd/MM/yyyy", null);
            course.Expiry = DateTime.ParseExact(courseForUpdate.Expiry, "dd/MM/yyyy", null);
            
            _context.Courses.Update(course);
            
            await _context.SaveChangesAsync();

            return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        }

        public CourseReadDto AddNewCourse(CourseReadDto newCourse)
        {
            if (newCourse == null)
                    throw new ArgumentNullException(nameof(newCourse));
            
            var course = new Course()
            {
                Id = new Guid(),
                BusinessId = newCourse.BusinessId,
                Title = newCourse.Title,
                CourseDate = DateTime.ParseExact(newCourse.CourseDate, "dd/MM/yyyy", null),
                Expiry = DateTime.ParseExact(newCourse.Expiry, "dd/MM/yyyy", null),
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            return newCourse;
        }

        public async Task<Course?> GetCourseByTitleAndDate(CourseReadDto courseDto)
        {
            return await _context.Courses.FirstOrDefaultAsync(
                c => c.Title == courseDto.Title && 
                c.CourseDate == DateTime.Parse(courseDto.CourseDate));
        }

        public void DeleteCourse(Course? course)
        {
            if(course == null) throw new ArgumentNullException(nameof(course));
            _context.Courses.Remove(course);
            _context.SaveChanges();
        }

        #endregion#

        #region FlaggedIssue
        public async Task<IEnumerable<FlaggedIssue>> ListAllFlaggedIssues()
        {
            return await _context.FlaggedIssues.ToListAsync();
        }

        public async Task<FlaggedIssue?> GetFlaggedIssueById(Guid id)
        {
            return await _context.FlaggedIssues
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<FlaggedIssueWithUser>> ListFlaggedIssuesByBusinessId(Guid businessId)
        {
            return await _context.FlaggedIssues
                .Include(f => f.Certificate)
                    .ThenInclude(c => c.Course)
                .Where(f => f.Certificate.Course.BusinessId == businessId)
                .Select(f => new FlaggedIssueWithUser
                {
                    Id = f.Id,
                    Type = f.Type,
                    Content = f.Content,
                    CertificateId = f.CertificateId,
                    CertNum = _context.Certificates
                        .Where(c => c.Id == f.CertificateId)
                        .Select(cn => cn.CertNumber)
                        .FirstOrDefault(),
                    CourseTitle = _context.Courses
                        .Where(c => c.Id == f.CertificateId)
                        .Select(co => co.Title)
                        .FirstOrDefault(),
                    WasRead = f.WasRead,
                    CreatedAt = f.CreatedAt,
                    EndUserEmail = _context.EndUsers
                        .Where(e => e.Id == f.Certificate.EndUserId)
                        .Select(e => e.Email)
                        .FirstOrDefault()
                })
                .ToListAsync();
    }
        
        public void FlaggedIssueWasRead(Guid flaggedIssueId)
        {
            var flaggedIssue = _context.FlaggedIssues.FirstOrDefault(f => f.Id == flaggedIssueId);

            if (flaggedIssue == null) 
                throw new ArgumentNullException(nameof(flaggedIssue));

            flaggedIssue.WasRead = true;
            
            _context.SaveChanges();
        }

        public void PostFlaggedIssue(FlaggedIssue? flaggedIssue)
        {
            if (flaggedIssue == null)
                throw new ArgumentNullException(nameof(flaggedIssue));

            flaggedIssue.Id = Guid.NewGuid();
            flaggedIssue.CreatedAt = DateTime.Now;

            _context.FlaggedIssues.Add(flaggedIssue);
            _context.SaveChanges();
        }

        public void DeleteFlaggedIssue(FlaggedIssue? flaggedIssue)
        {
            if (flaggedIssue == null)
                throw new ArgumentNullException(nameof(flaggedIssue));
            
            _context.FlaggedIssues.Remove(flaggedIssue);
            _context.SaveChanges();
        }
        #endregion

        #region Employee
        public async Task<IEnumerable<Employee>> ListAllEmployees()
        {
            return await _context.Employees
                .Include(b => b.Business)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee?>> GetEmployeesByBusinessId(Guid id)
        {
            return await _context.Employees
                .Where(e => e.BusinessId == id).ToListAsync();
        }

        public async Task<Employee?> GetEmployeeById(Guid id)
        {
            return await _context.Employees
                .Include(b => b.Business)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        //For Login Controller
        public async Task<Employee?> GetEmployeeByEmail(LoginDto empLoginDto)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == empLoginDto.Email);
        }

        public async Task<Employee?> UpdateEmployee(Guid id, EmployeeUpdateDto employeeUpdateDto)
        {
            Employee? emp = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

            emp.Name = employeeUpdateDto.Name;
            emp.Email = employeeUpdateDto.Email;
            emp.PasswordHash = HashPassword(employeeUpdateDto.Password);
            emp.ContactNumber = employeeUpdateDto.ContactNumber;
            emp.PrivilegeLevel = employeeUpdateDto.PrivilegeLevel;
            emp.BusinessId = employeeUpdateDto.BusinessId;

            _context.Employees.Add(emp);
            _context.SaveChanges();

            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }

        public EmployeeReadPartialDto AddNewEmployee(EmployeeCreateDto newEmp)
        {
            if (newEmp == null)
                throw new ArgumentNullException(nameof(newEmp));

                Employee emp = new Employee
                {
                    Id = new Guid(),
                    Name = newEmp.Name,
                    Email = newEmp.Email,
                    PasswordHash = HashPassword(newEmp.Password),
                    ContactNumber = newEmp.ContactNumber,
                    CreatedAt = DateTime.Now,
                    PrivilegeLevel = newEmp.PrivilegeLevel,
                    BusinessId = newEmp.BusinessId
                };

                _context.Employees.Add(emp);
                _context.SaveChanges();

                EmployeeReadPartialDto empPartialRead = new EmployeeReadPartialDto
                {
                    Id = emp.Id,
                    Name = emp.Name,
                    Email = emp.Email,
                    ContactNumber = emp.ContactNumber,
                    BusinessId = emp.BusinessId
                };

                return empPartialRead;
            }

        // Password check for login 
        public bool CheckEmpPassword(Employee emp, LoginDto login)
        {
            if (emp.PasswordHash == HashPassword(login.Password))
                return true;

            return false;
        }

        public void DeleteEmployee(Employee? emp)
        {
            if (emp == null)
                throw new ArgumentNullException(nameof(emp));

            _context.Employees.Remove(emp);
            _context.SaveChanges();
        }
        #endregion

        #region EndUser
        public async Task<IEnumerable<EndUser>> ListAllEndUsers()
        {
            return await _context.EndUsers
                .Include(e => e.EndUserCerts)
                .ToListAsync();
        }

        public async Task<EndUser?> GetEndUserById(Guid id)
        {
            return await _context.EndUsers
                .Where(e => e.Id == id)
                .Include(e => e.EndUserCerts)
                .FirstOrDefaultAsync();
        }

        public Task <EndUser?> FindEndUserByEmail(string email)
        {
            var user = _context.EndUsers.Where(e => e.Email == email).Select(e => e);

            var e =  _context.EndUsers.FirstOrDefault(e => e.Email.Contains(email));

            return Task.FromResult(e);

        }

        //For login controller
        public async Task<EndUser?> GetEndUserByEmail(LoginDto endUserLoginDto)
        {
            return await _context.EndUsers.FirstOrDefaultAsync(
                e => e.Email == endUserLoginDto.Email);
        }

        public async Task<EndUser?> UpdateEndUserDetails(Guid id, EndUserUpdateDto endUserUpdateDto)
        {
            EndUser? endUser = await _context.EndUsers.FirstOrDefaultAsync(e => e.Id == id);

            endUser.Name = endUserUpdateDto.Name;
            endUser.Email = endUserUpdateDto.Email;
            endUser.ContactNumber = endUserUpdateDto.ContactNumber;

            _context.SaveChanges();
            _context.EndUsers.Add(endUser);

            return await _context.EndUsers.FirstOrDefaultAsync(e => e.Id == id);
        }

        public EndUserReadPartialDto AddNewEndUser(EndUserCreateDto endUserForCreation)
        {
            if (endUserForCreation == null)
                throw new ArgumentNullException(nameof(endUserForCreation));

            EndUser endUser = new EndUser
            {
                Id = new Guid(),
                Name = endUserForCreation.Name,
                Email = endUserForCreation.Email,
                EmailVerified = false,
                PasswordHash = HashPassword(endUserForCreation.Password),
                ContactNumber = endUserForCreation.ContactNumber,
                CreatedAt = DateTime.Now,
                LastLoginDate = DateTime.Now
            };

            _context.EndUsers.Add(endUser);
            _context.SaveChanges();

            EndUserReadPartialDto endUserReadPartialDto = new EndUserReadPartialDto
            {
                Id = endUser.Id,
                Name = endUser.Name,
                Email = endUser.Email,
                ContactNumber = endUser.ContactNumber,
                IsLoggedIn = false
            };
           
            return endUserReadPartialDto;
        }

        // Password check for login 
        public bool CheckEndUserPassword(EndUser endUser, LoginDto login)
        {
            if(endUser.PasswordHash == HashPassword(login.Password))
                return true;

            return false;
        }

        public void DeleteEndUser(EndUser? endUser)
        {
            if (endUser == null)
                throw new ArgumentNullException(nameof(endUser));

            _context.EndUsers.Remove(endUser);
            _context.SaveChanges();
        }

        #endregion

        #region Login 

        public EndUserReadPartialDto SendEndUserForProps(EndUser endUser)
        {
            EndUserReadPartialDto endUserForProps = new EndUserReadPartialDto
            {
                Id = endUser.Id,
                Name = endUser.Name,
                Email = endUser.Email,
                ContactNumber = endUser.ContactNumber,
                IsLoggedIn = true
            };

            endUser.LastLoginDate = DateTime.Now;
            _context.SaveChanges();

            return endUserForProps;
        }

        public EmployeeReadPartialDto SendEmpForProps(Employee emp)
        {
            EmployeeReadPartialDto empForProps = new EmployeeReadPartialDto
            {
                Id = emp.Id,
                Name = emp.Name,
                Email = emp.Email,
                ContactNumber = emp.ContactNumber,
                BusinessId = emp.BusinessId,
                IsLoggedIn = true
            };

            emp.LastLoginDate = DateTime.Now;
            _context.SaveChanges();

            return empForProps;
        }

        public AdminPartialDto SendAdminForProps(Admin admin)
        {
            AdminPartialDto adminForProps = new AdminPartialDto
            {
                Id = admin.Id,
                Name = admin.Name,
                Email = admin.Email,
                ContactNumber = admin.ContactNumber,
                IsLoggedIn = true
            };

            admin.LastLoginDate = DateTime.Now;
            _context.SaveChanges();

            return adminForProps;
        }

        #endregion

        #region Security 
        public string HashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
        }
        #endregion

    }
}
