export async function fetchMenu() {
  const res = await fetch("https://localhost:5001/api/menu");
  if (!res.ok) throw new Error("Failed to load menu");
  return res.json();
}
``