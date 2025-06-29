using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactManager.Data;
using ContactManager.Models;
using ContactManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CsvHelper;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ContactManager.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContactsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Contacts
        public async Task<IActionResult> Index(string searchString, string filterType = "all", int page = 1, int pageSize = 12)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var contactsQuery = _context.Contacts.Where(c => c.UserId == userId);

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                contactsQuery = contactsQuery.Where(c => 
                    c.Name.Contains(searchString) || 
                    c.Email.Contains(searchString) ||
                    c.Phone.Contains(searchString));
            }

            // Apply favorite filter
            if (filterType == "favorites")
            {
                contactsQuery = contactsQuery.Where(c => c.IsFavorite);
            }

            // Calculate pagination
            var totalContacts = await contactsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalContacts / pageSize);
            
            // Get contacts for current page
            var contacts = await contactsQuery
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new ContactIndexViewModel
            {
                Contacts = contacts,
                SearchString = searchString,
                FilterType = filterType,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalContacts = totalContacts,
                FavoriteContacts = await _context.Contacts.CountAsync(c => c.UserId == userId && c.IsFavorite)
            };

            return View(viewModel);
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Challenge();
                }

                var contact = new Contact
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    IsFavorite = model.IsFavorite,
                    UserId = userId,
                    DateCreated = DateTime.Now,
                    LastModified = DateTime.Now
                };

                // Handle file upload
                if (model.ProfilePhoto != null && model.ProfilePhoto.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePhoto.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePhoto.CopyToAsync(fileStream);
                    }
                    
                    contact.ProfilePhotoPath = "/uploads/" + uniqueFileName;
                }

                _context.Add(contact);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Contact created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            
            if (contact == null)
            {
                return NotFound();
            }

            var viewModel = new ContactEditViewModel
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                Phone = contact.Phone,
                Address = contact.Address,
                IsFavorite = contact.IsFavorite,
                CurrentProfilePhotoPath = contact.ProfilePhotoPath,
                DateCreated = contact.DateCreated
            };

            return View(viewModel);
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContactEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            
            if (contact == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    contact.Name = model.Name;
                    contact.Email = model.Email;
                    contact.Phone = model.Phone;
                    contact.Address = model.Address;
                    contact.IsFavorite = model.IsFavorite;
                    contact.LastModified = DateTime.Now;

                    // Handle file upload
                    if (model.ProfilePhoto != null && model.ProfilePhoto.Length > 0)
                    {
                        // Delete old photo if exists
                        if (!string.IsNullOrEmpty(contact.ProfilePhotoPath))
                        {
                            var oldPhotoPath = Path.Combine(_webHostEnvironment.WebRootPath, contact.ProfilePhotoPath.TrimStart('/'));
                            if (System.IO.File.Exists(oldPhotoPath))
                            {
                                System.IO.File.Delete(oldPhotoPath);
                            }
                        }

                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePhoto.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ProfilePhoto.CopyToAsync(fileStream);
                        }
                        
                        contact.ProfilePhotoPath = "/uploads/" + uniqueFileName;
                    }

                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Contact updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(model);
        }

        // POST: Toggle Favorite
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int id)
        {
            var userId = _userManager.GetUserId(User);
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            
            if (contact == null)
            {
                return NotFound();
            }

            contact.IsFavorite = !contact.IsFavorite;
            contact.LastModified = DateTime.Now;
            
            await _context.SaveChangesAsync();
            
            var message = contact.IsFavorite ? "Contact marked as favorite!" : "Contact removed from favorites!";
            TempData["SuccessMessage"] = message;
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            
            if (contact != null)
            {
                // Delete photo file if exists
                if (!string.IsNullOrEmpty(contact.ProfilePhotoPath))
                {
                    var photoPath = Path.Combine(_webHostEnvironment.WebRootPath, contact.ProfilePhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(photoPath))
                    {
                        System.IO.File.Delete(photoPath);
                    }
                }

                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Contact deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Export CSV
        public async Task<IActionResult> ExportCsv()
        {
            var userId = _userManager.GetUserId(User);
            var contacts = await _context.Contacts
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("Name,Email,Phone,Address,IsFavorite,DateCreated");

            foreach (var contact in contacts)
            {
                csv.AppendLine($"\"{contact.Name}\",\"{contact.Email}\",\"{contact.Phone}\",\"{contact.Address}\",{contact.IsFavorite},{contact.DateCreated:yyyy-MM-dd}");
            }

            var fileName = $"contacts_export_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var bytes = Encoding.UTF8.GetBytes(csv.ToString());

            return File(bytes, "text/csv", fileName);
        }

        // GET: Export XML
        public async Task<IActionResult> ExportXml()
        {
            var userId = _userManager.GetUserId(User);
            var contacts = await _context.Contacts
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Name)
                .Select(c => new ContactExportModel
                {
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone,
                    Address = c.Address,
                    IsFavorite = c.IsFavorite,
                    DateCreated = c.DateCreated
                })
                .ToListAsync();

            var contactsList = new ContactsExportModel { Contacts = contacts };
            
            var serializer = new XmlSerializer(typeof(ContactsExportModel));
            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true });
            
            serializer.Serialize(xmlWriter, contactsList);
            var xml = stringWriter.ToString();

            var fileName = $"contacts_export_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
            var bytes = Encoding.UTF8.GetBytes(xml);

            return File(bytes, "application/xml", fileName);
        }

        // GET: Import Page
        public IActionResult Import()
        {
            return View();
        }

        // POST: Import XML
        [HttpPost]
        public async Task<IActionResult> ImportXml(IFormFile xmlFile)
        {
            if (xmlFile == null || xmlFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select an XML file to import.");
                return View("Import");
            }

            if (!xmlFile.FileName.ToLower().EndsWith(".xml"))
            {
                ModelState.AddModelError("", "Please select a valid XML file.");
                return View("Import");
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Challenge();
                }

                using var stream = xmlFile.OpenReadStream();
                var serializer = new XmlSerializer(typeof(ContactsExportModel));
                var importData = (ContactsExportModel)serializer.Deserialize(stream)!;

                var importedCount = 0;
                var duplicateCount = 0;

                foreach (var importContact in importData.Contacts)
                {
                    // Check for duplicates based on email
                    var existingContact = await _context.Contacts
                        .FirstOrDefaultAsync(c => c.UserId == userId && c.Email == importContact.Email);

                    if (existingContact == null)
                    {
                        var contact = new Contact
                        {
                            Name = importContact.Name,
                            Email = importContact.Email,
                            Phone = importContact.Phone,
                            Address = importContact.Address ?? string.Empty,
                            IsFavorite = importContact.IsFavorite,
                            DateCreated = DateTime.Now,
                            LastModified = DateTime.Now,
                            UserId = userId
                        };

                        _context.Contacts.Add(contact);
                        importedCount++;
                    }
                    else
                    {
                        duplicateCount++;
                    }
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Import completed! {importedCount} contacts imported successfully. {duplicateCount} duplicates skipped.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error importing XML file: {ex.Message}");
                return View("Import");
            }
        }

        private bool ContactExists(int id)
        {
            var userId = _userManager.GetUserId(User);
            return _context.Contacts.Any(e => e.Id == id && e.UserId == userId);
        }
    }
}
