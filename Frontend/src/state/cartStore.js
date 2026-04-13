import { useState } from "react";

export function useCart() {
  const [items, setItems] = useState([]);

  function addItem(menuItem) {
    setItems(prev => {
      const existing = prev.find(i => i.menuItemId === menuItem.id);
      if (existing) {
        return prev.map(i =>
          i.menuItemId === menuItem.id
            ? { ...i, quantity: i.quantity + 1 }
            : i
        );
      }
      return [...prev, { 
        menuItemId: menuItem.id, 
        name: menuItem.name, 
        price: menuItem.price,
        quantity: 1 
      }];
    });
  }

  function removeItem(id) {
    setItems(prev => prev.filter(i => i.menuItemId !== id));
  }

  function clear() {
    setItems([]);
  }

  const total = items.reduce((sum, i) => sum + i.price * i.quantity, 0);

  return { items, total, addItem, removeItem, clear };
}