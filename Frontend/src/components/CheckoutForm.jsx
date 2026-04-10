import { useState } from "react";

export default function CheckoutForm({ onSubmit }) {
  const [name, setName] = useState("");
  const [phone, setPhone] = useState("");
  const [pickupTime, setPickupTime] = useState("");

  function handleSubmit(e) {
    e.preventDefault();
    onSubmit({ name, phone, pickupTime });
  }

  return (
    <form onSubmit={handleSubmit}>
      <h2>Pickup Details</h2>

      <input
        placeholder="Your name"
        value={name}
        onChange={e => setName(e.target.value)}
        required
      />

      <input
        placeholder="Phone"
        value={phone}
        onChange={e => setPhone(e.target.value)}
        required
      />

      <input
        type="datetime-local"
        value={pickupTime}
        onChange={e => setPickupTime(e.target.value)}
        required
      />

      <button type="submit">Place Order</button>
    </form>
  );
}