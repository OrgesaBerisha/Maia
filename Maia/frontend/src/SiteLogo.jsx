import { Link } from 'react-router-dom'
import './SiteLogo.css'

function SiteLogo() {
  return (
    <Link to="/" className="site-logo" aria-label="MAIA — back to home">
      MAIA
    </Link>
  )
}

export default SiteLogo
