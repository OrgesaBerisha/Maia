import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useCart } from './CartContext.jsx'
import { useAuth } from './AuthContext.jsx'
import BottomNav from './BottomNav.jsx'
import SiteLogo from './SiteLogo.jsx'
import api from './api/axios.js'
import { loadStripe } from '@stripe/stripe-js'
import { Elements, CardElement, useStripe, useElements } from '@stripe/react-stripe-js'
import './CheckoutPage.css'

const stripePromise = loadStripe('pk_test_51TgBG5HmjGOrfE76nrrnXW6zwpLyGVIpbUcYU7c0REaYv9jlibq0krf7habEF97pyb0R1kPaU1OCtpXLz2V1fX2z0028MxmTmC')

const SHIPPING_COST = 4.99

const EMPTY_FORM = {
  fullName: '', email: '', phone: '',
  address: '', city: '', postalCode: '', country: '',
}

function Field({ label, name, type = 'text', value, onChange, required }) {
  return (
    <div className="field">
      <label className="field-label">{label}{required && ' *'}</label>
      <input
        className="field-input"
        type={type}
        name={name}
        value={value}
        onChange={onChange}
        required={required}
        autoComplete="on"
        spellCheck={false}
      />
    </div>
  )
}

function CheckoutForm() {
  const stripe = useStripe()
  const elements = useElements()
  const { bag, removeFromBag } = useCart()
  const { user } = useAuth()

  const fullName = user
    ? [user.firstName, user.lastName].filter(Boolean).join(' ')
    : ''

  const [form, setForm] = useState({
    ...EMPTY_FORM,
    fullName,
    email: user?.email ?? '',
  })
  const [paymentMethod, setPaymentMethod] = useState('cash')
  const [errors, setErrors]       = useState({})
  const [submitting, setSubmit]   = useState(false)
  const [confirmed, setConfirmed] = useState(false)
  const [orderRef, setOrderRef]   = useState('')
  const [orderError, setOrderError] = useState('')

  const subtotal = bag.reduce((s, i) => s + parseFloat(i.price), 0)
  const total    = subtotal + (bag.length > 0 ? SHIPPING_COST : 0)

  const handleChange = e => {
    setForm(f => ({ ...f, [e.target.name]: e.target.value }))
    setErrors(er => ({ ...er, [e.target.name]: '' }))
  }

  const validate = () => {
    const e = {}
    if (!form.fullName.trim())   e.fullName   = 'Required'
    if (!form.email.trim())      e.email      = 'Required'
    if (!form.address.trim())    e.address    = 'Required'
    if (!form.city.trim())       e.city       = 'Required'
    if (!form.postalCode.trim()) e.postalCode = 'Required'
    if (!form.country.trim())    e.country    = 'Required'
    return e
  }

  const handleSubmit = async e => {
    e.preventDefault()
    const errs = validate()
    if (Object.keys(errs).length) { setErrors(errs); return }

    setSubmit(true)
    setOrderError('')

    if (paymentMethod === 'card') {
      try {
        const { data } = await api.post('/payment/create-intent', { amount: total })
        const cardElement = elements.getElement(CardElement)
        const result = await stripe.confirmCardPayment(data.clientSecret, {
          payment_method: {
            card: cardElement,
            billing_details: { name: form.fullName, email: form.email },
          },
        })
        if (result.error) {
          setOrderError(result.error.message)
          setSubmit(false)
          return
        }
      } catch (err) {
        setOrderError('Payment failed. Please try again.')
        setSubmit(false)
        return
      }
    }

    try {
      const shippingAddress = `${form.address}, ${form.city} ${form.postalCode}, ${form.country}`
      await api.post('/Order', {
        fullName:       form.fullName,
        email:          form.email,
        phone:          form.phone,
        address:        form.address,
        city:           form.city,
        postalCode:     form.postalCode,
        country:        form.country,
        shippingAddress,
        paymentMethod,
        items: bag.map(i => ({
          productId:     i.id,
          productSource: i.productSource ?? 'women',
          productName:   i.name,
          imageUrl:      i.image ?? '',
          price:         parseFloat(i.price) || 0,
          quantity:      i.quantity ?? 1,
        })),
      })
    } catch (err) {
      const msg = err?.response?.data?.message ?? err?.response?.data ?? err.message
      setOrderError(typeof msg === 'string' ? msg : 'Order failed. Please try again.')
      console.error('Order API error:', msg)
      setSubmit(false)
      return
    }

    const ref = `MAIA-${Date.now().toString().slice(-6)}`
    setOrderRef(ref)
    for (const item of bag) {
      await removeFromBag(item.id, item.size, item.cartItemId)
    }
    setConfirmed(true)
    setSubmit(false)
  }

  if (confirmed) {
    return (
      <div className="checkout-page">
        <div className="checkout-confirmed">
          <p className="confirmed-ref">{orderRef}</p>
          <h2 className="confirmed-title">Order Confirmed</h2>
          <p className="confirmed-sub">
            Thank you, {form.fullName.split(' ')[0]}. We'll send updates to {form.email}.
          </p>
          <Link to="/" className="confirmed-btn">CONTINUE SHOPPING</Link>
        </div>
      </div>
    )
  }

  if (bag.length === 0) {
    return (
      <div className="checkout-page">
        <div className="checkout-confirmed">
          <h2 className="confirmed-title">Your bag is empty</h2>
          <Link to="/search" className="confirmed-btn">SHOP NOW</Link>
        </div>
      </div>
    )
  }

  return (
    <div className="checkout-page">
      <svg
        className="checkout-blob"
        viewBox="0 0 1440 220"
        preserveAspectRatio="none"
        xmlns="http://www.w3.org/2000/svg"
        aria-hidden="true"
      >
        <path
          d="M0,0 L1440,0 L1440,140
             C1340,162 1200,178 1060,164
             C920,150 800,118 660,132
             C520,146 380,178 240,188
             C160,194 80,190 0,196
             Z"
          fill="#d4c5b3"
        />
      </svg>

      <header className="checkout-header">
        <SiteLogo />
        <span className="checkout-title">CHECKOUT</span>
        <Link to="/bag" className="checkout-back">← BAG</Link>
      </header>

      <main className="checkout-main">
        <form className="checkout-form" onSubmit={handleSubmit} noValidate>
          <h3 className="form-section-title">DELIVERY DETAILS</h3>

          <div className="form-row">
            <Field label="Full Name" name="fullName" value={form.fullName} onChange={handleChange} required />
          </div>
          <div className="form-row form-row--2">
            <Field label="Email" name="email" type="email" value={form.email} onChange={handleChange} required />
            <Field label="Phone" name="phone" type="tel" value={form.phone} onChange={handleChange} />
          </div>
          <div className="form-row">
            <Field label="Address" name="address" value={form.address} onChange={handleChange} required />
          </div>
          <div className="form-row form-row--3">
            <Field label="City"        name="city"       value={form.city}       onChange={handleChange} required />
            <Field label="Postal Code" name="postalCode" value={form.postalCode} onChange={handleChange} required />
            <Field label="Country"     name="country"    value={form.country}    onChange={handleChange} required />
          </div>

          <h3 className="form-section-title" style={{ marginTop: 24 }}>PAYMENT METHOD</h3>
          <div className="payment-methods">
            <label className={`payment-option${paymentMethod === 'cash' ? ' payment-option--active' : ''}`}>
              <input type="radio" value="cash" checked={paymentMethod === 'cash'} onChange={() => setPaymentMethod('cash')} />
              <span>💵 Cash on Delivery</span>
            </label>
            <label className={`payment-option${paymentMethod === 'card' ? ' payment-option--active' : ''}`}>
              <input type="radio" value="card" checked={paymentMethod === 'card'} onChange={() => setPaymentMethod('card')} />
              <span>💳 Pay by Card</span>
            </label>
          </div>

          {paymentMethod === 'card' && (
            <div className="stripe-card-wrap">
              <CardElement options={{
                style: {
                  base: { fontSize: '16px', color: '#1a1208', fontFamily: 'inherit', '::placeholder': { color: '#aaa' } },
                  invalid: { color: '#e63946' },
                }
              }} />
            </div>
          )}

          {Object.values(errors).some(Boolean) && (
            <p className="form-error">Please fill in all required fields.</p>
          )}
          {orderError && (
            <p className="form-error">{orderError}</p>
          )}

          <button
            type="submit"
            className={`checkout-submit${submitting ? ' checkout-submit--loading' : ''}`}
            disabled={submitting || (paymentMethod === 'card' && !stripe)}
          >
            {submitting
              ? (paymentMethod === 'card' ? 'PROCESSING PAYMENT...' : 'PLACING ORDER...')
              : `PLACE ORDER — ${total.toFixed(2)} EUR`}
          </button>
        </form>

        <aside className="checkout-summary">
          <h3 className="form-section-title">ORDER SUMMARY</h3>

          <div className="summary-items">
            {bag.map((item, i) => (
              <div key={`${item.id}-${item.size}-${i}`} className="summary-item">
                <img src={item.image} alt={item.name} className="summary-img" />
                <div className="summary-item-info">
                  <span className="summary-item-name">{item.name}</span>
                  <span className="summary-item-size">Size: {item.size}</span>
                </div>
                <span className="summary-item-price">{item.price}</span>
              </div>
            ))}
          </div>

          <div className="summary-totals">
            <div className="summary-row">
              <span>SUBTOTAL</span><span>{subtotal.toFixed(2)} EUR</span>
            </div>
            <div className="summary-row">
              <span>SHIPPING</span><span>{SHIPPING_COST.toFixed(2)} EUR</span>
            </div>
            <div className="summary-divider" />
            <div className="summary-row summary-row--total">
              <span>TOTAL</span><span>{total.toFixed(2)} EUR</span>
            </div>
          </div>
        </aside>
      </main>

      <BottomNav />
    </div>
  )
}

export default function CheckoutPage() {
  return (
    <Elements stripe={stripePromise}>
      <CheckoutForm />
    </Elements>
  )
}

