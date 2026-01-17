using MyRecipeBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Domain.Entities
{
    public class Recipe : EntityBase
    {
        public string Title { get; set; } = string.Empty;
        public CookingTime? CookingTime { get; set; } // "?" -> can be null
        public Difficulty? Difficulty { get; set; }
        public IList<Ingredient> Ingredients { get; set; } = [];
        public IList<Instruction> Instructions { get; set; } = [];
        public IList<DishType> DishTypes { get; set; } = [];
        public string? ImageIdentifier { get; set; }
        public long UserId { get; set; }
    }
}
