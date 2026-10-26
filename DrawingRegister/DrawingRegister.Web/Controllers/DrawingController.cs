using DrawingRegister.Data.Context;
using DrawingRegister.Web.Models.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrawingRegister.Web.Controllers
{
    public class DrawingController : Controller
    {
        private readonly AppDbContext _context;

        public DrawingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // *** VERSION 1 - WITH INCLUDE ***
            /* 
                * One trade-off if we need only one prop from db Include(d => d.Project) loads the full Project object, but you only need its Name. Better use is Select V2
                * Include means “load the full entity, plus this related entity too.” You get tracked entity objects back, which is what you want when you’ll edit and save them (Edit/Delete actions).
                * Rule of thumb: Loading to modify → ise Include (or Find)
                
                Tracking:
                * Tracking means EF’s DbContext keeps a list of every entity it loaded, along with a snapshot of the values as they were when loaded. That list is called the change tracker.
                * When you call SaveChanges(), EF compares each tracked entity to its snapshot. If you changed drawing.Name after loading it, EF sees the difference and generates an UPDATE for just that row.
                * Tracked: needed for edit and delete, but costs memory and time per entity.
             */
            var drawingsV1 = await _context.Drawings
                .OrderBy(d=>d.Id)
                .Take(10)
                .Include(d => d.Project)
                .ToListAsync();

            var drawingsView1 = new List<IndexDrawingViewModel>();
            foreach (var drawing in drawingsV1) {
                drawingsView1.Add(new IndexDrawingViewModel
                {
                    Id = drawing.Id,
                    Name = drawing.Name,
                    DocumentNumber = drawing.DocumentNumber,
                    DrawingType = drawing.DrawingType,
                    DrawingStatus = drawing.DrawingStatus,
                    ReleaseDate = drawing.ReleaseDate,
                    //Project = drawing.Project
                });
            }

            // *** VERSION 2 - WITH SELECT ***
            /*
                * EF turns that into a join and fetches only those columns, with no Include needed. It also lets you add derived columns later, like a revision count or the latest revision letter.
                * EF sees d.Project.Name inside the projection and builds the join itself.
                * Select only describes the query. ToListAsync() is what sends it to the database.
                
                REMEMBER 
                * Mixing with Include: if you use Select, Include is ignored.
                * Select means “give me only this shape.” You get plain ViewModel objects back, nothing tracked, which is what you want for read-only screens like a list page.
                * Rule of thumb: reading to display → use Select
                * "Select" results aren’t tracked: a ViewModel is a plain object EF knows nothing about.
                * Untracked: faster and lighter, ideal for read-only lists. That matters when a page loads many rows.
             
             */

            var drawingsV2 = await _context.Drawings
                .OrderBy(drw => drw.Id)
                .Take(10)
                .Select(drw => new IndexDrawingViewModel
                {
                    Id = drw.Id,
                    Name = drw.Name,
                    DocumentNumber = drw.DocumentNumber,
                    DrawingType = drw.DrawingType,
                    DrawingStatus = drw.DrawingStatus,
                    ReleaseDate = drw.ReleaseDate,
                    ProjectName = drw.Project.Name
                })
                .ToListAsync();

            return View(drawingsV2);
        }
    }
}
