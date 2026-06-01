import { useState, useEffect } from 'react';
import './SizeModal.css';

const SIZES = ['XS', 'S', 'M', 'L', 'XL', 'XXL'];

function SizeModal({ product, onClose, onAddToBag }) {
  const [selected, setSelected] = useState(null);
  const [added, setAdded] = useState(false);

  /* close on Escape */
  useEffect(() => {
    const onKey = e => { if (e.key === 'Escape') onClose(); };
    window.addEventListener('keydown', onKey);
    return () => window.removeEventListener('keydown', onKey);
  }, [onClose]);

  const handleAdd = () => {
    if (!selected) return;
    onAddToBag({ ...product, size: selected });
    setAdded(true);
    setTimeout(onClose, 900);
  };

  return (
    <div className="modal-backdrop" onClick={onClose}>
      <div
        className="modal"
        onClick={e => e.stopPropagation()}
        role="dialog"
        aria-modal="true"
        aria-label={`Select size for ${product.name}`}
      >
        {/* Close */}
        <button className="modal-close" onClick={onClose} aria-label="Close">✕</button>

        {/* Product preview */}
        <div className="modal-product">
          <img src={product.image} alt={product.name} className="modal-img" />
          <div className="modal-product-info">
            <span className="modal-product-name">{product.name}</span>
            <span className="modal-product-price">{product.price}</span>
          </div>
        </div>

        <div className="modal-divider" />

        {/* Size grid */}
        <p className="modal-size-label">SELECT SIZE</p>
        <div className="modal-sizes">
          {SIZES.map(size => (
            <button
              key={size}
              className={`size-btn${selected === size ? ' size-btn--active' : ''}`}
              onClick={() => setSelected(size)}
            >
              {size}
            </button>
          ))}
        </div>

        {/* Add to bag */}
        <button
          className={`modal-add-btn${!selected ? ' modal-add-btn--disabled' : ''}${added ? ' modal-add-btn--done' : ''}`}
          onClick={handleAdd}
          disabled={!selected}
        >
          {added ? '✓ ADDED TO BAG' : 'ADD TO BAG'}
        </button>
      </div>
    </div>
  );
}

export default SizeModal;
