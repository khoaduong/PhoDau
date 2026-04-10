export async function submitOrder(order) {
  const res = await fetch("https://localhost:5001/api/orders", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(order)
  });

  if (!res.ok) throw new Error("Order failed");
  return res.json();
}