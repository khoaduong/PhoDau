export default function Cart({ items, total, onRemove }) {
  if (items.length === 0) return <p>Cart is empty</p>;

  return (
    <>
      <h2>Your Order</h2>
      <ul>
        {items.map(i => (
          <li key={i.menuItemId}>
            {i.name} × {i.quantity}
            <button onClick={() => onRemove(i.menuItemId)}>Remove</button>
          </li>
        ))}
      </ul>

      <strong>Total: ${total.toFixed(2)}</strong>
    </>
  );
}
