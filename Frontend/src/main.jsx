import { useState } from "react";
import OrderPage from "./pages/OrderPage";
import ConfirmationPage from "./pages/ConfirmationPage";

export default function App() {
  const [orderNumber, setOrderNumber] = useState(null);

  return orderNumber
    ? <ConfirmationPage orderNumber={orderNumber} />
    : <OrderPage onConfirmed={setOrderNumber} />;
}