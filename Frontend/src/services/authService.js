export const authService = {
  async login(username, password) {
    try {
      const response = await fetch('/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ username, password })
      });

      if (!response.ok) {
        if (response.status === 401) {
          return { data: null, error: 'Invalid credentials' };
        }
        const data = await response.json();
        return { data: null, error: data.error || 'Login failed' };
      }

      const data = await response.json();
      return { data, error: null };
    } catch (err) {
      console.error('Login API error:', err);
      return { data: null, error: 'Network error. Please try again.' };
    }
  }
};
