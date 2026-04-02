import { tasksService } from './services/tasksService.js';

export class TasksManager {
  constructor() {
    this.tasks = [];
    this.error = null;
    this.init();
  }

  init() {
    this.setupEventListeners();
    this.loadTasks();
  }

  setupEventListeners() {
    const form = document.getElementById('task-form');
    const input = document.getElementById('title');

    form.addEventListener('submit', async (e) => {
      e.preventDefault();
      const title = input.value.trim();

      if (!title) {
        this.showError('Please enter a task title');
        return;
      }

      if (title.length > 200) {
        this.showError('Title must be 200 characters or less');
        return;
      }

      const { data, error } = await tasksService.create(title);

      if (error) {
        this.showError(error);
        return;
      }

      input.value = '';
      this.clearError();
      this.tasks.push(data);
      this.renderTasks();
      console.log('Task added successfully:', data);
    });
  }

  async loadTasks() {
    const { data, error } = await tasksService.getAll();

    if (error) {
      this.showError(error);
      return;
    }

    this.tasks = data || [];
    this.renderTasks();
  }

  async toggleTask(id) {
    const { error } = await tasksService.toggle(id);

    if (error) {
      this.showError(error);
      return;
    }

    const task = this.tasks.find((t) => t.id === id);
    if (task) {
      task.isCompleted = !task.isCompleted;
      this.renderTasks();
    }
  }

  async deleteTask(id) {
    const { error } = await tasksService.delete(id);

    if (error) {
      this.showError(error);
      return;
    }

    this.tasks = this.tasks.filter((t) => t.id !== id);
    this.renderTasks();
  }

  renderTasks() {
    const tasksList = document.getElementById('tasks');
    this.clearError();

    if (this.tasks.length === 0) {
      tasksList.innerHTML = '<li class="empty">No tasks yet. Add one above!</li>';
      return;
    }

    tasksList.innerHTML = this.tasks
      .map(
        (task) => `
      <li class="task-item ${task.isCompleted ? 'completed' : ''}">
        <input 
          type="checkbox" 
          ${task.isCompleted ? 'checked' : ''} 
          onchange="tasksManager.toggleTask('${task.id}')"
        />
        <span class="task-text">${this.escapeHtml(task.title)}</span>
        <button class="delete-btn" onclick="tasksManager.deleteTask('${task.id}')">Delete</button>
        <span class="task-date">${new Date(task.createdAt).toLocaleDateString()}</span>
      </li>
    `
      )
      .join('');
  }

  showError(message) {
    console.error('Error:', message);
    const errorDiv = document.getElementById('task-error');
    if (errorDiv) {
      errorDiv.textContent = message;
      errorDiv.style.display = 'block';
      
      // Auto-clear error after 5 seconds
      setTimeout(() => {
        if (errorDiv) errorDiv.style.display = 'none';
      }, 5000);
    }
  }

  clearError() {
    const errorDiv = document.getElementById('task-error');
    if (errorDiv) {
      errorDiv.style.display = 'none';
    }
  }

  escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
  }
}

// Initialize on page load
window.tasksManager = new TasksManager();
