﻿using Microsoft.EntityFrameworkCore;
using Todo.Data.Models;

namespace Todo.Data;

public class TodoRepository: ITodoRepository
{
    private readonly TodoContext _context;

    public TodoRepository(TodoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> List()
    {
        return await _context
            .TodoItems
            .ToArrayAsync();
    }

    public async Task<Guid> Create(TodoItem newItem)
    {
        await _context.TodoItems.AddAsync(newItem);
        await _context.SaveChangesAsync();
        return newItem.Id;
    }

    public async Task<bool> UpdateAsync(TodoItem updatedItem)
    {
        var ret = false;
        var t = _context.TodoItems.Update(updatedItem);
        if (t.State == EntityState.Modified)
        {
            await _context.SaveChangesAsync();
            ret = true;
        }
        return ret;
    }
}