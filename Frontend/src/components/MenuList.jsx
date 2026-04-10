export default function MenuList({ menu, onAdd }) {
  return (
    <>
      {menu.map(category => (
        <section key={category.id}>
          <h2>{category.name}</h2>

          {category.items.map(item => (
            <div key={item.id}>
              <strong>{item.name}</strong> – ${item.price}
              <p>{item.description}</p>

              <button 
                disabled={!item.isAvailable}
                onClick={() => onAdd(item)}>
                Add
              </button>
            </div>
          ))}
        </section>
      ))}
    </>
  );
}