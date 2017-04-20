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
        private readonly personal_pageEntities db = new personal_pageEntities();
        // GET: Education_Form
        public async Task<ActionResult> Index()
        {
            return View(await db.Education_Form.ToListAsync());
        }

        // GET: Education_Form/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var education_Form = await db.Education_Form.FindAsync(id);
            if (education_Form == null)
            {
                return HttpNotFound();
            }
            return View(education_Form);
        }

        // GET: Education_Form/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Education_Form/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,name")] Education_Form education_Form)
        {
            if (ModelState.IsValid)
            {
                education_Form.id = Guid.NewGuid();
                db.Education_Form.Add(education_Form);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(education_Form);
        }

        // GET: Education_Form/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var education_Form = await db.Education_Form.FindAsync(id);
            if (education_Form == null)
            {
                return HttpNotFound();
            }
            return View(education_Form);
        }

        // POST: Education_Form/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,name")] Education_Form education_Form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(education_Form).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(education_Form);
        }

        // GET: Education_Form/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var education_Form = await db.Education_Form.FindAsync(id);
            if (education_Form == null)
            {
                return HttpNotFound();
            }
            return View(education_Form);
        }

        // POST: Education_Form/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var education_Form = await db.Education_Form.FindAsync(id);
            db.Education_Form.Remove(education_Form);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}