using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using personal_pages;

namespace Personal_Pages.Controllers
{
    public class Education_FormController : Controller
    {
        private readonly personal_pageEntities _db = new personal_pageEntities();
        // GET: Education_Form
        public async Task<ActionResult> Index()
        {
            return View(await _db.Education_Form.ToListAsync());
        }

        // GET: Education_Form/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var educationForm = await _db.Education_Form.FindAsync(id);
            if (educationForm == null)
            {
                return HttpNotFound();
            }
            return View(educationForm);
        }

        // GET: Education_Form/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Education_Form/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Education_Form educationForm)
        {
            if (ModelState.IsValid)
            {
                educationForm.id = Guid.NewGuid();
                _db.Education_Form.Add(educationForm);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(educationForm);
        }

        // GET: Education_Form/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var educationForm = await _db.Education_Form.FindAsync(id);
            if (educationForm == null)
            {
                return HttpNotFound();
            }
            return View(educationForm);
        }

        // POST: Education_Form/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Education_Form educationForm)
        {
            if (!ModelState.IsValid) return View(educationForm);
            _db.Entry(educationForm).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Education_Form/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var educationForm = await _db.Education_Form.FindAsync(id);
            if (educationForm == null)
            {
                return HttpNotFound();
            }
            return View(educationForm);
        }

        // POST: Education_Form/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var educationForm = await _db.Education_Form.FindAsync(id);
            _db.Education_Form.Remove(educationForm);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}