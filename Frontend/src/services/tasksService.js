const API_BASE = '/api';

function getAuthHeaders() {
  const token = localStorage.getItem('token');
  const headers = { 'Content-Type': 'application/json' };
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }
  return headers;
}

export const tasksService = {
  async getAll() {
    try {
      const response = await fetch(`${API_BASE}/tasks`, {
        headers: getAuthHeaders()
      });
      if (!response.ok) throw new Error('Failed to fetch tasks');
      return { data: await response.json(), error: null };
    } catch (error) {
      console.error('Error fetching tasks:', error);
      return { data: null, error: error.message };
    }
  },

  async getById(id) {
    try {
      const response = await fetch(`${API_BASE}/tasks/${id}`, {
        headers: getAuthHeaders()
      });
      if (!response.ok) throw new Error('Task not found');
      return { data: await response.json(), error: null };
    } catch (error) {
      return { data: null, error: error.message };
    }
  },

  async create(title) {
    try {
      if (!title || title.trim().length === 0) {
        return { data: null, error: 'Title is required' };
      }
      if (title.length > 200) {
        return { data: null, error: 'Title must be 200 characters or less' };
      }

      const response = await fetch(`${API_BASE}/tasks`, {
        method: 'POST',
        headers: getAuthHeaders(),
        body: JSON.stringify({ title: title.trim() }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        return { data: null, error: errorData.message || 'Failed to create task' };
      }

      return { data: await response.json(), error: null };
    } catch (error) {
      return { data: null, error: error.message };
    }
  },

  async toggle(id) {
    try {
      const response = await fetch(`${API_BASE}/tasks/${id}/toggle`, {
        method: 'PUT',
        headers: getAuthHeaders(),
      });

      if (!response.ok) throw new Error('Failed to toggle task');
      return { data: null, error: null };
    } catch (error) {
      return { data: null, error: error.message };
    }
  },

  async delete(id) {
    try {
      const response = await fetch(`${API_BASE}/tasks/${id}`, {
        method: 'DELETE',
        headers: getAuthHeaders()
      });

      if (!response.ok) throw new Error('Failed to delete task');
      return { data: null, error: null };
    } catch (error) {
      return { data: null, error: error.message };
    }
  },
};
