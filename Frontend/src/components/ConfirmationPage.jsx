export default function ConfirmationPage({ orderNumber }) {
  return (
    <>
      <h1>Thank you!</h1>
      <p>Your order number is:</p>
      <strong>{orderNumber}</strong>
      <p>Please pick up at your selected time.</p>
    </>
  );
}