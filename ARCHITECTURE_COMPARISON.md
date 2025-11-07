# Node.js vs .NET 架构对比（三层架构）

## Node.js/Express 正规架构（三层）

```
┌─────────────────────────────────────────────────┐
│  routes/todos.js (路由层)                        │
│  - 只负责 URL 映射                               │
│  - 不写业务逻辑                                  │
└─────────────────────────────────────────────────┘
                    ↓ 调用
┌─────────────────────────────────────────────────┐
│  controllers/todoController.js (控制器层)        │
│  - 接收请求、验证参数                            │
│  - 调用 service                                  │
│  - 返回响应                                      │
└─────────────────────────────────────────────────┘
                    ↓ 调用
┌─────────────────────────────────────────────────┐
│  services/todoService.js (业务逻辑层)            │
│  - 真正的业务逻辑                                │
│  - 数据库操作                                    │
│  - 复杂运算                                      │
└─────────────────────────────────────────────────┘
                    ↓ 使用
┌─────────────────────────────────────────────────┐
│  models/Todo.js (数据模型层)                     │
│  - 定义数据结构                                  │
│  - Mongoose Schema                               │
└─────────────────────────────────────────────────┘
```

---

## 示例代码

### 1. routes/todos.js (路由层)
```javascript
const express = require('express');
const router = express.Router();
const todoController = require('../controllers/todoController');

// 只负责映射 URL → Controller 方法
router.get('/',     todoController.getAll);
router.get('/:id',  todoController.getById);
router.post('/',    todoController.create);
router.put('/:id',  todoController.update);
router.delete('/:id', todoController.delete);

module.exports = router;
```

**职责：** 定义"哪个网址对应哪个 controller 方法"

---

### 2. controllers/todoController.js (控制器层)
```javascript
const todoService = require('../services/todoService');

// 控制器：接收请求 → 调用 service → 返回响应
exports.getAll = async (req, res) => {
  try {
    const todos = await todoService.getAllTodos(); // 调用 service
    res.json(todos);
  } catch (error) {
    res.status(500).json({ message: error.message });
  }
};

exports.getById = async (req, res) => {
  try {
    const todo = await todoService.getTodoById(req.params.id); // 调用 service
    if (!todo) {
      return res.status(404).json({ message: 'Not found' });
    }
    res.json(todo);
  } catch (error) {
    res.status(500).json({ message: error.message });
  }
};

exports.create = async (req, res) => {
  try {
    // 1. 验证参数
    if (!req.body.name) {
      return res.status(400).json({ message: 'Name is required' });
    }
    
    // 2. 调用 service
    const newTodo = await todoService.createTodo(req.body);
    
    // 3. 返回响应
    res.status(201).json(newTodo);
  } catch (error) {
    res.status(400).json({ message: error.message });
  }
};

// ... 其他方法
```

**职责：**
- 接收 HTTP 请求 (req, res)
- 验证参数
- 调用 Service 层
- 处理错误
- 返回 HTTP 响应

---

### 3. services/todoService.js (业务逻辑层)
```javascript
const Todo = require('../models/Todo');

// Service：真正的业务逻辑
exports.getAllTodos = async () => {
  // 复杂的业务逻辑可以写在这里
  const todos = await Todo.find();
  return todos;
};

exports.getTodoById = async (id) => {
  return await Todo.findById(id);
};

exports.createTodo = async (todoData) => {
  // 这里可以有复杂的业务规则
  // 例如：检查重复、计算额外字段、发送通知等
  
  const todo = new Todo({
    name: todoData.name,
    isComplete: todoData.isComplete || false,
    createdAt: new Date()
  });
  
  return await todo.save();
};

exports.updateTodo = async (id, todoData) => {
  // 这里可以有复杂的更新逻辑
  const todo = await Todo.findById(id);
  if (!todo) return null;
  
  todo.name = todoData.name;
  todo.isComplete = todoData.isComplete;
  todo.updatedAt = new Date();
  
  return await todo.save();
};

exports.deleteTodo = async (id) => {
  return await Todo.findByIdAndDelete(id);
};
```

**职责：**
- 真正的业务逻辑
- 数据库操作
- 复杂运算
- 与第三方 API 交互

---

### 4. models/Todo.js (数据模型层)
```javascript
const mongoose = require('mongoose');

const todoSchema = new mongoose.Schema({
  name: {
    type: String,
    required: true
  },
  isComplete: {
    type: Boolean,
    default: false
  },
  createdAt: {
    type: Date,
    default: Date.now
  }
});

module.exports = mongoose.model('Todo', todoSchema);
```

**职责：** 定义数据结构

---

## .NET 的对应架构

```
┌─────────────────────────────────────────────────┐
│  Controllers/TodoItemsController.cs              │
│  [Route("api/[controller]")]                     │
│  [HttpGet] GetAll() { ... }                      │
│                                                  │
│  = Node.js 的 routes + controllers 合体          │
└─────────────────────────────────────────────────┘
                    ↓ 调用
┌─────────────────────────────────────────────────┐
│  Services/TodoService.cs                         │
│  - 真正的业务逻辑                                │
│  - 数据库操作                                    │
│                                                  │
│  = Node.js 的 services                           │
└─────────────────────────────────────────────────┘
                    ↓ 使用
┌─────────────────────────────────────────────────┐
│  Models/TodoItem.cs                              │
│  - 定义数据结构                                  │
│                                                  │
│  = Node.js 的 models                             │
└─────────────────────────────────────────────────┘
```

---

## 关键差异

| 层级 | Node.js | .NET |
|------|---------|------|
| **路由层** | `routes/todos.js` | `[Route]` 属性 (写在 Controller 里) |
| **控制器层** | `controllers/todoController.js` | `Controllers/TodoItemsController.cs` |
| **服务层** | `services/todoService.js` | `Services/TodoService.cs` |
| **模型层** | `models/Todo.js` | `Models/TodoItem.cs` |

**.NET 的特色：**
- 把 **Routes** 和 **Controllers** 合并成一个档案
- 用 `[Route]`, `[HttpGet]` 等属性来定义路由
- Service 层是可选的（小项目可以不用，业务逻辑直接写在 Controller）

---

## 为什么要分三层？

### 好处 1：职责分离
- **Controller**：只管接收请求、返回响应
- **Service**：只管业务逻辑
- 各司其职，不会混乱

### 好处 2：可测试性
```javascript
// 测试 Service 层（不需要 HTTP 请求）
const todoService = require('./services/todoService');
const result = await todoService.createTodo({ name: 'Test' });
```

### 好处 3：可重用性
```javascript
// 同一个 Service 可以被不同的 Controller 调用
// 或者被 Cron Job、WebSocket 等其他地方调用
```

---

## Day 1 我们的简化版本

**今天为了快速学习，我们会简化架构：**
- 只用 **Controller** + **Model**
- 业务逻辑直接写在 Controller（因为很简单）
- **明天 Day 2** 再引入 Service 层 + 真实数据库

```
Day 1 (简化版)：
  Controller (路由 + 控制器 + 业务逻辑)
     ↓
  Model (数据结构)

Day 2 (完整版)：
  Controller (路由 + 控制器)
     ↓
  Service (业务逻辑)
     ↓
  Model (数据结构)
     ↓
  Database (SQL Server)
```

