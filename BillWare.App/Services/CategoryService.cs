using BillWare.App.Common;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class CategoryService : BaseCrudService<CategoryModel>, ICategoryService
    {
        public CategoryService(HttpClient http, 
                               LocalStorageHelper localStorageService) : base(http, localStorageService, "Category")
        {
        }
    }
}
