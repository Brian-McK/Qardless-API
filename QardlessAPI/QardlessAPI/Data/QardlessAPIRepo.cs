using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data.Dtos.Admin;
using QardlessAPI.Data.Dtos.Authentication;
using QardlessAPI.Data.Dtos.Business;
using QardlessAPI.Data.Dtos.Certificate;
using QardlessAPI.Data.Dtos.Course;
using QardlessAPI.Data.Dtos.Employee;
using QardlessAPI.Data.Dtos.EndUser;
using QardlessAPI.Data.Models;
using System.Security.Cryptography;
using System.Text;

namespace QardlessAPI.Data
{
    public class QardlessAPIRepo : IQardlessAPIRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly string role_RegisteredUser = "RegisteredUser";
        private readonly string role_Administrator = "Administrator";
        private readonly string role_Business = "Business";
        private readonly string role_Employee = "Employee";

        public QardlessAPIRepo(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));

            _userManager = userManager;
            _roleManager = roleManager;
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
            return await _context.Admins.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id.ToString());
        }

        public async Task<Admin?> UpdateAdminDetails(Guid id, AdminUpdateDto adminForUpdate)
        {
            Admin? admin = await _context.Admins.FirstOrDefaultAsync(e => e.Id == id.ToString());

            admin.Name = adminForUpdate.Name;
            admin.Email = adminForUpdate.Email;
            admin.PhoneNumber = adminForUpdate.PhoneNumber;

            _context.SaveChanges();
            _context.Admins.Add(admin);

            return await _context.Admins.FirstOrDefaultAsync(e => e.Id == id.ToString());
        }

        public async Task<AdminReadDto> AddNewAdmin(AdminCreateDto newAdmin)
        {
            if (newAdmin == null)
                throw new ArgumentNullException(nameof(newAdmin));

            var user_Admin = new Admin()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = newAdmin.Email,
                Email = newAdmin.Email,
                Name = newAdmin.Name
            };

            // insert the admin user into the DB
            await _userManager.CreateAsync(user_Admin, newAdmin.Password);

            // assign the "Administrator" role
            await _userManager.AddToRoleAsync(user_Admin, role_Administrator);

            // confirm the e-mail and remove lockout
            user_Admin.EmailConfirmed = true;
            user_Admin.LockoutEnabled = false;

            _context.Admins.Add(user_Admin);
            await _context.SaveChangesAsync();

            AdminReadDto adminReadDto = new AdminReadDto()
            {
                Id = new Guid(user_Admin.Id),
                Name = user_Admin.Name,
                Email = user_Admin.Email,
                PhoneNumber = user_Admin.PhoneNumber
            };

            return adminReadDto;
        }

        public void DeleteAdmin(Admin? admin)
        {
            if (admin == null)
                throw new ArgumentNullException(nameof(admin));

            _context.Admins.Remove(admin);
            _userManager.DeleteAsync(admin);
        }
        #endregion

        #region Business
        public async Task<IEnumerable<Business>> ListAllBusinesses()
        {
            return await _context.Businesses.ToListAsync();
        }

        public async Task<Business?> GetBusinessById(Guid id)
        {
            return await _context.Businesses.FirstOrDefaultAsync(b => b.Id == id.ToString());
        }

        public async Task<IEnumerable<Certificate?>> GetCertsDueForRenewal(Guid id)
        { 
            return await _context.Certificates
                .Include(c => c.Course)
                .Where(c => c.Course.BusinessId == id 
                && (c.Course.Expiry.CompareTo(DateTime.Now)) < 0)
                //&& (DateTime.Now - c.Course.Expiry).TotalDays.CompareTo(42) == 0)
                //&& (DateTime.Compare(c.Course.Expiry, DateTime.Now) < 42)
                //&& (DateTime.Compare(c.Course.Expiry, DateTime.Now) > 0)) 
                .ToListAsync();
        }

        // For login
        public async Task<Business?> GetBusinessByEmail(LoginDto businessLogin)
        {
            return await _context.Businesses.FirstOrDefaultAsync(
                e => e.Email == businessLogin.Email);
        }

        public async Task<Business?> UpdateBusinessDetails(Guid id, BusinessUpdateDto businessUpdate)
        {
            Business? business = await _context.Businesses.FirstOrDefaultAsync(b => b.Id == id.ToString());

            business.Name = businessUpdate.Name;
            business.Email = businessUpdate.Email;
            business.PhoneNumber = businessUpdate.PhoneNumber;

            _context.SaveChanges();
            _context.Businesses.Add(business);

            return await _context.Businesses.FirstOrDefaultAsync(b => b.Id == id.ToString());
        }

        public BusinessReadFullDto AddNewBusiness(BusinessCreateDto businessForCreation)
        {
            if (businessForCreation == null)
                throw new ArgumentNullException(nameof(businessForCreation));

            var user_Business = new Business()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = businessForCreation.Email,
                Email = businessForCreation.Email,
                Name = businessForCreation.Name
            };

            // insert the user into the DB
            await _userManager.CreateAsync(user_Business, businessForCreation.Password);

            // assign the "Business" role
            await _userManager.AddToRoleAsync(user_Business, role_Business);

            // confirm the e-mail and remove lockout
            user_Business.EmailConfirmed = true;
            user_Business.LockoutEnabled = false;

            _context.Businesses.Add(user_Business);
            await _context.SaveChangesAsync();

            BusinessReadFullDto businessReadFullDto = new BusinessReadFullDto()
            {
                Id = new Guid(user_Business.Id),
                Name = user_Business.Name,
                Email = user_Business.Email,
                PhoneNumber = user_Business.PhoneNumber
            };

            return businessReadFullDto;

        }

        public async void DeleteBusiness(Business? business)
        {
            if (business == null)
                throw new ArgumentNullException(nameof(business));

            _context.Businesses.Remove(business);
            _userManager.DeleteAsync(business);

            await _context.SaveChangesAsync();
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

        public async Task<IEnumerable<Certificate?>> GetCertificateByBusinessId(Guid id)
        {
            return await _context.Certificates
                .Include(c => c.Course)
                .Where(c => c.Course.BusinessId == id).ToListAsync();
        }

        public async Task<Certificate?> GetCertificateById(Guid id)
        {
            return await _context.Certificates
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == id);
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

            // AssignCert(cert);
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
        }
        #endregion

        #region Course 

        public async Task<IEnumerable<Course>> ListAllCourses()
        {
            return await _context.Courses.ToListAsync();
        }
        
        public async Task<IEnumerable<Course>> ListCoursesByBusinessId(Guid id)
        {
            return await _context.Courses.Where(c => c.BusinessId == id).OrderBy(c => c.CourseDate).ToListAsync();
        }

        public Task<Course> GetCourseById(Guid id)
        {
            var course = _context.Courses.Where(c => c.Id == id).FirstOrDefaultAsync();

            return course;
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

        public void DeleteCourse(Course? course)
        {
            if(course == null) throw new ArgumentNullException(nameof(course));
            _context .Courses.Remove(course);
        }

        #endregion#

        #region FlaggedIssue
        public async Task<IEnumerable<FlaggedIssue?>> GetFlaggedIssues()
        {
            return await _context.FlaggedIssues.ToListAsync();
        }

        public async Task<FlaggedIssue?> GetFlaggedIssue(Guid id)
        {
            return await _context.FlaggedIssues.FirstOrDefaultAsync(c => c.Id == id);
        }

        public void PutFlaggedIssue(Guid id, FlaggedIssue? flaggedIssue)
        {
            // Implemented in the controller
        }

        public void PostFlaggedIssue(FlaggedIssue? flaggedIssue)
        {
            if (flaggedIssue == null)
            {
                throw new ArgumentNullException(nameof(flaggedIssue));
            }

            flaggedIssue.Id = Guid.NewGuid();
            flaggedIssue.CreatedAt = DateTime.Now;

            _context.FlaggedIssues.Add(flaggedIssue);
        }

        public void DeleteFlaggedIssue(FlaggedIssue? flaggedIssue)
        {
            if (flaggedIssue == null)
            {
                throw new ArgumentNullException(nameof(flaggedIssue));
            }

            _context.FlaggedIssues.Remove(flaggedIssue);
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
                .FirstOrDefaultAsync(e => e.Id == id.ToString());
        }

        //For Login Controller
        public async Task<Employee?> GetEmployeeByEmail(LoginDto empLoginDto)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == empLoginDto.Email);
        }

        public async Task<Employee?> UpdateEmployee(Guid id, EmployeeUpdateDto employeeUpdateDto)
        {
            Employee? emp = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id.ToString());

            emp.Name = employeeUpdateDto.Name;
            emp.Email = employeeUpdateDto.Email;
            emp.PasswordHash = HashPassword(employeeUpdateDto.Password);
            emp.PhoneNumber = employeeUpdateDto.PhoneNumber;
            emp.PrivilegeLevel = employeeUpdateDto.PrivilegeLevel;
            emp.BusinessId = employeeUpdateDto.BusinessId;

            _context.SaveChanges();
            _context.Employees.Add(emp);

            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == id.ToString());
        }

        public EmployeeReadFullDto AddNewEmployee(EmployeeCreateDto newEmp)
        {
            if (newEmp == null)
                throw new ArgumentNullException(nameof(newEmp));

            Employee emp = new Employee
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = newEmp.Email,
                Email = newEmp.Email,
                Name = newEmp.Name,
                PhoneNumber = newEmp.PhoneNumber,
                PrivilegeLevel = newEmp.PrivilegeLevel,
                BusinessId = newEmp.BusinessId
            };

            // insert the user into the DB
            await _userManager.CreateAsync(emp, newEmp.Password);

            // assign the "Employee" role
            await _userManager.AddToRoleAsync(emp, role_Employee);

            // confirm the e-mail and remove lockout
            emp.EmailConfirmed = true;
            emp.LockoutEnabled = false;

            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();

            EmployeeReadFullDto empFullRead = new EmployeeReadFullDto
            {
                Id = new Guid(emp.Id),
                Name = emp.Name,
                Email = emp.Email,
                PhoneNumber = emp.PhoneNumber,
                BusinessId = emp.BusinessId
            };
            
            return empFullRead;
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
                .Where(e => e.Id == id.ToString())
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
            EndUser? endUser = await _context.EndUsers.FirstOrDefaultAsync(e => e.Id == id.ToString());

            endUser.Name = endUserUpdateDto.Name;
            endUser.Email = endUserUpdateDto.Email;
            endUser.PhoneNumber = endUserUpdateDto.PhoneNumber;

            _context.SaveChanges();
            _context.EndUsers.Add(endUser);

            return await _context.EndUsers.FirstOrDefaultAsync(e => e.Id == id.ToString());
        }

        public EndUserReadFullDto AddNewEndUser(EndUserCreateDto endUserForCreation)
        {
            if (endUserForCreation == null)
                throw new ArgumentNullException(nameof(endUserForCreation));

            EndUser user_RegisteredUser = new EndUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = endUserForCreation.Email,
                Email = endUserForCreation.Email,
                Name = endUserForCreation.Name,
                PhoneNumber = endUserForCreation.PhoneNumber
            };

            // insert the standard user into the DB
            await _userManager.CreateAsync(user_RegisteredUser, endUserForCreation.Password);

            // assign the "RegisteredUser" role
            await _userManager.AddToRoleAsync(user_RegisteredUser, role_RegisteredUser);

            // confirm the e-mail and remove lockout
            user_RegisteredUser.EmailConfirmed = true;
            user_RegisteredUser.LockoutEnabled = false;

            _context.EndUsers.Add(user_RegisteredUser);
            await _context.SaveChangesAsync();

            EndUserReadFullDto endUserReadFullDto = new EndUserReadFullDto
            {
                Id = new Guid(user_RegisteredUser.Id),
                Name = user_RegisteredUser.Name,
                Email = user_RegisteredUser.Email,
                PhoneNumber = user_RegisteredUser.PhoneNumber
            };
           
            return endUserReadFullDto;
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
            SaveChanges();

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
            SaveChanges();

            return empForProps;
        }

        public AdminReadDto SendAdminForProps(Admin admin)
        {
            AdminReadDto adminForProps = new AdminReadDto
            {
                Id = new Guid(admin.Id),
                Name = admin.Name,
                Email = admin.Email,
                PhoneNumber = admin.PhoneNumber
            };

            SaveChanges();

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
