const API_BASE = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5000'

export async function fetchMenu(signal) {
  const response = await fetch(`${API_BASE}/api/menu`, { signal })
  if (!response.ok) throw new Error(`Failed to load menu (${response.status})`)
  return response.json()
}
