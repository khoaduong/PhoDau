const API_BASE = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5000'

export async function submitOrder(order) {
  const response = await fetch(`${API_BASE}/api/orders`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(order),
  })

  if (!response.ok) throw new Error(`Order failed (${response.status})`)
  return response.json()
}
