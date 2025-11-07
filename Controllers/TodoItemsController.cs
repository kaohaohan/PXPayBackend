//using ... 請去recap C++課 引入容器、資料夾
using Microsoft.AspNetCore.Mvc;
using PXPayBackend.Models;
using System.Collections.Generic;
using System.Linq;


namespace PXPayBackend.Controllers
{
 [ApiController] //告訴.NET 這是 API Controller 
    [Route("api/[controller]")] 
    //inheritance
    public class TodoItemsController : ControllerBase
    { //static 共享這個數據_todos
        private static readonly List<TodoItem> _todos = new List<TodoItem>
        {
            new TodoItem { Id = 1, Name = "學習 C#", IsComplete = false },
            new TodoItem { Id = 2, Name = "準備面試", IsComplete = false }
        };
        
        // GET /api/todoitems
        [HttpGet]
         public ActionResult<IEnumerable<TodoItem>> GetTodoItems()
         {
            return _todos;
         }

        //GET /api/todoitems/{id}
        [HttpGet("{id}")]
          public ActionResult<TodoItem> GetTodoItem(long id)
        {
            var todoItem = _todos.Find(x => x.Id == id);
            
            if (todoItem == null)
            {
                return NotFound();
            }
            
            return todoItem;
        }
        // POST /api/todoitems
        [HttpPost]
        public ActionResult<TodoItem> PostTodoItem(TodoItem todoItem)
        {
            long nextId = (_todos.Count > 0) ? _todos.Max(x => x.Id) + 1 : 1;
            todoItem.Id = nextId;
            _todos.Add(todoItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // PUT /api/todoitems/{id}
        [HttpPut("{id}")]
        public IActionResult PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            var existingItem = _todos.Find(x => x.Id == id);
            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = todoItem.Name;
            existingItem.IsComplete = todoItem.IsComplete;

            return NoContent();
        }

        // DELETE /api/todoitems/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteTodoItem(long id)
        {
            var todoItem = _todos.Find(x => x.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _todos.Remove(todoItem);
            return NoContent();
        }

    }

}

/* 
筆記：

1. 複習node.js路由
router.get('/', (req, res) => {
    const data = userService.getAll();
    res.json(data);
});

2. 假資料庫
const todos = [
    { id: 1, name: "學習 C#", isComplete: false },
    { id: 2, name: "準備面試", isComplete: false }
];

3. IEnumerable<TodoItem> 像是Interface
不在乎傳的是List or array 
像是C++ template<typename Iterator>{
    for(auto it = begin; it != end; ++it) {
        std::cout << it->Name << std::endl;
    }
} 

4. Lambda 表達式
id 是 long 型別的變數
x 是 TodoItem 物件
var todoItem = _todos.Find(x => x.Id == id);

像是 JS 會這樣寫 const todoItem = todos.find(x => x.id === id); 
C++
auto it = std::find_if(todos.begin(), todos.end(), 
    [id](const TodoItem& x) { return x.Id == id; });
*/