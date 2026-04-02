const API_URL = "https://localhost:5001/api/tasks";

export async function getTasks() {
  const res = await fetch(API_URL);
  return res.json();
}

export async function createTask(title) {
  await fetch(API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ title })
  });
}

export async function toggleTask(id) {
  await fetch(`${API_URL}/${id}/toggle`, { method: "PUT" });
}

export async function deleteTask(id) {
  await fetch(`${API_URL}/${id}`, { method: "DELETE" });
}
``