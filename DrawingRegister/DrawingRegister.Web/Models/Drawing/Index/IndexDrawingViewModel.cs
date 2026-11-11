using DrawingRegister.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DrawingRegister.Web.Models.Drawing.Index
{
    /*
        The View Model describes one row of the table, and every row comes from a real drawing that always has a number, a name and a status. Those values are never “not chosen”, so they shouldn’t be nullable. 
     
     */
    public class IndexDrawingViewModel
    {
        public string? Query { get; set; }
        public Status? Status { get; set; }
        public int? ProjectId { get; set; }
        public string? SortBy { get; set; }
        public string? SortDir { get; set; }

        public List<RowDrawingViewModel> Drawings { get; set; } = new();

        public List<SelectListItem> Projects { get; set; } = new();

    }
}
