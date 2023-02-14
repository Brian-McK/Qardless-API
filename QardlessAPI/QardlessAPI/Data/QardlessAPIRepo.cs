using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardlessAPI.Data.Dtos.Certificate;
using QardlessAPI.Data.Dtos.EndUser;
using QardlessAPI.Data.Models;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;

namespace QardlessAPI.Data
{
    public class QardlessAPIRepo : IQardlessAPIRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public QardlessAPIRepo(ApplicationDbContext context, IMapper mapper)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context)); 
            _mapper = mapper ??
               throw new ArgumentNullException(nameof(mapper));
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        #region Admin
        public async Task<IEnumerable<Admin?>> GetAdmins()
        {
            return await _context.Admins.ToListAsync();
        }

        public async Task<Admin?> GetAdmin(Guid id)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Id == id);
        }

        public void PutAdmin(Guid id, Admin? admin)
        {
            // Implemented in the controller
        }

        public void PatchAdmin(Guid id, Admin? admin)
        {
            // Implemented in the controller
        }

        public void PostAdmin(Admin? admin)
        {
            if (admin == null)
            {
                throw new ArgumentNullException(nameof(admin));
            }

            admin.Id = Guid.NewGuid();
            admin.CreatedDate = DateTime.Now;
            admin.LastLoginDate = admin.CreatedDate;

            _context.Admins.Add(admin);
        }

        public void DeleteAdmin(Admin? admin)
        {
            if (admin == null)
            {
                throw new ArgumentNullException(nameof(admin));
            }

            _context.Admins.Remove(admin);
        }
        #endregion

        #region Business
        public async Task<IEnumerable<Business?>> GetBusinesses()
        {
            return await _context.Businesses.ToListAsync();
        }

        public async Task<Business?> GetBusiness(Guid id)
        {
            return await _context.Businesses.FirstOrDefaultAsync(a => a.Id == id);
        }

        public void PutBusiness(Guid id, Business? business)
        {
            // Implemented in the controller
        }

        public void PatchBusiness(Guid id, Business? business)
        {
            // Implemented in the controller
        }

        public void PostBusiness(Business? business)
        {
            if (business == null)
            {
                throw new ArgumentNullException(nameof(business));
            }

            business.Id = Guid.NewGuid();
            business.CreatedDate = DateTime.Now;

            _context.Businesses.Add(business);
        }

        public void DeleteBusiness(Business? business)
        {
            if (business == null)
            {
                throw new ArgumentNullException(nameof(business));
            }

            _context.Businesses.Remove(business);
        }
        #endregion

        #region Certificate
        public async Task<IEnumerable<Certificate?>> ListAllCertificates()
        {
            return await _context.Certificates.ToListAsync();
        }

        public async Task<IEnumerable<Certificate?>> GetCertificatesByEndUserId(Guid id)
        {
            return await _context.Certificates
                .Where(c => c.EndUserId == id).ToListAsync();
        }

        public async Task<Certificate?> GetCertificateById(Guid id)
        {
            return await _context.Certificates.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Certificate?> UpdateCertificate(Guid id, CertificateUpdateDto certForUpdateDto)
        {
            Certificate? cert = await _context.Certificates.FirstOrDefaultAsync(c => c.Id == id);

            cert.CourseTitle = certForUpdateDto.CourseTitle;
            cert.CertNumber = certForUpdateDto.CertNumber;
            cert.CourseDate = certForUpdateDto.CourseDate;
            cert.ExpiryDate = certForUpdateDto.ExpiryDate;
            cert.PdfUrl = certForUpdateDto.PdfUrl;
            cert.CreatedDate = DateTime.Now;
            cert.EndUserId = certForUpdateDto.EndUserId;
            cert.BusinessId = certForUpdateDto.BusinessId;
            
            _context.SaveChanges();
            _context.Certificates.Add(cert);

            return await _context.Certificates.FirstOrDefaultAsync(c => c.Id == id);
        }

        public void AddNewCertificate(Certificate? certificate)
        {
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));

            certificate.Id = Guid.NewGuid();
            certificate.CreatedDate = DateTime.Now;

            _context.Certificates.Add(certificate);
        }

        public void DeleteCertificate(Certificate? certificate)
        {
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));

            _context.Certificates.Remove(certificate);
        }
        #endregion

        #region Changelog
        public async Task<IEnumerable<Changelog?>> GetChangelogs()
        {
            return await _context.Changelogs.ToListAsync();
        }

        public async Task<Changelog?> GetChangelog(Guid id)
        {
            return await _context.Changelogs.FirstOrDefaultAsync(c => c.Id == id);
        }

        public void PutChangelog(Guid id, Changelog? changelog)
        {
            // Implemented in the controller
        }

        public void PatchChangelog(Guid id, Changelog? changelog)
        {
            // Implemented in the controller
        }

        public void PostChangelog(Changelog? changelog)
        {
            if (changelog == null)
            {
                throw new ArgumentNullException(nameof(changelog));
            }

            changelog.Id = Guid.NewGuid();
            changelog.CreatedDate = DateTime.Now;

            _context.Changelogs.Add(changelog);
        }

        public void DeleteChangelog(Changelog? changelog)
        {
            if (changelog == null)
            {
                throw new ArgumentNullException(nameof(changelog));
            }

            _context.Changelogs.Remove(changelog);
        }
        #endregion

        #region Employee
        public async Task<IEnumerable<Employee?>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<IEnumerable<Employee?>> GetEmployeesByBusinessId(Guid id)
        {
            return await _context.Employees
                .Where(e => e.BusinessId == id).ToListAsync();
        }

        public async Task<Employee?> GetEmployee(Guid id)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }

        public void PutEmployee(Guid id, Employee? employee)
        {
            // Implemented in the controller
        }

        public void PatchEmployee(Guid id, Employee? employee)
        {
            // Implemented in the controller
        }

        public void PostEmployee(Employee? employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            employee.Id = Guid.NewGuid();
            employee.CreatedDate = DateTime.Now;
            employee.LastLoginDate = employee.CreatedDate;

            _context.Employees.Add(employee);
        }

        public void DeleteEmployee(Employee? employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            _context.Employees.Remove(employee);
        }
        #endregion

        #region EndUser
        public async Task<IEnumerable<EndUser>> ListAllEndUsers()
        {
            return await _context.EndUsers.ToListAsync();
        }

        public async Task<EndUser?> GetEndUserById(Guid id)
        {
            return await _context.EndUsers.FirstOrDefaultAsync(e => e.Id == id);
        }

        //For login controller
        public async Task<EndUser?> GetEndUserByEmail(EndUserLoginDto endUserLoginDto)
        {
            return await _context.EndUsers.FirstOrDefaultAsync(
                e => e.Email == endUserLoginDto.Email && 
                e.PasswordHash == HashPassword(endUserLoginDto.Password));
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

        public string HashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
        }

        public EndUserReadPartialDto AddNewEndUser(EndUserCreateDto endUserForCreation)
        {
            if (endUserForCreation == null)
                throw new ArgumentNullException(nameof(endUserForCreation));

            EndUser endUser = new EndUser();
            endUser.Id = new Guid();
            endUser.Name = endUserForCreation.Name;
            endUser.Email = endUserForCreation.Email;
            endUser.EmailVerified = false;
            endUser.PasswordHash = HashPassword(endUserForCreation.Password);
            endUser.ContactNumber = endUserForCreation.ContactNumber;
            endUser.CreatedDate = DateTime.Now;
            endUser.LastLoginDate = endUser.CreatedDate;

            _context.EndUsers.Add(endUser);
            _context.SaveChanges();

            EndUserReadPartialDto endUserReadPartialDto = new EndUserReadPartialDto();
            endUserReadPartialDto.Id = endUser.Id;
            endUserReadPartialDto.Name = endUser.Name;
            endUserReadPartialDto.Email = endUser.Email;
            endUserReadPartialDto.ContactNumber = endUser.ContactNumber;
            endUserReadPartialDto.isLoggedin = true;

            return endUserReadPartialDto;
        }

        public void DeleteEndUser(EndUser? endUser)
        {
            if (endUser == null)
                throw new ArgumentNullException(nameof(endUser));

            _context.EndUsers.Remove(endUser);
        }

        #endregion
    }
}
