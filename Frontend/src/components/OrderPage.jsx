import { useEffect, useState } from "react";
import { fetchMenu } from "../api/menuApi";
import { submitOrder } from "../api/ordersApi";
import MenuList from "../components/MenuList";
import Cart from "../components/Cart";
import CheckoutForm from "../components/CheckoutForm";
import { useCart } from "../state/cartStore";

export default function OrderPage({ onConfirmed }) {
  const [menu, setMenu] = useState([]);
  const cart = useCart();

  useEffect(() => {
    fetchMenu().then(setMenu);
  }, []);

  async function handleSubmitCheckout(customer) {
    const order = {
      items: cart.items.map(i => ({
        menuItemId: i.menuItemId,
        quantity: i.quantity
      })),
      pickupTime: customer.pickupTime,
      customer
    };

    const result = await submitOrder(order);
    cart.clear();
    onConfirmed(result.orderNumber);
  }

  return (
    <>
      <h1>Order Pho Takeaway</h1>

      <MenuList menu={menu} onAdd={cart.addItem} />

      <Cart 
        items={cart.items}
        total={cart.total}
        onRemove={cart.removeItem}
      />

      {cart.items.length > 0 && (
        <CheckoutForm onSubmit={handleSubmitCheckout} />
      )}
    </>
  );
}