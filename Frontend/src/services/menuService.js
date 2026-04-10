const API_BASE = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5000'

export const menuService = {
  async getMenu(signal) {
    try {
      const response = await fetch(`${API_BASE}/api/menu`, { signal })

      if (!response.ok) {
        throw new Error(`Failed to load menu (${response.status})`)
      }

      const data = await response.json()
      return { data, error: null }
    } catch (error) {
      if (error?.name === 'AbortError') {
        return { data: null, error: null }
      }

      return { data: null, error: error?.message ?? 'Unable to load menu' }
    }
  },
}
