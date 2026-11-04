using DrawingRegister.Data.Context;
using DrawingRegister.Data.Entities;
using DrawingRegister.Web.Models.Drawing;
using DrawingRegister.Web.Models.Drawing.Add;
using DrawingRegister.Web.Models.Drawing.Edit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

      
        private async Task<List<SelectListItem>> PopulateProjects()
        {
            var projectsList = await _context.Projects.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name,
            }).ToListAsync();

            return projectsList;
        }

        [HttpGet]
        public async Task<IActionResult> AddDrawing()
        {          
            return View(new AddDrawingViewModel
            {
                Projects = await PopulateProjects()
            });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddDrawing(AddDrawingViewModel addDrawingViewModel)
        {
            /*
                ModelState - is a dictionary of errors, keyed by field name, that lives for the duration of one request. Model binding fills it automatically: if the user types “abc” into a number field, binding adds an error under DocumentNumber. ModelState.IsValid just means “that dictionary has no errors.”             
             */
            if (ModelState.IsValid)
            {
                // use AnyAsync not FindAsync and Find get the entity with this primary key
                /*
                    For anything else you write a condition, and these are the ones you’ll use constantly:

                    AnyAsync(d => ...): does at least one match? Returns bool. Use for yes/no checks like your duplicate test.
                    FirstOrDefaultAsync(d => ...): give me the first match, or null. Use when you need the entity itself.
                    SingleOrDefaultAsync(d => ...): like FirstOrDefault, but throws if more than one matches. Good when you expect exactly one, like a unique DocumentNumber.
                    CountAsync(d => ...): how many match? You’ll need this for the paging total.
                    Where(...): not a lookup but a filter that stays in the chain. This is what your search and filters will use.
                 
                 */
                var drawingNumber = await _context.Drawings.AnyAsync(drw=> drw.DocumentNumber == addDrawingViewModel.DocumentNumber);

                if(drawingNumber)
                {
                    /*
                        - nameof(x) gives the name of something, as a string, at compile time.
                                nameof(vm.DocumentNumber)   // "DocumentNumber"
                                nameof(Index)               // "Index"

                        It never reads the value. Use it whenever you need a name as text.

                        - typeof(X) gives a System.Type object describing a type, not a value or a name.
                                typeof(Status)              // a Type object for the Status enum

                        You used it with Enum.GetValues(typeof(Status)). Use it when an API wants the type itself.

                        Quick rule:

                        - Need the name as text → nameof
                        - Need the value → just read the property
                        - Need the type itself → typeof

                       */

                    /*
                        AddModelError(key, message) lets you add your own entry to that dictionary:

                        ModelState.AddModelError(nameof(addDrawingViewModel.DocumentNumber), "Drawing number already exists.");

                        * The first argument is the key: which field the error belongs to. nameof(...) gives you the string "DocumentNumber" without typos. It must match the property name (and so the input’s name) for the error to attach to that field.
                        * The second is the message the user sees.
                        * Passing an empty string "" as the key makes it a model-level error, not tied to any field.
                     
                      */
                    ModelState.AddModelError(nameof(addDrawingViewModel.DocumentNumber), "Drawing number already exists.");
                    addDrawingViewModel.Projects = await PopulateProjects();
                    return View(addDrawingViewModel);
                }

                var newDarwing = new Drawing
                {
                    DocumentNumber = addDrawingViewModel.DocumentNumber.Value,
                    Name = addDrawingViewModel.Name,
                    Path = addDrawingViewModel.Path,
                    DrawingType = addDrawingViewModel.DrawingType.Value,
                    DrawingStatus = addDrawingViewModel.DrawingStatus.Value,
                    ProjectId = addDrawingViewModel.ProjectId.Value
                };

                _context.Drawings.Add(newDarwing);
                await _context.SaveChangesAsync();
                /*
                      TempData: a short-lived store that survives one redirect. You set it in a POST, redirect, and the next request reads it once, then it’s gone. It’s for page-level messages after an action: “Drawing saved”, “Drawing deleted”.

                    AddModelError: adds an error to ModelState, which exists for the current request only. It’s for validation problems with the data the user just submitted, and you use it when you return View(...) (no redirect) so the form re-renders with the user’s input still in it, and the error sits next to the field.
                 
                    When to use which:

                    * User submitted something invalid, you re-show the form → ModelState / AddModelError.
                    * Action succeeded (or failed for a non-input reason), you redirect → TempData.
                  */
                TempData["Success"] = "Drawing Saved Successfully.";  

                return RedirectToAction(nameof(Index));
            }
            addDrawingViewModel.Projects = await PopulateProjects();
            return View(addDrawingViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditDrawing(int id)
        {
            var drawing = await _context.Drawings.FirstOrDefaultAsync(drw => drw.Id == id);

            if(drawing == null) return NotFound();            
                
            return View(new EditDrawingViewModel
            {
                DocumentNumber = drawing.DocumentNumber,
                Name = drawing.Name,
                Path = drawing.Path,
                DrawingType = drawing.DrawingType,
                DrawingStatus = drawing.DrawingStatus,
                ProjectId = drawing.ProjectId,
                Projects = await PopulateProjects()
            });                
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditDrawing(EditDrawingViewModel editDrawingViewModel)
        {

            if(ModelState.IsValid)
            {
                var drawing = await _context.Drawings.FirstOrDefaultAsync(drw => drw.Id == editDrawingViewModel.Id);

                if (drawing == null) return NotFound();
                
                // drawing do not point itself AND does a different drawing already own entered number?
                var checkDrwNumberDuplicated = await _context.Drawings.AnyAsync(drw => drw.Id != editDrawingViewModel.Id && drw.DocumentNumber == editDrawingViewModel.DocumentNumber);

                if (checkDrwNumberDuplicated)
                {
                    ModelState.AddModelError(nameof(editDrawingViewModel.DocumentNumber), "Drawing number already exists.");

                    editDrawingViewModel.Projects = await PopulateProjects();
                    return View(editDrawingViewModel);
                }

                drawing.DocumentNumber = editDrawingViewModel.DocumentNumber.Value;
                drawing.Name = editDrawingViewModel.Name;
                drawing.Path = editDrawingViewModel.Path;
                drawing.DrawingType = editDrawingViewModel.DrawingType.Value;
                drawing.DrawingStatus = editDrawingViewModel.DrawingStatus.Value;
                drawing.ProjectId = editDrawingViewModel.ProjectId.Value;

                /*
                        use of _context.Update(object);

                        Update is for disconnected entities, objects EF isn’t tracking. Update(entity) says “start tracking this, and assume every column changed.” Typical cases:

                        * An entity arrives from outside the context (an API body, a deserialized object) with no prior load.
                        * You want to skip the SELECT and just write: build new Drawing { Id = ..., ... }, call Update, save.      

                        The cost is every column goes into the UPDATE, and anything you didn’t set (like ReleaseDate) gets overwritten with null.
                        For a small project the performance is irrelevant; the real risk is the silent overwrite. For web forms, “load, change, save” is the safer default.
                    */

                await _context.SaveChangesAsync();
                TempData["Success"] = "Drawing Saved Successfully.";
                return RedirectToAction(nameof(Index));  
            }

            editDrawingViewModel.Projects = await PopulateProjects();
            return View(editDrawingViewModel);
        }

        [ValidateAntiForgeryToken] // protects against cross-site request forger
        [HttpPost]
        public async Task<IActionResult> DeleteDrawing(int id)
        {
            var drawing = await _context.Drawings.FirstOrDefaultAsync(drw => drw.Id == id);

            if (drawing == null) return NotFound();

            _context.Drawings.Remove(drawing);
            await _context.SaveChangesAsync();            

             TempData["Success"] = "Drawing Deleted Successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
