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
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        /*
            Expression Body: no braces, no return. It’s a single expression, and its result is the value. Same behaviour, less typing.

                public int TotalPages => (int)Math.Ceiling(TotalCount / (decimal)PageSize);
         
            Block Body: curly braces, can hold several statements, and needs an explicit return if it produces a value.
                
                public int TotalPages
                {
                    get
                    {
                        return (int)Math.Ceiling(TotalCount / (decimal)PageSize);
                    }
                }

            Where each is used:
            * Expression body: properties and methods whose whole job is one expression. Calculated properties are the classic case. Also short methods like int Double(int x) => x * 2;.
            * Block body: when you need several statements, local variables, if/else, loops, or early returns. Like SortLink and PaginationLink methods build and return a dictionary, so block bodies fit.
         */
        public int TotalPages => (int)Math.Ceiling(TotalCount / (decimal)PageSize); // => read only 

        public List<RowDrawingViewModel> Drawings { get; set; } = new();

        public List<SelectListItem> Projects { get; set; } = new();

    }
}
